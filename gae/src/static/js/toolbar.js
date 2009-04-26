
/**
 * @fileoverview Provides the core JavaScript functionality
 * for the flowered toolbar.
 */

$(function(){	
  // hover and event logic for toolbar buttons
  $(".fg-button:not(.ui-state-disabled)")
  .hover(function() { 
    $(this).addClass("ui-state-hover"); 
  }, function() { 
    $(this).removeClass("ui-state-hover"); 
  })
  .mousedown(function() {
    $(this).parents('.fg-buttonset-single:first').find(".fg-button.ui-state-active").removeClass("ui-state-active");
    if ($(this).is('.ui-state-active.fg-button-toggleable, .fg-buttonset-multi .ui-state-active')) {
     $(this).removeClass("ui-state-active");
    } else {
     $(this).addClass("ui-state-active");
    }
  })
  .mouseup(function() {
    if (!$(this).is('.fg-button-toggleable, .fg-buttonset-single .fg-button, .fg-buttonset-multi .fg-button')) {
      $(this).removeClass("ui-state-active");
    }
  })
  .click(function() {
	FLOWERED_VARS['current_flower'] = this.id;
  });

  // hover logic for toolbar
  $(".fg-toolbar-flowered")
  .hover(function() { 
    $(this).addClass("ui-state-opacity-100"); 
  }, function() { 
    $(this).removeClass("ui-state-opacity-100"); 
  });
  
  // Based on jQuery Right-Click Plugin 1.0.1 code by Cory S.N. LaViska
  // A Beautiful Site (http://abeautifulsite.net/)
  // Original copyright notice: This plugin is dual-licensed under the
  // GNU General Public License and the MIT License
  // and is copyright 2008 A Beautiful Site, LLC. 
  $.extend($.fn, {
    noContext: function() {
      $(this).each( function() {
        $(this)[0].oncontextmenu = function() {
          return false;
        }
      });
      return $(this);
    }
  });
});
	