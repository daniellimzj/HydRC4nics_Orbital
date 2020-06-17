# Copyright 2017 Amazon.com, Inc. or its affiliates. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except
# in compliance with the License. A copy of the License is located at
#
# https://aws.amazon.com/apache-2-0/
#
# or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
# specific language governing permissions and limitations under the License.
"Demo Flask application"
import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template

from flask_bootstrap import Bootstrap

from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired

from wtforms import StringField, IntegerField, RadioField, FormField, DateTimeField
from wtforms.validators import DataRequired

from .. import config
from .. import db

from . import forms as f

import dash
from . import dashboard

application = Flask(__name__)
application.secret_key = config.FLASK_SECRET
Bootstrap(application)


@application.route("/", methods=('GET', 'POST'))
def home():
    """Homepage route"""
    all_labels = ["No labels yet"]

    #####
    # s3 getting a list of photos in the bucket
    #####
    """
    s3_client = boto3.client('s3')
    prefix = "photos/"
    response = s3_client.list_objects(
        Bucket=config.PHOTOS_BUCKET,
        Prefix=prefix
    )
    
    if 'Contents' in response and response['Contents']:
        photos = [s3_client.generate_presigned_url(
            'get_object',
            Params={'Bucket': config.PHOTOS_BUCKET, 'Key': content['Key']}
            ) for content in response['Contents']]
            """
    photos = []


    form = f.PhotoForm()
    url = None
    """
    if form.validate_on_submit():
        image_bytes = util.resize_image(form.photo.data, (300, 300))
        if image_bytes:
            #######
            # s3 excercise - save the file to a bucket
            #######
            key = prefix + util.random_hex_bytes(8) + '.png'
            s3_client.put_object(
                Bucket=config.PHOTOS_BUCKET,
                Key=key,
                Body=image_bytes,
                ContentType='image/png'
            )
            # http://boto3.readthedocs.io/en/latest/guide/s3.html#generating-presigned-urls
            url = s3_client.generate_presigned_url(
                'get_object',
                Params={'Bucket': config.PHOTOS_BUCKET, 'Key': key})
                """

    return render_template_string("""
            {% extends "main.html" %}
            {% block content %}
            <h4>Upload Photo</h4>
            <form method="POST" enctype="multipart/form-data" action="{{ url_for('home') }}">
                {{ form.csrf_token }}
                  <div class="control-group">
                   <label class="control-label">Photo</label>
                    {{ form.photo() }}
                  </div>

                    &nbsp;
                   <div class="control-group">
                    <div class="controls">
                        <input class="btn btn-primary" type="submit" value="Upload">
                    </div>
                  </div>
            </form>

            {% if url %}
            <hr/>
            <h3>Uploaded!</h3>
            <img src="{{url}}" /><br/>
            {% for label in all_labels %}
            <span class="label label-info">{{label}}</span>
            {% endfor %}
            {% endif %}
            
            {% if photos %}
            <hr/>
            <h4>Photos</h4>
            {% for photo in photos %}
                <img width="150" src="{{photo}}" />
            {% endfor %}
            {% endif %}

            {% endblock %}
                """, form=form, url=url, photos=photos, all_labels=all_labels)


@application.route("/get-data", methods = ("GET", "POST"))
def getData():
    numRecent = 0
    start = None
    end = None
    sensorFlag = False

    sensorForm = f.SensorForm()

    readingsList = []

    if sensorForm.validate_on_submit():

        if sensorForm.selectSensor.data == "0":
            sensorId = None
            sensorFlag = False

        else:
            sensorId = sensorForm.selectSensor.data
            sensorFlag = True


        numRecent = sensorForm.selectReadings.recent.data if sensorForm.selectReadings.recent.data else 0
        start = sensorForm.selectReadings.start.data if sensorForm.selectReadings.start.data else None
        end = sensorForm.selectReadings.end.data if sensorForm.selectReadings.end.data else None

        if numRecent > 0:
            readingsList = db.getReadings(sensorId = sensorId, latest = int(numRecent))
            print(readingsList)
        
        elif (start and end):
            readingsList = db.getReadings(sensorId = sensorId, start = start, end = end)
            print(readingsList)

        else:
            readingsList = []

    else:
        readingsList = []

    return render_template("get-data.html", readingsList = readingsList, sensorForm = sensorForm, sensorFlag = sensorFlag)


@application.route("/view-commands", methods = ("GET", "POST"))
def viewCommands():

    numRecent = 0
    start = None
    end = None
    activeFlag = False
    commandsFlag = False

    commandsForm = f.CommandsForm()

    commandsList = []

    if commandsForm.validate_on_submit():

        if commandsForm.selectActuator.data == "0":
            actuatorId = None
            actuatorFlag = False

        else:
            actuatorId = sensorForm.selectActuator.data
            actuatorFlag = True

        activeFlag = True if commandsForm.selectCommandsType.data =='1' else False
        
        numRecent = commandsForm.selectCommands.recent.data if commandsForm.selectCommands.recent.data else 0
        start = commandsForm.selectCommands.start.data if commandsForm.selectCommands.start.data else None
        end = commandsForm.selectCommands.end.data if commandsForm.selectCommands.end.data else None

        if numRecent > 0:
            commandsList = db.getCommands(actuatorId = actuatorId, latest = int(numRecent), active = activeFlag)
            print(commandsList)
        
        elif (start and end):
            commandsList = db.getCommands(actuatorId = actuatorId, start = start, end = end, active = activeFlag)
            print(commandsList)

        else:
            commandsList = []

    else:
        commandsList = []

    return render_template("view-commands.html", commandsList = commandsList, commandsForm = commandsForm, commandsFlag = commandsFlag)

    

    return render_template("view-commands.html")


@application.route("/send-commands")
def sendCommands():
    return render_template("send-commands.html")  

app = dash.Dash(
        __name__,
        server=application,
        routes_pathname_prefix='/dash/'
    )

dashboard.startDashboard(app)

@application.route("/readings")
def readingsPage():
    return render_template_string("""
            {% extends "main.html" %}
            {% block content %}
            <iframe src="http://localhost:8080/dash" width=800 height=600>
            {% endblock %}""")


if __name__ == "__main__":
    # http://flask.pocoo.org/docs/0.12/errorhandling/#working-with-debuggers
    # https://docs.aws.amazon.com/cloud9/latest/user-guide/app-preview.html
    use_c9_debugger = False
    application.run(use_debugger=not use_c9_debugger, debug=True,
                    use_reloader=not use_c9_debugger, host='0.0.0.0', port=8080)
