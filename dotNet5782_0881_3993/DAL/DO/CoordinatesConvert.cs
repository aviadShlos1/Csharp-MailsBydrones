using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class CoordinatesConvert
    {
        #region Convert decima to sexagesimal function (Bonus)

        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert">The number to convert</param>
        /// <param name="side"></param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertDecimalDegreesToSexagesimal(double decimalValueToConvert, string LongOrLat)
        {
            string direction = null;
            if (LongOrLat == "Longitude")
            {
                if (decimalValueToConvert >= 0)
                    direction = "N";
                else direction = "S";
            }

            if (LongOrLat == "Latitude")
            {
                if (decimalValueToConvert >= 0)
                    direction = "E";
                else direction = "W";
            }
            int sec = (int)Math.Round(decimalValueToConvert * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;

            return string.Format("{0}° {1}' {2}'' {3}", Math.Abs(deg), Math.Abs(min), Math.Abs(sec), direction);// return the complited number
        }

        #endregion Convert decima to sexagesimal function (Bonus)
    }
}
