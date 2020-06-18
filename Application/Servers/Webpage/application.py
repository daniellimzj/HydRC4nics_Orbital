import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template

from flask_bootstrap import Bootstrap


from .. import db
from .. import config

from . import forms as f

import dash
from . import dashboard

application = Flask(__name__)
application.secret_key = config.FLASK_SECRET
Bootstrap(application)

@application.route("/", methods=('GET', 'POST'))
def home():
    return render_template('main.html')


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
        
        elif (start and end):
            readingsList = db.getReadings(sensorId = sensorId, start = start, end = end)

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
        
        elif (start and end):
            commandsList = db.getCommands(actuatorId = actuatorId, start = start, end = end, active = activeFlag)

        else:
            commandsList = []

    else:
        commandsList = []

    return render_template("view-commands.html", commandsList = commandsList, commandsForm = commandsForm, commandsFlag = commandsFlag)

    

    return render_template("view-commands.html")


@application.route("/send-commands", methods = ("GET", "POST"))
def sendCommands():

    success = None
    commandForm = f.CommandForm()

    if commandForm.validate_on_submit():
        command = db.addCommand(actuatorId = commandForm.selectActuator.data, value = commandForm.value.data,
                             units = commandForm.units.data, issuer = commandForm.issuer.data,
                             purpose = commandForm.purpose.data, executeDate = commandForm.executeDate.data,
                             repeat = commandForm.repeat.data)
        print(command)
        success = "Command Sent Successfully!"
    
    else:
        success = None

    return render_template("send-commands.html", commandForm = commandForm, success = success)  

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


