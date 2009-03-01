
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

"""The main Geochat application.

Contains the MainHandler, which handles root requests to the server, along
with several other template-driven pages that don't have any significant DB
interaction.

  MainHandler: Handles requests to /
  HelpHandler: Handles requests to /help
"""

import datetime
import logging
import os
import time

from google.appengine.api import users
from google.appengine.ext import db
from google.appengine.ext import webapp
from google.appengine.ext.webapp import template
from google.appengine.ext.webapp.util import run_wsgi_app

import datamodel
import json


class MainHandler(webapp.RequestHandler):
  
  """Handles requests to /
  
  MainHandler handles requests for the server root, presenting the main user
  interface for Geochat. It relies on the geochat.html template, with most
  of the heavy lifting occuring client-side through JavaScript linked there.
  """

  def get(self):

    template_data = {}
    
    template_data = {
      'auth_url': users.CreateLoginURL(self.request.uri),
      'auth_text': 'Sign in',
      'user_email': '',
      'user_nickname': '',
      'initial_latitude': 37.4221,
      'initial_longitude': -122.0837,
    }

    template_path = os.path.join(os.path.dirname(__file__), 'flowered.html')
    self.response.headers['Content-Type'] = 'text/html'
    self.response.out.write(template.render(template_path, template_data)) 
    
class HelpHandler(webapp.RequestHandler):
  
  """Handles requests to /help
  
  HelpHandler handles requests to /help, serving up the template
  found in help.html.
  """
  
  def get(self):

    template_data = {}

    user = users.get_current_user()
    if user:
      template_data = {
        'auth_url': users.CreateLogoutURL(self.request.uri),
        'auth_text': 'Sign out',
        'user_email': user.email(),
      }
    else:
      template_data = {
        'auth_url': users.CreateLoginURL(self.request.uri),
        'auth_text': 'Sign in',
        'user_email': '',
      }
        
    template_path = os.path.join(os.path.dirname(__file__), 'help.html')
    self.response.headers['Content-Type'] = 'text/html'
    self.response.out.write(template.render(template_path, template_data))


if __name__ == '__main__':
  application = webapp.WSGIApplication([('/', MainHandler),
                                        ('/help', HelpHandler),], debug = True)

  run_wsgi_app(application)


