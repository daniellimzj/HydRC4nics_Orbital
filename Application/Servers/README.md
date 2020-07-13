# Application Servers

## Config module

Use environment variables

```bash
export BOT_TOKEN=REPLACE
export FLASK_SECRET=REPLACE
export DB_URL=REPLACE
```

## DB module

Functions to access database through REST API

```python
# Returns string
getOperatorToken(expire)

# Returns all sensors without readings in sensor response format, use for startup
getAllSensors()

# Returns all actuators without commands in actuator response format, use for startup
getAllActuators()

# If using readingId, returns in reading response format
# If using other filters, returns in sensor response format
getReadings(readingId=None, sensorId=None, latest=None, start=None, end=None)

# If using commandId, returns in command response format
# If using other filters, returns in actuator response format
getCommands(commandId=None, actuatorId=None, latest=None, start=None, end=None, active=False)

# Returns sensor response
addSensor(position, sensorType, token)

# Returns actuator response
addActuator(position, actuatorType, token)

# Returns command response
addCommand(actuatorId, value, units, issuer, purpose, executeDate, token, repeat=0)

# Returns sensor response
updateSensor(sensorId, position, sensorType, token)

# Returns actuator response
updateActuator(actuatorId, position, actuatorType, token)

# Call this without repeat parameter to stop repeating command, returns command response
updateCommand(actuatorId, commandId, value, units, issuer, purpose, issueDate, executeDate, token, repeat=0)

# Returns sensor response
deleteSensor(sensorId, token)

# Returns actuator response
deleteActuator(actuatorId, token)

# Returns command response
deleteCommand(commandId, token)

# Returns register response
register(email, name, password)

# Returns login response
login(email, password)

# Returns register response
getUsers(token)
```

### Response formats

Sensor Response:

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "position": "string",
    "type": "string",
    "readings": [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "value": {
                "value": 0,
                "units": "string",
                "timeStamp": "2020-05-20T15:36:26.828+08:00"
            }
        }
    ]
}
```

Reading Response:

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "value": 0,
    "units": "string",
    "timeStamp": "2020-05-20T15:41:06.102+08:00"
}
```

Actuator Response:

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "position": "string",
    "type": "string",
    "commands": [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "value": {
                "value": 0,
                "units": "string",
                "issuer": "string",
                "purpose": "string",
                "issueDate": "2020-05-20T15:43:34.052+08:00",
                "executeDate": "2020-05-20T15:43:34.052+08:00",
                "repeat": 0,
                "completed": false
            }
        }
    ]
}
```

Command Response:

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "value": 0,
    "units": "string",
    "issuer": "string",
    "purpose": "string",
    "issueDate": "2020-05-20T15:50:11.590+08:00",
    "executeDate": "2020-05-20T15:50:11.590+08:00",
    "repeat": 0,
    "completed": false
}
```

Register Response:

```json
{
    "id": "e607a2b5-a5cd-4940-a039-49a5242213af",
    "name": "string",
    "email": "user@example.com",
    "claims": [
            {
                "issuer": "LOCAL AUTHORITY",
                "originalIssuer": "LOCAL AUTHORITY",
                "properties": {},
                "subject": null,
                "type": "org",
                "value": "none",
                "valueType": "http://www.w3.org/2001/XMLSchema#string"
            }
        ]
    }
```

Login Response:

```json
{
    "id": "e607a2b5-a5cd-4940-a039-49a5242213af",
    "token": "JWT",
    "name": "string"
}
```
