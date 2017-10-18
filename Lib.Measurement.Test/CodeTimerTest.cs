using System;
using System.Threading;

using Xunit;

namespace Lib.Measurement.Test
{
    public class CodeTimerTest
    {
        [Fact]
        public void TimerConstructorTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "TimerConstructorTest");
            ICheckpoint[] checkpoints = new ICheckpoint[] { timer.Start(), timer.Stop() };
            foreach (ICheckpoint checkpoint in checkpoints)
            {
                Assert.Equal(checkpoint.ClassName, typeof(CodeTimerTest).FullName);
                Assert.Equal(checkpoint.EventName, "TimerConstructorTest");
            }
        }

        [Fact]
        public void TimerDisposeTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "DisposeTest", true);
            timer.Dispose();
            long elapsed = timer.ElapsedTime;
            Assert.False(timer.Running);
            Assert.Equal(elapsed, timer.ElapsedTime);
            Assert.True(timer.ElapsedTime > 0);
        }

        [Fact]
        public void StartTimerTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "StartTimerTest");
            Assert.Equal(0, timer.ElapsedTime);
            Assert.Equal("start", timer.Start().SubeventName);
            Assert.Null(timer.Start());
            Assert.True(timer.Started);
            Assert.True(timer.Running);
            long elapsed = timer.ElapsedTime;
            Thread.Sleep(1000);
            Assert.True(timer.ElapsedTime > 0);
            Assert.NotEqual(elapsed, timer.ElapsedTime);
            timer.Stop();
        }

        [Fact]
        public void StopTimerTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "StopTimerTest", true);
            Assert.Equal("stop", timer.Stop().SubeventName);
            long elapsed = timer.ElapsedTime;
            Assert.True(timer.Started);
            Assert.False(timer.Running);
            Assert.Equal(elapsed, timer.ElapsedTime);
            Assert.True(timer.ElapsedTime > 0);
            Assert.Null(timer.Stop());
        }

        [Fact]
        public void FailTimerTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "FailTimerTest", true);
            Assert.Equal("fail", timer.Fail().SubeventName);
            long elapsed = timer.ElapsedTime;
            Assert.True(timer.Started);
            Assert.False(timer.Running);
            Assert.Equal(elapsed, timer.ElapsedTime);
            Assert.True(timer.ElapsedTime > 0);
            Assert.Null(timer.Fail());
        }

        [Fact]
        public void GetJournalTextTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "GetJournalTextTest", true);
            timer.Stop();
            Assert.NotNull(timer.GetJournalText());
        }

        [Fact]
        public void StartedTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "StartedTest");
            Assert.False(timer.Started);
            timer.Start();
            Assert.True(timer.Started);
            timer.Stop();
            Assert.True(timer.Started);

            timer = new CodeTimer(typeof(CodeTimerTest), "StartedTest2");
            Assert.False(timer.Started);
            timer.Start();
            Assert.True(timer.Started);
            timer.Fail();
            Assert.True(timer.Started);
        }

        [Fact]
        public void RunningTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "RunningTest");
            Assert.False(timer.Running);
            timer.Start();
            Assert.True(timer.Running);
            timer.Stop();
            Assert.False(timer.Running);

            timer = new CodeTimer(typeof(CodeTimerTest), "RunningTest2");
            Assert.False(timer.Running);
            timer.Start();
            Assert.True(timer.Running);
            timer.Fail();
            Assert.False(timer.Running);
        }

        [Fact]
        public void TimerEventTest()
        {
            CodeTimer timer = new CodeTimer(typeof(CodeTimerTest), "TimerEventTest", true);
            Assert.Equal("testEvent", timer.Event("testEvent").SubeventName);
            timer.Stop();
        }
    }
}
