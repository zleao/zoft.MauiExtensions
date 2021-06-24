using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace zoft.MauiExtensions.Core.Extensions
{
    /// <summary>
    /// Extensions for string type
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the specified source string is null or empty.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source string is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Determines whether the specified source string is null or white space.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if he specified source string is null or white space; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Formats the specified source template.
        /// Deals with null templates
        /// </summary>
        /// <param name="sourceTemplate">The source template.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string FormatTemplate(this string sourceTemplate, params object[] args)
        {
            if (string.IsNullOrEmpty(sourceTemplate))
                return sourceTemplate ?? string.Empty;

            return string.Format(sourceTemplate, args);
        }

        /// <summary>
        /// Parses the source string value to enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="source">The source enum value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> ignore case.</param>
        /// <returns></returns>
        public static TEnum Parse2Enum<TEnum>(this string source, bool ignoreCase = false)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), source, ignoreCase);
        }

        /// <summary>
        /// Parses the source string value to decimal.
        /// </summary>
        /// <param name="source">The source value.</param>
        /// <param name="defaultValue">The value if null.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        public static decimal Parse2Decimal(this string source, decimal defaultValue = 0M, bool throwException = false)
        {
            decimal d = defaultValue;

            if (!source.IsNullOrEmpty())
            {
                try
                {
                    d = decimal.Parse(source.Replace(',', '.').Trim(), NumberStyles.Any, new CultureInfo("en-US"));
                }
                catch (Exception ex) when (!throwException)
                {
                    Console.WriteLine($"string.Parse2Decimal - {ex.GetFullDescription()}");
                }
            }

            return d;
        }

        /// <summary>
        /// Parses the source string value to boolean.
        /// </summary>
        /// <param name="source">The source value.</param>
        /// <param name="defaultValue">The default value. Aplicable to null or empty string</param>
        /// <returns></returns>
        public static bool Parse2Boolean(this string source, bool defaultValue = false)
        {
            if (source.IsNullOrEmpty())
                return defaultValue;

            #region Test for true/false string

            if (source.Equals(bool.FalseString))
            {
                return false;
            }

            if (source.Equals(bool.TrueString))
            {
                return true;
            }

            #endregion

            #region Test for integer values

            if (int.TryParse(source, out var intSourceValue))
            {
                return intSourceValue > 0;
            }

            #endregion

            try
            {
                return Convert.ToBoolean(source);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Parses the source string value to int32.
        /// </summary>
        /// <param name="source">The source value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int Parse2Int(this string source, int defaultValue = 0)
        {
            if (source.IsNullOrEmpty())
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(source);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Truncates the specified text.
        /// Deals with null strings
        /// </summary>
        /// <param name="source">The text.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Truncate(this string source, int length)
        {
            if (source.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return source.Length > length ? source.Substring(0, length) : source;
        }

        /// <summary>
        /// Determines whether the specified value is hexadecimal.
        /// Deals with null strings
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified text is hex; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHex(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return HexRegex.IsMatch(source.Trim().ToLower());
        }
        private static readonly Regex HexRegex = new Regex("^[0-9a-f]+$");

        /// <summary>
        /// Determines whether the specified source is numeric.
        /// Deals with null strings
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static bool IsNumeric(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return NumericRegEx.IsMatch(source);
        }
        private static readonly Regex NumericRegEx = new Regex("^\\d+(\\.\\d+)?$");

        /// <summary>
        /// Determines whether the specified source is integer.
        /// Deals with null strings
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static bool IsInteger(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return IntRegEx.IsMatch(source);
        }
        private static readonly Regex IntRegEx = new Regex("^\\d+$");

        /// <summary>
        /// Gets the binary from hex.
        /// Deals with null strings
        /// </summary>
        /// <param name="source">The value.</param>
        /// <returns></returns>
        public static string GetBinaryFromHex(this string source)
        {
            if (!source.IsHex())
                return "";

            return Convert.ToString(Convert.ToInt32(source, 16), 2);
        }

        /// <summary>
        /// Adds a line break to the text, if not empty.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string AddCrIfNotEmpty(this string text)
        {
            return text.IsNullOrEmpty() ? "" : text + Environment.NewLine;
        }
    }
}
