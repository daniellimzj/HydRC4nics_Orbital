# Arduino

An Arduino Mega will be used as bridge between the Raspberry Pi and the Sensors and Actuators. They will communicate using through the USART peripheral (Serial communication). Data from the sensors will be read every X minutes and sent back to the Database server while commands will be received through polling. (May implement interrupt based later on)

## Data Formats

All data will be formatted as strings as such:

```C
// Readings
"Value1,Units1,Value2,Units2,Value3,Units3,...,ValueN,UnitsN"

// Commands
"Position:Value"
```

## References

<https://arduino.github.io/arduino-cli/installation/>
<https://www.arduino.cc/reference/en/language/functions/external-interrupts/attachinterrupt/>
