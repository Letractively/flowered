
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
  var Person = function(id, lat, lng) {

    var me = this;
    window.marker[id] = this;

    this.id = id;
    this.point = new GLatLng(lat, lng);
    this.marker = new GMarker(this.point, {draggable: true});
   
    map.addOverlay(this.marker);
    this.marker.setImage(GEOCHAT_IMAGES['marker-user']);

    var mark_id = this.id;
    var marker = this.marker;
    
    // Handle drop events for this Person's marker. Note that this fires off
    // an Ajax call updating the user's location.
    GEvent.addListener(this.marker, 'dragend', function() {
      updateUserPosition(id, marker);
    });
    // Handle right click events for this Marks's marker. Note that this fires off
    // an Ajax call deleting the mark.   
    GEvent.addListener(this.marker, 'fwd_singlerightclick', function() {
      map.removeOverlay(marker);
      deleteMark(id);
    });   
  };
 
  /**
   * Move this Person to the specified latitude and longitude.
   * @param {number} lat The latitude to move to.
   * @param {number} lng The longitude to move to.
   */  
  Person.prototype.move = function(lat, lng) {
    if (this.point.lat() != lat || this.point.lng() != lng) {
      this.point = new GLatLng(lat, lng);
      this.marker.setLatLng(this.point);
    }
  };
  
  /**
   * Move this Person to the specified latitude and longitude.
   * @param {number} lat The latitude to move to.
   * @param {number} lng The longitude to move to.
   */
  Person.prototype.add = function() {
    $.post('/event/add', {
    	'id': this.id,
        'latitude': this.point.lat(),
        'longitude': this.point.lng()
    });
  };  
  
  /**
   * Makes an Ajax call to update the user's position in the Flowered DB and
   * cache.
   */
  var updateUserPosition = function(id, marker) {
    $.post('/event/move', {
        'id': id,
        'latitude': marker.getLatLng().lat(),
        'longitude': marker.getLatLng().lng()
    });
  }
 
  /**
   * Deletes a marker in the Flowered DB and cache
   */
  var deleteMark = function(id) {
	$.post('/event/delete', {
	   'id': id
    });
  }  

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
   * Causes a chat event for the currently active user, including an Ajax
   * call against the Flowered datastore.
   * @param {DOM} chatInput The input field to pull chat contents from.
   */
  /* window.say = function(chatInput, marker) {
    var chat = chatInput.value;
    if (chat) {
      chatInput.value = '';
      $.post('/event/chat', {
        'contents': chat,
        'latitude': marker.getLatLng().lat(),
        'longitude': marker.getLatLng().lng(),
        'zoom': map.getZoom()
      });
    }
  }; */
  
  /**
   * A callback for updates containing chat events.
   * @param {Object} data A JSON object containing event data.
   */
  window.chatCallback = function(data) {
    var events = data.chats;
    for (var i = 0; i < events.length; ++i) {
                
      var speaker = null;
      
      // Verify whether the speaker exists. If not, create them.
      if (!window.marker[events[i].user.id]) {
        speaker = new Person(
          events[i].user.key,
          events[i].latitude,
          events[i].longitude);
      } else {
        speaker = window.marker[events[i].user.id];
      }
      
      // Update the speaker's chat bubble.
      // speaker.move(events[i].latitude, events[i].longitude);
      // speaker.say(events[i].contents);
      
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
        new Person(
           move.id,
           move.latitude,
           move.longitude);
      } else {
        var mover = window.marker[move.id];
        //if (mover != user) {
          mover.move(move.latitude, move.longitude);        
        //}
      }
    }
  }

  window.addCallback = function(data) {
    var events = data.adds;

    for (var i = 0; i < events.length; ++i) {
                
      var speaker = null;
      
      // alert('id1=' + events[i].id);
      
      speaker = new Person(
        events[i].id,
        events[i].latitude,
        events[i].longitude);
      
      // Update the speaker's chat bubble.
      speaker.move(events[i].latitude, events[i].longitude);
      // speaker.say(events[i].contents);
      
    }    
  };
  
  /**
   * A callback for when an update request succeeds.
   * @param {string} json JSON data to be evaluated and passed on to event
   *   callbacks.
   */
  window.updateSuccess = function(json) {
    var data = eval('(' + json + ')');
    lastUpdate = data.timestamp;
    chatCallback(data);
    moveCallback(data);
    window.setTimeout(update, GEOCHAT_VARS['update_interval'])
  }
  
  /**
   * A callback for when updates fail. Presents an error to the user and
   * forces a lengthier delay between updates.
   */
  window.updateError = function() {
    // alert('An update error occured! Trying again in a bit.');
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
        '&zoom=', map.getZoom(),
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
        var user = new Person(
          createRandomKey(24),
          point.lat(),
          point.lng());
        user.add();
      });
      GEvent.addListener(map, 'singlerightclick', function(point, src, overlay) {
	    if (overlay) {
	      if (overlay instanceof GMarker) {
	    	// fowward event to marker
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
