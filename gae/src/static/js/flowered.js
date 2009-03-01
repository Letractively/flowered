
/**
 * @fileoverview Provides the core JavaScript functionality for the Flowered
 *   application.
 */

(function($) {

  var map = null;
  var lastUpdate = 0;

  window.marker = {}

  /**
   * Represents each person active within Flowered.
   * @param {string} id The person's ID.
   * @param {number} latitude The person's starting latitude.
   * @param {number} longitude The person's starting longitude.
   * @constructor
   */
  var Marker = function(id, lat, lng) {

    var me = this;
    window.marker[id] = this;

    this.id = id;
    this.point = new GLatLng(lat, lng);
    this.marker = new GMarker(this.point, {draggable: true});
   
    map.addOverlay(this.marker);
    this.marker.setImage(GEOCHAT_IMAGES['marker']);
   
    // Handle drop events for this marker's marker. Note that this fires off
    // an Ajax call updating the user's location.
    GEvent.addListener(this.marker, 'dragend', function() {
      me.update();
    });
    // Handle right click events for this Marks's marker. Note that this fires off
    // an Ajax call deleting the mark.   
    GEvent.addListener(this.marker, 'fwd_singlerightclick', function() {
      map.removeOverlay(me.marker);
      me.remove();
    });   
  };
 
  /**
   * Move this marker to the specified latitude and longitude.
   * @param {number} lat The latitude to move to.
   * @param {number} lng The longitude to move to.
   */  
  Marker.prototype.move = function(lat, lng) {
    if (this.point.lat() != lat || this.point.lng() != lng) {
      this.point = new GLatLng(lat, lng);
      this.marker.setLatLng(this.point);
    }
  };
  
  /**
   * Adds this marker to the Flowered DB and cache.
   * Makes an Ajax call to add the markers's position to the Flowered DB and
   * cache.
   */
  Marker.prototype.add = function() {
    $.post('/event/add', {
    	'id': this.id,
        'latitude': this.point.lat(),
        'longitude': this.point.lng()
    });
  };  

  /**
   * Removes this marker from the Flowered DB and cache.
   * Makes an Ajax call to remove the markers's position in the Flowered DB and
   * cache.
   */
  Marker.prototype.remove = function() {
	$.post('/event/remove', {
	    'id': this.id
	});
  };  

  /**
   * Removes this marker from the Flowered DB and cache.
   * Makes an Ajax call to update the markers's position from the Flowered DB and
   * cache.
   */
  Marker.prototype.update = function() {
    $.post('/event/move', {
        'id': this.id,
        'latitude': this.marker.getLatLng().lat(),
        'longitude': this.marker.getLatLng().lng()
    });
  };    
  
  var createRandomKey = function(length) {
	var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	var random = 'key:';
	for (var i = 0; i < length; i++) {
	  var position = Math.floor(Math.random() * (chars.length - 1));
	  random += chars.charAt(position);
    }
	return random;
  }
  
  /**
   * A callback for updates containing add events.
   * @param {Object} data A JSON object containing event data.
   */
  window.addCallback = function(data) {	  
    var adds = data.adds;    
    for (var i = 0; i < adds.length; ++i) {
      var add = adds[i];
      if (!window.marker[add.id]) {
        var marker = new Marker(
          add.id,
          add.latitude,
          add.longitude);
      }
    }  
  };
  
  /**
   * A callback for updates containing move events.
   * @param {Object} data A JSON object containing event data.
   */
  window.moveCallback = function(data) {
    var moves = data.moves;
    for (var i = 0; i < moves.length; ++i) {
      var move = moves[i];
      if (!window.marker[move.id]) {
        new Marker(
           move.id,
           move.latitude,
           move.longitude);
      } else {
        var mover = window.marker[move.id];
        mover.move(move.latitude, move.longitude);        
      }
    }
  }

  /**
   * A callback for updates containing remove events.
   * @param {Object} data A JSON object containing event data.
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
  }
  
  /**
   * A callback for when an update request succeeds.
   * @param {string} json JSON data to be evaluated and passed on to event
   *   callbacks.
   */
  window.updateSuccess = function(json) {
    var data = eval('(' + json + ')');
    lastUpdate = data.timestamp;
    addCallback(data);
    moveCallback(data);
    removeCallback(data);
    window.setTimeout(update, GEOCHAT_VARS['update_interval'])
  }
  
  /**
   * A callback for when updates fail. Presents an error to the user and
   * forces a lengthier delay between updates.
   */
  window.updateError = function() {
    window.setTimeout(update, GEOCHAT_VARS['error_interval'])
  }

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
      data: [
        'min_latitude=', min.lat(),
        '&min_longitude=', min.lng(),
        '&max_latitude=', max.lat(),
        '&max_longitude=', max.lng(),
        '&since=', lastUpdate
      ].join(''),
      success: updateSuccess,
      error: updateError
    });
  }

  /**
   * A callback for when an update request succeeds.
   * @param {string} json JSON data to be evaluated and passed on to event
   *   callbacks.
   */
  window.blaSuccess = function(json) {
    var data = eval('(' + json + ')');
    addCallback(data);
  }
  
  /**
   * A callback for when updates fail. Presents an error to the user and
   * forces a lengthier delay between updates.
   */
  window.blaError = function() {
    // alert('An bla error occured! Trying again in a bit.');
  }
  
  window.bla = function() {
	var bounds = map.getBounds();
	var min = bounds.getSouthWest();
	var max = bounds.getNorthEast();
	$.ajax({
	  type: 'GET',
      url: '/event/bla',
      data: [
        'min_latitude=', min.lat(),
        '&min_longitude=', min.lng(),
        '&max_latitude=', max.lat(),
        '&max_longitude=', max.lng()
      ].join(''),
      success: blaSuccess,
      error: blaError
    });
  }  
  
  window.onload = function() {
    if (GBrowserIsCompatible()) {
      
      // Initialize the map
      var mapDiv = document.getElementById('map');
      map = new GMap2(mapDiv);
      map.addControl(new GMapTypeControl());     
      map.addControl(new GLargeMapControl());
      map.addControl(new GScaleControl());
      // map.addControl(new GOverviewMapControl(new GSize(200, 150)));
      // map.enableGoogleBar();
      // map.setMapType(G_SATELLITE_MAP);

      map.enableContinuousZoom();     
      map.disableDoubleClickZoom();

      GEvent.clearListeners(map.getDragObject(), 'dblclick');
      GEvent.addListener(map, 'click', function(overlay, point) {
        var user = new Marker(
          createRandomKey(24),
          point.lat(),
          point.lng());
        user.add();
      });
      GEvent.addListener(map, 'singlerightclick', function(point, src, overlay) {
	    if (overlay) {
	      if (overlay instanceof GMarker) {
	    	// foward event to marker
	        GEvent.trigger(overlay, 'fwd_singlerightclick');
	      }
	    }
	  });
           
      var latitude = GEOCHAT_VARS['initial_latitude'];
      var longitude = GEOCHAT_VARS['initial_longitude'];       
      map.setCenter(new GLatLng(latitude, longitude), 13);
      
      // map.openInfoWindow(map.getCenter(),
      //        document.createTextNode("Hello, world"));
      
      bla();
      update();
    }
  };
  
  window.onunload = function() {
    GUnload();
  };

})(jQuery);
