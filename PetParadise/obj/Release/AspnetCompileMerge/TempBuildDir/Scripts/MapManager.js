class MapManager {
    constructor(mapElement, option) {
        this.defaultLocation = new google.maps.LatLng(0, 0);
        this.option = option || {};
        this.map = new google.maps.Map(mapElement, this.option);
        this.marker = new google.maps.Marker({
            map: this.map,
            draggable: true,
            animation: google.maps.Animation.DROP,
            position: this.defaultLocation
        });
        this.position = this.defaultLocation;
        this.bindEvents(this.marker);
    }

    setPosition(position) {
        this.map.setCenter(position);
        this.marker.setPosition(position);
        this.position = position;
    }

    getPosition() {
        return this.position;
    }

    getMap() {
        return this.map;
    }

    getMarker() {
        return this.marker;
    }

    getGeocodePosition(position, success_callback, failure_callback) {
        position = position || {
            latLng: this.position
        };
        const geocoder = new google.maps.Geocoder();
        geocoder.geocode(position,
        (res, status) => {
            if (status == google.maps.GeocoderStatus.OK) {
                success_callback(res);
            }
            else {
                failure_callback();
            }
        })
    }
    static getBrowserLocation(cb) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(cb,
            () => {
                return null;
            });
        }
        else {
            return null;
        }
    }


    bindEvents(marker) {
        google.maps.event.addListener(marker, "dragend", () => {
            const pos = {
                lat: marker.getPosition().lat(),
                lng: marker.getPosition().lng()
            }

            this.setPosition(pos);
            this.getGeocodePosition(null, (res) => {
                $("#map-result").text(res[0].formatted_address);
            })
            $("#latitude").val(pos.lat);
            $("#longitude").val(pos.lng);
        });
    }
}
