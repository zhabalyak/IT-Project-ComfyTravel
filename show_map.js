$(".choice").on("submit", function(){
    $.ajax({
        url: '/index.js', /*Куда пойдет запрос, обычно ссылка на файл с маршрутизацией/сам файл с сервером, если все маршруты в нём же*/
        method: 'get', /* метод: тк мы не собираемся менять данные на сервере, а лишь берём их, используем get */
        dataType: 'html', /*Тип данных в ответе - в данном случае мы должны вернуть ту же страницу main_map.html */
        data: $(this).serialize(), /* то, что передаётся в запросе. нам нужна форма целиком, serialize надо будет подключить */
        /* Метод .serialize() (из query) возвращает строку пригодную для передачи через URL строку.
        (это вот когда пишется после каких-то действий типа
        ?*название элемента*=*значение*?тд)
        Для успешной сериализации элемент формы должен содержать атрибут name. - это есть
        Значения чекбоксов, радио кнопок будет включено в строку, если они были выделены.*/
        success: ymaps.ready(function(answer_html)
            {
                var myMap = new ymaps.Map('map', {
                        center: [55.806059, 49.177079],
                        zoom: 11,
                        controls: ['zoomControl']
                    }),

                    button_show = answer_html.document.getElementById("btn_show_map");


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
            }) /* success - это что мы выполняем в случае удачного ответа от сервера
            В переменной answer_html содержится ответ от index.js */
        });
});