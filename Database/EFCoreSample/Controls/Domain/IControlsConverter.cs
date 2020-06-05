using EFCoreSample.Controls.Data;
using EFCoreSample.Controls.Requests;
using EFCoreSample.Controls.Responses;

namespace EFCoreSample.Controls.Domain
{
    public interface IControlsConverter
    {
        CommandResponse ToCommandResponse(Command command);
        CommandValue ToCommandValue(CommandRequest commandRequest);
        Command ToCommandDomainModel(CommandDataModel commandDataModel);
        CommandDataModel ToCommandDataModel(Command command);
        ActuatorResponse ToActuatorResponse(Actuator actuator);
        ActuatorValue ToActuatorValue(ActuatorRequest actuatorRequest);
        Actuator ToActuatorDomainModel(ActuatorDataModel actuatorDataModel);
        ActuatorDataModel ToActuatorDataModel(Actuator actuator);
    }
}