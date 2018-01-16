using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Radikool6.Classes
{
    public static class Utility
    {
        public static class Text
        {
            public static DateTime StringToDate(string src)
            {
                src = Regex.Replace(src, "[^0-9]", "");
                if (src.Length < 14)
                {
                    return DateTime.MinValue;
                }

                DateTime res;
                //  src = src.Substring(0, 4) + "/" + src.Substring(4, 2) + "/" + src.Substring(6, 2) + " " + src.Substring(8, 2) + ":" + src.Substring(10, 2) + ":" + src.Substring(12, 2);
                int year = int.TryParse(src.Substring(0, 4), out year) ? year : 0;
                int month = int.TryParse(src.Substring(4, 2), out month) ? month : 0;
                int day = int.TryParse(src.Substring(6, 2), out day) ? day : 0;
                int hour = int.TryParse(src.Substring(8, 2), out hour) ? hour : 0;
                int min = int.TryParse(src.Substring(10, 2), out min) ? min : 0;
                int sec = int.TryParse(src.Substring(12, 2), out sec) ? sec : 0;

                if (year == 0)
                {
                    return DateTime.MinValue;
                }

                res = new DateTime(year, month, day, hour, min, sec, new GregorianCalendar());
                return res;

            }
        }
    }
}