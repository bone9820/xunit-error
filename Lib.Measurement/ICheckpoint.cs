using System;

namespace Lib.Measurement
{
    public interface ICheckpoint
    {
        String ApplicationName
        {
            get;
        }

        String ClassName
        {
            get;
        }

        String EventName
        {
            get;
        }

        long Offset
        {
            get;
        }

        DateTime ProcessStartTime
        {
            get;
        }

        DateTime GetCurrentOffsetDate
        {
            get;
        }


        String ProjectName
        {
            get;
        }

        String SubeventName
        {
            get;
        }
    }
}