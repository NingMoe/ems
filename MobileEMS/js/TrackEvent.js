function load_js(t){var e=document.createElement("script");e.type="text/javascript";e.async=true;e.src=t;var m=document.getElementsByTagName("script")[0];m.parentNode.insertBefore(e,m)}function SendEvent(t,e){commitUrl="http://track.hujiang.com/event?";commitUrl+="cid="+t;commitUrl+="&eid="+e;commitUrl+="&siteid=0";commitUrl+="&p=";commitUrl+="&hj_urlref="+encodeURIComponent(document.referrer);commitUrl+="&hj_t="+hj_t;load_js(commitUrl)}function SendEvent(t,e,m,r){commitUrl="http://track.hujiang.com/event?";commitUrl+="cid="+t;commitUrl+="&eid="+e;if(m==undefined)commitUrl+="&p=";else commitUrl+="&p="+m;commitUrl+="&siteid=0";if(r==undefined)commitUrl+="&hjid=";else commitUrl+="&hjid="+r;commitUrl+="&hj_urlref="+encodeURIComponent(document.referrer);commitUrl+="&hj_t="+hj_t;load_js(commitUrl)}