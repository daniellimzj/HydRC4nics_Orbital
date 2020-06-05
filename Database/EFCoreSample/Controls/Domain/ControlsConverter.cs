using System;
using System.Linq;
using EFCoreSample.Controls.Data;
using EFCoreSample.Controls.Requests;
using EFCoreSample.Controls.Responses;

namespace EFCoreSample.Controls.Domain
{
    public class ControlsConverter : IControlsConverter
    {
        public CommandResponse ToCommandResponse(Command command)
        {
            return new CommandResponse(command.Id, command.Value.Value, command.Value.Units, command.Value.Issuer,
                command.Value.Purpose, command.Value.IssueDate, command.Value.ExecuteDate, 
                (int)command.Value.Repeat.TotalMinutes, command.Value.Completed);
        }

        public CommandValue ToCommandValue(CommandRequest commandRequest)
        {
            return new CommandValue(commandRequest.Value, commandRequest.Units, commandRequest.Issuer,
                commandRequest.Purpose, commandRequest.IssueDate, commandRequest.ExecuteDate, 
                TimeSpan.FromMinutes(commandRequest.Repeat), false);
        }

        public Command ToCommandDomainModel(CommandDataModel commandDataModel)
        {
            return new Command(commandDataModel.Id, 
                new CommandValue(commandDataModel.Value, commandDataModel.Units, commandDataModel.Issuer, 
                    commandDataModel.Purpose, commandDataModel.IssueDate, commandDataModel.ExecuteDate,
                    commandDataModel.Repeat, commandDataModel.Completed));
        }

        public CommandDataModel ToCommandDataModel(Command command)
        {
            return new CommandDataModel
            {
                Id = command.Id,
                Value = command.Value.Value,
                Units = command.Value.Units,
                Issuer = command.Value.Issuer,
                Purpose = command.Value.Purpose,
                IssueDate = command.Value.IssueDate,
                ExecuteDate = command.Value.ExecuteDate,
                Repeat = command.Value.Repeat,
                Completed = command.Value.Completed
            };
        }

        public ActuatorResponse ToActuatorResponse(Actuator actuator)
        {
            return new ActuatorResponse(actuator.Id, actuator.Value.Position, actuator.Value.Type, 
                actuator.Commands?.Select(ToCommandResponse).ToList());
        }

        public ActuatorValue ToActuatorValue(ActuatorRequest actuatorRequest)
        {
            return new ActuatorValue(actuatorRequest.Position, actuatorRequest.Type);
        }

        public Actuator ToActuatorDomainModel(ActuatorDataModel actuatorDataModel)
        {
            return new Actuator(actuatorDataModel.Id, new ActuatorValue(actuatorDataModel.Position, actuatorDataModel.Type))
            {
                Commands = actuatorDataModel.Commands?.Select(ToCommandDomainModel).ToList()
            };
        }

        public ActuatorDataModel ToActuatorDataModel(Actuator actuator)
        {
            return new ActuatorDataModel
            {
                Id = actuator.Id,
                Position = actuator.Value.Position,
                Type = actuator.Value.Type,
                Commands = actuator.Commands?.Select(ToCommandDataModel).ToList()
            };
        }
    }
}