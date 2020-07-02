import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template, flash, Response, session

from flask_bootstrap import Bootstrap

from flask_session import Session

from .. import db
from .. import config

from . import forms as f

from . import dashboard

application = Flask(__name__)
application.secret_key = config.FLASK_SECRET
Bootstrap(application)

application.config["SESSION_PERMANENT"] = False
application.config["SESSION_TYPE"] = "filesystem"
Session(application)

############################################

@application.route("/", methods=('GET', 'POST'))
def home():
    session["user"] = {}
    login = bool(session["user"])

    return render_template('index.html', login = login)

############################################

@application.route("/login", methods=("GET", "POST"))
def login():

    login = bool(session["user"])
    loginForm = f.LoginForm()
    
    return render_template('login.html', login = login, loginForm = loginForm)


@application.route("/signup", methods=("GET", "POST"))
def signup():

    login = bool(session["user"])
    signupForm = f.SignupForm()
    
    return render_template('signup.html', login = login, signupForm = signupForm)

@application.route("/logout", methods=("GET", "POST"))
def logout():
    return render_template('index.html', login = login)


############################################

@application.route("/get-data", methods = ("GET", "POST"))
def getData():
    numRecent = 0
    start = None
    end = None
    sensorFlag = False

    sensorForm = f.SensorForm()
        
    sensorsList = db.getAllSensors()
    choices = [(100, "All Sensors")]
    for sensor in sensorsList:
        choices.append((sensor['id'], f"{sensor['type']} {sensor['position']}"))

    sensorForm.selectSensor.choices = choices

    readingsList = []

    if sensorForm.validate_on_submit():

        if sensorForm.selectSensor.data == "100":
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

############################################

@application.route("/view-commands", methods = ("GET", "POST"))
def viewCommands():

    numRecent = 0
    start = None
    end = None

    activeFlag = False
    commandsFlag = False
    actuatorFlag = False

    commandsForm = f.CommandsForm()
    
    actuatorsList = db.getAllActuators()
    choices = [(100, "All Actuators")]
    for actuator in actuatorsList:
        choices.append((actuator['id'], f"{actuator['type']} {actuator['position']}"))

    commandsForm.selectActuator.choices = choices

    commandsList = []

    if commandsForm.validate_on_submit():

        if commandsForm.selectActuator.data == "100":
            actuatorId = None
            actuatorFlag = False

        else:
            actuatorId = commandsForm.selectActuator.data
            actuatorFlag = True

        activeFlag = True if commandsForm.selectCommandsType.data =='1' else False
        
        numRecent = commandsForm.selectCommands.recent.data if commandsForm.selectCommands.recent.data else 0
        start = commandsForm.selectCommands.start.data if commandsForm.selectCommands.start.data else None
        end = commandsForm.selectCommands.end.data if commandsForm.selectCommands.end.data else None

        commandsList = db.getCommands(actuatorId = actuatorId, latest = int(numRecent), start = start, end = end, active = activeFlag)
        print(commandsList, end = '\n\n\n')

    else:
        commandsList = []

    return render_template("view-commands.html", commandsList = commandsList, commandsForm = commandsForm, commandsFlag = commandsFlag, actuatorFlag = actuatorFlag)

############################################

@application.route("/send-commands", methods = ("GET", "POST"))
def sendCommands():

    success = None
    commandForm = f.CommandForm()

    actuatorsList = db.getAllActuators()
    commandForm.selectActuator.choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    if commandForm.validate_on_submit():
        command = db.addCommand(actuatorId = commandForm.selectActuator.data, value = commandForm.value.data,
                             units = commandForm.units.data, issuer = commandForm.issuer.data,
                             purpose = commandForm.purpose.data, executeDate = commandForm.executeDate.data,
                             repeat = commandForm.repeat.data)
        success = "Command Sent Successfully!"
    
    else:
        success = None

    return render_template("send-commands.html", commandForm = commandForm, success = success)  

