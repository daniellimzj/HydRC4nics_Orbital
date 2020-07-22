import datetime
from . import forms as f
from flask import render_template

############################################

def sessionExpired(expiretime):
    if expiretime is None:
        return False
    return datetime.datetime.now() > expiretime

############################################

def resetSession(session):
    session["user"] = {}
    session["operator"] = False
    session["expiretime"] = None
    session["login"] = False

############################################

def checkExpiry(session):
    if not session["login"]:
        loginForm = f.LoginForm()
        return True, render_template('login.html', login = session["login"], loginForm = loginForm)

    if sessionExpired(session["expiretime"]):
        resetSession(session)
        return True, render_template("session-expired.html", login = session["login"])

    if not session["operator"]:
        return True, render_template('unauthorised.html', login = session["login"])

    return False, None