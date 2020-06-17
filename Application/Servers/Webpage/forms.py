# Sets up all the forms that will be used in application.py

import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template

from flask_bootstrap import Bootstrap

from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired

from wtforms import StringField, IntegerField, RadioField, FormField
from wtforms.fields.html5 import DateTimeLocalField
from wtforms.validators import DataRequired

from .. import config
from .. import db


# reusable form
class RangeForm(FlaskForm):
    recent = IntegerField('recent')
    start = DateTimeLocalField('start', format = '%Y-%m-%dT%H:%M')
    end = DateTimeLocalField('end', format = '%Y-%m-%dT%H:%M')

    def validate(self):
        return (self.recent or (self.startDay and self.startTime and self.endDay and self.endTime))


# to get data
class SensorForm(FlaskForm):

    sensorsList = db.getAllSensors()
    choices = [(0, "All Sensors")]
    for sensor in sensorsList:
        choices.append((sensor['id'], f"{sensor['type']} {sensor['position']}"))

    selectSensor = RadioField(choices = choices)
    selectReadings = FormField(RangeForm)

    def validate(self):
        return (self.selectSensor and self.selectReadings)

# to view commands
class CommandsForm(FlaskForm):

    actuatorsList = db.getAllActuators()
    choices = [(0, "All Actuators")]
    for actuator in actuatorsList:
        choices.append((actuator['id'], f"{actuator['type']} {actuator['position']}"))
    
    selectActuator = RadioField(choices = choices)
    selectCommandsType = RadioField(choices = [(0, "All Commands"), (1, "Only Active Commands")])
    selectCommands = FormField(RangeForm)

    def validate(self):
        return (self.selectActuator and self.selectCommandsType and self.selectCommands)

# to send commands
class CommandForm(FlaskForm):
    actuatorsList = db.getAllActuators()
    choices = [(actuator['id'], f"{actuator['type']} {actuator['position']}") for actuator in actuatorsList]

    selectActuator = RadioField(choices = choices)
    value = IntegerField('value', validators = [DataRequired()])
    units = StringField('units', validators = [DataRequired()])
    issuer = StringField('issuer', validators = [DataRequired()])
    purpose = StringField('purpose', validators = [DataRequired()])
    executeDate = DateTimeLocalField('executeDate', format = '%Y-%m-%dT%H:%M')
    repeat = IntegerField('repeat', validators = [DataRequired()])