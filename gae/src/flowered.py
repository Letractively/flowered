
# Copyright 2008 Google Inc.
# Copyright 2009 Daniel Schubert
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

"""The main Flowered application.

Contains the MainHandler, which handles root requests to the server, along
with several other template-driven pages that don't have any significant DB
interaction.

  SchwerinMainHandler: Handles requests to /schwerin
  SchwerinStandaloneHandler: Handles requests to /schwerin/standalone
  WorldMainHandler: Handles requests to /world
  WorldStandaloneHandler: Handles requests to /world/standalone
"""

import datetime
import logging
import os
import time
import string

from google.appengine.api import users
from google.appengine.ext import db
from google.appengine.ext import webapp
from google.appengine.ext.webapp import template
from google.appengine.ext.webapp.util import run_wsgi_app

import datamodel
import json

logging.info('Loading %s, app version = %s',
  __name__, os.getenv('CURRENT_VERSION_ID'))

# Set to true if we want to have our webapp print stack traces, etc
_DEBUG = True


class BaseRequestHandler(webapp.RequestHandler):
  """Handles all requests
    
  BaseRequestHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """
  
  # The different project output types we support: locations,
  # zoom level and template file names
  _PROJECT_TYPES = {
    'world': ['52.523405, 13.4114', '15', 'flowered.html'],
    'standalone_world': ['52.523405, 13.4114', '15', 'standalone.html'],
    'schwerin': ['53.625706, 11.416855', '15', 'flowered.html'],
    'standalone_schwerin': ['53.625706, 11.416855', '15', 'standalone.html'],}

  def render_to_response(self, project_name):
          
    # Choose a template based on the project name
    if project_name not in BaseRequestHandler._PROJECT_TYPES:
      project_name = 'world'
    project_data = BaseRequestHandler._PROJECT_TYPES[project_name]

    # Decode project data    
    location = project_data[0]
    zoom = project_data[1]
    template_file = project_data[2]
      
    # Read location data or use default value
    if self.request.get('ll') == '':
      initial_location = location
      initial_latitude, _, initial_longitude = initial_location.partition(",")
    else:
      initial_location = self.request.get('ll').lower()      
      initial_latitude, _, initial_longitude = initial_location.partition(",")
     
    # Read zoom level or use default value
    if self.request.get('z') == '':
      initial_zoom = zoom
    else:
      initial_zoom = self.request.get('z').lower()      

    # javascript:void(prompt('',gApplication.getMap().getCenter()))
      
    template_data = {}
        
    # Assembly template data
    template_data = {
      'project_id': project_name,
      'initial_latitude': initial_latitude,
      'initial_longitude': initial_longitude,
      'initial_zoom': initial_zoom,
      'current_version_id' : self.version(),
    }

    # Apply data to site templates
    template_path = os.path.join(os.path.dirname(__file__), 'templates', template_file)
    self.response.headers['Content-Type'] = 'text/html'
    self.response.out.write(template.render(template_path, template_data))
    
  def version(self):     
    current_version = os.getenv('CURRENT_VERSION_ID')
    version = string.split(current_version, '.') 
    
    if len(version) >= 2:
        return string.lower(version[0])
    else:
        return 'n/a'


class SchwerinHandler(BaseRequestHandler):
  """Handles requests to /schwerin
  
  WorldHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):
    self.render_to_response('schwerin')


class StandaloneSchwerinHandler(BaseRequestHandler):
  """Handles requests to /schwerin/standalone
  
  WorldHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):
    self.render_to_response('standalone_schwerin')
    

class WorldHandler(BaseRequestHandler):
  """Handles requests to /world
  
  WorldHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):
    # self.render_to_response('52.523405, 13.4114', 15, 'flowered.html')
    self.render_to_response('world')


class StandaloneWorldHandler(BaseRequestHandler):
  """Handles requests to /world/standalone
  
  WorldHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):
    self.render_to_response('standalone_world')


class RedirectHandler(webapp.RequestHandler):
  """Handles requests to /

  RedirectHandler handles requests for the server root, presenting the main user
  interface for Flowered and redirects the user to the appropiate sub project
  """

  def get(self):
    self.redirect('/world')


def main():
  # logging.getLogger().setLevel(logging.DEBUG)   
  application = webapp.WSGIApplication([
    ('/schwerin/standalone.*', StandaloneSchwerinHandler),
    ('/schwerin.*', SchwerinHandler),
    ('/world/standalone.*', StandaloneWorldHandler),
    ('/world.*', WorldHandler),
    ('/.*', RedirectHandler)
    ], debug = _DEBUG)
  run_wsgi_app(application)
  
if __name__ == '__main__':
  main()
  