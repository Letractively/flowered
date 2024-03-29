
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

from google.appengine.ext import db

"""Database models used in the Flowered application.

"""

class Mark(db.Model):
  timestamp = db.DateTimeProperty(auto_now_add = True)
  geopt = db.GeoPtProperty()
  type = db.StringProperty()
  project = db.StringProperty()

class Service(db.Model):
  name = db.StringProperty()
  user = db.StringProperty()
  password = db.StringProperty()
  active = db.BooleanProperty()
