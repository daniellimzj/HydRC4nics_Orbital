# Sets up all the forms that will be used in application.py

import sys
import requests
import datetime

from flask import Flask, render_template_string, render_template, flash

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

    selectSensor = SelectField("Select a sensor:", validators = [DataRequired()])
    selectReadings = FormField(RangeForm)

    def validate(self):
        if self.selectReadings.recent.data and (self.selectReadings.start.data or self.selectReadings.end.data):
            flash("Select either recent readings or a range, not both!")
            return False
        return True
        
        

# to view commands
class CommandsForm(FlaskForm):
    
    selectActuator = SelectField("Select an Actuator:")
    selectCommandsType = SelectField("Select the type of commands:", choices = [(1, "Active Commands"), (2, "Recent Commands"), (3, "Time Range")])
    selectCommands = FormField(RangeForm)

    def validate(self):

        if self.selectCommandsType.data == "1" and (self.selectCommands.recent.data or self.selectCommands.start.data or self.selectCommands.end.data):
            flash("If viewing active commands, do not fill in recent and time range fields!")
            return False

        if self.selectCommandsType.data == "2" and (not self.selectCommands.recent.data or self.selectCommands.start.data or self.selectCommands.end.data):
            flash("Select either recent readings or a range, not both!")
            return False
        
        if self.selectCommandsType.data == "3" and (self.selectCommands.recent.data or not self.selectCommands.start.data or not self.selectCommands.end.data):
            flash("Select either recent readings or a range, not both!")
            return False

        return True



# to send commands
class CommandForm(FlaskForm):

    selectActuator = SelectField("Select an Actuator:")
    value = IntegerField('Value:', validators = [DataRequired()])
    units = StringField('Units:', validators = [DataRequired()])
    issuer = StringField('Issuer:', validators = [DataRequired()])
    purpose = StringField('Purpose:', validators = [DataRequired()])
    executeDate = DateTimeLocalField('Date to execute:', format = '%Y-%m-%dT%H:%M')
    repeat = IntegerField('Minutes between repetitions:')



# to update commands
class UpdateCommandForm(FlaskForm):
    value = IntegerField('New value:', validators = [DataRequired()])
    issuer = StringField('New issuer:', validators = [DataRequired()])
    purpose = StringField('New purpose:', validators = [DataRequired()])
    executeDate = DateTimeLocalField('New date to execute:', format = '%Y-%m-%dT%H:%M', validators = [DataRequired()])
    repeat = IntegerField('New minutes between repetitions:', validators = [DataRequired()])



# to add an actuator or a sensor
class AddForm(FlaskForm):
    position = StringField('Position:', validators = [DataRequired()])
    type = StringField('Type:', validators = [DataRequired()])



# to update an actuator
class UpdateActuatorForm(FlaskForm):

    selectActuator = SelectField("Select an Actuator:")
    position = StringField('New Position:', validators = [DataRequired()])
    type = StringField('New Type:', validators = [DataRequired()])



# to update a sensor
class UpdateSensorForm(FlaskForm):
    
    selectSensor = SelectField("Select an Sensor:")
    position = StringField('New Position:', validators = [DataRequired()])
    type = StringField('New Type:', validators = [DataRequired()])