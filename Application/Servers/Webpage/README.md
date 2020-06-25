# Webpage

## Implementation

The webpage is created using Flask as the framework to control the webpage. Flask-WTF, an extension building on WTForms, is used to handle form creation and validation, for users to send commands and choose which data they wish to view. Jinja is used within Flask to dynamically update webpages through using Python variables to generate each webpage. The styling and appearance of the webpage is decided with Bootstrap, due to its convenience and ease of use. Dash is used to create aesthetically pleasing and functional graphs for viewing of data. For the production server, Waitress is used to create a stable Web Server Gateway Interface (WSGI) server. The login system will be implemented using JSON Web Tokens (JWT) in future updates.

## References

### General
- <https://docs.python.org/3/library/datetime.html>
- <https://jinja.palletsprojects.com/en/2.11.x/templates/>
- <https://stackoverflow.com/questions/394809/does-python-have-a-ternary-conditional-operator>
- <https://cs50.harvard.edu/web/2018/notes/2/>


### Flask & WTForms
- <https://stackoverflow.com/questions/49697545/flask-wtform-datetimefield-rendering-issue>
- <https://stackoverflow.com/questions/52825708/wtforms-datetimelocalfield-data-is-none-after-submit>
- <https://wtforms.readthedocs.io/en/2.3.x/fields/#>
- <https://stackoverflow.com/questions/21815067/how-do-i-validate-wtforms-fields-against-one-another>

### Dash

- <https://medium.com/plotly/introducing-jupyterdash-811f1f57c02e>
- <https://dash.plotly.com/live-updates>
- <https://community.plotly.com/t/dcc-loading-for-loading-graph-s/23039>
- <https://towardsdatascience.com/how-to-embed-bootstrap-css-js-in-your-python-dash-app-8d95fc9e599e>
- <https://dash-bootstrap-components.opensource.faculty.ai/>

### Downloadable CSV

- <https://stackoverflow.com/questions/30024948/flask-download-a-csv-file-on-clicking-a-button>
