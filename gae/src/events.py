
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

"""Handlers for Geochat user events.

Contains several RequestHandler subclasses used to handle put and get
operations, along with any helper functions. This script is designed to be
run directly as a WSGI application, and within Geochat handles all URLs
under /event.

  UpdateHandler: Handles user requests for updated lists of events.
  ChatHandler: Handles user chat input events.
  MoveHandler: Handles user movement events.
  RefreshCache(): Checks the age of the cache, and updates if necessary.
"""

# TODO Cache sync problems.
# TODO Problem with duplicate messages.
# TODO Spam controls.

import datetime
import logging
import os
import time

import datamodel
import json

from google.appengine.api import users
from google.appengine.ext import db
from google.appengine.ext import webapp
from google.appengine.ext.webapp.util import run_wsgi_app

# The time interval between syncs as a timedelta.
sync_interval = datetime.timedelta(0, 10)

# A datetime indicating the last time the chat cache was synced from the DB.
last_sync = datetime.datetime.now() - sync_interval

# A list storing the move cache.
move_cache = []

# A list storing the add cache.
add_cache = []

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
    zoom = self.request.get('zoom')
    if self.request.get('since') == '':
      since = 0
    else:
      since = float(self.request.get('since'))
    since_datetime = datetime.datetime.fromtimestamp(since)
    
    # Restrict latitude/longitude to restrict bulk downloads.
    if (max_latitude - min_latitude) > 1:
      max_latitude = min_latitude + 1
    if (max_longitude - min_longitude) > 1:
      max_longitude = min_longitude + 1
      
    add_events = []
    move_events = []
    remove_events = []
    
    if since > 0:
      RefreshCache()     

      for entry in add_cache:
        if (entry.timestamp > since_datetime and
            entry.latitude > min_latitude and
            entry.latitude < max_latitude and
            entry.longitude > min_longitude and
            entry.longitude < max_longitude):
          add_events.append(entry) 
 
      for entry in move_cache:
        if (entry.timestamp > since_datetime and
            entry.latitude > min_latitude and
            entry.latitude < max_latitude and
            entry.longitude > min_longitude and
            entry.longitude < max_longitude):
          move_events.append(entry) 

      for entry in remove_cache:
        if (entry.timestamp > since_datetime and
            entry.latitude > min_latitude and
            entry.latitude < max_latitude and
            entry.longitude > min_longitude and
            entry.longitude < max_longitude):
          remove_events.append(entry) 
                        
    output = {
        'timestamp': time.time(),
        'adds': add_events,
        'moves': move_events,
        'removes': remove_events,
      }

    self.response.headers['Content-Type'] = 'text/plain'
    self.response.out.write(json.encode(output));


class BlaHandler(webapp.RequestHandler):
  
  """Handles user requests for updated lists of events.
  
  UpdateHandler only accepts "get" events, sent via web forms. It expects each
  request to include "min_latitude", "min_longitude", "max_latitude",
  "max_longitude", "zoom", and "since" fields.
  """
  
  def get(self):
    #global add_cache

    min_latitude = float(self.request.get('min_latitude'))
    min_longitude = float(self.request.get('min_longitude'))
    max_latitude = float(self.request.get('max_latitude'))
    max_longitude = float(self.request.get('max_longitude'))
    
    # Restrict latitude/longitude to restrict bulk downloads.
    if (max_latitude - min_latitude) > 1:
      max_latitude = min_latitude + 1
    if (max_longitude - min_longitude) > 1:
      max_longitude = min_longitude + 1
    
    add_events = []
    
    # Sync the chat cache.
    query = db.Query(datamodel.Mark)
    query.order('timestamp')
    #add_list = list(query.fetch(100))
    
    for entry in query:
    #for entry in add_list:
      if (entry.latitude > min_latitude and
          entry.latitude < max_latitude and
          entry.longitude > min_longitude and
          entry.longitude < max_longitude):
        add_events.append(entry)       
              
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

    # Update current mark's position and timestamp
    mark.timestamp = datetime.datetime.now()
    mark.latitude = float(self.request.get('latitude'))
    mark.longitude = float(self.request.get('longitude'))
    mark.put()
     
    # Append to the move cache, so we don't need to wait for a refresh.
    move_cache.append(mark)

class AddHandler(webapp.RequestHandler):

  def post(self):
    global add_cache
    
    # Create and insert the a new mark event.
    event = datamodel.Mark(key_name = self.request.get('id'))
    event.timestamp = datetime.datetime.now()
    event.type = str(self.request.get('type'))
    event.latitude  = float(self.request.get('latitude'))
    event.longitude = float(self.request.get('longitude'))
    event.put()

    # Append to the add cache, so we don't need to wait on a refresh.
    add_cache.append(event) 
    
class DeleteHandler(webapp.RequestHandler):

  def post(self):
    global remove_cache  

    # Get the mark to delete and return if not exists.
    mark = datamodel.Mark.get_by_key_name(self.request.get('id'))
    if mark == None:      
      return

    # Delete mark from datastore
    db.delete(mark)

    # Append to the delete cache, so we don't need to wait for a refresh.
    mark.timestamp = datetime.datetime.now()
    remove_cache.append(mark)
             
def RefreshCache():
  
  """Check the freshness of chat and move caches, and refresh if necessary.
  
  RefreshCache relies on the globals "sync_interval" and "last_sync" to
  determine the age of the existing cache and whether or not it should be
  updated. All output goes to "chat_cache" and "move_cache" globals.
  """
  
  global sync_interval
  global last_sync
  #global chat_cache
  global add_cache
  global move_cache
  global remove_cache
  
  now = datetime.datetime.now()
  sync_frame = sync_interval * 2
  
  if last_sync < now - sync_interval:
    
    # Sync the chat cache.
    #query = db.Query(datamodel.ChatEvent)
    #query.filter('timestamp > ', now - sync_frame)
    #query.order('timestamp')
    #chat_cache = list(query.fetch(100))
    last_sync = datetime.datetime.now()
    # logging.info('Cache refreshed.')
    
    # Trim the move cache.
    add_cache = add_cache[-100:]
    move_cache = move_cache[-100:]
    remove_cache = remove_cache[-100:]
    
def BlaRefreshCache():
  
  """Check the freshness of chat and move caches, and refresh if necessary.
  
  RefreshCache relies on the globals "sync_interval" and "last_sync" to
  determine the age of the existing cache and whether or not it should be
  updated. All output goes to "chat_cache" and "move_cache" globals.
  """
  
  #global add_cache
  
  # Sync the chat cache.
  #query = db.Query(datamodel.Mark)
  #query.order('timestamp')
  #add_cache = list(query.fetch(100))

  #logging.info('Bla cache refreshed.')
    
  # Trim the move cache.
  #move_cache = move_cache[-100:]
  
def main():
  
  """Main method called when the script is executed directly.
  
  This method is called each time the script is launched, and also has the
  effect of enabling caching for global variables.
  """
  
  application = webapp.WSGIApplication(
      [
        ('/event/update', UpdateHandler),
        ('/event/add', AddHandler),
        ('/event/remove', DeleteHandler),
        ('/event/bla', BlaHandler),
        ('/event/move', MoveHandler)
      ],
      debug = True)
  run_wsgi_app(application)

if __name__ == '__main__':
  main()
