
function ga_read_cookie(c) {
	for (var c = c + "=", d = document.cookie.split(";"), b = 0; d.length > b; b++) {
		for (var a = d[b];
		" " == a.charAt(0); ) a = a.substring(1, a.length);
		if (0 == a.indexOf(c)) return decodeURIComponent(a.substring(c.length, a.length))
	}
	return null
}

function ga_cookie_exist(name) {
	return ga_read_cookie(name) != null;
}

function ga_load_js(src) {
	var sc = document.createElement('script');
	sc.type = 'text/javascript';
	sc.async = true;
	sc.src = src; 
	var s = document.getElementsByTagName('script')[0];
	s.parentNode.insertBefore(sc, s);
}

function ga_check_channel() {
    if (location.hostname.indexOf(".hujiang.com") != -1 || location.hostname.indexOf(".hjclass.com") !=-1 ) {

		if (ga_cookie_exist("channel_source") ||
			location.search.indexOf("sem_source=") != -1 || 
			location.search.indexOf("pp_source=") != -1 ) {

			var common_host = "common.hjfile.cn";
			if (location.hostname.indexOf("dev.") != -1) {
				common_host = "dev.common.hjfile.cn";
			}
			ga_load_js("http://" + common_host + "/analytics/channel/channel.js");
		}
	}
}


ga_check_channel();

var ga = ga || function() {(ga.q = ga.q || []).push(arguments)};

ga("create", "UA-28846272-5", "mc.hujiang.com");

function ga_class_buy_succ() {
    ga("send", "pageview", location.pathname + "_succ" + location.search);

    ga("require", "ecommerce", "ecommerce.js");

    ga("ecommerce:addTransaction", {
      "id": ga_class_id + "_" + ga_userid,  // Transaction ID. Required.
      "affiliation": "",                	// Affiliation or store name.
      "revenue": ga_user_pay,           	// Grand Total.
      "shipping": "0",                  	// Shipping.
      "tax": "0"                        	// Tax.
    });
    
    ga("ecommerce:addItem", {
      "id": ga_class_id + "_" + ga_userid,          // Transaction ID. Required.
      "name": ga_userid + "_" + ga_username,       	// Product name. Required.
      "sku": ga_class_id + "_" + ga_class_name,  	// SKU/code.
      "category": ga_class_lang,        			// Category or variation.
      "price": ga_user_pay,                         // Unit price.
      "quantity": "1"                               // Quantity.
    });
    
    ga("ecommerce:send");
}

function ga_class_charge_succ() {
	_gaq.push(['_trackPageview', '/mc_charge_succ']);
}

var ga_start_time = -1;
var ga_has_viewdemo = false;

function ga_demo_play() {
	ga_has_viewdemo = true;
	if (ga_start_time == -1) {
		ga_start_time = new Date();
	}

	ga('send', 'event', "demo_play", ga_class_id, ga_class_name);
}

function ga_exchange_click(button_location, class_id) {
	class_id = class_id.toString();

	if (ga_has_viewdemo) {
		var time_use = new Date() - ga_start_time;
		ga('send', 'timing', 'debug_demo_time_use', class_id, time_use);
	}

	ga('send', 'event', 'on_exchange', class_id, ga_has_viewdemo.toString());
	ga('send', 'event', 'exchange_click', button_location, class_id);
}

(function() {

	var ga_class_lang = window.ga_class_lang || "";
	var ga_class_id = window.ga_class_id || "";
	var ga_class_name = window.ga_class_name || "";
	var ga_class_price = window.ga_class_price || "";
	var ga_user_pay = window.ga_user_pay || "";

	var ga_userid = window.ga_userid || "";
	var ga_username = window.ga_username || "";
	var ga_user = ga_username + "(" + ga_userid + ")";

	ga("set", "dimension5", ga_user);

	function mark_intro_param() {
		ga("set", "dimension1", ga_class_lang);
		ga("set", "dimension3", ga_class_id);
		ga("set", "dimension4", ga_class_name);
		ga("set", "metric1", ga_class_price);
	}

	function mark_exchange_param() {
		mark_intro_param();
		ga("set", "metric2", ga_user_pay);
	}

	var url = location.pathname + location.search;

	if (/\/\d+\/intro/.test(location.pathname)) {
		mark_intro_param();
	}

	if (/\/\d+\/exchange/.test(location.pathname)) {
		mark_exchange_param();
	}

	if (url.indexOf("/course/search?k=") != -1) {
		if (ga_site_search_count == "0") {
			ga("send", "pageview", url + "&cate=not_find");
		} else {
			ga("send", "pageview", url + "&cate=find");
		}
	} else {
		ga("send", "pageview");
	}

	var dic_cate = {};
	$("#type-nav .type-ul li span").each(function() {
	    var name = $(this).text();
	    var key = $(this).parent().attr("data-nav");
	    dic_cate[key] = name;
	});

	// var console = console || { log: function () { } };
	var canTouch = "ontouchstart" in window;
	var click = canTouch ? "tap" : "click";

	$(".sub-nav li").on(click, function(){
	    var name = $(this).find("a").text();
	    var lang = $(this).parent().attr("class");
	    lang = lang.replace("sub-nav ", "");

	    ga("send", "pageview", "/cate/" + dic_cate[lang] + "/" + name);

	    //console.log(dic_cate[lang] + "," + name)
	});
})();

