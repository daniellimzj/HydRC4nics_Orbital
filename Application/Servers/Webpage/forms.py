# Sets up all the forms that will be used in application.py

import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template

from flask_bootstrap import Bootstrap

from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired

from wtforms import StringField, IntegerField, RadioField, FormField, SelectField
from wtforms.fields.html5 import DateTimeLocalField
from wtforms.validators import DataRequired, Optional, ValidationError

from .. import config
from .. import db


# reusable form
class RangeForm(FlaskForm):
    recent = IntegerField('Most recent:', validators = [Optional()])
    start = DateTimeLocalField('Or choose a range from:', format = '%Y-%m-%dT%H:%M', validators = [Optional()])
    end = DateTimeLocalField('To:', format = '%Y-%m-%dT%H:%M', validators = [Optional()])
        


# to get data
class SensorForm(FlaskForm):
    sensorsList = db.getAllSensors()
    choices = [(100, "All Sensors")]
    for sensor in sensorsList:
        choices.append((sensor['id'], f"{sensor['type']} {sensor['position']}"))

    selectSensor = SelectField("Select a sensor:", choices = choices, validators = [DataRequired()])
    selectReadings = FormField(RangeForm)

    def validate(self):
        if self.selectReadings.recent.data and (self.selectReadings.start.data or self.selectReadings.end.data):
            print("orh horh")
            return False
        return True
        
        

# to view commands
class CommandsForm(FlaskForm):

    actuatorsList = db.getAllActuators()
    choices = [(100, "All Actuators")]
    for actuator in actuatorsList:
        choices.append((actuator['id'], f"{actuator['type']} {actuator['position']}"))
    
    selectActuator = SelectField("Select an Actuator:", choices = choices)
    selectCommandsType = SelectField("Select the type of commands:", choices = [(0, "All Commands"), (1, "Only Active Commands")])
    selectCommands = FormField(RangeForm)

    def validate(self):
        return (self.selectActuator and self.selectCommandsType and self.selectCommands)

# to send commands
class CommandForm(FlaskForm):
    actuatorsList = db.getAllActuators()
    choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    selectActuator = SelectField("Select an Actuator:", choices = choices)
    value = IntegerField('Value:', validators = [DataRequired()])
    units = StringField('Units:', validators = [DataRequired()])
    issuer = StringField('Issuer:', validators = [DataRequired()])
    purpose = StringField('Purpose:', validators = [DataRequired()])
    executeDate = DateTimeLocalField('Date to execute:', format = '%Y-%m-%dT%H:%M')
    repeat = IntegerField('Number of times to repeat:')

# to update commands
class UpdateCommandForm(FlaskForm):
    value = IntegerField('New value:', validators = [DataRequired()])
    issuer = StringField('New issuer:', validators = [DataRequired()])
    purpose = StringField('New purpose:', validators = [DataRequired()])
    executeDate = DateTimeLocalField('New date to execute:', format = '%Y-%m-%dT%H:%M', validators = [DataRequired()])
    repeat = IntegerField('New number of times to repeat:', validators = [DataRequired()])

# to add an actuator or a sensor
class AddForm(FlaskForm):
    position = StringField('Position:', validators = [DataRequired()])
    type = StringField('Type:', validators = [DataRequired()])

# to update an actuator
class UpdateActuatorForm(FlaskForm):
    actuatorsList = db.getAllActuators()
    choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]
    
    selectActuator = SelectField("Select an Actuator:", choices = choices)
    position = StringField('New Position:', validators = [DataRequired()])
    type = StringField('New Type:', validators = [DataRequired()])

# to update a sensor
class UpdateSensorForm(FlaskForm):
    sensorsList = db.getAllSensors()
    choices = [(sensor['id'], f"{sensor['type']} {sensor['position']}") for sensor in sensorsList]
    
    selectSensor = SelectField("Select an Sensor:", choices = choices)
    position = StringField('New Position:', validators = [DataRequired()])
    type = StringField('New Type:', validators = [DataRequired()])