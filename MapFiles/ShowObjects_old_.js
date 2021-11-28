ymaps.ready(function () 
{
	var myMap = new ymaps.Map('map', {
            center: [55.806059, 49.177079],
            zoom: 11,
            controls: ['zoomControl']
        }),

        button_show = document.getElementById("btn_show");


    button_show.addEventListener("click", function(e) {
        points_pack = get_points()
        points = points_pack[0]
        points_names = points_pack[1]

        // for (point of points)
        for (i = 0; i < points.length; i++)
        {
            myPlacemark = new ymaps.Placemark(points[i], {hintContent: points_names[i]});
            myMap.geoObjects.add(myPlacemark);
        }   
    });

    function get_points (e) {
        var points = [[[55.806059, 49.177076], [55.811681, 49.100693]],
                        ["точка 1", "точка 2"]]
        return points;
    }  

});