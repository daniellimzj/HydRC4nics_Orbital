#include "DHT.h"

//Constants
#define DHTPIN 2     // what pin we're connected to
#define DHTTYPE DHT22   // DHT 22  (AM2302)
DHT dht(DHTPIN, DHTTYPE); // Initialize DHT sensor for normal 16mhz Arduino

//Variables
int hum;  //Stores humidity value
int temp; //Stores temperature value

int sensorPin = A0;    // select the input pin for the potentiometer
int ledPin = 13;      // select the pin for the LED
int sensorValue = 0;  // variable to store the value coming from the sensor

char inputString[200] = "";         // a string to hold incoming data

unsigned long prev = millis();
long second = 1000;
unsigned long interval = 30 * 60 * second; // 30 minute

void setup() {
  // declare the ledPin as an OUTPUT:
  pinMode(ledPin, OUTPUT);
  pinMode(sensorPin, INPUT);
  pinMode(DHTPIN, INPUT);
  digitalWrite(ledPin, LOW);

  dht.begin();
  // initialize serial:
  Serial.begin(9600);
}

void loop() {
  if (millis() - prev >= interval) {
    //Read data and store it to variables hum and temp
    hum = dht.readHumidity() * 10;
    temp= dht.readTemperature() * 10;
    //Print temp and humidity values to serial monitor
    Serial.print(hum);
    Serial.print(",e-01 %,");
    Serial.print(temp);
    Serial.println(",e-01 C,");
    // read the value from the sensor:
    //sensorValue = analogRead(sensorPin);
    //Serial.println(sensorValue);
    prev = millis();
  }
  
  int inChar;
  while (Serial.available()) {
    // get the new byte:
    inChar = (char)Serial.readBytesUntil('\n', inputString, sizeof(inputString) - 1);
    // add it to the inputString:
    inputString[inChar] = 0;
    // Read a command pair
    int pin, value;
    if (inputString != 0)
    {
        // Split the command in two values
        char* separator = strchr(inputString, ':');
        if (separator != 0)
        {
            // Actually split the string in 2: replace ':' with 0
            *separator = 0;
            pin = atoi(inputString);
            ++separator;
            value = atoi(separator);
    
            // Do something with servoId and position
            digitalWrite(pin, value);
        }
    }
    // clear the string:
    inputString[0] = 0;
  }
}
