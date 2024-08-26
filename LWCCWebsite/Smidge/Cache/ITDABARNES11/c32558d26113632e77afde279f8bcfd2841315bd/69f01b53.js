/*
 AngularJS v1.8.3
 (c) 2010-2020 Google LLC. http://angularjs.org
 License: MIT
*/
(function(s,e){'use strict';function O(e){var g=[];B(g,D).chars(e);return g.join("")}var C=e.$$minErr("$sanitize"),E,g,F,G,H,q,D,I,J,B;e.module("ngSanitize",[]).provider("$sanitize",function(){function h(a,d){return A(a.split(","),d)}function A(a,d){var c={},b;for(b=0;b<a.length;b++)c[d?q(a[b]):a[b]]=!0;return c}function t(a,d){d&&d.length&&g(a,A(d))}function P(a){for(var d={},c=0,b=a.length;c<b;c++){var k=a[c];d[k.name]=k.value}return d}function K(a){return a.replace(/&/g,"&amp;").replace(Q,function(a){var c=
a.charCodeAt(0);a=a.charCodeAt(1);return"&#"+(1024*(c-55296)+(a-56320)+65536)+";"}).replace(u,function(a){return"&#"+a.charCodeAt(0)+";"}).replace(/</g,"&lt;").replace(/>/g,"&gt;")}function z(a){for(;a;){if(a.nodeType===s.Node.ELEMENT_NODE)for(var d=a.attributes,c=0,b=d.length;c<b;c++){var k=d[c],f=k.name.toLowerCase();if("xmlns:ns1"===f||0===f.lastIndexOf("ns1:",0))a.removeAttributeNode(k),c--,b--}(d=a.firstChild)&&z(d);a=v("nextSibling",a)}}function v(a,d){var c=d[a];if(c&&I.call(d,c))throw C("elclob",
d.outerHTML||d.outerText);return c}var y=!1,f=!1;this.$get=["$$sanitizeUri",function(a){y=!0;f&&g(m,l);return function(d){var c=[];J(d,B(c,function(b,c){return!/^unsafe:/.test(a(b,c))}));return c.join("")}}];this.enableSvg=function(a){return H(a)?(f=a,this):f};this.addValidElements=function(a){y||(G(a)&&(a={htmlElements:a}),t(l,a.svgElements),t(r,a.htmlVoidElements),t(m,a.htmlVoidElements),t(m,a.htmlElements));return this};this.addValidAttrs=function(a){y||g(L,A(a,!0));return this};E=e.bind;g=e.extend;
F=e.forEach;G=e.isArray;H=e.isDefined;q=e.$$lowercase;D=e.noop;J=function(a,d){null===a||void 0===a?a="":"string"!==typeof a&&(a=""+a);var c=M(a);if(!c)return"";var b=5;do{if(0===b)throw C("uinput");b--;a=c.innerHTML;c=M(a)}while(a!==c.innerHTML);for(b=c.firstChild;b;){switch(b.nodeType){case 1:d.start(b.nodeName.toLowerCase(),P(b.attributes));break;case 3:d.chars(b.textContent)}var k;if(!(k=b.firstChild)&&(1===b.nodeType&&d.end(b.nodeName.toLowerCase()),k=v("nextSibling",b),!k))for(;null==k;){b=
v("parentNode",b);if(b===c)break;k=v("nextSibling",b);1===b.nodeType&&d.end(b.nodeName.toLowerCase())}b=k}for(;b=c.firstChild;)c.removeChild(b)};B=function(a,d){var c=!1,b=E(a,a.push);return{start:function(a,f){a=q(a);!c&&w[a]&&(c=a);c||!0!==m[a]||(b("<"),b(a),F(f,function(c,f){var e=q(f),h="img"===a&&"src"===e||"background"===e;!0!==L[e]||!0===N[e]&&!d(c,h)||(b(" "),b(f),b('="'),b(K(c)),b('"'))}),b(">"))},end:function(a){a=q(a);c||!0!==m[a]||!0===r[a]||(b("</"),b(a),b(">"));a==c&&(c=!1)},chars:function(a){c||
b(K(a))}}};I=s.Node.prototype.contains||function(a){return!!(this.compareDocumentPosition(a)&16)};var Q=/[\uD800-\uDBFF][\uDC00-\uDFFF]/g,u=/([^#-~ |!])/g,r=h("area,br,col,hr,img,wbr"),x=h("colgroup,dd,dt,li,p,tbody,td,tfoot,th,thead,tr"),p=h("rp,rt"),n=g({},p,x),x=g({},x,h("address,article,aside,blockquote,caption,center,del,dir,div,dl,figure,figcaption,footer,h1,h2,h3,h4,h5,h6,header,hgroup,hr,ins,map,menu,nav,ol,pre,section,table,ul")),p=g({},p,h("a,abbr,acronym,b,bdi,bdo,big,br,cite,code,del,dfn,em,font,i,img,ins,kbd,label,map,mark,q,ruby,rp,rt,s,samp,small,span,strike,strong,sub,sup,time,tt,u,var")),
l=h("circle,defs,desc,ellipse,font-face,font-face-name,font-face-src,g,glyph,hkern,image,linearGradient,line,marker,metadata,missing-glyph,mpath,path,polygon,polyline,radialGradient,rect,stop,svg,switch,text,title,tspan"),w=h("script,style"),m=g({},r,x,p,n),N=h("background,cite,href,longdesc,src,xlink:href,xml:base"),n=h("abbr,align,alt,axis,bgcolor,border,cellpadding,cellspacing,class,clear,color,cols,colspan,compact,coords,dir,face,headers,height,hreflang,hspace,ismap,lang,language,nohref,nowrap,rel,rev,rows,rowspan,rules,scope,scrolling,shape,size,span,start,summary,tabindex,target,title,type,valign,value,vspace,width"),
p=h("accent-height,accumulate,additive,alphabetic,arabic-form,ascent,baseProfile,bbox,begin,by,calcMode,cap-height,class,color,color-rendering,content,cx,cy,d,dx,dy,descent,display,dur,end,fill,fill-rule,font-family,font-size,font-stretch,font-style,font-variant,font-weight,from,fx,fy,g1,g2,glyph-name,gradientUnits,hanging,height,horiz-adv-x,horiz-origin-x,ideographic,k,keyPoints,keySplines,keyTimes,lang,marker-end,marker-mid,marker-start,markerHeight,markerUnits,markerWidth,mathematical,max,min,offset,opacity,orient,origin,overline-position,overline-thickness,panose-1,path,pathLength,points,preserveAspectRatio,r,refX,refY,repeatCount,repeatDur,requiredExtensions,requiredFeatures,restart,rotate,rx,ry,slope,stemh,stemv,stop-color,stop-opacity,strikethrough-position,strikethrough-thickness,stroke,stroke-dasharray,stroke-dashoffset,stroke-linecap,stroke-linejoin,stroke-miterlimit,stroke-opacity,stroke-width,systemLanguage,target,text-anchor,to,transform,type,u1,u2,underline-position,underline-thickness,unicode,unicode-range,units-per-em,values,version,viewBox,visibility,width,widths,x,x-height,x1,x2,xlink:actuate,xlink:arcrole,xlink:role,xlink:show,xlink:title,xlink:type,xml:base,xml:lang,xml:space,xmlns,xmlns:xlink,y,y1,y2,zoomAndPan",
!0),L=g({},N,p,n),M=function(a,d){function c(b){b="<remove></remove>"+b;try{var c=(new a.DOMParser).parseFromString(b,"text/html").body;c.firstChild.remove();return c}catch(d){}}var b;try{b=!!c("")}catch(f){b=!1}if(b)return c;if(!d||!d.implementation)throw C("noinert");b=d.implementation.createHTMLDocument("inert");var e=(b.documentElement||b.getDocumentElement()).querySelector("body");return function(a){e.innerHTML=a;d.documentMode&&z(e);return e}}(s,s.document)}).info({angularVersion:"1.8.3"});
e.module("ngSanitize").filter("linky",["$sanitize",function(h){var g=/((s?ftp|https?):\/\/|(www\.)|(mailto:)?[A-Za-z0-9._%+-]+@)\S*[^\s.;,(){}<>"\u201d\u2019]/i,t=/^mailto:/i,q=e.$$minErr("linky"),s=e.isDefined,z=e.isFunction,v=e.isObject,y=e.isString;return function(f,e,u){function r(e){e&&l.push(O(e))}function x(f,h){var g,a=p(f);l.push("<a ");for(g in a)l.push(g+'="'+a[g]+'" ');!s(e)||"target"in a||l.push('target="',e,'" ');l.push('href="',f.replace(/"/g,"&quot;"),'">');r(h);l.push("</a>")}if(null==
f||""===f)return f;if(!y(f))throw q("notstring",f);for(var p=z(u)?u:v(u)?function(){return u}:function(){return{}},n=f,l=[],w,m;f=n.match(g);)w=f[0],f[2]||f[4]||(w=(f[3]?"http://":"mailto:")+w),m=f.index,r(n.substr(0,m)),x(w,f[0].replace(t,"")),n=n.substring(m+f[0].length);r(n);return h(l.join(""))}}])})(window,window.angular);
//# sourceMappingURL=/umbraco/lib/angular-sanitize/angular-sanitize.min.js.map;