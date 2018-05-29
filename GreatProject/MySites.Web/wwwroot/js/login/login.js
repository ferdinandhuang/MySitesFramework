$(function () {
    var username = localStorage.username;
    if (username != undefined && username != '') {
        $('#username').val(username);
        $('#rememberme').attr('checked', 'ture');
    }
});

//输入验证
$(function () {
    $('#sign_in').validate({
        highlight: function (input) {
            console.log(input);
            $(input).parents('.form-line').addClass('error');
        },
        unhighlight: function (input) {
            $(input).parents('.form-line').removeClass('error');
        },
        errorPlacement: function (error, element) {
            $(element).parents('.input-group').append(error);
        }
    });
});

$(function () {
    //登陆
    $('#submit').click(function () {
        if ($('#sign_in').valid()) {
            var username = $('#username').val();
            var pdw = $('#password').val();
            var password = sha256.hmac(username, pdw);

            //记住我
            var rememberme = $('#rememberme').is(':checked');
            if (rememberme) {
                localStorage.username = username;
            }
            else {
                localStorage.username = "";
            }

            var data = { Username: username, Password: password };

            //提交
            $.post('/Login/Login', data, function (resultdata) {
                console.log(resultdata, 123);
            }, JSON);
        }
    });

    //
    $('#forget').click(function () {
        $.post('/Login/wawaAsync', function (data) {

        }, JSON);
    });
});