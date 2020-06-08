# Copyright 2017 Amazon.com, Inc. or its affiliates. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except
# in compliance with the License. A copy of the License is located at
#
# https://aws.amazon.com/apache-2-0/
#
# or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
# specific language governing permissions and limitations under the License.
"Demo Flask application"
import sys

import requests
import boto3
from flask import Flask, render_template_string
from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired

import config
import util

application = Flask(__name__)
application.secret_key = config.FLASK_SECRET

### FlaskForm set up
class PhotoForm(FlaskForm):
    """flask_wtf form class the file upload"""
    photo = FileField('image', validators=[
        FileRequired()
    ])


@application.route("/", methods=('GET', 'POST'))
def home():
    """Homepage route"""
    all_labels = ["No labels yet"]

    #####
    # s3 getting a list of photos in the bucket
    #####
    s3_client = boto3.client('s3')
    prefix = "photos/"
    response = s3_client.list_objects(
        Bucket=config.PHOTOS_BUCKET,
        Prefix=prefix
    )
    photos = []
    if 'Contents' in response and response['Contents']:
        photos = [s3_client.generate_presigned_url(
            'get_object',
            Params={'Bucket': config.PHOTOS_BUCKET, 'Key': content['Key']}
            ) for content in response['Contents']]


    form = PhotoForm()
    url = None
    if form.validate_on_submit():
        image_bytes = util.resize_image(form.photo.data, (300, 300))
        if image_bytes:
            #######
            # s3 excercise - save the file to a bucket
            #######
            key = prefix + util.random_hex_bytes(8) + '.png'
            s3_client.put_object(
                Bucket=config.PHOTOS_BUCKET,
                Key=key,
                Body=image_bytes,
                ContentType='image/png'
            )
            # http://boto3.readthedocs.io/en/latest/guide/s3.html#generating-presigned-urls
            url = s3_client.generate_presigned_url(
                'get_object',
                Params={'Bucket': config.PHOTOS_BUCKET, 'Key': key})

    return render_template_string("""
            {% extends "main.html" %}
            {% block content %}
            <h4>Upload Photo</h4>
            <form method="POST" enctype="multipart/form-data" action="{{ url_for('home') }}">
                {{ form.csrf_token }}
                  <div class="control-group">
                   <label class="control-label">Photo</label>
                    {{ form.photo() }}
                  </div>

                    &nbsp;
                   <div class="control-group">
                    <div class="controls">
                        <input class="btn btn-primary" type="submit" value="Upload">
                    </div>
                  </div>
            </form>

            {% if url %}
            <hr/>
            <h3>Uploaded!</h3>
            <img src="{{url}}" /><br/>
            {% for label in all_labels %}
            <span class="label label-info">{{label}}</span>
            {% endfor %}
            {% endif %}
            
            {% if photos %}
            <hr/>
            <h4>Photos</h4>
            {% for photo in photos %}
                <img width="150" src="{{photo}}" />
            {% endfor %}
            {% endif %}

            {% endblock %}
                """, form=form, url=url, photos=photos, all_labels=all_labels)


@application.route("/info")
def info():
    "Webserver info route"
    metadata = "http://169.254.169.254"
    instance_id = requests.get(metadata +
                               "/latest/meta-data/instance-id").text
    availability_zone = requests.get(metadata +
                                     "/latest/meta-data/placement/availability-zone").text

    return render_template_string("""
            {% extends "main.html" %}
            {% block content %}
            <b>instance_id</b>: {{instance_id}} <br/>
            <b>availability_zone</b>: {{availability_zone}} <br/>
            <b>sys.version</b>: {{sys_version}} <br/>
            {% endblock %}""",
                                  instance_id=instance_id,
                                  availability_zone=availability_zone,
                                  sys_version=sys.version)


if __name__ == "__main__":
    # http://flask.pocoo.org/docs/0.12/errorhandling/#working-with-debuggers
    # https://docs.aws.amazon.com/cloud9/latest/user-guide/app-preview.html
    use_c9_debugger = False
    application.run(use_debugger=not use_c9_debugger, debug=True,
                    use_reloader=not use_c9_debugger, host='0.0.0.0', port=8080)
