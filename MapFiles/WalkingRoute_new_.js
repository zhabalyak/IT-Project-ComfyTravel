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
    var route_mode = document.getElementById('route_mode').textContent.split(", ");
    //mode = 1, тогда пешеходный/общ транспорт, 
    //     = 2, тогда на машине

    //console.log(points_pack_x);
    //console.log(points_pack_y);

    let points_pack = [points_pack_x, points_pack_y];

    //console.log(points_pack);

    show_route_on_map(points_pack, route_mode);

    function show_route_on_map (points_pack, mode) 
    {
        //points parsing
        points = new Array(points_pack[0].length);
        for (i = 0; i < points_pack[0].length; i++) {
            points[i] = [parseFloat(points_pack[0][i].replace(",", ".")), parseFloat(points_pack[1][i].replace(",", "."))];
        }


        var multiRoute = new ymaps.multiRouter.MultiRoute(
        {
            referencePoints: points,
            params: {
                //nип маршрутизации - пешеходный ('auto' - для машины)
                routingMode: 'pedestrian' 
            }
        }, {
        // чтобы сделать точки А, Б... невидимыми
            //wayPointVisible:false,
        // внешний вид линии пешеходного маршрута
            routeActivePedestrianSegmentStrokeStyle: "solid",
            routeActivePedestrianSegmentStrokeColor: "#56f067",
        // для авто-маршрута
            routeActiveStrokeColor: "#E63E92",
        // фокусировка карты на маршруте (его целиком видно дб)
            boundsAutoApply: true
        });

        if (mode == 2)
        {
            multiRoute.model.setParams({routingMode: 'auto'})
        }

        myMap.geoObjects.add(multiRoute);
    } 

});