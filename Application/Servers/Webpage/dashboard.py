import pandas as pd
import dash_core_components as dcc
import dash_html_components as html
import dash
import plotly.express as px
from dash.dependencies import Input, Output
import dash_bootstrap_components as dbc

from .. import db

def getReadingsAsDf(num):
    data = db.getReadings(latest=20)
    df = pd.json_normalize(data = data, 
                            record_path ='readings',  
                            meta =['id', 'position', 'type'],
                            record_prefix='reading_') 

    df.columns = ['readingId', 'Value', 'readingUnits', 'Time', 'sensorId', 'sensorPosition', 'sensorType']

    df['Sensor'] = df.sensorType + ' ' + df.sensorPosition

    return df

def startDashboard(server):

    app = dash.Dash(
            __name__,
            server=server,
            routes_pathname_prefix='/dash/',
            external_stylesheets=[dbc.themes.LITERA]
        )

    df = getReadingsAsDf(20)

    app.layout = html.Div([
        dcc.Loading(id = "loading-icon", 
                children=dcc.Graph(id='graph'),
                type="default"),
        html.Div([
            html.Hr(),
            html.Label([
                "Sensors",
                dcc.Dropdown(
                    id='sensor-dropdown', clearable=False,
                    value=df.Sensor.unique(), options=[
                        {'label': c, 'value': c}
                        for c in df.Sensor.unique()
                    ], multi=True)
            ])
        ]),
        dcc.Interval(
            id='interval-component',
            interval=60*60*1000, # in milliseconds
            n_intervals=0
        ),
    ])

    # Define callback to update graph
    @app.callback(
        Output('graph', 'figure'),
        [Input("sensor-dropdown", "value"),
        Input('interval-component', 'n_intervals')]
    )
    def update_figure(Sensors, n):
        df = getReadingsAsDf(20)

        fig = px.line(
            df[df.Sensor.isin(Sensors)], x="Time", y="Value", color="Sensor",
            render_mode="webgl", title="Sensor Readings"
        )

        return fig