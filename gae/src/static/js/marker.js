FLOWERED_IMAGES = {
  'f00': {
    'image': '/static/images/f00.png',
    'shadow': '/static/images/shadow-f00.png',
    'iconSize': {'width': '66', 'heigth': '70'},
    'shadowSize': {'width': '102', 'heigth': '70'},
    'anchor': {'x': '33', 'y': '35'},
  },
};

//var map; // Must be initialized first.
//
//GEvent.addListener(map, 'click', 
//    function(overlay, point) {
//        if (overlay) {
//            map.removeOverlay(overlay);
//        } else {
//            addMarker(point);
//        }
//    });
//
//function addMarker(point) {
//    var icon = new GIcon();
//    icon.image = "images/f00.png";
//    icon.shadow = "images/shadow-f00.png";
//    icon.iconSize = new GSize(66.0, 70.0);
//    icon.shadowSize = new GSize(102.0, 70.0);
//    icon.iconAnchor = new GPoint(33.0, 35.0);
//    icon.infoWindowAnchor = new GPoint(33.0, 35.0);
//        
//    map.addOverlay(new GMarker(point, icon));
//}