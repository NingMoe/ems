
/**
* 创建全局对象Utils
* @module Utils
* @title Utils Global
*/
var Utils = {    
    /*数字相加*/
    add: function(a,b) {
        var c, d, e;
        try {
            c = a.toString().split(".")[1].length;
        } catch (f) {
            c = 0;
        }
        try {
            d = b.toString().split(".")[1].length;
        } catch (f) {
            d = 0;
        }
        return e = Math.pow(10, Math.max(c, d)), (this.mul(a, e) + this.mul(b, e)) / e;
    },
    /*数字相减*/
    sub: function(a,b) {
        var c, d, e;
        try {
            c = a.toString().split(".")[1].length;
        } catch (f) {
            c = 0;
        }
        try {
            d = b.toString().split(".")[1].length;
        } catch (f) {
            d = 0;
        }
        return e = Math.pow(10, Math.max(c, d)), (this.mul(a, e) - this.mul(b, e)) / e;
    },
    /*数字相除*/
    div: function(a,b) {
        var c,
        d,
        e = 0,
        f = 0;
        try {
            e = a.toString().split(".")[1].length;
        } catch (g) {
        }
        try {
            f = b.toString().split(".")[1].length;
        } catch (g) {
        }
        return c = Number(a.toString().replace(".", "")), d = Number(b.toString().replace(".", "")), this.mul(c / d, Math.pow(10, f - e));
    },
    /*相加
*arr 数组
*/
    sum: function(arr) {
        var res = "0";
        for (var i = 0; i < arr.length; i++) {            
            res = this.add(res, arr[i]);
        }
        return res;
    },
    /*数字相乘*/
    mul:function(a, b) {
        var c = 0,
            d = a.toString(),
            e = b.toString();
        try {
            c += d.split(".")[1].length;
        } catch (f) { }
        try {
            c += e.split(".")[1].length;
        } catch (f) { }
        return Number(d.replace(".", "")) * Number(e.replace(".", "")) / Math.pow(10, c);
    },
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },/*获取url参数*/
    getUrlVar: function (name) {
        return this.getUrlVars()[name];
    }
};

/*增加对命名空间的支持*/
window.usingNamespace = function (a) {
    var ro = window;
    if (!(typeof (a) === "string" && a.length != 0)) { return ro; }
    var co = ro;
    var nsp = a.split(".");
    for (var i = 0; i < nsp.length; i++) {
        var cp = nsp[i];
        if (!ro[cp]) { ro[cp] = {}; };
        co = ro = ro[cp];
    };
    return co;
};


var hui = {};
hui.g = function (id, parentNode) {
    if (!parentNode || parentNode == hui.bocument || parentNode == hui.bocument.body) {
        return hui.dom ? hui.dom.getElementById(id) : document.getElementById(id);
    }
    else {
        var i, len, k, v,
            childNode,
            elements,
            list,
            childlist,
            node;
        elements = [], list = [parentNode];

        while (list.length) {
            childNode = list.pop();
            if (!childNode) continue;
            if (childNode.id == id) {
                break;
            }
            elements.push(childNode);
            childlist = childNode.childNodes;
            if (!childlist || childlist.length < 1) continue;
            for (i = 0, len = childlist.length; i < len; i++) {
                node = childlist[i];
                list.push(node);
            }
        }
        return (childNode.id == id ? childNode : null);
    }
};

hui.c = function (searchClass, node, tag) {
    if (document.getElementsByClassName) {
        var nodes = (node || document).getElementsByClassName(searchClass), result = nodes;
        if (tag != undefined) {
            result = [];
            for (var i = 0, len = nodes.length; i < len; i++) {
                if (tag === '*' || nodes[i].tagName.toUpperCase() === tag.toUpperCase()) {
                    result.push(nodes[i]);
                }
            }
        }
        return result;
    }
    else {
        searchClass = searchClass != null ? String(searchClass).replace(/\s+/g, ' ') : '';
        node = node || document;
        tag = tag || '*';

        var classes = searchClass.split(' '),
            elements = (tag === '*' && node.all) ? node.all : node.getElementsByTagName(tag),
            patterns = [],
            returnElements = [],
            current,
            match;

        var i = classes.length;
        while (--i >= 0) {
            patterns.push(new RegExp('(^|\\s)' + classes[i] + '(\\s|$)'));
        }
        var j = elements.length;
        while (--j >= 0) {
            current = elements[j];
            match = false;
            for (var k = 0, kl = patterns.length; k < kl; k++) {
                match = patterns[k].test(current.className);
                if (!match) { break; }
            }
            if (match) { returnElements.push(current); }
        }
        return returnElements;
    }
};

