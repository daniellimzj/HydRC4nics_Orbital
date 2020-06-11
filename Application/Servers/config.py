"""Central configuration"""
import os

BOT_TOKEN = os.environ['BOT_TOKEN']
FLASK_SECRET = os.environ['FLASK_SECRET']
DB_URL = os.environ['DB_URL']
JWT_SECRET = os.environ['JWT_SECRET']