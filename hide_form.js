$.ajax({
    url: '/index.js', /*Куда пойдет запрос, обычно ссылка на файл с маршрутизацией/сам файл с сервером, если все маршруты в нём же*/
    method: 'get', /* метод: нам нужно только показать форму, ничего не меняем в данных, поэтому get */
    dataType: 'html', /*Тип данных в ответе - в данном случае мы должны вернуть ту же страницу main_map.html */
    data: {button: 'hide'}, /* то, что передаётся в запросе. по логике нам надо следить за кнопкой, надеюсь, это работает */
    success: function(answer_html){
        let content = answer_html.document.querySelector('.login_form');
        let btn_hide = answer_html.document.querySelector('.hide');

        btn_hide.addEventListener("click", function hide() {
            content.style.visibility = 'hidden';
        });
    } /* success - это что мы выполняем в случае удачного ответа от сервера
            В переменной answer_html содержится ответ от index.js */
});