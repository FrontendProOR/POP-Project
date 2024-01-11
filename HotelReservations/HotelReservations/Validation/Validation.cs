using System.Text.RegularExpressions;

namespace HotelReservations.Validation
{
    public class Validation
    {
        public Validation()
        {
        }

        public static bool IsValidJMBG(string jmbg)
        {
            // Regex pattern for Bosnian JMBG
            string regexPattern = @"^\d{13}$";

            if (!Regex.IsMatch(jmbg, regexPattern))
            {
                return false;
            }

            int day = int.Parse(jmbg.Substring(0, 2));
            int month = int.Parse(jmbg.Substring(2, 2));
            int yearPart = int.Parse(jmbg.Substring(4, 3));

            // Validate day, month, and year
            if (day < 1 || day > 31 || month < 1 || month > 12)
            {
                return false;
            }

            int year;
            if (yearPart >= 900)
            {
                year = 1900 + yearPart;
            }
            else
            {
                year = 2000 + yearPart;
            }

            // Additional custom validation logic if needed

            return true;
        }

    }
}
