using Dlrsoft.VBScript.Parser;
using System;
#if !NETSTANDARD
using Microsoft.VisualBasic;
using System.Drawing;
#else
using System.Globalization;
#endif
using System.Linq;
using System.Text;

namespace Dlrsoft.VBScript.Runtime
{
    public class BuiltInFunctions
    {
        const string ZERO = "0"; 

        public static object Abs(object number)
        {
            if (number is Nullable)
                return null;

            if (number is sbyte)
                return Math.Abs((sbyte)number);

            if (number is short)
                return Math.Abs((short)number);

            if (number is int)
                return Math.Abs((int)number);

            if (number is long)
                return Math.Abs((long)number);

            if (number is float)
                return Math.Abs((float)number);

            if (number is double)
                return Math.Abs((double)number);

            if (number is decimal)
                return Math.Abs((decimal)number);

            throw new ArgumentException("Expect numeric argument", "number");
        }

        public static object[] Array(params object[] arglist)
        {
            return arglist;
        }

        public static byte Asc(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("String cannot be null or empty", "s");

            return (byte)s[0];
        }

        public static byte AscB(string s)
        {
            return Asc(s);
        }

        public static char AscW(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("String cannot be null or empty", "s");

            return s[0];
        }

        public static double Atn(object number)
        {
            if (IsNumericInternal(number))
                return Math.Atan(Convert.ToDouble(number));

            throw new ArgumentException("Expect numeric argument", "number");
        }

        public static bool CBool(object expression)
        {
            Type t = expression.GetType();
            if (!t.IsPrimitive)
            {
                if (t.IsCOMObject)
                {
                    expression = HelperFunctions.GetDefaultPropertyValue(expression);
                }
                else if (t == typeof(ErrObject))
                {
                    expression = ((ErrObject)expression).Number;
                }
            }
            //return VB
            return Convert.ToBoolean(expression);
        }

        public static byte CByte(object expression)
        {
            return Convert.ToByte(expression);
        }

        public static decimal CCur(object expression)
        {
            return Convert.ToDecimal(expression);
        }

        public static DateTime CDate(object date)
        {
            return Convert.ToDateTime(date);
        }

        public static double CDbl(object expression)
        {
            return Convert.ToDouble(expression);
        }

        public static char Chr(int charcode)
        {
            return Convert.ToChar(charcode);
        }

        public static short CInt(object expression)
        {
            return Convert.ToInt16(expression);
        }

        public static int CLng(object expression)
        {
            return Convert.ToInt32(expression);
        }

        public static double Cos(object number)
        {
            if (IsNumericInternal(number))
                return Math.Cos(Convert.ToDouble(number));

            throw new ArgumentException("Expect numeric argument", "number");
        }

        public static object CreateObject(string progId)
        {
            return CreateObject(progId, null);
        }


        public static object CreateObject(string progId, string location)
        {
            Type type = Type.GetTypeFromProgID(progId);
            if (type == null)
            {
                throw new SystemException("Unable to locate COM with progId : " + progId);
            }

            if (string.IsNullOrEmpty(location))
            {
                return Activator.CreateInstance(type);
            }
            else
            {
#if !NETSTANDARD
                return Activator.GetObject(type, location);
#else
                throw new NotImplementedException();
#endif
            }
        }

            public static float CSng(object expression)
        {
            return Convert.ToSingle(expression);
        }

        public static string CStr(object expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }
            else if (expression is string @string)
            {
                return @string;
            }
            else if (expression is DateTime time)
            {
                return time.ToShortDateString();
            }
            else if (expression is DBNull)
            {
                throw new ArgumentException("Cannot convert DBNull to string");
            }
            else if (expression is Exception exception)
            {
                return exception.Message;
            }
            else
            {
                return expression.ToString();
            }
        }

        public static DateTime Date()
        {
            return DateTime.Today;
        }

