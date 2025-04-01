using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public static class TimeHelper
    {
        public static string TimeFormatter(this string time)
        {
            // string time = "Aug3, 2022 20:25:00 UTC";

            // Define the input format
            string inputFormat = "MMMd, yyyy HH:mm:ss 'UTC'";

            // Parse the string into a DateTime object
            if (DateTime.TryParseExact(time, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime))
            {
                // Convert to the desired format
                string formattedTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                return formattedTime;
            }
            else
            {
                return time;
            }
        }
    }
}