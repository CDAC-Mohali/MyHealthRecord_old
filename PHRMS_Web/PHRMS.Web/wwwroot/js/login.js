// Login Form

$(function() {
    var button = $('#loginButton');
    var registerbutton = $('#registerButton');

    var box = $('#loginBox');
    var registerbox = $('#registerBox');

    var form = $('#loginForm');
    var regform = $('#registerform');

    button.removeAttr('href');
    registerbutton.removeAttr('href');

    button.mouseup(function (login) {
        box.toggle();
        button.toggleClass('active');
        registerbutton.removeClass('active');
        registerbox.hide();
    });

    registerbutton.mouseup(function (register) {
        registerbox.toggle();
        registerbutton.toggleClass('active');
        button.removeClass('active');
        box.hide();
    });

    form.mouseup(function() { 
        return false;
    });

    regform.mouseup(function () {
        return false;
    });

    $(this).mouseup(function(login) {
        if(!($(login.target).parent('#loginButton').length > 0)) {
            button.removeClass('active');
            box.hide();
        }
    });

    $(this).mouseup(function (register) {
        if (!($(register.target).parent('#registerButton').length > 0)) {
            registerbutton.removeClass('active');
            registerbox.hide();
        }
    });
});
