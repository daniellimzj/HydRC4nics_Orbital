# Sets up all the forms that will be used in application.py

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

class PhotoForm(FlaskForm):
    """flask_wtf form class the file upload"""
    photo = FileField('image', validators=[
        FileRequired()
    ])

# reusable form
class RangeForm(FlaskForm):
    recent = IntegerField('recent')
    start = DateTimeField('start', id = 'datepick')
    end = DateTimeField('end', format =  '%y-%m-%d %H:%M')

    def validate(self):
        return (self.recent or (self.start and self.end))


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