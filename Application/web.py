#will run the webpage
from Servers.Webpage.application import application as app

if __name__=='__main__':
    app.run(debug=True, host='0.0.0.0', port=8080)