using System;

namespace EFCoreSample.Controls.Responses
{
    public class CommandResponse
    {
        public CommandResponse(Guid id, int value, string units, string issuer, string purpose, DateTime issueDate, DateTime executeDate, int repeat, bool completed)
        {
            Id = id;
            Value = value;
            Units = units;
            Issuer = issuer;
            Purpose = purpose;
            IssueDate = issueDate;
            ExecuteDate = executeDate;
            Repeat = repeat;
            Completed = completed;
        }

        public Guid Id { get; }
        public int Value { get; }
        public string Units { get; }
        public string Issuer { get; }
        public string Purpose { get; }
        public DateTime IssueDate { get; }
        public DateTime ExecuteDate { get; }
        public int Repeat { get; }
        public bool Completed { get; }
    }
}