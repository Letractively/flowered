(function($){var map=null;var lastUpdate=0;window.marker={};var Marker=function(id,lat,lng,type){var me=this;window.marker[id]=this;this.id=id;this.point=new GLatLng(lat,lng);this.type=type;this.project=FLOWERED_VARS.project_id;var flower=FLOWERED_IMAGES[this.type];var icon=new GIcon();icon.image=flower.image;icon.shadow=flower.shadow;icon.iconSize=new GSize(flower.iconSize.width,flower.iconSize.height);icon.shadowSize=new GSize(flower.shadowSize.width,flower.shadowSize.height);icon.iconAnchor=new GPoint(flower.anchor.x,flower.anchor.y);var markerOptions={icon:icon,draggable:true};this.marker=new GMarker(this.point,markerOptions);map.addOverlay(this.marker);GEvent.addListener(this.marker,"dragend",function(){me.update()});GEvent.addListener(this.marker,"fwd_singlerightclick",function(){map.removeOverlay(me.marker);me.remove()})};Marker.prototype.move=function(lat,lng){if(this.point.lat()!=lat||this.point.lng()!=lng){this.point=new GLatLng(lat,lng);this.marker.setLatLng(this.point)}};Marker.prototype.add=function(){$.ajax({type:"POST",url:"/event/add",cache:false,data:{id:this.id,latitude:this.point.lat(),longitude:this.point.lng(),type:this.type,project:this.project},timeout:5000})};Marker.prototype.remove=function(){$.ajax({type:"POST",url:"/event/delete",cache:false,data:{id:this.id},timeout:5000})};Marker.prototype.update=function(){$.ajax({type:"POST",url:"/event/move",cache:false,data:{id:this.id,latitude:this.marker.getLatLng().lat(),longitude:this.marker.getLatLng().lng()},timeout:5000})};var createRandomKey=function(length){var chars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";var random="key:";for(var i=0;i<length;i++){var position=Math.floor(Math.random()*(chars.length-1));random+=chars.charAt(position)}return random};window.addCallback=function(data){var adds=data.adds;for(var i=0;i<adds.length;++i){var add=adds[i];if(!window.marker[add.id]){var marker=new Marker(add.id,add.geopt.lat,add.geopt.lon,add.type)}}};window.moveCallback=function(data){var moves=data.moves;for(var i=0;i<moves.length;++i){var move=moves[i];if(!window.marker[move.id]){var marker=new Marker(move.id,move.geopt.lat,move.geopt.lon,move.type)}else{var mover=window.marker[move.id];mover.move(move.geopt.lat,move.geopt.lon)}}};window.removeCallback=function(data){var removes=data.removes;for(var i=0;i<removes.length;++i){var remove=removes[i];if(window.marker[remove.id]){var remover=window.marker[remove.id];map.removeOverlay(remover.marker)}}};window.updateSuccess=function(json){var data=eval("("+json+")");lastUpdate=data.timestamp;window.addCallback(data);window.moveCallback(data);window.removeCallback(data);window.setTimeout(window.update,FLOWERED_VARS.update_interval)};window.updateError=function(){window.setTimeout(window.update,FLOWERED_VARS.error_interval)};window.update=function(){var bounds=map.getBounds();var min=bounds.getSouthWest();var max=bounds.getNorthEast();$.ajax({type:"GET",url:"/event/update",cache:false,data:["min_latitude=",min.lat(),"&min_longitude=",min.lng(),"&max_latitude=",max.lat(),"&max_longitude=",max.lng(),"&since=",lastUpdate].join(""),timeout:5000,success:window.updateSuccess,error:window.updateError})};window.initialSuccess=function(json){var data=eval("("+json+")");window.addCallback(data)};window.initialError=function(){};window.initial=function(){var bounds=map.getBounds();var min=bounds.getSouthWest();var max=bounds.getNorthEast();$.ajax({type:"GET",url:"/event/initial",cache:false,data:["min_latitude=",min.lat(),"&min_longitude=",min.lng(),"&max_latitude=",max.lat(),"&max_longitude=",max.lng()].join(""),timeout:20000,success:window.initialSuccess,error:window.initialError})};$.fn.initializeInteractiveMap=function(){if(GBrowserIsCompatible()){var mapDiv=document.getElementById("map");map=new GMap2(mapDiv);map.setMapType(G_SATELLITE_MAP);map.addControl(new GLargeMapControl());map.addControl(new GScaleControl());map.enableContinuousZoom();map.disableDoubleClickZoom();GEvent.clearListeners(map.getDragObject(),"dblclick");GEvent.addListener(map,"click",function(overlay,point){if(point){var marker=new Marker(createRandomKey(24),point.lat(),point.lng(),FLOWERED_VARS.current_flower);marker.add()}});GEvent.addListener(map,"singlerightclick",function(point,src,overlay){if(overlay){if(overlay instanceof GMarker){GEvent.trigger(overlay,"fwd_singlerightclick")}}});GEvent.addListener(map,"moveend",function(point,src,overlay){window.initial()});var latitude=FLOWERED_VARS.initial_latitude;var longitude=FLOWERED_VARS.initial_longitude;var zoom=FLOWERED_VARS.initial_zoom;map.setCenter(new GLatLng(latitude,longitude),zoom);window.update()}else{alert("Sorry, the Google Maps API is not compatible with this browser")}};$.fn.initializeStandaloneMap=function(){if(GBrowserIsCompatible()){var mapDiv=document.getElementById("map");map=new GMap2(mapDiv);map.setMapType(G_SATELLITE_MAP);var latitude=FLOWERED_VARS.initial_latitude;var longitude=FLOWERED_VARS.initial_longitude;var zoom=FLOWERED_VARS.initial_zoom;map.setCenter(new GLatLng(latitude,longitude),zoom);window.initial();window.update()}else{alert("Sorry, the Google Maps API is not compatible with this browser")}};window.onunload=function(){GUnload()}})(jQuery);