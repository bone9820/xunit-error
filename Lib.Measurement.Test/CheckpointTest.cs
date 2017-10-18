using System;
using System.Diagnostics;

using Xunit;

namespace Lib.Measurement.Test
{
    public class CheckpointTest
    {
        [Fact]
        public void ApplicationNameTest()
        {
            Checkpoint target = new Checkpoint(this.ApplicationNameTest, "test");
            string actual;
            actual = target.ApplicationName;
            Assert.Equal(Process.GetCurrentProcess().MainModule.ModuleName, actual);
        }

        [Fact]
        public void ClassNameTest()
        {
            Checkpoint target = new Checkpoint(this.ClassNameTest, "test");
            string actual;
            actual = target.ClassName;
            Assert.Equal(this.GetType().FullName, actual);
        }

        [Fact]
        public void ProjectNameTest()
        {
            Checkpoint target = new Checkpoint(this.ProjectNameTest, "test");
            string actual;
            actual = target.ProjectName;
            Assert.Equal(this.GetType().Module.Name, actual);
        }

        [Fact]
        public void EventNameTest()
        {
            Checkpoint target = new Checkpoint(this.EventNameTest, "test");
            string actual;
            actual = target.EventName;
            Assert.Equal("EventNameTest", actual);
        }

        [Fact]
        public void SubeventNameTest()
        {
            Checkpoint target = new Checkpoint(this.SubeventNameTest, "test");
            string actual;
            actual = target.SubeventName;
            Assert.Equal("test", actual);

            target = new Checkpoint(this.SubeventNameTest, null);
            actual = target.SubeventName;
            Assert.Null(actual);
        }

        [Fact]
        public void ProcessStartTimeTest()
        {
            DateTime now = DateTime.UtcNow;
            Checkpoint target = new Checkpoint(this.ProcessStartTimeTest, "test");
            DateTime actual;
            actual = target.ProcessStartTime;
            Assert.True(actual < now);
        }

        [Fact]
        public void OffsetTest()
        {
            Checkpoint target = new Checkpoint(this.OffsetTest, "test");
            long actual;
            actual = target.Offset;
            long estimated = (DateTime.UtcNow - target.ProcessStartTime).Ticks * 100;
            Assert.True((actual - 0.5e9) <= estimated);
            Assert.True((actual + 0.5e9) > estimated);
        }

        [Fact]
        public void GetJournalTextTest()
        {
            Checkpoint target = new Checkpoint(this.GetJournalTextTest, "test");
            string actual;
            actual = target.GetJournalText();
            Assert.NotNull(actual);
        }

        [Fact]
        public void CheckpointConstructorTest()
        {
            Checkpoint target = new Checkpoint(typeof(CheckpointTest), "CheckpointConstructorTest", "test");
            Assert.Equal(target.ProjectName, this.GetType().Module.Name);
            Assert.Equal(target.ClassName, this.GetType().FullName);
            Assert.Equal(target.EventName, "CheckpointConstructorTest");
            Assert.Equal(target.SubeventName, "test");
        }
    }
}
