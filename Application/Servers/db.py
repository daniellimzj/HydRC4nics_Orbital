"""API calls to database"""
import requests
from . import config
from datetime import datetime

import base64
import hmac
import hashlib
import json

# Returns string
def getOperatorToken(expire):
    header = b'{"alg":"HS256","typ":"JWT"}'

    payload = b'{"org":"nus","job":"operator","nbf":%d,"exp":%d,"iat":%d}' % (int(datetime.now().timestamp()), int(datetime.now().timestamp()) + expire, int(datetime.now().timestamp()))

    headerEncoded = base64.urlsafe_b64encode(header).rstrip(b"=").decode('utf-8')
    payloadEncoded = base64.urlsafe_b64encode(payload).rstrip(b"=").decode('utf-8')

    data = f'{headerEncoded}.{payloadEncoded}'
    signature = base64.urlsafe_b64encode(hmac.new(config.JWT_SECRET.encode('utf-8'), data.encode('utf-8'), hashlib.sha256).digest())
    sig = signature.rstrip(b"=").decode('utf-8')

    return f'{data}.{sig}'

def _url(path):
    return f'{config.DB_URL}:5020{path}'

def _identity(path):
    return f'{config.DB_URL}:8090{path}'

# Returns all sensors without readings in sensor response format, use for startup
def getAllSensors():
    r = requests.get(_url('/Sensor/latest/0'))
    
    if r.status_code is not 200:
        print(f'GET /Sensor/latest/0 {r.status_code}')

    return r.json()

# Returns all actuators without commands in actuator response format, use for startup
def getAllActuators():
    r = requests.get(_url('/Actuator/latest/0'))
    
    if r.status_code is not 200:
        print(f'GET /Actuator/latest/0 {r.status_code}')

    return r.json()

# If using readingId, returns in reading response format
# If using other filters, returns in sensor response format
def getReadings(readingId=None, sensorId=None, latest=None, start=None, end=None):
    if readingId is None:
        path = '/Sensor'

        if sensorId is not None:
            path = f'{path}/{sensorId}'

        if latest is None:
            if start is not None:
                path = f'{path}/range/{start.strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
                if end is None:
                    path = f'{path}/{datetime.now().strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
                else:
                    path = f'{path}/{end.strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
        else:
            path = f'{path}/latest/{latest}'
    else:
        path = f'/Reading/{readingId}'

    r = requests.get(_url(path))
    
    if r.status_code is not 200:
        print(f'GET {path} {r.status_code}')

    return r.json()

# If using commandId, returns in command response format
# If using other filters, returns in actuator response format
def getCommands(commandId=None, actuatorId=None, latest=None, start=None, end=None, active=False):
    if commandId is None:
        path = '/Actuator'

        if actuatorId is not None:
            path = f'{path}/{actuatorId}'
            
        if active is False:
            if latest is None:
                if start is not None:
                    path = f'{path}/range/{start.strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
                    if end is None:
                        path = f'{path}/{datetime.now().strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
                    else:
                        path = f'{path}/{end.strftime("%Y-%m-%dT%H:%M:%S+08:00")}'
            else:
                path = f'{path}/latest/{latest}'
        else:
            path = f'{path}/active'
    else:
        path = f'/Command/{commandId}'

    r = requests.get(_url(path))
    
    if r.status_code is not 200:
        print(f'GET {path} {r.status_code}')

    return r.json()

# Returns sensor response
def addSensor(position, sensorType):
    payload = {'position': position, 'type': sensorType}

    r = requests.post(_url('/Sensor'), json=payload)

    return r.json()

# Returns actuator response
def addActuator(position, actuatorType):
    payload = {'position': position, 'type': actuatorType}

    r = requests.post(_url('/Actuator'), json=payload)

    return r.json()

# Returns command response
def addCommand(actuatorId, value, units, issuer, purpose, executeDate, repeat=0):
    payload = {'value': value,
                'units': units,
                'issuer': issuer,
                'purpose': purpose,
                'issueDate': datetime.now().strftime("%Y-%m-%dT%H:%M:%S+08:00"),
                'executeDate': executeDate.strftime("%Y-%m-%dT%H:%M:%S+08:00"),
                'repeat': repeat}

    r = requests.post(_url(f'/Command/{actuatorId}'), json=payload)

    return r.json()

# Returns sensor response
def updateSensor(sensorId, position, sensorType):
    payload = {'position': position, 'type': sensorType}

    r = requests.put(_url(f'/Sensor/{sensorId}'), json=payload)

    return r.json()

# Returns actuator response
def updateActuator(actuatorId, position, actuatorType):
    payload = {'position': position, 'type': actuatorType}

    r = requests.put(_url(f'/Actuator/{actuatorId}'), json=payload)

    return r.json()

# Call this without repeat parameter to stop repeating command, returns command response
def updateCommand(actuatorId, commandId, value, units, issuer, purpose, issueDate, executeDate, repeat=0):
    payload = {'value': value,
                'units': units,
                'issuer': issuer,
                'purpose': purpose,
                'issueDate': issueDate.strftime("%Y-%m-%dT%H:%M:%S+08:00"),
                'executeDate': executeDate.strftime("%Y-%m-%dT%H:%M:%S+08:00"),
                'repeat': repeat}

    r = requests.put(_url(f'/Command/{actuatorId}/{commandId}'), json=payload)

    return r.json()

# Returns sensor response
def deleteSensor(sensorId):
    r = requests.delete(_url(f'/Sensor/{sensorId}'))

    return r.json()

# Returns actuator response
def deleteActuator(actuatorId):
    r = requests.delete(_url(f'/Actuator/{actuatorId}'))

    return r.json()

# Returns command response
def deleteCommand(commandId):
    r = requests.delete(_url(f'/Command/{commandId}'))

    return r.json()

# Returns register response
def register(email, name, password):
    payload = {'email': email,
                'name': name,
                'password': password}

    r = requests.post(_identity('/Account/register'), json=payload)

    return r.json()

# Returns login response
def login(email, password):
    payload = {'email': email,
                'password': password}

    r = requests.post(_identity('/Account/login'), json=payload)

    return r.json()

def getUsers(token):
    headers={'Authorization': f'Bearer {token}'}

    r = requests.get(_identity('/Account/users'), headers=headers)

    if r.status_code is not 200:
        print(f'GET /Account/users {r.status_code}')

    return r.json()