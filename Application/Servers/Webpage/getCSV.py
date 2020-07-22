from flask import Response

def main(application):
    
    @application.route("/getReadingsCSV")
    def getReadingsCSV():
        with open("../Database/csv/readings.csv") as fp:
            csv = fp.read()

        return Response(
            csv,
            mimetype="text/csv",
            headers={"Content-disposition":
                    "attachment; filename=readings.csv"})

    ############################################

    @application.route("/getSensorsCSV")
    def getSensorsCSV():
        with open("../Database/csv/sensors.csv") as fp:
            csv = fp.read()

        return Response(
            csv,
            mimetype="text/csv",
            headers={"Content-disposition":
                    "attachment; filename=sensors.csv"})

    ############################################

    @application.route("/getCommandsCSV")
    def getCommandsCSV():
        with open("../Database/csv/commands.csv") as fp:
            csv = fp.read()

        return Response(
            csv,
            mimetype="text/csv",
            headers={"Content-disposition":
                    "attachment; filename=commands.csv"})

    ############################################

    @application.route("/getActuatorsCSV")
    def getActuatorsCSV():
        with open("../Database/csv/actuators.csv") as fp:
            csv = fp.read()

        return Response(
            csv,
            mimetype="text/csv",
            headers={"Content-disposition":
                    "attachment; filename=actuators.csv"})