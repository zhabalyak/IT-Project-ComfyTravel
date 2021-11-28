ymaps.ready(function () 
{
	var myMap = new ymaps.Map('map', {
            center: [55.806059, 49.177079],
            zoom: 11,
	       controls: ['zoomControl']
        }, {
            buttonMaxWidth: 300
        });

    //для тестового запуска
    // get_points_test();
    // function get_points_test () 
    // {
    //     var points = [[55.806638, 49.143446],
    //                 [55.836450, 49.248555],
    //                 [55.865467, 49.260131],
    //                 [55.824050, 49.167794],
    //                 [55.796690, 49.216312]];
    //     show_route_on_map(points, 2);
    // }  

    function take_points (points, mode) 
    {
        //mode = 1, тогда пешеходный/общ транспорт, 
        //     = 2, тогда на машине
        show_route_on_map(points);
    }  

    function show_route_on_map (points, mode) 
    {
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