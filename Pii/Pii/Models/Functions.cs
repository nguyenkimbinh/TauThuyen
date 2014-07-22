using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pii.Models
{
    public class Functions
    {
        public static string DateDiffInWords(DateTime dateTimeBegin, DateTime dateTimeEnd)
        {
            TimeSpan diff = dateTimeBegin.Subtract(dateTimeEnd);

            if (diff.Days > 10)
            {
                string s = dateTimeEnd.ToString();
                return s;
            }
            if (diff.Days > 1)
                return string.Format("{0} days ago", Convert.ToInt32(diff.Days));
            if (diff.Days == 1)
                return "Yesterday";
            if (diff.Hours > 1)
                return string.Format("{0} hours ago", Convert.ToInt32(diff.Hours));
            if (diff.Minutes > 1)
                return string.Format("{0} minutes ago", Convert.ToInt32(diff.Minutes));
            if (diff.Seconds > 1)
                return string.Format("{0} seconds ago", Convert.ToInt32(diff.Seconds));

            //we should not reach here
            return string.Empty;
        }
    }
}