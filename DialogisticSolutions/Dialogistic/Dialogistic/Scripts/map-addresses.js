// Extends jquerry to return json type from controller
jQuery.extend({
    getValues: function (url) {
        var result = null;
        $.ajax({
            url: url,
            type: 'get',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                result = data;
            }
        });
        return result;
    }
});

// Setup Map
$(document).ready(function () {
    var planes = $.getValues("/Admins/MapAddresses");

    mapboxgl.accessToken = 'pk.eyJ1IjoiY2FlY3VzIiwiYSI6ImNqdmw1eTgzbjB5dXQ0NHFyeXU2Z3czajAifQ.cT_98KEDesgr14FFbPaJ3w';

    var map = new mapboxgl.Map({
        container: 'mapid', // container id
        style: 'mapbox://styles/mapbox/streets-v11', // stylesheet location
        center: [-90.120854, 36.152006], // starting position [lng, lat]
        zoom: 2 // starting zoom
    });

    // Once the map is loaded, add all the markers
    map.on('load', function () {
        for (var i = 0; i < planes.length; i++) {
            var marker = new mapboxgl.Marker()
                .setLngLat([planes[i][1], planes[i][0]])
                .addTo(map);
        }

    });

});