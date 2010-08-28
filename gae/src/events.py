
# Copyright 2009 Daniel Schubert
# Copyright 2008 Google Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# 
#     http://www.apache.org/licenses/LICENSE-2.0
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

"""Handlers for Flowered user events.

Contains several RequestHandler subclasses used to handle put and get
operations, along with any helper functions. This script is designed to be
run directly as a WSGI application, and within Flowered handles all URLs
under /event.

  UpdateHandler: Handles user requests for updated lists of events.
  ChatHandler: Handles user chat input events.
  MoveHandler: Handles user movement events.
  RefreshCache(): Checks the age of the cache, and updates if necessary.
"""


import datetime
import logging
import os
import time

import datamodel
import json

from google.appengine.api import users
from google.appengine.ext import db 
from google.appengine.runtime.apiproxy_errors import CapabilityDisabledError
from google.appengine.ext import webapp
from google.appengine.ext.webapp.util import run_wsgi_app

# The time interval between syncs as a timedelta.
sync_interval = datetime.timedelta(0, 10)

# A datetime indicating the last time the chat cache was synced from the DB.
last_sync = datetime.datetime.now() - sync_interval

# A list storing the add cache.
add_cache = []

# A list storing the move cache.
move_cache = []

# A list storing the delete cache.
remove_cache = []


class UpdateHandler(webapp.RequestHandler):
  """Handles user requests for updated lists of events.
  
  UpdateHandler only accepts "get" events, sent via web forms. It expects each
  request to include "min_latitude", "min_longitude", "max_latitude",
  "max_longitude", "zoom", and "since" fields.
  """
  
  def get(self):
    global sync_interval
    global last_sync
    global add_cache
    global move_cache
    global remove_cache

    min_latitude = float(self.request.get('min_latitude'))
    min_longitude = float(self.request.get('min_longitude'))
    max_latitude = float(self.request.get('max_latitude'))
    max_longitude = float(self.request.get('max_longitude'))
    # zoom = self.request.get('zoom')
    
    if self.request.get('since') == '':
      since = 0
    else:
      since = float(self.request.get('since'))
      
    since_datetime = datetime.datetime.fromtimestamp(since)
    
    # Restrict latitude/longitude to restrict bulk downloads.
    #if (max_latitude - min_latitude) > 1:
    #  max_latitude = min_latitude + 1
    #if (max_longitude - min_longitude) > 1:
    #  max_longitude = min_longitude + 1
      
    add_events = []
    move_events = []
    remove_events = []
    
    if since > 0:
      RefreshCache()     

      for entry in add_cache:
        if (entry.timestamp > since_datetime and
            entry.geopt.lat > min_latitude and
            entry.geopt.lat < max_latitude and
            entry.geopt.lon > min_longitude and
            entry.geopt.lon < max_longitude):
          add_events.append(entry) 
 
      for entry in move_cache:
        if (entry.timestamp > since_datetime and
            entry.geopt.lat > min_latitude and
            entry.geopt.lat < max_latitude and
            entry.geopt.lon > min_longitude and
            entry.geopt.lon < max_longitude):
          move_events.append(entry) 

      for entry in remove_cache:
        if (entry.timestamp > since_datetime and
            entry.geopt.lat > min_latitude and
            entry.geopt.lat < max_latitude and
            entry.geopt.lon > min_longitude and
            entry.geopt.lon < max_longitude):
          remove_events.append(entry) 
                        
    output = {
        'timestamp': time.time(),
        'adds': add_events,
        'moves': move_events,
        'removes': remove_events,
      }

    self.response.headers['Content-Type'] = 'text/plain'
    self.response.out.write(json.encode(output));