        public static DateTime DateAdd(string interval, object number, object date)
        {
            if (string.IsNullOrEmpty(interval))
            {
                throw new ArgumentException("interval is required.");
            }

            int theNumber= 0;
            if (number == null)
            {
                throw new ArgumentException("number is required.");
            }
            else if (IsNumericInternal(number))
            {
                theNumber = Convert.ToInt32(number);
            }
            else if (number is string @string)
            {
                theNumber  = int.Parse(@string);
            }
            else
            {
                throw new ArgumentException("invalid number.");
            }

            DateTime theDate;
            if (date == null)
            {
                throw new ArgumentException("date is required.");
            }
            else if(date is DBNull)
            {
                throw new ArgumentException("date cannot be dbNull.");
            }
            else if (date is DateTime time)
            {
                theDate = time;
            }
            else if (date is string @string)
            {
                theDate = DateTime.Parse(@string);
            }
            else
            {
                throw new ArgumentException("date must be DateTime or string.");
            }

            switch (interval.ToLower())
            {
                case "yyyy":
                    return theDate.AddYears(theNumber);
                case "q":
                    return theDate.AddMonths(3 * theNumber);
                case "m":
                    return theDate.AddMonths(theNumber);
                case "y":
                case "d":
                case "w":
                    return theDate.AddDays(theNumber);
                case "ww":
                    return theDate.AddDays(7 * theNumber);
                case "h":
                    return theDate.AddHours(theNumber);
                case "n":
                    return theDate.AddMinutes(theNumber);
                case "s":
                    return theDate.AddSeconds(theNumber);
                default:
                    throw new ArgumentException("invalid interval.");
            }
        }

        public static int DateDiff(string interval, object date1, object date2)
        {
            if (string.IsNullOrEmpty(interval))
            {
                throw new ArgumentException("interval is required.");
            }

            DateTime theDate1;
            if (date1 == null)
            {
                throw new ArgumentException("date1 is required.");
            }
            else if (date1 is DBNull)
            {
                throw new ArgumentException("date1 cannot be dbNull.");
            }
            else if (date1 is DateTime)
            {
                theDate1 = (DateTime)date1;
            }
            else if (date1 is string)
            {
                theDate1 = DateTime.Parse((string)date1);
            }
            else
            {
                throw new ArgumentException("date1 must be DateTime or string.");
            }

            DateTime theDate2;
            if (date2 == null)
            {
                throw new ArgumentException("date2 is required.");
            }
            else if (date2 is DBNull)
            {
                throw new ArgumentException("date2 cannot be dbNull.");
            }
            else if (date2 is DateTime)
            {
                theDate2 = (DateTime)date2;
            }
            else if (date2 is string)
            {
                theDate2 = DateTime.Parse((string)date2);
            }
            else
            {
                throw new ArgumentException("date2 must be DateTime or string.");
            }

            TimeSpan timeSpan = theDate2.Subtract(theDate1);

            switch (interval.ToLower())
            {
                case "yyyy":
                    return theDate2.Year - theDate1.Year;
                case "q":
                    return (theDate2.Year - theDate1.Year) * 4 + theDate2.Month/3 - theDate1.Month/3;
                case "m":
                    return (theDate2.Year - theDate1.Year) * 12 + theDate2.Month / 3 - theDate1.Month / 3;
                case "y":
                case "d":
                case "w":
                    return timeSpan.Days;
                case "ww":
                    return timeSpan.Days/7;
                case "h":
                    return timeSpan.Hours;
                case "n":
                    return timeSpan.Minutes;
                case "s":
                    return timeSpan.Seconds;
                default:
                    throw new ArgumentException("invalid interval.");
            }
        }

        public static int DatePart(string interval, object date)
        {
            if (string.IsNullOrEmpty(interval))
            {
                throw new ArgumentException("interval is required.");
            }

            DateTime theDate;
            if (date == null)
            {
                throw new ArgumentException("date is required.");
            }
            else if (date is DBNull)
            {
                throw new ArgumentException("date cannot be dbNull.");
            }
            else if (date is DateTime time)
            {
                theDate = time;
            }
            else if (date is string @string)
            {
                theDate = DateTime.Parse(@string);
            }
            else
            {
                throw new ArgumentException("date must be DateTime or string.");
            }

            switch (interval.ToLower())
            {
                case "yyyy":
                    return theDate.Year;
                case "q":
                    return theDate.Month / 3;
                case "m":
                    return theDate.Month;
                case "y":
                    return theDate.DayOfYear;
                case "d":
                    return theDate.Day;
                case "w":
                    return (int)theDate.DayOfWeek;
                case "ww":
                    return theDate.Day / 7;
                case "h":
                    return theDate.Hour;
                case "n":
                    return theDate.Minute;
                case "s":
                    return theDate.Second;
                default:
                    throw new ArgumentException("invalid interval.");
            }
        }

