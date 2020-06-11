#will run the webpage
from Servers.Webpage import application

if __name__=='__main__':
    application.application.run(debug=True, host='0.0.0.0', port=8080)