using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Attribute
{
    class StartTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.UtcNow;
        }
    }
}