        public static DateTime DateSerial(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        public static DateTime DateValue(object date)
        {
            if (date is string @string)
            {
                date = DateTime.Parse(@string);
            }

            if (date is DateTime)
            {
                return ((DateTime)date).Date;
            }
            throw new ArgumentException("date must be DateTime value or a string that represents a date.");
        }

        public static object Day(object date)
        {
            if (date is DBNull)
            {
                return date;
            }
            else if (date is string)
            {
                date = DateTime.Parse((string)date);
            }

            if (date is DateTime)
            {
                return ((DateTime)date).Day;
            }
            throw new ArgumentException("date must be DateTime value or a string that represents a date.");
        }

        public static string Escape(object charString)
        {
            return System.Uri.EscapeDataString((string)charString);
        }

        public static object Eval(string expression)
        {
            throw new NotImplementedException("Eval function is not yet implemented.");
        }

        public static object Exp(object number)
        {
            return Math.Exp(Convert.ToDouble(number));
        }

        public static object Filter(object inputstrings, object value)
        {
            return Filter(inputstrings, value, true);
        }

        public static object Filter(object inputstrings, object value, object include)
        {
            return Filter(inputstrings, value, include, BuiltInConstants.vbBinaryCompare);
        }
        
        public static object Filter(object inputstrings, object value, object include, object compare)
        {
            if (!(inputstrings is object[])) throw new ArgumentException("inputstrings must be an array.");
            StringComparison comparison = (int)compare == 0 ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;

            if ((bool)include)
            {
                return ((object[])inputstrings).Where<object>(e =>
                    (Convert.ToString(e)).IndexOf(Convert.ToString(value), comparison) > -1);
            }
            else
            {
                return ((object[])inputstrings).Where<object>(e =>
                    (Convert.ToString(e)).IndexOf(Convert.ToString(value), comparison) == -1);
            }
        }

        public static object Fix(object number)
        {
#if !NETSTANDARD
            return Conversion.Fix(number);
#else
            return Math.Truncate((decimal)number);
#endif
        }

#if !NETSTANDARD
        public static object FormatCurrency(object expression)
        {
            return FormatCurrency(expression, -1);
        }

        public static object FormatCurrency(object expression, object numDigitsAfterDecimal)
        {
            return FormatCurrency(expression, numDigitsAfterDecimal, BuiltInConstants.vbUseDefault);
        }

        public static object FormatCurrency(object expression, object numDigitsAfterDecimal, object includeLeadingDigit)
        {
            return FormatCurrency(expression, numDigitsAfterDecimal, includeLeadingDigit, BuiltInConstants.vbUseDefault);
        }

        public static object FormatCurrency(object expression, object numDigitsAfterDecimal, object includeLeadingDigit, object useParentsForNegativeNumbers)
        {
            return FormatCurrency(expression, numDigitsAfterDecimal, includeLeadingDigit, useParentsForNegativeNumbers, BuiltInConstants.vbUseDefault);
        }

        public static object FormatCurrency(object expression, object numDigitsAfterDecimal, object includeLeadingDigit, object useParentsForNegativeNumbers, object groupDigits)
        {
            return Strings.FormatCurrency(expression, (int)numDigitsAfterDecimal, (TriState)includeLeadingDigit, (TriState)useParentsForNegativeNumbers, (TriState)groupDigits);
        }
#endif

        public static object FormatDateTime(object date, object namedFormat)
        {
#if !NETSTANDARD
            return Strings.FormatDateTime((DateTime)date, (DateFormat)namedFormat);
#else
            // TODO: Implement format
            return Convert.ToDateTime(date).ToString();
#endif
        }
#if !NETSTANDARD

        public static object FormatNumber(object expression)
        {
            return FormatNumber(expression, -1);
        }

        public static object FormatNumber(object expression, object numberDigistAfterDecimal)
        {
            return FormatNumber(expression, numberDigistAfterDecimal, BuiltInConstants.vbUseDefault);
        }

        public static object FormatNumber(object expression, object numberDigistAfterDecimal, object includeLeadingDigits)
        {
            return FormatNumber(expression, numberDigistAfterDecimal, includeLeadingDigits, BuiltInConstants.vbUseDefault);
        }

        public static object FormatNumber(object expression, object numberDigistAfterDecimal, object includeLeadingDigits, object userParentsForNegativeNumbers)
        {
            return FormatNumber(expression, numberDigistAfterDecimal, includeLeadingDigits, userParentsForNegativeNumbers, BuiltInConstants.vbUseDefault);
        }

        public static object FormatNumber(object expression, object numberDigistAfterDecimal, object includeLeadingDigits, object userParentsForNegativeNumbers, object groupDigits)
        {
            return Strings.FormatNumber(expression, (int)numberDigistAfterDecimal, (TriState)includeLeadingDigits, (TriState)userParentsForNegativeNumbers, (TriState)groupDigits);
        }

        public static object FormatPercent(object expression)
        {
            return FormatPercent(expression, -1);
        }

        public static object FormatPercent(object expression, object numberDigistAfterDecimal)
        {
            return FormatPercent(expression, numberDigistAfterDecimal, BuiltInConstants.vbUseDefault);
        }

        public static object FormatPercent(object expression, object numberDigistAfterDecimal, object includeLeadingDigits)
        {
            return FormatPercent(expression, numberDigistAfterDecimal, includeLeadingDigits, BuiltInConstants.vbUseDefault);
        }

        public static object FormatPercent(object expression, object numberDigistAfterDecimal, object includeLeadingDigits, object userParentsForNegativeNumbers)
        {
            return FormatPercent(expression, numberDigistAfterDecimal, includeLeadingDigits, userParentsForNegativeNumbers, BuiltInConstants.vbUseDefault);
        }

        public static object FormatPercent(object expression, object numberDigistAfterDecimal, object includeLeadingDigits, object userParentsForNegativeNumbers, object groupDigits)
        {
            return Strings.FormatPercent(expression, (int)numberDigistAfterDecimal, (TriState)includeLeadingDigits, (TriState)userParentsForNegativeNumbers, (TriState)groupDigits);
        }
#endif

        public static object GetLocale()
            => System.Globalization.CultureInfo.CurrentCulture.LCID;

#if !NETSTANDARD
        public static object GetObject(object pathname)
        {
            return GetObject(pathname, null);
        }

        public static object GetObject(object pathname, object className)
        {
            return Interaction.GetObject((string)pathname, (string)className);
        }
#endif

        public static object GetRef(object procName)
        {
            throw new NotImplementedException("GetRef function is not implemendted.");
        }

        public static object Hex(object number)
        {
#if !NETSTANDARD
            return Conversion.Hex(number);
#else
            if (number == null)
            {
                return ZERO;
            }
            else if (number is DBNull)
            {
                return number;
            }
            else
            {
                return string.Format("{0:X}", number);
            }
#endif
        }

        public object Hour(object time)
        {
#if !NETSTANDARD
            return DateAndTime.Hour(Convert.ToDateTime(time));
#else
            return Convert.ToDateTime(time).Hour;
#endif
        }

#if !NETSTANDARD
        public object InputBox(object prompt)
        {
            return InputBox(prompt, null, null, null, null);
        }

        public object InputBox(object prompt, object title, object defaultResponse, object xpos, object ypos)
        {
            return InputBox(prompt, title, defaultResponse, xpos, ypos, null, null);
        }

        public object InputBox(object prompt, object title, object defaultResponse, object xpos, object ypos, object helpfile, object context)
        {
            return Interaction.InputBox(Convert.ToString(prompt), Convert.ToString(title), Convert.ToString(defaultResponse), Convert.ToInt32(xpos), Convert.ToInt32(ypos)); 
        }
#endif
        public static object Instr(object string1, object string2)
        {
            return Instr((object)1, string1, string2, BuiltInConstants.vbBinaryCompare);
        }

        public static object Instr(object arg1, object arg2, object arg3)
        {
            if (IsNumericInternal(arg1))
            {
                //arg1 is start
                return Instr(arg1, arg2, arg3, BuiltInConstants.vbBinaryCompare);
            }
            else
            {
                return Instr((object)1, arg1, arg2, arg3);
            }
        }

        public static object Instr(object start, object string1, object string2, object compare)
        {
            if (string1 == null || string2 == null) return null;

            if (string1.Equals(string.Empty)) return 0;

            if (string2.Equals(string.Empty)) return start;

            string1 = Convert.ToString(string1);
            string2 = Convert.ToString(string2);
            if (Convert.ToInt32(start) > ((string)string1).Length) return 0;

            return ((string)string1).IndexOf((string)string2,
                Convert.ToInt32(start) - 1,
                compare.Equals(BuiltInConstants.vbBinaryCompare) ?
                StringComparison.InvariantCulture :
                StringComparison.InvariantCultureIgnoreCase) + 1;
        }

        public static object InstrB(object string1, object string2) 
            => InstrB((object)1, string1, string2, BuiltInConstants.vbBinaryCompare);

        public static object InstrB(object arg1, object arg2, object arg3)
        {
            if (IsNumericInternal(arg1))
            {
                //arg1 is start
                return InstrB(arg1, arg2, arg3, BuiltInConstants.vbBinaryCompare);
            }
            else
            {
                return InstrB((object)1, arg1, arg2, arg3);
            }
        }

        public static object InstrB(object start, object string1, object string2, object compare)
        {
            if (string1 == null || string2 == null) return null;

            if (string1.Equals(string.Empty)) return 0;

            if (string2.Equals(string.Empty)) return start;

            string1 = Convert.ToString(string1);
            string2 = Convert.ToString(string2);
            if (Convert.ToInt32(start) > ((string)string1).Length) return 0;

            return ((string)string1).IndexOf((string)string2,
                Convert.ToInt32(start) - 1,
                compare.Equals(BuiltInConstants.vbBinaryCompare) ?
                StringComparison.InvariantCulture :
                StringComparison.InvariantCultureIgnoreCase) + 1;
        }
        public static object InStrRev(object string1, object string2)
        {
            return InStrRev(string1, string2, -1);
        }

        public static object InStrRev(object string1, object string2, object start)
        {
            return InStrRev(string1, string2, start, BuiltInConstants.vbBinaryCompare);
        }
        public static object InStrRev(object string1, object string2, object start, object compare)
        {
#if !NETSTANDARD
            return Strings.InStrRev(Convert.ToString(string1), Convert.ToString(string2), Convert.ToInt32(start), (CompareMethod)compare); 
#else
            if (string1 == DBNull.Value || string1 == null || string2 == DBNull.Value || string2 == null)
                return null;
            var startIndex = Convert.ToInt32(start);
            var instance = Convert.ToString(string1);
            if (startIndex == -1)
                startIndex = Convert.ToString(string1).Length;
            else if (startIndex > instance.Length)
                return 0;
            else if (startIndex == 0 || startIndex < -1)
                throw new ArgumentOutOfRangeException(nameof(start));
            startIndex -= 1;
            return instance.LastIndexOf(
                Convert.ToString(string2),
                startIndex,
                compare.Equals(BuiltInConstants.vbBinaryCompare) ?
                    StringComparison.InvariantCulture :
                    StringComparison.InvariantCultureIgnoreCase
            ) + 1;
#endif

        }
        public static object Int(object number)
        {
#if !NETSTANDARD
            return Conversion.Int(number);
#else
            return Convert.ToInt32(number);
#endif
        }


        public static object IsArray(object varname)
        {
#if !NETSTANDARD
            return Information.IsArray(varname);
#else
            return varname.GetType().IsArray;
#endif
        }

        public static object IsDate(object expression)
        {
#if !NETSTANDARD
            return Information.IsDate(expression);
#else
            return expression is DateTime;
#endif
        }

        public static object IsEmpty(object expression)
        {
            return (expression == null);
        }

        public static object IsNull(object expression)
        {
            return (expression is DBNull);
        }

        public static object IsNumeric(object expression)
        {
#if !NETSTANDARD
            return Information.IsNumeric(expression);
#else
            return IsNumericInternal(expression);
#endif
        }

        public static object IsObject(object expression)
        {
            return expression.GetType().IsCOMObject;
        }

        public static object Join(object list, object delimiter)
        {
#if !NETSTANDARD
            if (list is string[])
                return Strings.Join((string[])list, Convert.ToString(delimiter));
            else
                return Strings.Join((object[])list, Convert.ToString(delimiter));
#else
            if (list is string[] strings)
                return string.Join(Convert.ToString(delimiter), strings);
            else
                return string.Join(Convert.ToString(delimiter), (object[])list);
#endif
        }

        public static object LBound(object arrayname)
        {
            return LBound(arrayname, 1);
        }

        public static object LBound(object arrayname, object dimension)
        {
            if (arrayname == null) throw new ArgumentException("arrayname is required");

            if (dimension == null) throw new ArgumentException("dimension is required");

            if (arrayname.GetType().IsArray)
            {
                return ((Array)arrayname).GetLowerBound(Convert.ToInt32(dimension) - 1);
            }
            else
            {
                throw new ArgumentException("parameter is not an array.");
            }
        }

        public static object LCase(object expression)
        {
#if !NETSTANDARD
            if (expression is char)
                return Strings.LCase((char)expression);
            else
                return Strings.LCase(Convert.ToString(expression));
#else
            if (expression is char @char)
                return @char.ToString().ToLower();
            else
                return Convert.ToString(expression).ToLower();
#endif
        }

        public static object Left(object str, object length)
        {
            if (str == null || str is DBNull) return str;

            return (Convert.ToString(str)).Substring(Convert.ToInt32(length));
        }

        public static object Len(object str)
        {
#if !NETSTANDARD
            return Strings.Len(Convert.ToString(str));
#else
            return Convert.ToString(str).Length;
#endif
        }

        public static object LoadPicture(object picturename)
        {
            throw new NotImplementedException("LoadPicture function is not implemented");
        }

        public static object Log(object number)
        {
            return Math.Log(Convert.ToDouble(number));
        }


        public static object LTrim(object str)
        {
            if (str == null || str is DBNull) return str;
#if !NETSTANDARD
            return Strings.LTrim(Convert.ToString(str));
#else

            return Convert.ToString(str).TrimStart();
#endif
        }

            public static object Mid(object str, object start)
        {
            if (str == null || str is DBNull) return str;

            return Convert.ToString(str).Substring(Convert.ToInt32(start) - 1);
        }

        public static object Mid(object str, object start, object length)
        {
            if (str == null || str is DBNull) return str;

            return Convert.ToString(str).Substring(Convert.ToInt32(start) - 1, Convert.ToInt32(length));
        }

        public static object MidB(object str, object start)
        {
            if (str == null || str is DBNull) return str;

            return Convert.ToString(str).Substring(Convert.ToInt32(start) - 1);
        }

        public static object MidB(object str, object start, object length)
        {
            if (str == null || str is DBNull) return str;

            return Convert.ToString(str).Substring(Convert.ToInt32(start) - 1, Convert.ToInt32(length));
        }

        public static object Minute(object time)
        {
#if !NETSTANDARD
            return DateAndTime.Minute(Convert.ToDateTime(time));
#else
            return Convert.ToDateTime(time).Minute;
#endif
        }

        public static object Month(object date)
        {
#if !NETSTANDARD
            return DateAndTime.Month(Convert.ToDateTime(date));
#else
            return Convert.ToDateTime(date).Month;
#endif
        }

        public static object MonthName(object month)
        {
            return MonthName(month, false);
        }

        public static object MonthName(object month, object abbreviate)
        {
#if !NETSTANDARD
            return DateAndTime.MonthName(Convert.ToInt32(month), Convert.ToBoolean(abbreviate));
#else
            if (Convert.ToBoolean(abbreviate))
                return DateTimeFormatInfo.GetInstance(null).GetAbbreviatedMonthName(Convert.ToInt32(month));
            else
                return DateTimeFormatInfo.GetInstance(null).GetMonthName(Convert.ToInt32(month));
#endif
        }

#if !NETSTANDARD
        public static object MsgBox(object prompt)
        {
            return MsgBox(prompt, BuiltInConstants.vbOKOnly);
        }

        public static object MsgBox(object prompt, object buttons)
        {
            return MsgBox(prompt, buttons, string.Empty);
        }

        public static object MsgBox(object prompt, object buttons, object title)
        {
            return MsgBox(prompt, buttons, title, null, null);
        }

        public static object MsgBox(object prompt, object buttons, object title, object helpfile, object context)
        {
            return Interaction.MsgBox(prompt, (MsgBoxStyle)buttons, title);
        }
#endif
        public static DateTime Now()
        {
            return DateTime.Now;
        }

        public static object Oct(object number)
        {
            if (number == null)
            {
                return ZERO;
            }
            else if (number is DBNull)
            {
                return number;
            }
            else
            {
                return Convert.ToString(Convert.ToInt32(number), 8);
            }
        }

        public static object Replace(object expression, object find, object replacement)
        {
            return Replace(expression, find, replacement, 1);
        }

        public static object Replace(object expression, object find, object replacement, object start)
        {
            return Replace(expression, find, replacement, start, -1);
        }

        public static object Replace(object expression, object find, object replacement, object start, object count)
        {
            return Replace(expression, find, replacement, start, count, BuiltInConstants.vbBinaryCompare);
        }

        public static object Replace(object expression, object find, object replacement, object start, object count, object compare)
        {
#if !NETSTANDARD
            return Strings.Replace(Convert.ToString(expression), Convert.ToString(find), Convert.ToString(replacement), Convert.ToInt32(start), Convert.ToInt32(count), (CompareMethod)compare);
#else
            var expressionS = Convert.ToString(expression);
            var findS = Convert.ToString(find);
            var replacementS = Convert.ToString(replacement);
            var startI = Convert.ToInt32(start);
            var countI = Convert.ToInt32(count);
            if (countI < -1)
                throw new ArgumentException("Argument must be grater than or equal to -1", nameof(expression));
            if (startI <= 0)
                throw new ArgumentException("Must be greater than zero.", nameof(start));
            if (expressionS == null || startI > expressionS.Length)
                return null;
            if (startI != 1)
                expressionS = expressionS.Substring(startI - 1);
            if (findS == null || findS.Length == 0 || countI == 0)
                return expressionS;
            if (countI == -1)
                countI = expressionS.Length;
            var expressionLength = expressionS.Length;
            var findLength = findS.Length;
            int findLocation;
            int replacements = 0;
            CompareInfo compareInfo;
            CompareOptions compareOptions;
            var builder = new StringBuilder(expressionLength);
            if (Convert.ToInt32(compare) == BuiltInConstants.vbTextCompare)
            {
                compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType;
            }
            else
            {
                compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                compareOptions = CompareOptions.Ordinal;
            }

            while(startI < expressionLength)
            {
                if (replacements == countI)
                {
                    builder.Append(expressionS.Substring(startI));
                    break;
                }
                findLocation = compareInfo.IndexOf(expressionS, findS, startI, compareOptions);
                if (findLocation < 0)
                {
                    builder.Append(expressionS.Substring(startI));
                    break;
                }
                else
                {
                    builder.Append(expressionS.Substring(startI, findLocation - startI));
                    builder.Append(replacementS);
                    replacements += 1;
                    startI = findLocation + findLength;
                }
            }

            return builder.ToString();
            
#endif
        }

        public static object RGB(object red, object green, object blue)
        {
#if !NETSTANDARD
            return Information.RGB(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
#else
            return Math.Min(Convert.ToInt32(red), 255) 
                + (Math.Min(Convert.ToInt32(green), 255) * 256) 
                + (Math.Min(Convert.ToInt32(blue), 255) * 65536);
#endif
        }

        public static object Right(object str, object length)
        {
            if (str == null || str is DBNull) return str;

            str = Convert.ToString(str);

            int strLength = ((string)str).Length;
            int retLength = Convert.ToInt32(length);

            if (retLength > strLength)
                return str;
            else
                return ((string)str).Substring(strLength - retLength, retLength);
        }

#if !NETSTANDARD
        public static object Rnd()
        {
            return VBMath.Rnd();
        }

        public static object Rnd(object number)
        {
            return VBMath.Rnd((float)number);
        }
#endif

        public static object Round(object expression, object numdecimalplaces)
        {
            return Math.Round(Convert.ToDouble(expression));
        }

#if !NETSTANDARD
        public static object RTrim(object str)
        {
            return Strings.RTrim((string)str);
        }
#endif

        public static object ScriptEngine()
        {
            return "VBScript";
        }

        public static object ScriptEngineBuildVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
        }

        public static object ScriptEngineMajorVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
        }

