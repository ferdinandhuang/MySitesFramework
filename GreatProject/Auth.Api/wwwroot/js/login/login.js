//��ס�˺�
$(function () {
    var username = localStorage.username;
    if (username != undefined && username != '') {
        $('#username').val(username);
        $('#rememberme').attr('checked', 'ture');
    }
});

//��ȡ���������
function getQueryString(name) {

    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

// ��ת�����ĵ�¼
$(function () {
    var redirectURL = getQueryString("redirect");
    localStorage.redirectURL = redirectURL;
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
            $.post('/Login/Login', data, function (resultdata, textStatus, XMLHttpRequest) {
                //Redirect Back
                var redirectURL = localStorage.redirectURL;

                $.StandardPost(redirectURL, { access: ' + localStorage.dangguitoken + ', refresh: ' + localStorage.dangguire_token + ' });
                
                return;
                if (redirectURL && redirectURL != undefined && redirectURL != '' && redirectURL != 'null') {
                    localStorage.redirectURL = '';
                    window.location.replace(redirectURL + '?access=' + localStorage.dangguitoken + '&refresh=' + localStorage.dangguire_token);
                };
            }, 'json');
        }
    });

    //
    $('#forget').click(function () {
        $.post('/Login/wawaAsync', function (data) {

        }, 'json');
    });
});

$.extend({
    StandardPost: function (url, args) {
        var form = $("<form method='post'></form>"),
            input;
        form.attr({ "action": url });
        $.each(args, function (key, value) {
            input = $("<input type='hidden'>");
            input.attr({ "name": key });
            input.val(value);
            form.append(input);
        });
        form.submit();
    }
});