hui.addClass = function (element, className) {
    if (~'[object Array][object NodeList]'.indexOf(Object.prototype.toString.call(element))) {
        for (var i = 0, len = element.length; i < len; i++) {
            hui.addClass(element[i], className);
        }
    }
    else if (element) {
        hui.removeClass(element, className);
        element.className = (element.className + ' ' + className).replace(/(\s)+/ig, ' ');
    }
    return element;
};
// Support * and ?, like hui.removeClass(elem, 'daneden-*');

hui.removeClass = function (element, className) {
    if (~'[object Array][object NodeList]'.indexOf(Object.prototype.toString.call(element))) {
        for (var i = 0, len = element.length; i < len; i++) {
            hui.removeClass(element[i], className);
        }
    }
    else if (element) {
        var list = className.replace(/\s+/ig, ' ').split(' '),
            /* Attention: str need two spaces!! */
            str = (' ' + (element.className || '').replace(/(\s)/ig, '  ') + ' '),
            name,
            rex;
        // 用list[i]移除str
        for (var i = 0, len = list.length; i < len; i++) {
            name = list[i];
            name = name.replace(/(\*)/g, '\\S*').replace(/(\?)/g, '\\S?');
            rex = new RegExp(' ' + name + ' ', 'ig');
            str = str.replace(rex, ' ');
        }
        str = str.replace(/(\s)+/ig, ' ');
        str = str.replace(/^(\s)+/ig, '').replace(/(\s)+$/ig, '');
        element.className = str;
    }
    return element;
};
(function ($) {
    $.extend({ HuJiang: function () { } });
    $.fn.extend({});
    $.extend($.HuJiang, {
        EncodeUrl: function (str) { return escape(str).replace(/\*/g, "%2A").replace(/\+/g, "%2B").replace(/-/g, "%2D").replace(/\./g, "%2E").replace(/\//g, "%2F").replace(/@/g, "%40").replace(/_/g, "%5F"); },
        DecodeUrl: function (str) { return unescape(str); },
        EncodeHtml: function (str) { return str.replace(/</g, "%3C").replace(/>/g, "%3E"); },
        EncodeOriHtml: function (str) { return str.replace(/</g, "&lt;").replace(/>/g, "&gt;"); },
        RemoveSpCharacter: function (str) {
            if (!str) return "";
            return str.replace(/\//g, "").replace(/%/g, "").replace(/\?/g, "").replace(/</g, "").replace(/>/g, "").replace(/\*/g, "").replace(/\\/g, "").replace(/"/g, "").replace(/#/g, "").replace(/\|/g, "").replace(/&/g, "").replace(/\./g, "").replace(/:/g, "").replace(/\+/g, "");
        },
        TrimLeft: function (str) { return str == null ? "" : str.toString().replace(/^\s+/, ""); },
        TrimRight: function (str) { return str == null ? "" : str.toString().replace(/\s+$/, ""); },
        Trim: function (v) { return v.replace(/^\s+|\s+$/g, "") },
        PadLeft: function (str, c, count) { while (str.length < count) { str = c + str; } return str; },
        PadRight: function (str, c, count) { while (str.length < count) { str += c; } return str; },
        IsNullOrEmpty: function (str) { return !(typeof (str) === "string" && str.replace(/^\s+|\s+$/g, "").length != 0); },
        StringFormat: function (source, params) {
            if (arguments.length == 1)
                return function () {
                    var args = $.makeArray(arguments);
                    args.unshift(source);
                    return $.InfoSky.StringFormat.apply(this, args);
                };
            if (arguments.length > 2 && params.constructor != Array) { params = $.makeArray(arguments).slice(1); }
            if (params.constructor != Array) { params = [params]; }
            $.each(params, function (i, n) { source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n); });
            return source;
        },
        IsNumeric: function (obj) { return !isNaN(parseFloat(obj)) && isFinite(obj); },
        FormatNumber: function (s, n) {
            var str = s.toString();
            var num = parseInt(str);
            var text = num.toString();
            var i = 0;
            text += '.';

            if (num < s) {
                var l = str.split(".")[1];
                for (i = 0; i < l.length && i < n; i++) {
                    text += l[i];
                }
            }
            for (; i < n; i++) { text += '0'; }

            return text;
        },
        KeyCode: { Tab: 9, Enter: 13, A: 65, Z: 90, Shift_A: 97, Shift_Z: 122 },
        Cookie: {
            get: function (name) {
                var cookieName = encodeURIComponent(name) + "=",
                cookieStart = document.cookie.indexOf(cookieName),
                cookieValue = null;
                if (cookieStart > -1) {
                    var cookieEnd = document.cookie.indexOf(";", cookieStart);
                    if (cookieEnd == -1) {
                        cookieEnd = document.cookie.length;
                    }
                    cookieValue = decodeURIComponent(document.cookie.substring(cookieStart + cookieName.length, cookieEnd));
                }
                return cookieValue;
            },
            set: function (name, value, expires, path, domain, secure) {
                var cookieText = encodeURIComponent(name) + "=" + encodeURIComponent(value);
                if (expires instanceof Date)
                    cookieText += "; expires=" + expires.toGMTString();
                if (path)
                    cookieText += "; path=" + path;
                if (domain)
                    cookieText += "; domain=" + domain;
                if (secure)
                    cookieText += "; secure";
                document.cookie = cookieText;
            },
            unset: function () { }
        },
        BuildResource: function (key) {
            if (typeof clientmessage != "undefined" && clientmessage && clientmessage[key])
                return decodeURIComponent(clientmessage[key].replace(/\+/g, " "));
            return "";
        },
        /*数字相减*/
        sub: function (a, b) {
            var c, d, e;
            try {
                c = a.toString().split(".")[1].length;
            } catch (f) {
                c = 0;
            }
            try {
                d = b.toString().split(".")[1].length;
            } catch (f) {
                d = 0;
            }
            return e = Math.pow(10, Math.max(c, d)), (this.mul(a, e) - this.mul(b, e)) / e;
        },
        /*数字相乘*/
        mul: function (a, b) {
            var c = 0,
                d = a.toString(),
                e = b.toString();
            try {
                c += d.split(".")[1].length;
            } catch (f) { }
            try {
                c += e.split(".")[1].length;
            } catch (f) { }
            return Number(d.replace(".", "")) * Number(e.replace(".", "")) / Math.pow(10, c);
        },
        /*数字相加*/
        add: function (a, b) {
            var c, d, e;
            try {
                c = a.toString().split(".")[1].length;
            } catch (f) {
                c = 0;
            }
            try {
                d = b.toString().split(".")[1].length;
            } catch (f) {
                d = 0;
            }
            return e = Math.pow(10, Math.max(c, d)), (this.mul(a, e) + this.mul(b, e)) / e;
        },
        /*参数说明 re 为正则表达式 s 为要判断的字符*/
        testRgexp: function (re, s) {
            return re.test(s);
        },
        /*验证一个字符串是否是非负浮点数*/
        noneDecimalRgexp: function (text) {
            var re = /^\d+(\.\d+)?$/;
            return this.testRgexp(re, text);
        },
        /*判断是否超过小数点2位*/
        dotFormat: function (value) {
            var dot = value.indexOf(".");
            if (dot != -1) {
                var dotCnt = value.substring(dot + 1, value.length);
                if (dotCnt.length > 2) {
                    return false;
                }
            }
            return true;
        },
        getUrlParm: function () {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]);
            return null; //返回参数值
        },
        getUrlVars: function () {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        },
        getUrlVar: function (name) {
            return this.getUrlVars()[name];
        }
    });
})(jQuery);