        public static object ScriptEngineMinorVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
        }


        public static object Second(object time)
        {
#if !NETSTANDARD
            return DateAndTime.Second(Convert.ToDateTime(time));
#else
            return Convert.ToDateTime(time).Second;
#endif
        }


        public static object SetLocale(object lcid)
        {
#if !NETSTANDARD
            int prevLcid = (int)GetLocale();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo((int)lcid);
            return prevLcid;
#else
            string prevLcid = (string)GetLocale();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo((string)lcid);
            return prevLcid;
#endif
        }

        public static object Sgn(object number)
        {
            return Math.Sign(Convert.ToDouble(number));
        }

        public static object Sin(object number)
        {
            return Math.Sin(Convert.ToDouble(number));
        }

        public static object Space(object number)
        {
#if !NETSTANDARD
            return Strings.Space(Convert.ToInt32(number));
#else
            int count = Convert.ToInt32(number);
            if(count >= 0)
                return new String(Chr(32), count);
            throw new ArgumentException("Must be greater than zero.", nameof(number));
#endif
        }
        public static object Split(object expression)
        {
            return Split(expression, " ");
        }

        public static object Split(object expression, object delimiter)
        {
            return Split(expression, delimiter, -1);
        }

        public static object Split(object expression, object delimiter, object count)
        {
            return Split(expression, delimiter, count, BuiltInConstants.vbBinaryCompare);
        }
        
        public static object Split(object expression, object delimiter, object count, object compare)
        {
#if !NETSTANDARD
            return Strings.Split(Convert.ToString(expression), Convert.ToString(delimiter), Convert.ToInt32(count), (CompareMethod)compare);
#else
            return Convert.ToString(expression).Split(new[] { Convert.ToChar(delimiter) }, Convert.ToInt32(count));
#endif

        }


        public static object Sqr(object number)
        {
            return Math.Sqrt(Convert.ToDouble(number));
        }

