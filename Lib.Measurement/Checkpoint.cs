using System;
using System.Diagnostics;
using System.Text;

namespace Lib.Measurement
{
    public sealed class Checkpoint : ICheckpoint
    {
        private static readonly String MApplicationName = Process.GetCurrentProcess().MainModule.ModuleName;
        private static readonly DateTime MStartTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();

        private readonly long m_time = Stopwatch.GetTimestamp();

        private const char TabChar = '\t';
        private const string CheckpointJournalText = "{3:F3}: {0}.{1} ({2})";
        private const string CheckpointJournalTextProjectName = "Project Name: {0}";
        private const string CheckpointJournalTextApplicationName = "Application Name: {0}";
        private const string CheckpointJournalTextProcessStartTime = "Process Start Time: {0:f} UTC";

        public Checkpoint(Type objType, string eventName)
            : this(objType, eventName, null)
        {
        }

        public Checkpoint(Action method, string subeventName)
            : this(method.Method.DeclaringType, method.Method.Name, subeventName)
        {
        }

        public Checkpoint(Type objType, string eventName, string subeventName)
        {
            ClassName = objType.FullName;
            ProjectName = objType.Module.Name;
            EventName = eventName;
            SubeventName = subeventName;
            Publish();
        }

        private void Publish()
        {
            TimeSpan offset = new TimeSpan();

            if (MStartTime != null)
            {
                offset = DateTime.Now.ToUniversalTime().Subtract(MStartTime);
            }

            //TODO: Put in logic for storing a collection of checkpoints
        }

        public string ProjectName { get; }

        public string ClassName { get; }

        public string EventName { get; }

        public string SubeventName { get; }

        public string ApplicationName => MApplicationName;

        public long Offset => CheckpointData.InternalOffsetToOffset(m_time);

        public DateTime ProcessStartTime => MStartTime;

        public DateTime GetCurrentOffsetDate => MStartTime.AddTicks(Offset / 100);

        public String GetJournalText()
        {
            StringBuilder journalText = new StringBuilder();
            journalText.AppendFormat(CheckpointJournalText, ClassName, EventName, SubeventName, Offset * 1.0e-9);
            journalText.AppendLine();

            journalText.Append(TabChar);
            journalText.AppendFormat(CheckpointJournalTextProjectName, ProjectName);
            journalText.AppendLine();

            journalText.Append(TabChar);
            journalText.AppendFormat(CheckpointJournalTextApplicationName, ApplicationName);
            journalText.AppendLine();

            journalText.Append(TabChar);
            journalText.AppendFormat(CheckpointJournalTextProcessStartTime, ProcessStartTime.ToString());
            journalText.AppendLine();

            return journalText.ToString();
        }
    }
}
