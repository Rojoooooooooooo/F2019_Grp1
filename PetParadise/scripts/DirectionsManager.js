class DirectionsManager {
    constructor(map) {
        this.directionsService = new google.maps.DirectionsService();
        this.directionsRenderer = new google.maps.DirectionsRenderer();
        this.directionsRenderer.setMap(map);
    }

    calcRoute(origin, destination) {
        const req = {
            origin,
            destination,
            travelMode: google.maps.TravelMode.DRIVING,
            unitSystem: google.maps.UnitSystem.METRIC
        }

        this.directionsService.route(req, (res, status) => {
            if(status == google.maps.DirectionsStatus.OK){
                console.log(res)
                const { start_address, end_address, distance, duration } = res.routes[0].legs[0];
                this.directionsRenderer.setDirections(res);
                
                this.updateResultBox(start_address, end_address, distance.text, duration.text);
            }
            else {
                this.directionsRenderer.setDirections({routes : []});
                console.log("unavailable")
            }
        })
    }

    updateResultBox(from, to, distance, duration) {
        $("#from-location-holder").text(from);
        $("#to-location-holder").text(to);
        $("#distance-holder").text(distance);
        $("#duration-holder").text(duration);
        
    }
}