# P1Reader
P1 Reader for Dutch PowerMeters

# Features
- Supports a live (debug) view to test your connection
- InfluxDB output for saving P1 data every 10 seconds

# Issues
Please use the github issue's feature to request help with any issues

# Usage
```
Usage:  [options] [command]

Options:
  -? | -h | --help  Show help information

Commands:
  debug     Show debug information
  influxdb  Write to influxdb

Use " [command] --help" for more information about a command.
```

## Debug mode
```
Usage:  debug [options]

Options:
  -? | -h | --help  Show help information
  --port            Serial port to read from (Example: "/dev/ttyUSB0")
  --baudrate        Baudrate to read serial port with (Example: 115200 or 9600)
  --stopbits        Stopbits to read serial port with (Example: 1)
  --databits        Databits to read serial port with (Example: 7 or 8)
  --parity          Databits to read serial port with (Example: 1 (Even) or 0 (None))
```

## InfluxDB mode
```
Usage:  influxdb [options]

Options:
  -? | -h | --help  Show help information
  --logging         Log level to display (0 = Verbose, 1 = Debug, 2 = Information, 3 = Warning, 4 = Error, 5 = Fatal)
  --port            Serial port to read from (Example: "/dev/ttyUSB0")
  --baudrate        Baudrate to read serial port with (Example: 115200 or 9600)
  --stopbits        Stopbits to read serial port with (Example: 1)
  --databits        Databits to read serial port with (Example: 7 or 8)
  --parity          Databits to read serial port with (Example: 1 (Even) or 0 (None))
  --influxhost      InfluxDB server host
  --influxdatabase  InfluxDB database name
  --influxusername  InfluxDB database username
  --influxpassword  InfluxDB database password
```

# Deployment
Use the included publish profile to publish to a folder. Make sure to copy over the contents from the linux-arm folder.

On the raspberry run the following command
```
sudo nano /etc/rc.local
```

Add a new line with the application to run in the background (&).
Example:
```
/home/pi/linux-arm/P1ReaderApp influxdb --port /dev/ttyUSB0 --baudrate 115200 --stopbits 1 --databits 7 --parity 1 --influxhost http://<ip>:<port> --influxdatabase p1 --influxusername <username> --influxpassword <password> &
```