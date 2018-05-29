$(function () {
    var username = localStorage.username;
    if (username != undefined && username != '') {
        $('#username').val(username);
        $('#rememberme').attr('checked', 'ture');
    }
});

//������֤
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
    //��½
    $('#submit').click(function () {
        if ($('#sign_in').valid()) {
            var username = $('#username').val();
            var pdw = $('#password').val();
            var password = sha256.hmac(username, pdw);

            //��ס��
            var rememberme = $('#rememberme').is(':checked');
            if (rememberme) {
                localStorage.username = username;
            }
            else {
                localStorage.username = "";
            }

            var data = { Username: username, Password: password };

            //�ύ
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