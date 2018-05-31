//ajax拦截统一处理
$(function () {
    $.ajaxSetup({
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', localStorage.dangguitoken);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //通过XMLHttpRequest取得响应头:token
            var dangguitoken = XMLHttpRequest.getResponseHeader("Authorization");
            if (dangguitoken != undefined) {
                localStorage.dangguitoken = dangguitoken;
            }

            //通过XMLHttpRequest获取响应结果
            var res = XMLHttpRequest.responseText;
            try {
                var jsonData = JSON.parse(res);
                if (jsonData.state == -1) {
                    //如果超时就处理 ，指定要跳转的页面(比如登陆页)
                    alert(jsonData.msg);
                    console.log('json response Error')
                    window.location.replace("/login");
                } else if (jsonData.state == 0) {
                    //其他的异常情况,给个提示。
                    alert(jsonData.msg);
                } else {
                    //正常情况就不统一处理了
                }
            } catch (e) {
            }
        },
        statusCode: {
            401: function () {
                //跳转登录
                var redirectURL = encodeURIComponent(location.href);
                window.location.replace("http://localhost:6001/login?redirect=" + redirectURL);
                console.log('401');
            },
            403: function () {
                //跳转登录
                var redirectURL = encodeURIComponent(location.href);
                window.location.replace("http://localhost:6001/login?redirect=" + redirectURL);
                console.log('403');
            },
            404: function () {
                alert('数据获取/输入失败，没有此服务。404');
            },
            504: function () {
                alert('数据获取/输入失败，服务器没有响应。504');
            },
            500: function () {
                alert('服务器有误。500');
            }
        }
    });
});