class InitialHandler(webapp.RequestHandler):
  """Handles user requests for updated lists of events.
  
  InitialHandler only accepts "get" events, sent via web forms. It expects each
  request to include "min_latitude", "min_longitude", "max_latitude",
  and "max_longitude" fields.
  """
  
  def get(self):
    min_latitude = float(self.request.get('min_latitude'))
    min_longitude = float(self.request.get('min_longitude'))
    max_latitude = float(self.request.get('max_latitude'))
    max_longitude = float(self.request.get('max_longitude'))
    
    # Restrict latitude/longitude to restrict bulk downloads.
    #if (max_latitude - min_latitude) > 1:
    #  max_latitude = min_latitude + 1
    #if (max_longitude - min_longitude) > 1:
    #  max_longitude = min_longitude + 1
     
    # Sync the add cache.
    min_geopt = db.GeoPt(min_latitude, min_longitude)
    max_geopt = db.GeoPt(max_latitude, max_longitude)
    query = datamodel.Mark.gql('WHERE geopt > :min_geopt AND geopt < :max_geopt ',
                               min_geopt = min_geopt, max_geopt = max_geopt)
    add_events = query.fetch(1000)
             
    output = {
        'timestamp': time.time(),
        'adds': add_events
    }

    self.response.headers['Content-Type'] = 'text/plain'
    self.response.out.write(json.encode(output));
  

class MoveHandler(webapp.RequestHandler):
  """Handles user movement events.
  
  MoveHandler only provides a post method for receiving new user co-ordinates,
  and doesn't store any data to the datastore as ChatHandler does with
  ChatEvents, instead just adding straight to the local cache.
  """
  
  def post(self):
    global move_cache

    # Get the mark to modify and return if not exists.
    mark = datamodel.Mark.get_by_key_name(self.request.get('id'))
    if mark == None:      
      return

    # Update current mark's position and timestamp.
    mark.timestamp = datetime.datetime.now()
    mark.geopt = db.GeoPt(float(self.request.get('latitude')),
                          float(self.request.get('longitude')))
    
    try:
      mark.put()
    except CapabilityDisabledError:
      # fail gracefully here 
      pass
  
    #logging.info('#### move=' + str(mark.geopt))
     
    # Append to the move cache, so we don't need to wait for a refresh.
    #add_cache.remove(mark)
    move_cache.append(mark)

class AddHandler(webapp.RequestHandler):
  def post(self):
    global add_cache
    
    # Create new mark.
    mark = datamodel.Mark(key_name = self.request.get('id'))
    mark.timestamp = datetime.datetime.now()
    mark.geopt = db.GeoPt(float(self.request.get('latitude')),
                           float(self.request.get('longitude')))
    mark.type = str(self.request.get('type'))
    mark.project = str(self.request.get('project'))
    
    # Add mark to datastore.
    try:
      mark.put()
    except CapabilityDisabledError:
      # fail gracefully here 
      pass

    # Append to the add cache, so we don't need to wait on a refresh.
    add_cache.append(mark) 
    
class DeleteHandler(webapp.RequestHandler):
  def post(self):
    global remove_cache

    # Get the mark to delete and return if not exists.
    mark = datamodel.Mark.get_by_key_name(self.request.get('id'))
    if mark == None:      
      return

    # Delete mark from datastore.
    try:
      db.delete(mark)
    except CapabilityDisabledError:
      # fail gracefully here 
      pass

    # Append to the delete cache, so we don't need to wait for a refresh.
    mark.timestamp = datetime.datetime.now()
    #add_cache.remove(mark)
    remove_cache.append(mark)
             
def RefreshCache():
  """Check the freshness of chat and move caches, and refresh if necessary.
  
  RefreshCache relies on the globals "sync_interval" and "last_sync" to
  determine the age of the existing cache and whether or not it should be
  updated. All output goes to "chat_cache" and "move_cache" globals.
  """
  
  global sync_interval
  global last_sync
  global add_cache
  global move_cache
  global remove_cache
  
  now = datetime.datetime.now()
  sync_frame = sync_interval * 2
  
  if last_sync < now - sync_interval:  
    last_sync = datetime.datetime.now()
 
    # Trim the caches.
    add_cache = add_cache[-100:]
    move_cache = move_cache[-100:]
    remove_cache = remove_cache[-100:]
    #add_cache = add_cache[:500]
    #move_cache = move_cache[:500]
    #remove_cache = remove_cache[:500]
    
 
def main():
  """Main method called when the script is executed directly.
  
  This method is called each time the script is launched, and also has the
  effect of enabling caching for global variables.
  """
  # logging.getLogger().setLevel(logging.DEBUG)
  application = webapp.WSGIApplication([
    ('/event/initial', InitialHandler),
    ('/event/add', AddHandler),
    ('/event/move', MoveHandler),
    ('/event/delete', DeleteHandler),
    ('/event/update', UpdateHandler),
    ], debug = True)
  run_wsgi_app(application)

if __name__ == '__main__':
  main()
