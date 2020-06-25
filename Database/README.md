# Database Server

This server uses EFCore with a 3-tier architecture. Database uses SQLite. The database will store sensor data polled from the microcontroller and commands to be sent to the microcontroller. Interfaces will be able to request information from and send instructions to the database through a REST API view layer.

The identity server uses JWT for claim-based athorization with a master user (detailed in ./Identity/master.txt) able to add and remove claims for registered users.

## Data Structure Summary

One sensor has many readings. One actuator has many commands. Sensors and actuators are labelled by their position in the structure.

## Automatic Monitoring and Controls

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

To stop repeating commands, set `"repeat": 0`.

### Identity

- Register
- Login
- Edit claims

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

Register Request:

```json
{
    "email": "user@example.com",
    "password": "string",
    "name": "string"
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

Login Request:

```json
{
    "email": "user@example.com",
    "password": "string"
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

Claim Request:

```json
{
    "email": "user@example.com",
    "type": "string",
    "value": "string"
}
```

## API Endpoints

| HTTP Method   | Claim Auth    | Endpoint
| ------------- | -----------   | ----------
| POST/GET      |               | /Sensor
| PUT/DELETE/GET|               | /Sensor/{id}
| GET           |               | /Sensor/latest/{num}
| GET           |               | /Sensor/range/{start}/{end}
| GET           |               | /Sensor/{id}/latest/{num}
| GET           |               | /Sensor/{id}/range/{start}/{end}
| GET           |               | /Reading
| POST          |               | /Reading/{sensorId}
| PUT/DELETE/GET|               | /Reading/{id}
| POST          |               | /Reading/serial/start/{com}
| GET           |               | /Reading/serial/stop
| POST/GET      |               | /Actuator
| PUT/DELETE/GET|               | /Actuator/{id}
| GET           |               | /Actuator/active
| GET           |               | /Actuator/latest/{num}
| GET           |               | /Actuator/range/{start}/{end}
| GET           |               | /Actuator/{id}/active
| GET           |               | /Actuator/{id}/latest/{num}
| GET           |               | /Actuator/{id}/range/{start}/{end}
| GET           |               | /Command
| POST          |               | /Command/{actuatorId}
| DELETE/GET    |               | /Command/{id}
| PUT           |               | /Command/{actuatorId}/{id}
| POST          |               | /Account/register
| POST          |               | /Account/login
| PUT           |               | /Account/update
| DELETE        | master        | /Account/delete/{email}
| POST          | master        | /Account/claim/add
| POST          | master        | /Account/claim/remove
| GET           | nus           | /Account/users
| GET           | nus           | /Account/users/{claimType}/{claimValue}

## References

### Serial Comms

<https://www.instructables.com/id/Serial-Port-Programming-With-NET/>
<https://stackoverflow.com/questions/1243070/how-to-read-and-write-from-the-serial-port>

### Entity Framework Core

<https://docs.microsoft.com/en-us/ef/core/>
<https://www.notion.so/The-Humble-Programmer-4fb5828ed2274c7888b20d98de6c8706>

### Filtering

<https://stackoverflow.com/questions/15378136/entity-framework-ordering-includes>

### Background services (Not used as it starts on startup)

<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio>

### Fire and forget

<https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices?view=aspnetcore-3.1>

```C#
[HttpGet("/fire-and-forget-3")]
public IActionResult GoodFireAndForget()
{
    string path = HttpContext.Request.Path;
    _ = Task.Run(async () =>
    {
        await Task.Delay(1000);

        Log(path);
    });

    return Accepted();
}
```

<https://stackoverflow.com/questions/51572637/access-dbcontext-service-from-background-task>

### Concurrent Dictionary for CancellationTokenSource

<https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=netcore-3.1>
<https://stackoverflow.com/questions/54713001/how-to-cancel-one-particular-task-out-of-n-number-of-tasks>
<https://stackoverflow.com/questions/55800600/persistent-in-memory-concurrent-dictionary-in-asp-net-core>

### Swagger with JWT

<https://stackoverflow.com/questions/43447688/setting-up-swagger-asp-net-core-using-the-authorization-headers-bearer>

### Master User

<https://stackoverflow.com/questions/43731437/how-to-make-a-single-admin-user-for-mvc-net-core-app>

### Status code 405 to 401

<https://stackoverflow.com/questions/59408865/net-core-api-returns-405method-not-allowed-when-having-authorize-attribute>
