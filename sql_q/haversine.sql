CREATE PROCEDURE Haversine @custLat decimal(8, 6), @custLng decimal(9,6) 
AS
SELECT
*,
(
   6371 * --km
   acos(cos(radians(@custLat)) * 
   cos(radians(Latitude)) * 
   cos(radians(Longitude) - 
   radians(@custLng)) + 
   sin(radians(@custLat)) * 
   sin(radians(Latitude)))
) AS Distance 
FROM clinic_address 
ORDER BY Distance ASC