#if !NETSTANDARD
        public static object StrComp(object string1, object string2)
        {
            return StrComp(string1, string2, BuiltInConstants.vbBinaryCompare);
        }

        public static object StrComp(object string1, object string2, object compare)
        {
            return Strings.StrComp(Convert.ToString(string1), Convert.ToString(string2), (CompareMethod)compare);
        }

        public static object String(object number, object character)
        {
            if (character is char)
                return Strings.StrDup(Convert.ToInt32(number), (char)character);
            else if (character is string)
                return Strings.StrDup(Convert.ToInt32(number), (string)character);
            else
                return Strings.StrDup(Convert.ToInt32(number), character);
        }

        public static object StrReverse(object string1)
        {
            return Strings.StrReverse(Convert.ToString(string1));
        }
#endif

        public static object Tan(object number)
        {
            return Math.Tan(Convert.ToDouble(number));
        }

#if !NETSTANDARD
        public static object Time()
        {
            return DateAndTime.TimeOfDay;
        }

        public static object Timer()
        {
            return DateAndTime.Timer;
        }

        public static object TimeSerial(object hour, object minute, object second)
        {
            return DateAndTime.TimeSerial(Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(second));
        }

        public static object TimeValue(object time)
        {
            return DateAndTime.TimeValue(Convert.ToString(time));
        }
