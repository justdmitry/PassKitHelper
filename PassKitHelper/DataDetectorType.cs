namespace PassKitHelper
{
    using System;

    [Flags]
    public enum DataDetectorType
    {
        None = 0,

        PhoneNumber = 1,

        Link = 2,

        Address = 4,

        CalendarEvent = 8,

        All = PhoneNumber | Link | Address | CalendarEvent,
    }
}
