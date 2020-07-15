"""
Functions and variables for the creation of menus
"""

from telegram import InlineKeyboardMarkup, InlineKeyboardButton, ParseMode, CallbackQuery
from telegram.ext import Updater, CommandHandler, MessageHandler, CallbackQueryHandler, ConversationHandler

from . import Telebot as tb
from .. import db

# Function to build menus
def build_menu(buttons, n_cols, header_buttons=None, footer_buttons=None):
    menu = [buttons[i:i + n_cols] for i in range(0, len(buttons), n_cols)]
    if header_buttons:
        menu.insert(0, [header_buttons])
    if footer_buttons:
        menu.append([footer_buttons])
    return menu

# Function to initialise menus in /start
def initialiseMenus():

    global sensorMenu, actuatorMenu

    sensorsList = db.getAllSensors()
    actuatorsList = db.getAllActuators()
    
    actuatorOptions = []
    sensorOptions = []

    for sensor in sensorsList:
        temptext = str(sensor['type']) + ' ' + str(sensor['position'])
        tempdata = str(sensor['id'])
        sensorOptions.append(InlineKeyboardButton(text = temptext, callback_data= tempdata))
    
    sensorOptions.append(InlineKeyboardButton(text = "Back to Start ↩", callback_data="START"))

    for actuator in actuatorsList:
        temptext = str(actuator['type']) + ' ' + str(actuator['position'])
        tempdata = str(actuator['id'])
        actuatorOptions.append(InlineKeyboardButton(text = temptext, callback_data= tempdata))

    actuatorOptions.append(InlineKeyboardButton(text = "Back to Start ↩", callback_data="START"))

    sensorMenu = build_menu(sensorOptions, n_cols = 1, header_buttons= None, footer_buttons= None)
    actuatorMenu = build_menu(actuatorOptions, n_cols = 1, header_buttons= None, footer_buttons=None)
    


# Lists for static menus
startOptionsAdmin = [
    InlineKeyboardButton(text = "Send Command ⚙", callback_data = "ACTUATOR"),
    InlineKeyboardButton(text = "Get Data 📈", callback_data= "DATA"),
    InlineKeyboardButton(text = "Help! 🤡", callback_data = "HELP")
]

startOptionsNormal = [
    InlineKeyboardButton(text = "Get Data 📈", callback_data= "DATA"),
    InlineKeyboardButton(text = "Help! 🤡", callback_data = "HELP")
]

dataOptions = [
    InlineKeyboardButton(text = "All recent data", callback_data = "RECENT"),
    InlineKeyboardButton(text = "Choose a sensor", callback_data = "SENSORS"),
    InlineKeyboardButton(text = "Back to Start ↩", callback_data="START")
]



# Creating static menus
startMenuAdmin = build_menu(startOptionsAdmin, n_cols = 1, header_buttons= None, footer_buttons=None)
startMenuNormal = build_menu(startOptionsNormal, n_cols = 1, header_buttons= None, footer_buttons=None)
dataMenu = build_menu(dataOptions, n_cols = 1, header_buttons= None, footer_buttons=None)
startOverMenu = build_menu([InlineKeyboardButton(text = "Back to Start ↩", callback_data = "START")], n_cols = 1, header_buttons= None, footer_buttons= None)

