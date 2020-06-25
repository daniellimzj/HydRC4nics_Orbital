"""
Define a few command handlers and callback query handlers. These usually take the two arguments update and context.
"""

import datetime
import os

from telegram import InlineKeyboardMarkup, InlineKeyboardButton, ParseMode, CallbackQuery
from telegram.ext import Updater, CommandHandler, MessageHandler, CallbackQueryHandler, ConversationHandler, Filters

from . import Telebot as tb
from . import menus as mn
from .. import db


INITIAL, START, ACTUATORS, COMMAND, DATA, SENSORS, READINGS = range(7)


# Command handler to start conversation handler
def start(update, context):

    # Some initialisation
    global user, chatid, admin
    user = update.message.from_user
    chatid = update.message.chat.id
    admin = True #user.username in tb.administrators


    mn.initialiseMenus()

    #Actual message
    context.bot.send_message(text = f'Hi {str(user.first_name)}! You have {("normal", "administrator")[admin]} privileges. What would you like to do?',
                             chat_id = chatid,
                             parse_mode = ParseMode.HTML,
                             reply_markup = (InlineKeyboardMarkup(mn.startMenuNormal), InlineKeyboardMarkup(mn.startMenuAdmin))[admin])
    return START

# Callback query handler to loop conversation handler
def startOver(update, context):

    context.bot.send_message(text = f'Hi {str(user.first_name)}! You have {("normal", "administrator")[admin]} privileges. What would you like to do?',
                             chat_id = chatid,
                             parse_mode = ParseMode.HTML,
                             reply_markup = (InlineKeyboardMarkup(mn.startMenuNormal), InlineKeyboardMarkup(mn.startMenuAdmin))[admin])
    return START

# Handler for start options
def startHandler(update, context):
    query = update.callback_query

    if query.data == "HELP":
        
        with open("help.txt") as myfile:
            messageText = myfile.read()
        
        context.bot.send_message(
                                text = messaegText,
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return START
    
    if query.data == "ACTUATOR":
        context.bot.send_message(
                                text = f'What actuator would you like to control?',
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.actuatorMenu))
        return ACTUATORS

    elif query.data == "DATA":
        context.bot.send_message(
                                text = f'What data would you like to view?',
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.dataMenu))
        return DATA


# Handler for actuators
def ActuatorsHandler(update, context):
    query = update.callback_query

    # Pass query information for use in commandHandler
    global idToBePassed
    idToBePassed = query.data

    context.bot.send_message(
                            text = f'Please send me the value you would like to set this actuator to\n(Must be between 0 and 100 inclusive)',
                             chat_id = chatid,
                             parse_mode = ParseMode.HTML,
                             reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
    return COMMAND


# Handler for commands
def commandHandler(update, context):

    reply = update.message.text

    if int(reply) in range(0,101):

        commandsList = db.getCommands(actuatorId = idToBePassed)
        actuator = f"{commandsList['type']} {commandsList['position']}"
        if commandsList['type'] == 'light':
            inputUnits = '%'
        else: inputUnits = 'units'

        db.addCommand(actuatorId = idToBePassed, 
                      value = int(reply),
                      units = inputUnits,
                      issuer = str(user.username),
                      purpose = "Telegram Command",
                      executeDate = datetime.datetime.now() + datetime.timedelta(seconds = 5),
                      repeat=0)

        context.bot.send_message(
                            text = f"{actuator}'s value has been set to {reply} {inputUnits}.",
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return COMMAND
    
    else:
        context.bot.send_message(
                    text = "Please enter a value between 0 and 100 inclusive!",
                        chat_id = chatid,
                        parse_mode = ParseMode.HTML,
                        reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return COMMAND

#Handler for data viewing options
def dataHandler(update, context):

    query = update.callback_query
    if query.data == "RECENT":
        viewdata = db.getReadings(latest = 1)
        print(viewdata)

        messageText = "<b>All Recent Readings:</b>\n\n"

        for sensor in viewdata:
            sensorText = str(sensor['type']) + ' ' + str(sensor['position'])
            readingText = str(sensor['readings'][0]['value']) + ' ' + str(sensor['readings'][0]['units'])
            timeText = datetime.datetime.strptime(sensor['readings'][0]['timeStamp'], '%Y-%m-%dT%H:%M:%S.%f').strftime("%m/%d/%y at %I:%M%p")
            messageText = messageText + sensorText + ': ' + readingText  + ' on ' + timeText + '\n'

        context.bot.send_message(
                                text = messageText,
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return DATA

    elif query.data == "SENSORS":
        context.bot.send_message(
                                text = f'What sensor would you like to view data from?',
                                chat_id = chatid,
                                parse_mode = ParseMode.HTML,
                                reply_markup = InlineKeyboardMarkup(mn.sensorMenu))
        return SENSORS

# Handler to view individual sensor menu
def sensorsHandler(update, context):

    query = update.callback_query
    global idToBePassed
    idToBePassed = query.data


    context.bot.send_message(
                            text = "How many of the latest readings would you like to view?",
                            chat_id = chatid,
                            parse_mode = ParseMode.HTML,
                            reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
    return READINGS

def readingsHandler(update, context):

    reply = update.message.text

    if int(reply) in range(0, 11):
        viewdata = db.getReadings(sensorId = idToBePassed, latest = int(reply))
        print(viewdata)
        sensorText = str(viewdata['type']) + ' ' + str(viewdata['position'])
        messageText = f"<b>{sensorText} Readings:</b>\n\n"

        for reading in viewdata['readings']:   
            readingText = str(reading['value']) + ' ' + str(reading['units'])
            timeText = datetime.datetime.strptime(reading['timeStamp'], '%Y-%m-%dT%H:%M:%S.%f').strftime("%m/%d/%y at %I:%M%p")
            messageText = messageText + readingText + ' on ' + timeText + '\n'

        context.bot.send_message(
                            text = messageText,
                            chat_id = chatid,
                            parse_mode = ParseMode.HTML,
                            reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return READINGS

    else:
        context.bot.send_message(
            text = "Please enter a value between 0 and 10 inclusive!",
                chat_id = chatid,
                parse_mode = ParseMode.HTML,
                reply_markup = InlineKeyboardMarkup(mn.startOverMenu))
        return READINGS
