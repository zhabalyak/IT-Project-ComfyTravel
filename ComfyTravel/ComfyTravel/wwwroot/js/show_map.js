ymaps.ready(function () {
    var myMap = new ymaps.Map('map', {
        center: [55.806059, 49.177079],
        zoom: 11,
        controls: ['zoomControl']
    });

    var points_pack_x = document.getElementById('points_x').textContent.split(", ");
    var points_pack_y = document.getElementById('points_y').textContent.split(", ");
    var points_pack_names = document.getElementById('points_names').textContent.split(", ");

    //console.log(points_pack_x);
    //console.log(points_pack_y);
    //console.log(points_pack_names);

    let points_pack = [[points_pack_x, points_pack_y], points_pack_names];

    //console.log(points_pack);

    take_points(points_pack);

    function take_points(points_pack) {
        show_points_on_map(points_pack);
    }

    function show_points_on_map(points_pack) {
        points = points_pack[0]
        points_names = points_pack[1]

        //console.log(points_names.length);

        for (i = 0; i < points_names.length; i++) {
            myPlacemark = new ymaps.Placemark([parseFloat(points[0][i].replace(",", ".")), parseFloat(points[1][i].replace(",", "."))], { hintContent: points_names[i] });
            myMap.geoObjects.add(myPlacemark);
        }
    }

    //function show_points_on_map(points_pack) {
    //    points = points_pack[0]
    //    points_names = points_pack[1]

    //    for (i = 0; i < points.length; i++) {
    //        myPlacemark = new ymaps.Placemark(points[i], { hintContent: points_names[i] });
    //        myMap.geoObjects.add(myPlacemark);
    //    }
    //}

});