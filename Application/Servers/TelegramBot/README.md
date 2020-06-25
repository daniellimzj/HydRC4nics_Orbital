# Telegram Bot

## Implementation

The Python Telegram Bot wrapper is used to access the Telegram Bot API. Users will enter a 'conversation' with the Bot when they first send the /start command, and based on button and text inputs, sends back commands through the functions defined in the db.py module.

## References

### Processing Text Responses

- <https://github.com/python-telegram-bot/python-telegram-bot/wiki/Extensions-%E2%80%93-Your-first-Bot>
- <https://github.com/python-telegram-bot/python-telegram-bot/blob/master/telegram/ext/messagehandler.py>

### Conversation and Callback Query Handlers

- <https://python-telegram-bot.readthedocs.io/en/stable/telegram.ext.callbackqueryhandler.html>  
- <https://python-telegram-bot.readthedocs.io/en/stable/telegram.callbackquery.html#telegram.CallbackQuery.data>  
- <https://python-telegram-bot.readthedocs.io/en/stable/telegram.ext.conversationhandler.html?highlight=conversation%20handler>  
- <https://stackoverflow.com/questions/51699902/how-to-return-a-command-when-pressing-on-a-button-with-telegram-bot>  

### Menu and Button Creation

- <https://github.com/python-telegram-bot/python-telegram-bot/wiki/Code-snippets#build-a-menu-with-buttons>s>  
- <https://python-telegram-bot.readthedocs.io/en/stable/telegram.inlinekeyboardmarkup.html?highlight=inline%20keyboard%20button>  
- <https://python-telegram-bot.readthedocs.io/en/stable/telegram.inlinekeyboardbutton.html>

### Message Formatting

- <https://stackoverflow.com/questions/10805589/convert-json-date-string-to-python-datetime>
- <https://docs.python.org/3/library/datetime.html#strftime-and-strptime-behavior>
- <https://github.com/python-telegram-bot/python-telegram-bot/wiki/Code-snippets#message-formatting-bold-italic-code->