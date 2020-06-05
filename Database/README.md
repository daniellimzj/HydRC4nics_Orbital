# Database

This server uses EFCore with a 3-tier architecture. Database uses SQLite. The database will store sensor data polled from the microcontroller and commands to be sent to the microcontroller. Interfaces will be able to request information from and send instructions to the database through a REST API view layer.

The identity server uses JWT for claim-based athorization with a master user (detailed in ./Identity/master.txt) able to add and remove claims for registered users.

## Data Structure Summary

One sensor has many readings. One actuator has many commands. Sensors and actuators are labelled by their position in the structure.

## System Functions

### Monitoring

- Start and stop the serial port
- Set the serial data packet format
- Read asynchronously from the serial port
- Store readings in the database
- Access the latest reading from a sensor
- Access the all latest readings
- Access readings from a certain time interval
- Access all readings

### Controls

- Receive commands from users
- Send command through the serial port
- Store commands in the database
- Set a timer for a command
- Repeat commands
- See active commands

## JSON Formats

Sensor Request:

```json
{
    "position": "string",
    "type": "string"
}
```

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

Reading Request:

```json
{
    "value": 0,
    "units": "string",
    "timeStamp": "2020-05-20T15:37:46.071+08:00"
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

Serial Request:

```json
// Sequence of Sensors
[
  "3fa85f64-5717-4562-b3fc-2c963f66afa6"
]
```

Serial Response:

```json
// SerialPort object
```

Actuator Request:

```json
{
    "position": "string",
    "type": "string"
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
                "repeat": 0
            }
        }
    ]
}
```

Command Request:

```json
{
    "value": 0,
    "units": "string",
    "issuer": "string",
    "purpose": "string",
    "issueDate": "2020-05-20T15:50:11.590+08:00",
    "executeDate": "2020-05-20T15:50:11.590+08:00",
    "repeat": 0
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
    "repeat": 0
}
```