############################################

@application.route("/add-actuators", methods = ("GET", "POST"))
def addActuators():

    addForm = f.AddForm()

    if addForm.validate_on_submit():
        actuator = db.addActuator(position = addForm.position.data, actuatorType = addForm.type.data)

    return render_template("add-actuators.html", addForm = addForm)

############################################

@application.route("/add-sensors", methods = ("GET", "POST"))
def addSensors():

    addForm = f.AddForm()

    if addForm.validate_on_submit():
        actuator = db.addSensor(position = addForm.position.data, sensorType = addForm.type.data)

    return render_template("add-sensors.html", addForm = addForm)

############################################

@application.route("/update-command/<string:actuatorId>:<string:commandId>", methods = ("GET", "POST"))
def updateCommand(actuatorId, commandId):

    command = db.getCommands(commandId = commandId)
    actuator = db.getCommands(actuatorId = actuatorId)

    updateForm = f.UpdateCommandForm()

    if updateForm.validate_on_submit():
        print(updateForm.value.data, end='\n\n\n')
        newCommand = db.updateCommand(actuatorId = actuatorId, commandId = commandId,
                                   value = updateForm.value.data, units = command['units'],
                                   issuer = updateForm.issuer.data, purpose = updateForm.purpose.data,
                                   issueDate = datetime.datetime.now(), executeDate = updateForm.executeDate.data,
                                   repeat = updateForm.repeat.data)

    return render_template("update-command.html", actuatorId = actuatorId, commandId = commandId, updateForm = updateForm, command = command, actuator = actuator)
    
############################################

@application.route("/update-actuators", methods = ("GET", "POST"))
def updateActuators():

    updateForm = f.UpdateActuatorForm()
    
    actuatorsList = db.getAllActuators()
    updateForm.selectActuator.choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    if updateForm.validate_on_submit():
        actuator = db.updateActuator(actuatorId = updateForm.selectActuator.data,
                       position = updateForm.position.data,
                       actuatorType = updateForm.type.data)

    return render_template("update-actuators.html", updateForm = updateForm)

############################################

@application.route("/update-sensors", methods = ("GET", "POST"))
def updateSensors():

    updateForm = f.UpdateSensorForm()
    
    sensorsList = db.getAllSensors()
    updateForm.selectSensor.choices = [(sensor['id'], f"{sensor['type']} {sensor['position']}") for sensor in sensorsList]

    if updateForm.validate_on_submit():
        sensor = db.updateSensor(sensorId = updateForm.selectSensor.data,
                       position = updateForm.position.data,
                       sensorType = updateForm.type.data)

    return render_template("update-sensors.html", updateForm = updateForm)

dashboard.startDashboard(application)

@application.route("/readings")
def readingsPage():

    return render_template_string("""
            {% extends "main.html" %}
            {% block content %}
            <iframe src="/dash" style="height: 100vh; width: 100%; scrolling: no; frameborder: 0">
            {% endblock %}""")

@application.route("/downloads")
def downloadsPage():
    return render_template_string("""
        {% extends "main.html" %}
        {% block content %}
        <a href="{{ url_for('getCSV') }}">Click me to download latest.</a>
        {% endblock %}""")

@application.route("/getCSV")
def getCSV():
    # with open("outputs/Adjacency.csv") as fp:
    #     csv = fp.read()
    csv = '1,2,3\n4,5,6\n'
    return Response(
        csv,
        mimetype="text/csv",
        headers={"Content-disposition":
                 "attachment; filename=mydata.csv"})

if __name__ == "__main__":
    # http://flask.pocoo.org/docs/0.12/errorhandling/#working-with-debuggers
    # https://docs.aws.amazon.com/cloud9/latest/user-guide/app-preview.html
    use_c9_debugger = False
    application.run(use_debugger=not use_c9_debugger, debug=True,
                    use_reloader=not use_c9_debugger, host='0.0.0.0', port=8080)


