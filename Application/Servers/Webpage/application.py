import sys
import requests
import datetime
import dateutil

from flask import Flask, render_template_string, render_template, flash, Response, session

from flask_bootstrap import Bootstrap

from flask_session import Session

from .. import db
from .. import config

from . import forms as f
from . import util
from . import dashboard
from . import getCSV

application = Flask(__name__)
application.secret_key = config.FLASK_SECRET
Bootstrap(application)

application.config["SESSION_PERMANENT"] = False
application.config["SESSION_TYPE"] = "filesystem"
Session(application)

dashboard.startDashboard(application)
getCSV.main(application)

############################################

@application.route("/", methods=('GET', 'POST'))
def home():
    if session.get("user") is None:
        session["user"] = {}
    session["login"] = bool(session["user"])

    return render_template('index.html', login = session["login"])

############################################

@application.route("/login", methods=("GET", "POST"))
def login():

    loginForm = f.LoginForm()
    loginFailed = False

    if loginForm.validate_on_submit():

        session["user"] = db.login(loginForm.email.data, loginForm.password.data)

        if session["user"] =="401":
            util.resetSession(session)
            loginFailed = True
            return render_template('login.html', login = session["login"], loginForm = loginForm, loginFailed = loginFailed)

        session["login"] = bool(session["user"])

        session["operator"] = False

        for claim in session["user"]["claims"]:
            if claim["value"] == "operator" or claim["value"] == "master":
                session["operator"] = True

        session["expiretime"] = datetime.datetime.now() + datetime.timedelta(minutes = 30)

        return render_template('index.html', login = session["login"])
    
    return render_template('login.html', login = session["login"], loginForm = loginForm)

############################################

@application.route("/signup", methods=("GET", "POST"))
def signup():

    signupForm = f.SignupForm()
    register = None

    if signupForm.validate_on_submit():

        register = db.register(signupForm.email.data, signupForm.name.data, signupForm.password.data)
    
    return render_template('signup.html', login = session["login"], signupForm = signupForm, register = register)

############################################

@application.route("/logout", methods=("GET", "POST"))
def logout():

    util.resetSession(session)
    global operator
    operator = False

    return render_template('logout.html', login = session["login"])

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

    return render_template("get-data.html", readingsList = readingsList, sensorForm = sensorForm,
                           sensorFlag = sensorFlag, login = session["login"])

############################################

@application.route("/view-commands", methods = ("GET", "POST"))
def viewCommands():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

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

    else:
        commandsList = []

    return render_template("view-commands.html", commandsList = commandsList, commandsForm = commandsForm,
                           commandsFlag = commandsFlag, actuatorFlag = actuatorFlag, login = session["login"])

############################################

@application.route("/send-commands", methods = ("GET", "POST"))
def sendCommands():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

    success = False
    commandForm = f.CommandForm()

    actuatorsList = db.getAllActuators()
    commandForm.selectActuator.choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    if commandForm.validate_on_submit():
        _ = db.addCommand(actuatorId = commandForm.selectActuator.data, value = commandForm.value.data,
                             units = commandForm.units.data, issuer = commandForm.issuer.data,
                             purpose = commandForm.purpose.data, executeDate = commandForm.executeDate.data,
                             token = session["user"]["token"], repeat = commandForm.repeat.data)
        success = True
    
    else:
        success = False

    return render_template("send-commands.html", commandForm = commandForm, success = success, login = session["login"])  

############################################

@application.route("/add-actuators", methods = ("GET", "POST"))
def addActuators():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

    actuator = None

    addForm = f.AddForm()

    if addForm.validate_on_submit():
        actuator = db.addActuator(position = addForm.position.data, actuatorType = addForm.type.data, token = session["user"]["token"])

    return render_template("add-actuators.html", addForm = addForm, login = session["login"], actuator = actuator)

############################################

@application.route("/add-sensors", methods = ("GET", "POST"))
def addSensors():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

    sensor = None

    addForm = f.AddForm()

    if addForm.validate_on_submit():

        sensor = db.addSensor(position = addForm.position.data, sensorType = addForm.type.data, token = session["user"]["token"])

    return render_template("add-sensors.html", addForm = addForm, login = session["login"], sensor = sensor)

############################################

@application.route("/update-command/<string:actuatorId>:<string:commandId>", methods = ("GET", "POST"))
def updateCommand(actuatorId, commandId):
    
    expired, page = util.checkExpiry(session)
    if expired:
        return page

    command = db.getCommands(commandId = commandId)
    actuator = db.getCommands(actuatorId = actuatorId)

    newCommand = None

    updateForm = f.UpdateCommandForm()

    if updateForm.validate_on_submit():
        newCommand = db.updateCommand(actuatorId = actuatorId, commandId = commandId,
                                   value = updateForm.value.data, units = command['units'],
                                   issuer = updateForm.issuer.data, purpose = updateForm.purpose.data,
                                   issueDate = datetime.datetime.now(), executeDate = updateForm.executeDate.data,
                                   token = session["user"]["token"], repeat = updateForm.repeat.data)

    return render_template("update-command.html", actuatorId = actuatorId, commandId = commandId, updateForm = updateForm,
                           command = command, actuator = actuator, login = session["login"], newCommand = newCommand)
    
############################################

@application.route("/update-actuators", methods = ("GET", "POST"))
def updateActuators():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

    updateForm = f.UpdateActuatorForm()
    
    actuatorsList = db.getAllActuators()
    updateForm.selectActuator.choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    actuator = None

    if updateForm.validate_on_submit():
        actuator = db.updateActuator(actuatorId = updateForm.selectActuator.data,
                       position = updateForm.position.data,
                       actuatorType = updateForm.type.data,
                       token = session["user"]["token"])

    return render_template("update-actuators.html", updateForm = updateForm, login = session["login"], actuator = actuator)

############################################

@application.route("/update-sensors", methods = ("GET", "POST"))
def updateSensors():

    expired, page = util.checkExpiry(session)
    if expired:
        return page

    updateForm = f.UpdateSensorForm()
    
    sensorsList = db.getAllSensors()
    updateForm.selectSensor.choices = [(sensor['id'], f"{sensor['type']} {sensor['position']}") for sensor in sensorsList]

    sensor = None

    if updateForm.validate_on_submit():
        sensor = db.updateSensor(sensorId = updateForm.selectSensor.data,
                       position = updateForm.position.data,
                       sensorType = updateForm.type.data,
                       token = session["user"]["token"])

    return render_template("update-sensors.html", updateForm = updateForm, login = session["login"], sensor = sensor)

############################################

@application.route("/readings")
def readingsPage():
    return render_template("dashboard.html", login = session["login"])

############################################

@application.route("/downloads")
def downloadsPage():
    return render_template("csv.html", login = session["login"])

############################################

@application.errorhandler(500)
def error(e):
    return render_template("error.html", login = session["login"])

############################################

@application.template_filter('datetimeformat')
def datetimeformat(value, format="%m/%d/%y at %I:%M:%S%p"):
    return dateutil.parser.parse(value).strftime(format)

############################################

if __name__ == "__main__":
    # http://flask.pocoo.org/docs/0.12/errorhandling/#working-with-debuggers
    application.run(use_debugger=True, debug=True,
                    use_reloader=True, host='0.0.0.0', port=8080)


