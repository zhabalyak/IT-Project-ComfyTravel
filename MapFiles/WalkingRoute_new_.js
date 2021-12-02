ymaps.ready(function () {
    var myMap = new ymaps.Map('map', {
            center: [55.806059, 49.177079],
            zoom: 11,
           controls: ['zoomControl']
        }, {
            buttonMaxWidth: 300
        });

    var points_pack_x = document.getElementById('points_x').textContent.split(", ");
    var points_pack_y = document.getElementById('points_y').textContent.split(", ");
    var points_names = document.getElementById('points_names').textContent.split(", ");
    var route_mode = document.getElementById('route_mode').textContent.split(", ");
    //mode = 1, тогда пешеходный/общ транспорт, 
    //     = 2, тогда на машине
    //     = 3, тогда обществ транспорт

    //console.log(points_pack_x);
    //console.log(points_pack_y);

    let points_pack = [points_pack_x, points_pack_y];

    //console.log(points_pack);

    show_route_on_map(points_pack, points_names, route_mode);

    function show_route_on_map (points_pack, points_names, mode) 
    {
        //points parsing
        points = new Array(points_pack[0].length);
        for (i = 0; i < points_pack[0].length; i++) {
            points[i] = [parseFloat(points_pack[0][i].replace(",", ".")), 
                         parseFloat(points_pack[1][i].replace(",", "."))];
        }


        var multiRoute = new ymaps.multiRouter.MultiRoute(
        {
            referencePoints: points,
            params: {
                //nип маршрутизации - пешеходный
                routingMode: 'pedestrian' 
            }
        }, {
        // сделать точки А, Б... невидимыми
            wayPointVisible:false,
        // убрать метки с км для пешеходного типа
            routeActiveMarkerVisible: false,
        // откл возможность кликать на маршрут
            routeOpenBalloonOnClick: false,
        // для авто-маршрута
            routeActiveStrokeStyle: "solid",
            routeActiveStrokeColor: "#E63E92",
        // внешний вид линии пешеходного маршрута
            routeActivePedestrianSegmentStrokeColor: "#56f067",
        // фокусировка карты на маршруте (его целиком видно дб)
            boundsAutoApply: true
        });

        if (mode == 2)
        {
            multiRoute.model.setParams({routingMode: 'auto'})
        }
        // if (mode == 3)
        // {
        //     multiRoute.model.setParams({routingMode: 'masstransit'})
        // }
        myMap.geoObjects.add(multiRoute);


        for (i = 0; i < points.length; i++)
        {
            myPlacemark = new ymaps.Placemark(points[i], 
                          {hintContent: points_names[i], iconContent: i+1});
            myMap.geoObjects.add(myPlacemark);
        }  

    } 

});