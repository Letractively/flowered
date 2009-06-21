
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

  MainHandler: Handles requests to /
  StandaloneHandler: Handles requests to /standalone
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

class MainHandler(webapp.RequestHandler):  
  """Handles requests to /
  
  MainHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self): 
    template_data = {}
    
    template_data = {
      'project_id': 'schwerin',
      'initial_latitude': 53.625706,
      'initial_longitude': 11.416855,
      'initial_zoom': 15,
      'current_version_id' : self.version()
    }

    template_path = os.path.join(os.path.dirname(__file__), 'templates', 'flowered.html')
    self.response.headers['Content-Type'] = 'text/html'
    self.response.out.write(template.render(template_path, template_data))
 
  def version(self):     
    current_version = os.getenv('CURRENT_VERSION_ID')
    version = string.split(current_version, '.') 
    
    if len(version) >= 2:
        return string.lower(version[0])
    else:
        return 'n/a'
        

class StandaloneHandler(webapp.RequestHandler):
  """Handles requests to /standalone
  
  MainHandler handles requests for the server root, presenting the main user
  interface for Flowered. It relies on the flowered.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):
    template_data = {}
        
    template_data = {
      'project_id': 'schwerin',
      'initial_latitude': 53.625706,
      'initial_longitude': 11.416855,
      'initial_zoom': 15,
    }

    template_path = os.path.join(os.path.dirname(__file__), 'templates', 'standalone.html')
    self.response.headers['Content-Type'] = 'text/html'
    self.response.out.write(template.render(template_path, template_data)) 

class RedirectHandler(webapp.RequestHandler):
  """Handles requests to /

  RedirectHandler handles requests for the server root, presenting the main user
  interface for Flowered and redirects the user to the appropiate sub project
  """

  def get(self):   
    self.redirect('/schwerin')


def main():
  # logging.getLogger().setLevel(logging.DEBUG)   
  application = webapp.WSGIApplication([
    ('/schwerin/standalone.*', StandaloneHandler),
    ('/schwerin.*', MainHandler),
    ('/.*', RedirectHandler)
    ], debug = True)
  run_wsgi_app(application)
  
if __name__ == '__main__':
  main()
  