function ga_get_hjid() {

    function _utf8_decode(utftext) {  
        var string = "";  
        var i = 0;  
        var c = c1 = c2 = 0;  
        while ( i < utftext.length ) {  
            c = utftext.charCodeAt(i);  
            if (c < 128) {  
                string += String.fromCharCode(c);  
                i++;  
            } else if((c > 191) && (c < 224)) {  
                c2 = utftext.charCodeAt(i+1);  
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));  
                i += 2;  
            } else {  
                c2 = utftext.charCodeAt(i+1);  
                c3 = utftext.charCodeAt(i+2);  
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));  
                i += 3;  
            }  
        }  
        return string;  
    }  

    function decode_base64(input) {
        var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        var output = "";  
        var chr1, chr2, chr3;  
        var enc1, enc2, enc3, enc4;  
        var i = 0;  
        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");  
        while (i < input.length) {  
            enc1 = _keyStr.indexOf(input.charAt(i++));  
            enc2 = _keyStr.indexOf(input.charAt(i++));  
            enc3 = _keyStr.indexOf(input.charAt(i++));  
            enc4 = _keyStr.indexOf(input.charAt(i++));  
            chr1 = (enc1 << 2) | (enc2 >> 4);  
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);  
            chr3 = ((enc3 & 3) << 6) | enc4;  
            output = output + String.fromCharCode(chr1);  
            if (enc3 != 64) {  
                output = output + String.fromCharCode(chr2);  
            }  
            if (enc4 != 64) {  
                output = output + String.fromCharCode(chr3);  
            }  
        }  
        output = _utf8_decode(output);  
        return output;   
    }

    var cookie_value = ga_read_cookie("hj_token");
    if (cookie_value == null || cookie_value.indexOf("|") == -1) {
        return "0";
    }
    try {
        return parseInt(decode_base64(cookie_value.split("|")[1]), 16).toString();
    } catch(e) {
        return "0";
    }
}

(function(i,s,o,g,r,a,m){i["GoogleAnalyticsObject"]=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,"script","//common.hjfile.cn/analytics/google/analytics.js","ga");


//#赵昕 2014年3月10日19:13:57

function piwik_track_site(name, idsite) {
	window._paq = window._paq || [];
	_paq.push(["setSiteId", idsite]);
	_paq.push(['setTrackerUrl', 'http://track.yeshj.com/piwik/piwik.php']);

	_paq.push(['setCustomVariable', 1, 'ga_user_info', ga_get_hjid(), 'visit']); 

	var m_a6 = location.hash.match("#a6=(\\d+,\\d+,\\d+,\\d+)");
	if (m_a6 != null) {
		_paq.push(['setCustomVariable', 1, 'a6', m_a6[1], 'page']); 
	}

	_paq.push(["trackPageView"]);
	_paq.push(["enableLinkTracking"]);
}

function piwik_track_hj() {
	var m = /\.(hujiang|hjenglish)\.com$/.exec(location.hostname);
	if (m == null) {
		return;
	}
	var hostname = m[1];

    ga_load_js("http://common.hjfile.cn/analytics/piwik/piwik.js");

    if (hostname == "hujiang") {
        setTimeout(function() {
            if (window.Piwik) {
                var piwikTracker = Piwik.getTracker();
	            piwikTracker.setSiteId("8");
	            piwikTracker.setCookieDomain("*.hujiang.com");
                piwikTracker.setTrackerUrl( "http://track.yeshj.com/piwik/piwik.php");
                piwikTracker.setCustomVariable(1, "ga_user_info", ga_get_hjid(), "visit");

                var m_a6 = location.hash.match("#a6=(\\d+,\\d+,\\d+,\\d+)");
                if (m_a6 != null) {
                    piwikTracker.setCustomVariable(1, "a6", m_a6[1], "page");
                }

                piwikTracker.trackPageView();
            }            
        }, 3000);
    }
} 

piwik_track_hj();
piwik_track_site("class_mc", "45");
//------------------
//#ga_js_load_finish#

//Visitor log
function generateMixed(n) {
	var chars = ['0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'];
     var res = "";
     for(var i = 0; i < n ; i ++) {
         var id = Math.ceil(Math.random()*35);
         res += chars[id];
     }
     return res;
}
//ga_load_js("http://track.hujiang.com/log?siteid=45&urlref="+encodeURIComponent(document.referrer)+"&_="+generateMixed(16));
//ga_load_js("http://common.hjfile.cn/analytics/tracking/class_mc_rtn.js");