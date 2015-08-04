using System;
using System.Diagnostics;

namespace ImageScan
{
    class Utilities
    {
        #region GetModulo10CheckDigit

        /// <summary>
        /// Get modulo 10/3 check digit
        /// </summary>
        /// <param name="parcelNumber">The parcel number to find check digit for</param>
        /// <returns>The check digit</returns>
        public static int GetModulo10CheckDigit(string parcelNumber)
        {
            int sum, counter, checkDigit, remainder;

            sum = 0;
            checkDigit = 3; // NOTE: Modulo 10/3 uses alternating 1/3
            counter = parcelNumber.Length;

            while (counter > 0)
            {
                sum = sum + (int.Parse(parcelNumber.Substring(counter, 1)) * checkDigit);
                checkDigit = checkDigit == 3 ? 1 : 3;
                counter = counter - 1;
            }

            remainder = 10 - int.Parse(sum.ToString().Substring(sum.ToString().Length, 1));

            if (remainder == 10)
                return 0;
            else
                return remainder;
        }

        #endregion

        #region IsParcelNumberPostFinlandStandard

        /// <summary>
        /// Check if a parcel number is Post Finland Standard (format [J]JFINNNNNNNNNNNNNNNNNN)
        /// </summary>
        /// <param name="parcelNumber">The parcel number to check</param>
        /// <returns>True if parcel number is Post Finland Standard, false if not</returns>
        public static bool IsParcelNumberPostFinlandStandard(string parcelNumber)
        {
            if (parcelNumber == null)
                return false;

            parcelNumber = parcelNumber.Trim();

            if (parcelNumber.Length == 0)
                return false;

            try
            {
                if (parcelNumber.ToUpper().Substring(0, 4).Equals("JJFI") ||
                    parcelNumber.ToUpper().Substring(0, 3).Equals("JFI"))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region IsParcelNumberPostStandard

        /// <summary>
        /// Check if a parcel number is Post Standard (format XXNNNNNNNNNXX)
        /// </summary>
        /// <param name="parcelNumber">The parcel number to check</param>
        /// <returns>True if parcel number is Post Standard, false if not</returns>
        public static bool IsParcelNumberPostStandard(string parcelNumber)
        {
            if (parcelNumber == null)
                return false;

            parcelNumber = parcelNumber.Trim();

            if (parcelNumber.Length == 0)
                return false;

            try
            {
                if (parcelNumber.Length == 13 && IsNumeric(parcelNumber.Substring(0, 2)) == false && 
                    IsNumeric(parcelNumber.Substring(2, 9)) == true &&
                    IsNumeric(parcelNumber.Substring(11, 2)) == false)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region IsParcelNumberPostUPU

        /// <summary>
        /// Check if a parcel number is Post UPU (format NNNNNNNNNNNXX)
        /// </summary>
        /// <param name="parcelNumber">The parcel number to check</param>
        /// <returns>True if parcel number is Post UPU, false if not</returns>
        public static bool IsParcelNumberPostUPU(string parcelNumber)
        {
            if (parcelNumber == null)
                return false;

            parcelNumber = parcelNumber.Trim();

            if (parcelNumber.Length == 0)
                return false;

            try
            {
                if (parcelNumber.Length == 13 && IsNumeric(parcelNumber.Substring(0, 11)) == true &&
                    IsNumeric(parcelNumber.Substring(11, 2)) == false)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region IsParcelNumberGS1

        /// <summary>
        /// Check if a parcel number is GS-1 (format 18 or 20 digits)
        /// </summary>
        /// <param name="parcelNumber">The parcel number to check</param>
        /// <returns>True if parcel number is GS-1, false if not</returns>
        public static bool IsParcelNumberGS1(string parcelNumber)
        {
            if (parcelNumber == null)
                return false;

            parcelNumber = parcelNumber.Trim();

            if (parcelNumber.Length == 0)
                return false;

            try
            {
                if (IsNumeric(parcelNumber) && (parcelNumber.Length == 18 ||
                    (parcelNumber.Length == 20 && parcelNumber.Substring(0, 2).Equals("00"))))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region IsNumeric

        /// <summary>
        /// Check if a parcel number string is numeric only
        /// </summary>
        /// <param name="parcelNumber">The parcel number to check</param>
        /// <returns>True if string is numeric only, false if not</returns>
        private static bool IsNumeric(string parcelNumber)
        {
            try
            {
                long result = long.MinValue;

                if (long.TryParse(parcelNumber, out result))
                {
                    return true;
                }
                else
                {
                    return false;
                }

                //foreach (byte b in parcelNumber)
                //{
                //    if (b < 48 || b > 57)
                //        return false;
                //}

                //return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region WriteToEventlog

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToEventlog(string message)
        {
            EventLog log = new EventLog();
            log.Source = "Application";
            log.WriteEntry(message, System.Diagnostics.EventLogEntryType.Error);
            log.Dispose();
        }

        #endregion
    }
}
