window.onload = function () {

    let content = document.querySelector('.login_form');
    let btn_show = document.querySelector('.btn1');
    let btn_hide = document.querySelector('.hide');

    btn_show.addEventListener("click", function show() {
        content.style.display = 'block';
    });

    btn_hide.addEventListener("click", function hide() {
        content.style.display = 'none';
    });

}