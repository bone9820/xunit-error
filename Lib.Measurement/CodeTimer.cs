using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Lib.Measurement
{
    public class CodeTimer : IDisposable
    {
        private enum TimerState
        {
            Init,
            Running,
            Stopped,
            Failed
        };

        private readonly Type _type;
        private readonly string _eventName;
        private readonly bool _stopOnDispose;
        private TimerState _state = TimerState.Init;

        private long _time = 0;
        private long _stop = 0;

        private const string CodetimerJournalText = "{0}.{1}: {2} ms.";
        private const string CodetimerJournalTextFailed = "{0}.{1}: {2} ms. (FAILED)";

        public CodeTimer(Type type, string eventName)
            : this(type, eventName, false, false)
        { }

        public CodeTimer(Type type, string eventName, bool start)
            : this(type, eventName, start, false)
        { }

        public CodeTimer(MethodBase method, bool start)
            : this(method.DeclaringType, method.Name, start)
        { }

        public CodeTimer(Type type, string eventName, bool start, bool stopOnDispose)
        {
            _type = type;
            _eventName = eventName;
            _stopOnDispose = stopOnDispose;

            if (start)
            {
                Start();
            }
        }

        public void Dispose()
        {
            if (_stopOnDispose) Stop();
            else Fail();
        }

        public ICheckpoint Event(string subeventName)
        {
            return new Checkpoint(_type, _eventName, subeventName);
        }

        public ICheckpoint Start()
        {
            if (_state != TimerState.Init) return null;
            _state = TimerState.Running;
            var result = new Checkpoint(_type, _eventName, "start");
            _time = result.Offset;
            return result;
        }

        public ICheckpoint Stop()
        {
            if (_state != TimerState.Running) return null;
            _state = TimerState.Stopped;
            var result = new Checkpoint(_type, _eventName, "stop");
            _stop = result.Offset;
            _time = _stop - _time;
            return result;
        }

        public ICheckpoint Fail()
        {
            if (_state != TimerState.Running) return null;
            _state = TimerState.Stopped;
            var result = new Checkpoint(_type, _eventName, "fail");
            _stop = result.Offset;
            _time = _stop - _time;
            return result;
        }

        public bool Started => _state != TimerState.Init;

        public bool Running => _state == TimerState.Running;

        public long ElapsedTime
        {
            get
            {
                switch (_state)
                {
                    case TimerState.Init:
                        return 0;
                    case TimerState.Running:
                        return CheckpointData.GetOffset() - _time;
                    case TimerState.Failed:
                    case TimerState.Stopped:
                        return _time;
                    default:
                        return 0;
                }
            }
        }

        public string GetJournalText()
        {
            var journalText = new StringBuilder();
            journalText.AppendFormat(
                _state == TimerState.Failed ? CodetimerJournalTextFailed : CodetimerJournalText, _type.FullName,
                _eventName, ElapsedTime * 1e-6);
            journalText.AppendLine();

            return journalText.ToString();
        }

        public static void Time(Action action)
        {
            using (var timer = new CodeTimer(action.Method, true))
            {
                action();
                timer.Stop();
            }
        }

        public static void Time<T>(Action<T> action, T param)
        {
            using (var timer = new CodeTimer(action.Method, true))
            {
                action(param);
                timer.Stop();
            }
        }

        public static void Time<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
        {
            using (var timer = new CodeTimer(action.Method, true))
            {
                action(param1, param2);
                timer.Stop();
            }
        }

        public static void Time<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
        {
            using (var timer = new CodeTimer(action.Method, true))
            {
                action(param1, param2, param3);
                timer.Stop();
            }
        }

        public static void Time<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            using (var timer = new CodeTimer(action.Method, true))
            {
                action(param1, param2, param3, param4);
                timer.Stop();
            }
        }
    }
}
