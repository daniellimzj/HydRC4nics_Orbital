using System;
using System.Threading;

namespace EFCoreSample.Controls.Domain
{
    public class CommandValue
    {
        public CommandValue(int value, string units, string issuer, string purpose, DateTime issueDate, DateTime executeDate, TimeSpan repeat, bool completed)
        {
            Value = value;
            Units = units;
            Issuer = issuer;
            Purpose = purpose;
            IssueDate = issueDate;
            ExecuteDate = executeDate;
            Repeat = repeat;
            Completed = completed;
        }

        public int Value { get; }
        public string Units { get; }
        public string Issuer { get; }
        public string Purpose { get; }
        public DateTime IssueDate { get; set; }
        public DateTime ExecuteDate { get; set; }
        public TimeSpan Repeat { get; }
        public bool Completed { get; set; }
    }
}