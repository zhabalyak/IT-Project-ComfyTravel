$.ajax({
    url: '../Controllers/MainController.cs', /*Куда пойдет запрос, обычно ссылка на файл с маршрутизацией/сам файл с сервером, если все маршруты в нём же*/
    method: 'get', /* метод: нам нужно только показать форму, ничего не меняем в данных, поэтому get*/
    dataType: 'cshtml', /*Тип данных в ответе - в данном случае мы должны вернуть ту же страницу main_map.html */
    data: {button: 'btn1'}, /* то, что передаётся в запросе. по логике нам надо следить за кнопкой, надеюсь, это работает */
    success: function(answer_html){
        let content = answer_html.document.querySelector('.login_form');
        let btn_show = answer_html.document.querySelector('.btn1');

        btn_show.addEventListener("click", function show() {
            content.style.visibility = 'visible';
        });
    } /* success - это что мы выполняем в случае удачного ответа от сервера
    В переменной answer_html содержится ответ от index.js */
});