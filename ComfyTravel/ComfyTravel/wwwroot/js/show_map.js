ymaps.ready(function () {
    var myMap = new ymaps.Map('map', {
        center: [55.806059, 49.177079],
        zoom: 11,
        controls: ['zoomControl']
    });

    // для тестового запуска
    // get_points_test();
    // function get_points_test () {
    //     var points = [[[55.806059, 49.177076], [55.811681, 49.100693]],
    //                     ["точка 1", "точка 2"]]
    //     show_points_on_map(points);
    // }

    function take_points(points_pack) {
        show_points_on_map(points_pack);
    }

    function show_points_on_map(points_pack) {
        points = points_pack[0]
        points_names = points_pack[1]

        for (i = 0; i < points.length; i++) {
            myPlacemark = new ymaps.Placemark(points[i], { hintContent: points_names[i] });
            myMap.geoObjects.add(myPlacemark);
        }
    }

});