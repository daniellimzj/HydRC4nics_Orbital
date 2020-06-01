# Application

## Telegram Bot

The Telegram Bot provides a chat-like interface for operators and interested individuals to view data and statistics from the system. Additionally, operators will be able to send commands using Telegram to remotely control the system.

Source code and further documentation can be found in `./Servers/TelegramBot`.

### Bot Functions

- Differentiate users based on Telegram ID
- Make different sets of commands available based on the user
- Convert commands sent to the bot to HTTP requests
- Format data responses (JSON) to a form appropriate to the Telegram interface

## Webpage

The webpage shares a common database with the Telegram Bot and provides the same functionality. The only difference will be the interface, where the webpage can make use of it’s increased flexibility to display more information and provide more options when controlling the system.

Source code and further documentation can be found in `./Servers/Webpage`.

### Web Functions

- Differentiate users based on a login system
- Make different sets of commands available based on the user
- Creation of different routes for sending commands and retrieving data
- Convert commands sent to webpage to HTTP requests
- Format data responses (JSON) to a form appropriate to the web interface
- Provide option to visualize data with tools such as graphs
- Provide option to download data in CSV format
- Aesthetically pleasing UI
