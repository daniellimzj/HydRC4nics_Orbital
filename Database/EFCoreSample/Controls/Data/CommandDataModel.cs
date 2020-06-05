using System;

namespace EFCoreSample.Controls.Data
{
    public class CommandDataModel
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Units { get; set; }
        public string Issuer { get; set; }
        public string Purpose { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExecuteDate { get; set; }
        public TimeSpan Repeat { get; set; }
        public bool Completed { get; set; }
    }
}