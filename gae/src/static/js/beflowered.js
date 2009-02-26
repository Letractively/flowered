
/**
 * @fileoverview Provides the core JavaScript functionality for the Flowered
 *   application.
 */
 
(function($) {

  var map = null;
  var lastUpdate = 0;

  window.people = {}

  /**
   * Represents each person active within Flowered.
   * @param {string} name The name of the person in question.
   * @param {string} email The person's email address.
   * @param {number} latitude The person's starting latitude.
   * @param {number} longitude The person's starting longitude.
   * @constructor
   */
  var Person = function(name, email, lat, lng) {

    var me = this;
    window.people[email] = this;

    this.name = name;
    this.email = email;
    this.point = new GLatLng(lat, lng);
    this.marker = new GMarker(this.point, {draggable: true});
   
    map.addOverlay(this.marker);
    this.marker.setImage(GEOCHAT_IMAGES['marker-user']);
      
    // Handle drop events for this Person's marker. Note that this fires off
    // an Ajax call updating the user's location.
    var marker = this.marker;
    GEvent.addListener(this.marker, 'dragend', function() {
      updateUserPosition(marker);
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
   * Makes an Ajax call to update the user's position in the Flowered DB and
   * cache.
   */
  var updateUserPosition = function(marker) {
    $.post('/event/move', {
      'latitude': marker.getLatLng().lat(),
      'longitude': marker.getLatLng().lng(),
      'zoom': map.getZoom()
    });
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
      if (!window.people[events[i].user.email]) {
        speaker = new Person(
          events[i].user.nickname,
          events[i].user.email,
          events[i].latitude,
          events[i].longitude);
      } else {
        speaker = window.people[events[i].user.email];
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
      if (!window.people[move.user.email]) {
        new Person(
           move.user.nickname,
           move.user.email,
           move.latitude,
           move.longitude);
      } else {
        var mover = window.people[move.user.email];
        if (mover != user) {
          mover.move(move.latitude, move.longitude);        
        }
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
    chatCallback(data);
    moveCallback(data);
    window.setTimeout(update, GEOCHAT_VARS['update_interval'])
  }
  
  /**
   * A callback for when updates fail. Presents an error to the user and
   * forces a lengthier delay between updates.
   */
  window.updateError = function() {
    alert('An update error occured! Trying again in a bit.');
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
   
  window.onload = function() {
    if (GBrowserIsCompatible()) {
      
      // Initialize the map
      var mapDiv = document.getElementById('map');
      map = new GMap2(mapDiv);
      map.addControl(new GMapTypeControl());     
      map.addControl(new GLargeMapControl());
      map.addControl(new GScaleControl());
      map.addControl(new GOverviewMapControl(new GSize(200, 150)));
      map.enableGoogleBar();
      // map.setMapType(G_SATELLITE_MAP);

      map.enableContinuousZoom();     
      map.disableDoubleClickZoom();

      GEvent.clearListeners(map.getDragObject(), 'dblclick');
      GEvent.addListener(map, 'click', function(overlay, point) { 	    	  
          var user = new Person(
              GEOCHAT_VARS['user_nickname'],
              GEOCHAT_VARS['user_email'],
              point.lat(),
              point.lng());
          // updateUserPosition(); 	  
    	  
        // user.move(point.lat(), point.lng());
        // updateUserPosition();
      });
           
      var latitude = GEOCHAT_VARS['initial_latitude'];
      var longitude = GEOCHAT_VARS['initial_longitude'];       
      map.setCenter(new GLatLng(latitude, longitude), 13);
      
      map.openInfoWindow(map.getCenter(),
              document.createTextNode("Hello, world"));
      
      update();
    }
  };
  
  window.onunload = function() {
    GUnload();
  };

})(jQuery);
