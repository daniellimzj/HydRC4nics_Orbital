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
    admin = user.username in tb.administrators


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