#endif

        public static object Trim(object str)
        {
#if !NETSTANDARD
            return Strings.Trim(Convert.ToString(str));
#else
            return Convert.ToString(str).Trim();
#endif
        }

        public static object TypeName(object varname)
        {
#if !NETSTANDARD
            return Information.TypeName(varname);
#else
            if (varname == null) return null;
            return varname.GetType().Name;
#endif
        }

        public static object UBound(object arrayname)
        {
            return UBound(arrayname, 1);
        }

        public static object UBound(object arrayname, object dimension)
        {
            if (arrayname == null) throw new ArgumentException("arrayname is required");

            if (dimension == null) throw new ArgumentException("dimension is required");

            if (arrayname.GetType().IsArray)
            {
                return ((Array)arrayname).GetUpperBound(Convert.ToInt32(dimension) - 1);
            }
            else
            {
                throw new ArgumentException("parameter is not an array.");
            }
        }

        public static object UCase(object expression)
        {
            if (expression is char) {
#if !NETSTANDARD
                return Strings.UCase((char)expression);
#else
                return ((char)expression).ToString().ToUpper();
#endif
            } else {
#if !NETSTANDARD
                return Strings.UCase(Convert.ToString(expression));
#else
                return Convert.ToString(expression).ToUpper();
#endif
            }
        }

        public static object Unescape(object charString)
        {
            if (charString == null) return null;

            return System.Uri.UnescapeDataString(Convert.ToString(charString));
        }

        public static object VarType(object varname)
        {
            if (varname == null)
            {
                return BuiltInConstants.vbEmpty;
            }
            else if (varname is DBNull)
            {
                return BuiltInConstants.vbNull;
            }
            else if (varname is short)
            {
                return BuiltInConstants.vbInteger;
            }
            else if (varname is int)
            {
                return BuiltInConstants.vbLong;
            }
            else if (varname is float)
            {
                return BuiltInConstants.vbSingle;
            }
            else if (varname is double)
            {
                return BuiltInConstants.vbDouble;
            }
            else if (varname is decimal)
            {
                return BuiltInConstants.vbCurrency;
            }
            else if (varname is DateTime)
            {
                return BuiltInConstants.vbDate;
            }
            else if (varname is string)
            {
                return BuiltInConstants.vbString;
            }
            else if (varname.GetType().IsCOMObject)
            {
                return BuiltInConstants.vbObject;
            }
            else if (varname is Exception)
            {
                return BuiltInConstants.vbError;
            }
            else if (varname is bool)
            {
                return BuiltInConstants.vbBoolean;
            }
            else if (varname is byte)
            {
                return BuiltInConstants.vbByte;
            }
            else if (varname.GetType().IsArray)
            {
                return BuiltInConstants.vbArray;
            }
            else
            {
                return BuiltInConstants.vbVariant;
            }
            //todo: vbVaraint is not quite right since it is supposed for array of variant
            //todo: vbDataObject not implemented
        }

