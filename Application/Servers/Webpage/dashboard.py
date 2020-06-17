import pandas as pd
import dash_core_components as dcc
import dash_html_components as html
import dash
import plotly.express as px
from dash.dependencies import Input, Output

from .. import db

def startDashboard(app):
    app.layout = html.Div([
        html.H1("Dash Demo"),
        dcc.Graph(id='graph'),
        html.Label([
            "colorscale",
            dcc.Dropdown(
                id='colorscale-dropdown', clearable=False,
                value='plasma', options=[
                    {'label': c, 'value': c}
                    for c in px.colors.named_colorscales()
                ])
        ]),
    ])

    # Define callback to update graph
    @app.callback(
        Output('graph', 'figure'),
        [Input("colorscale-dropdown", "value")]
    )
    def update_figure(colorscale):
        data = db.getReadings(latest=20)

        df = pd.json_normalize(data = data, 
                                record_path ='readings',  
                                meta =['id', 'position', 'type'],
                                record_prefix='reading_') 

        df.columns = ['readingId', 'readingValue', 'readingUnits', 'readingTimeStamp', 'sensorId', 'sensorPosition', 'sensorType']

        return px.line(
            df, x="readingTimeStamp", y="readingValue", color="sensorPosition",
            render_mode="webgl", title="Tips"
        )