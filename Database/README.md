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