#if !NETSTANDARD
        public static object Weekday(object date)
        {
            return DateAndTime.Weekday(Convert.ToDateTime(date), (FirstDayOfWeek)BuiltInConstants.vbSunday);
        }

        public static object Weekday(object date, object firstdayofweek)
        {
            return DateAndTime.Weekday(Convert.ToDateTime(date), (FirstDayOfWeek)firstdayofweek);
        }

        public static object WeekdayName(object weekday)
        {
            return WeekdayName(weekday, false);
        }

        public static object WeekdayName(object weekday, object abbreviate)
        {
            return WeekdayName(weekday, abbreviate, BuiltInConstants.vbSunday);
        }

        public static object WeekdayName(object weekday, object abbreviate, object firstdayofweek)
        {
            return DateAndTime.WeekdayName(Convert.ToInt32(weekday), Convert.ToBoolean(abbreviate), (FirstDayOfWeek)firstdayofweek);
        }
#endif

        public static object Year(object date)
        {
#if !NETSTANDARD
            return DateAndTime.Year(Convert.ToDateTime(date));
#else
            return Convert.ToDateTime(date).Year;
#endif
        }

        private static bool IsNumericInternal(object number)
        {
            if (number == null) return false;

            if (number is Nullable || number is sbyte || number is short || number is long || number is float || number is double || number is decimal)
                return true;

            double result;
            return double.TryParse(number.ToString(), out result);
        }
    }
}
