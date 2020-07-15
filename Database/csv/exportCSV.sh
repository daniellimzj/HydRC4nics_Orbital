#!/bin/bash
sqlite3 -header -csv ../EFCoreSample/EFCoreSample.db "SELECT * FROM Readings;" > readings.csv
sqlite3 -header -csv ../EFCoreSample/EFCoreSample.db "SELECT * FROM Sensors;" > sensors.csv
sqlite3 -header -csv ../EFCoreSample/EFCoreSample.db "SELECT * FROM Commands;" > commands.csv
sqlite3 -header -csv ../EFCoreSample/EFCoreSample.db "SELECT * FROM Actuators;" > actuators.csv