
/**
 * @fileoverview Provides the core JavaScript functionality for the Flowered
 *               application.
 */

(function($) {

  var map = null;
  var lastUpdate = 0;

  window.marker = {};

  /**
	 * Represents each person active within Flowered.
	 * 
	 * @param {string}
	 *            id The person's ID.
	 * @param {number}
	 *            latitude The person's starting latitude.
	 * @param {number}
	 *            longitude The person's starting longitude.
	 * @constructor
	 */
  var Marker = function(id, lat, lng, type) {

    var me = this;
    window.marker[id] = this;

    this.id = id;
    this.point = new GLatLng(lat, lng);
    this.type = type;
    this.project = FLOWERED_VARS['project_id'];
       
    var markerOptions = { icon: me.createIcon(), draggable: me.isDraggable() };
    this.marker = new GMarker(this.point, markerOptions);
    map.addOverlay(this.marker);
  
    // Handle drop events for this marker's marker. Note that this fires off
    // an Ajax call updating the user's location.
    GEvent.addListener(this.marker, 'dragend', function() {  	
      me.update();
	});
	// Handle right click events for this Marker's marker. Note that this fires
	// off
	// an Ajax call deleting the mark.
    GEvent.addListener(this.marker, 'fwd_singlerightclick', function() {
      if (me.isDeleteable())
      {
        map.removeOverlay(me.marker);
        me.remove();
      }
 	});
  };
 
  /** Creates a new marker icon.
    * 
	* @return {GIcon}
	*             The newly created icon.
	*/
  Marker.prototype.createIcon = function() {
	var flower = FLOWERED_IMAGES[this.type];   
	var icon = new GIcon();
	icon.image = flower.image;
	icon.shadow = flower.shadow;
	icon.iconSize = new GSize(flower.iconSize.width, flower.iconSize.height);
	icon.shadowSize = new GSize(flower.shadowSize.width, flower.shadowSize.height);
	icon.iconAnchor = new GPoint(flower.anchor.x, flower.anchor.y);
	return icon;
  }  
  
  /** Return if the marker is deleteable.
    *
    * @return {Bool}
    *             Returns t deleteable property of the marker.
	* 
	*/
  Marker.prototype.isDeleteable = function() {
    var flower = FLOWERED_IMAGES[this.type];
	return flower.properties.deleteable;
  }
  
  /** Return if the marker is draggable.
	*
	* @return {Bool}
    *             Returns the draggable property of the marker.
	*/
  Marker.prototype.isDraggable = function() {
	var flower = FLOWERED_IMAGES[this.type];
	return flower.properties.draggable;
}
  
  /**
	* Move this marker to the specified latitude and longitude.
	* 
	* @param {number}
	*            lat The latitude to move to.
	* @param {number}
	*            lng The longitude to move to.
	*/  
  Marker.prototype.move = function(lat, lng) {
    if (this.point.lat() != lat || this.point.lng() != lng) {
      this.point = new GLatLng(lat, lng);
      this.marker.setLatLng(this.point);
    }
  };
  
  /**
	* Adds this marker to the Flowered DB and cache. Makes an Ajax call to add
	* the markers's position to the Flowered DB and cache.
	*/
  Marker.prototype.add = function() {
    $.ajax({
       type: 'POST',
       url: '/event/add',
       cache: false,
       data: {
    	 'id': this.id,
         'latitude': this.point.lat(),
         'longitude': this.point.lng(),
         'type': this.type,
         'project': this.project
       },
       timeout: 5000
     });
  }; 

  /**
	* Removes this marker from the Flowered DB and cache. Makes an Ajax call to
	* remove the markers's position in the Flowered DB and cache.
	*/
  Marker.prototype.remove = function() {
    $.ajax({
       type: 'POST',
       url: '/event/delete',
       cache: false,
       data: {
         'id': this.id
       },
       timeout: 5000
    });
  };  

  /**
	* Removes this marker from the Flowered DB and cache. Makes an Ajax call to
	* update the markers's position from the Flowered DB and cache.
	*/
  Marker.prototype.update = function() {
    $.ajax({
       type: 'POST',
       url: '/event/move',
       cache: false,
       data: {
         'id': this.id,
         'latitude': this.marker.getLatLng().lat(),
         'longitude': this.marker.getLatLng().lng()
       },
       timeout: 5000
    });
  };    
  
  /**
	* Creates a random key.
	* 
	* @param {length}
	*            length of the key to create plus 4 additional tokens.
	*/
  var createRandomKey = function(length) {
	var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	var random = 'key:';
	for (var i = 0; i < length; i++) {
	  var position = Math.floor(Math.random() * (chars.length - 1));
	  random += chars.charAt(position);
    }
	return random;
  };
  
  /**
	* A callback for updates containing add events.
	* 
	* @param {Object}
	*            data A JSON object containing event data.
	*/
  window.addCallback = function(data) {	  
    var adds = data.adds;    
    for (var i = 0; i < adds.length; ++i) {
      var add = adds[i];
      if (!window.marker[add.id]) {
        var marker = new Marker(
          add.id,
          add.geopt.lat,
          add.geopt.lon,
          add.type);
      }
    }
  };
  
  /**
   	* A callback for updates containing move events.
	* 
	* @param {Object}
	*            data A JSON object containing event data.
	*/
  window.moveCallback = function(data) {
    var moves = data.moves;
    for (var i = 0; i < moves.length; ++i) {
      var move = moves[i];
      if (!window.marker[move.id]) {
        var marker = new Marker(
           move.id,
           move.geopt.lat,
           move.geopt.lon,
           move.type);
      } else {
        var mover = window.marker[move.id];
        mover.move(move.geopt.lat, move.geopt.lon);        
      }
    }
  };

  /**
	* A callback for updates containing remove events.
	* 
	* @param {Object}
	*            data A JSON object containing event data.
	*/
  window.removeCallback = function(data) {
    var removes = data.removes;
    for (var i = 0; i < removes.length; ++i) {
      var remove = removes[i];
      if (window.marker[remove.id]) {
        var remover = window.marker[remove.id];
        map.removeOverlay(remover.marker);
      }
    }
  };
  
  /**
	* A callback for when an update request succeeds.
	* 
	* @param {string}
	*            json JSON data to be evaluated and passed on to event
	*            callbacks.
	*/
  window.updateSuccess = function(json) {
    var data = eval('(' + json + ')');
    lastUpdate = data.timestamp;
    window.addCallback(data);
    window.moveCallback(data);
    window.removeCallback(data);
    window.setTimeout(window.update, FLOWERED_VARS['update_interval']);
  };
  
  /**
	* A callback for when updates fail. Presents an error to the user and
	* forces a lengthier delay between updates.
	*/
  window.updateError = function() {
    window.setTimeout(window.update, FLOWERED_VARS['error_interval']);
  };

  /**
	* The main update handler, used to query the Flowered DB and cache for
	* updated events. Successful queries pass the results on to the
	* chatCallback and moveCallback functions.
	*/
  window.update = function() {
    var bounds = map.getBounds();
    var min = bounds.getSouthWest();
    var max = bounds.getNorthEast();
    $.ajax({
      type: 'GET',
      url: '/event/update',
      cache: false,
      data: [
        'min_latitude=', min.lat(),
        '&min_longitude=', min.lng(),
        '&max_latitude=', max.lat(),
        '&max_longitude=', max.lng(),
        '&since=', lastUpdate
      ].join(''),
      timeout: 5000,
      success: window.updateSuccess,
      error: window.updateError
    });
  };

  /**
	* A callback for when an update request succeeds.
	* 
	* @param {string}
	*            json JSON data to be evaluated and passed on to event
	*            callbacks.
	*/
  window.initialSuccess = function(json) {
    var data = eval('(' + json + ')');
    window.addCallback(data);
  };
  
  /**
	* A callback for when updates fail. Presents an error to the user and
	* forces a lengthier delay between updates.
	*/
  window.initialError = function() {
    // window.setTimeout(window.initial, FLOWERED_VARS['initial_interval']);
  };
  
  window.initial = function() {
	var bounds = map.getBounds();
	var min = bounds.getSouthWest();
	var max = bounds.getNorthEast();
	$.ajax({
	  type: 'GET',
      url: '/event/initial',
      cache: false,
      data: [
        'min_latitude=', min.lat(),
        '&min_longitude=', min.lng(),
        '&max_latitude=', max.lat(),
        '&max_longitude=', max.lng()
      ].join(''),
      timeout: 20000,
      success: window.initialSuccess,
      error: window.initialError
    });
  };
 
  $.fn.initializeInteractiveMap = function() {
    if (GBrowserIsCompatible()) {
           
      var mapOptions = {
        googleBarOptions : {
    	  showOnLoad : true,
          style : 'new',
        }
      };
      var mapDiv = document.getElementById('map');
      map = new GMap2(mapDiv, mapOptions);
      map.setMapType(G_SATELLITE_MAP);
      // map.setUIToDefault();
     
      map.addControl(new GLargeMapControl());
      map.addControl(new GScaleControl());

      map.enableContinuousZoom();     
      map.disableDoubleClickZoom();
      
      GEvent.clearListeners(map.getDragObject(), 'dblclick');
      GEvent.addListener(map, 'click', function(overlay, point) {
    	if (point) {
          var marker = new Marker(
            createRandomKey(24),
            point.lat(),
            point.lng(),
            FLOWERED_VARS['current_flower']);
          marker.add();
    	}
      });     
      GEvent.addListener(map, 'singlerightclick', function(point, src, overlay) {
    	if (overlay) {
	      if (overlay instanceof GMarker) {
	    	// forward event to marker
	        GEvent.trigger(overlay, 'fwd_singlerightclick');
	      }
	    }
	  });
      GEvent.addListener(map, 'moveend', function(point, src, overlay) {
    	  window.initial();
  	  });
      
      var latitude = FLOWERED_VARS['initial_latitude'];
      var longitude = FLOWERED_VARS['initial_longitude'];
      var zoom = FLOWERED_VARS['initial_zoom'];
      map.setCenter(new GLatLng(latitude, longitude), zoom);
      
      var searchbox = String(FLOWERED_VARS['show_searchbox'].toLowerCase());
      if (searchbox == 'true') {
    	  // console.log('strings match');
    	  map.enableGoogleBar();
      } else {
    	  // console.log('strings don\'t match');
      }
      // console.log('searchbox=%s', searchbox);
            
      window.update();
    }
    // display a warning if the browser was not compatible
    else { 
      alert("Sorry, the Google Maps API is not compatible with this browser"); 
    } 
  };
  
  $.fn.initializeStandaloneMap = function() {
    if (GBrowserIsCompatible()) {

      var mapDiv = document.getElementById('map');
      map = new GMap2(mapDiv);
      map.setMapType(G_SATELLITE_MAP);
     
      var latitude = FLOWERED_VARS['initial_latitude'];
      var longitude = FLOWERED_VARS['initial_longitude'];    
      var zoom = FLOWERED_VARS['initial_zoom'];
      map.setCenter(new GLatLng(latitude, longitude), zoom);
      
      window.initial();
      window.update();
    }
    // display a warning if the browser was not compatible
    else { 
      alert("Sorry, the Google Maps API is not compatible with this browser"); 
    } 
  };
  
  window.onunload = function() {
    GUnload();
  };

  // $("#msg").ajaxError(function(event, request, settings) {
  // $(this).append("<li>Error requesting page " + settings.url + "</li>");
  // });
  
})(jQuery);
