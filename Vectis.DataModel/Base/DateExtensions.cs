using System.Collections.Generic;
using System.Linq;
using Vectis.DataModel;

namespace System
{
    /// <summary>
    /// A partial class of extensions to <see cref="DateTime"/>.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Truncates the time time from a date/time.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime TruncateTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }


        /// <summary>
        /// First day of the date's month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }


        /// <summary>
        /// Last day of the date's month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return FirstDayOfMonth(date).AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// Modifies a date according to the given convention, applying the (optional) supplied list of dates as a holiday calendar.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="convention">The daycount convention to be applied.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime Modify(this DateTime date, Convention convention, SortedSet<DateTime> calendar = null)
        {
            DateTime newDate = date.TruncateTime();
            calendar ??= new SortedSet<DateTime>();

            if (convention == Convention.None)
            {
                return newDate;
            }
            else if (convention == Convention.Preceding)
            {
                while (newDate.Date.DayOfWeek == DayOfWeek.Saturday ||
                    newDate.Date.DayOfWeek == DayOfWeek.Sunday ||
                    calendar.Contains(newDate))
                {
                    if (newDate.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        newDate = newDate.AddDays(-2);
                    }
                    else if (newDate.Date.DayOfWeek == DayOfWeek.Monday)
                    {
                        newDate = newDate.AddDays(-3);
                    }
                    else
                    {
                        newDate = newDate.AddDays(-1);
                    }
                }
            }
            else if (convention == Convention.Following)
            {
                while (newDate.Date.DayOfWeek == DayOfWeek.Saturday ||
                    newDate.Date.DayOfWeek == DayOfWeek.Sunday ||
                    calendar.Contains(newDate))
                {
                    if (newDate.Date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        newDate = newDate.AddDays(2);
                    }
                    else if (newDate.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        newDate = newDate.AddDays(3);
                    }
                    else
                    {
                        newDate = newDate.AddDays(1);
                    }
                }
            }
            else if (convention == Convention.ModifiedFollowing)
            {
                DateTime followingDate = newDate.Modify(Convention.Following, calendar);

                if (followingDate.Date.Month == newDate.Month)
                {
                    newDate = followingDate;
                }
                else
                {
                    newDate = newDate.Modify(Convention.Preceding, calendar);
                }
            }
            else
            {
                throw new ArgumentException("'" + convention.ToString() + "' is an invalid convention");
            }

            return newDate;
        }


        /// <summary>
        /// Adds a number of years to a date applying optional daycount convention and holiday calendar.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="years">The number of years to advance or regress if negative.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddYears(this DateTime date, int years, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddYears(years).Modify(convention, calendar);
        }


        /// <summary>
        /// Adds a number of months to a date applying optional daycount convention and holiday calendar.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="months">The number of months to advance or regress if negative.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddMonths(this DateTime date, int months, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddMonths(months).Modify(convention, calendar);
        }


        /// <summary>
        /// Adds a number of weeks to a date applying optional daycount convention and holiday calendar.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weeks">The number of weeks to advance or regress if negative.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddWeeks(this DateTime date, int weeks, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddDays(weeks*7).Modify(convention, calendar);
        }


        /// <summary>
        /// Adds a number of days to a date applying optional daycount convention and holiday calendar.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days">The number of days to advance or regress if negative.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddDays(this DateTime date, int days, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddDays(days).Modify(convention, calendar);
        }


        /// <summary>
        /// Adds a number of business days to a date applying optional daycount convention and holiday calendar.
        /// </summary>
        /// <param name="businessDays"></param>
        /// <param name="days">The number of business days to advance or regress if negative.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddBusinessDays(this DateTime date, int businessDays, SortedSet<DateTime> calendar = null)
        {
            DateTime newDate = date.TruncateTime();

            if (businessDays < 0)
            {
                for (int i = 0; i < -businessDays; i++)
                {
                    newDate = newDate.AddDays(-1, Convention.Preceding, calendar);
                }
            }
            else
            {
                for (int i = 0; i < businessDays; i++)
                {
                    newDate = newDate.AddDays(1, Convention.Following, calendar);
                }
            }

            return newDate;
        }


        /// <summary>
        /// Returns the next IMM futures date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NextIMM(this DateTime date)
        {
            DateTime newDate = date.TruncateTime();
            DateTime nextIMM = newDate;
            int quarter = (nextIMM.Month - 1) / 3 + 1;

            nextIMM = new DateTime(nextIMM.Year, quarter * 3, 1);

            if ((int)nextIMM.DayOfWeek < 4)
            {
                nextIMM = nextIMM.AddDays(17 - (int)nextIMM.DayOfWeek);
            }
            else
            {
                nextIMM = nextIMM.AddDays(24 - (int)nextIMM.DayOfWeek);
            }

            if (nextIMM <= newDate)
            {
                return nextIMM.AddMonths(1).NextIMM();
            }

            return nextIMM;
        }


        /// <summary>
        /// Subtracts the given period expressed as a string.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="periodString">A case insensitive string representing a period as a set of concatenated elements each conforming to a [number][period type] pattern.
        /// The period type can be "y" for years, "m" of months, "w" for weeks, "d" for days, "b" for business days and either "i" or"f" for IMM futures periods.
        /// Examples are "1y3m2d", "5w3d", "8bd".</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime SubtractPeriod(this DateTime date, string periodString, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.AddPeriod(periodString, convention, calendar, true);
        }


        /// <summary>
        /// Adds the given period expressed as a string.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="periodString">A case insensitive string representing a period as a set of concatenated elements each conforming to a [number][period type] pattern.
        /// The period type can be "y" for years, "m" of months, "w" for weeks, "d" for days, "b" for business days and either "i" or"f" for IMM futures periods.
        /// Examples are "1y3m2d", "5w3d", "8bd".</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddPeriod(this DateTime date, string periodString, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.AddPeriod(periodString, convention, calendar, false);
        }


        /// <summary>
        /// Private function adds or subtracts the given period expressed as a string.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="periodString">A case insensitive string representing a period as a set of concatenated elements each conforming to a [number][period type] pattern.
        /// The period type can be "y" for years, "m" of months, "w" for weeks, "d" for days, "b" for business days and either "i" or"f" for IMM futures periods.
        /// Examples are "1y3m2d", "5w3d", "8bd".</param>
        /// <param name="convention">See <see cref="Convention">.</param>
        /// <param name="calendar">The holiday calendar.</param>
        /// <param name="negate">Subtracts the period if True, otherwise adds it.</param>
        private static DateTime AddPeriod(this DateTime date, string periodString, Convention convention, SortedSet<DateTime> calendar, bool negate)
        {
            DateTime newDate = date.TruncateTime();;
            string[] split = periodString.Split(new Char[] { 'Y', 'M', 'W', 'D', 'B', 'I', 'F' });
            int location = -1;
            string previousString = "";
            bool previousBlank = false;

            foreach (string s in split)
            {
                if (s.Length == 0)
                {
                    previousBlank = true;
                    continue;
                }

                if (previousBlank || !int.TryParse(s, out int period))
                {
                    throw new ArgumentException("Invalid period string: '" + periodString + "'");
                }
                    
                if (negate)
                {
                    period = -period;
                }

                location += s.Length + 1;

                switch (periodString.Substring(location, 1))
                {
                    case "Y":
                        if (previousString != "") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        newDate = newDate.AddYears(period);
                        previousString = "Y";
                        break;

                    case "M":
                        if (previousString != "" &&
                            previousString != "Y") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        newDate = newDate.AddMonths(period);
                        previousString = "M";
                        break;

                    case "W":
                        if (previousString != "" &&
                            previousString != "Y" &&
                            previousString != "M") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        newDate = newDate.AddWeeks(period);
                        previousString = "W";
                        break;

                    case "D":
                        if (previousString != "" &&
                            previousString != "Y" &&
                            previousString != "M" &&
                            previousString != "W") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        newDate = newDate.AddDays(period);
                        previousString = "D";
                        break;

                    case "B":
                        if (previousString != "") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        newDate = newDate.AddBusinessDays(period, calendar);
                        previousString = "B";
                        break;

                    case "F":
                    case "I":
                        if (previousString != "") throw new ArgumentException("Invalid period string: '" + periodString + "'");
                        for (int i = 0; i < period; i++)
                        {
                            newDate = newDate.NextIMM();
                        }
                        previousString = "I";
                        break;
                }
            }

            newDate = newDate.Modify(convention, calendar);

            return newDate;
        }


        /// <summary>
        /// Subtracts the given period.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period">A <see cref="Period"/>.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime SubtractPeriod(this DateTime date, Period period, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddPeriod(period, convention, calendar, true);
        }


        /// <summary>
        /// Adds the given period.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period">A <see cref="Period"/>.</param>
        /// <param name="convention">Optional daycount convention, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static DateTime AddPeriod(this DateTime date, Period period, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            return date.TruncateTime().AddPeriod(period, convention, calendar, false);
        }


        /// <summary>
        /// Private function adds or subtracts the given period.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period">A <see cref="Period"/>.</param>
        /// <param name="convention">See <see cref="Convention">.</param>
        /// <param name="calendar">The holiday calendar.</param>
        /// <param name="negate">Subtracts the period if True, otherwise adds it.</param>
        private static DateTime AddPeriod(this DateTime date, Period period, Convention convention, SortedSet<DateTime> calendar, bool negate)
        {
            DateTime newDate = date.TruncateTime();
            int mult = (negate) ? -1 : 1;
            
            switch (period.Type)
            {
                case PeriodType.Year:
                    newDate = newDate.AddYears(mult * period.NumPeriods);
                    break;

                case PeriodType.Month:
                    newDate = newDate.AddMonths(mult * period.NumPeriods);
                    break;

                case PeriodType.Week:
                    newDate = newDate.AddWeeks(mult * period.NumPeriods);
                    break;

                case PeriodType.BusinessDay:
                    newDate = newDate.AddBusinessDays(mult * period.NumPeriods, calendar);
                    break;

                case PeriodType.Day:
                    newDate = newDate.AddDays(mult * period.NumPeriods);
                    break;

                case PeriodType.IMM:
                    if (mult * period.NumPeriods < 0)
                    {
                        throw new ArgumentException("Cannot subtract IMM periods from a date");
                    }

                    for (int i = 0; i < mult * period.NumPeriods; i++)
                    {
                        newDate = newDate.NextIMM();
                    }
                    break;
            }

            newDate = newDate.Modify(convention, calendar);

            return newDate;
        }


        /// <summary>
        /// The number of calendar days to a the endDate for the optional daycount, convention and holiday calendar./>
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">The date to which the number of days is to be calculated.</param>
        /// <param name="daycount">Optional daycount, defaulting to <see cref="Daycount.DcACTACT"/></param>
        /// <param name="convention">Optional daycount convention adjusting both start and end dates, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static long DaysTo(this DateTime startDate, DateTime endDate, Daycount daycount = Daycount.DcACTACT, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            DateTime d1 = startDate.Modify(convention, calendar);
            DateTime d2 = endDate.Modify(convention, calendar);

            switch (daycount)
            {
                case Daycount.DcACT360:
                case Daycount.DcACT365:
                case Daycount.DcACTACT:
                case Daycount.DcACT36525:
                    return (long)(d2 - d1).TotalDays;

                case Daycount.Dc30360:
                    int day1 = d1.Day;
                    int day2 = d2.Day;

                    if (day1 >= 30)
                    {
                        if (day2 >= 30) day2 = 30;
                        day1 = 30;
                    }

                    return 360 * (d2.Year - d1.Year) + 30 * (d2.Month - d1.Month) + (day2 - day1);

                case Daycount.Dc30E360:
                    return 360 * (long)(d2.Year - d1.Year) + 30 * (long)(d2.Month - d1.Month) + (long)(Math.Min(d2.Day, 30) - Math.Min(d1.Day, 30));
            }

            throw new ArgumentException("Invalid DaysTo call");
        }


        /// <summary>
        /// The number of business days to a the endDate for the optional daycount, convention and holiday calendar./>. Assumes that the initial
        /// date is a good business day (maybe needs a revisit).
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">The date to which the number of days is to be calculated.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static long BusinessDaysTo(this DateTime startDate, DateTime endDate, SortedSet<DateTime> calendar = null)
        {
            calendar ??= new SortedSet<DateTime>();

            DateTime adjStart = startDate.Modify(Convention.Following, calendar);
            DateTime adjEnd = endDate.Modify(Convention.Preceding, calendar);
            var calBetween = calendar.Where(d => d >= adjStart && d < adjEnd);

            int adj = 0;

            if (adjStart.Date.DayOfWeek < adjEnd.Date.DayOfWeek)
            {
                adj = adjEnd.Date.DayOfWeek - adjStart.Date.DayOfWeek;
                adjStart = adjStart.AddDays(adj);
            }
            else if (adjStart.Date.DayOfWeek > adjEnd.Date.DayOfWeek)
            {
                adj = 7 - (adjStart.Date.DayOfWeek - adjEnd.Date.DayOfWeek);
                adjStart = adjStart.AddDays(adj);
            }

            long result = adj + adjStart.DaysTo(adjEnd, Daycount.DcACTACT) * 5 / 7;
            result -= calBetween.Count();
                
            return result;
        }


        /// <summary>
        /// The year fraction to a the endDate for the optional daycount, convention and holiday calendar./>
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">The date to which the number of days is to be calculated.</param>
        /// <param name="daycount">Optional daycount, defaulting to <see cref="Daycount.DcACTACT"/></param>
        /// <param name="convention">Optional daycount convention adjusting both start and end dates, defaulting to <see cref="Convention.None"/>.</param>
        /// <param name="calendar">Optional holiday calendar, defaulting to an empty calendar.</param>
        /// <returns></returns>
        public static double YearFraction(this DateTime startDate, DateTime endDate, Daycount daycount = Daycount.DcACTACT, Convention convention = Convention.None, SortedSet<DateTime> calendar = null)
        {
            startDate = startDate.TruncateTime();
            endDate = endDate.TruncateTime();

            double days = startDate.DaysTo(endDate, daycount, convention, calendar);

            switch (daycount)
            {
                case Daycount.DcACT360:
                case Daycount.Dc30360:
                case Daycount.Dc30E360:
                    return days / 360;

                case Daycount.DcACT365:
                    return days / 365;

                case Daycount.DcACTACT:
                    double yf = 0;

                    DateTime d1 = startDate.Modify(convention, calendar);
                    DateTime d2 = endDate.Modify(convention, calendar);
                    DateTime currDate = d1;
                    DateTime nextDate = new(d1.Year + 1, 1, 1);

                    for (int i = 0; i <= d2.Year - d1.Year; i++)
                    {
                        if (i == d2.Year - d1.Year)
                        {
                            yf += (double)(d2 - currDate).TotalDays / (DateTime.IsLeapYear(currDate.Year) ? 366 : 365);
                        }
                        else
                        {
                            yf += (nextDate - currDate).TotalDays / (DateTime.IsLeapYear(currDate.Year) ? 366 : 365);
                        }

                        currDate = nextDate;
                        nextDate = nextDate.AddYears(1);
                    }

                    return yf;

                case Daycount.DcACT36525:
                    return days / 365.25;
            }

            throw new ArgumentException("Invalid YearFraction call");
        }


        /// <summary>
        /// Expresses a period string as a number of years. 
        /// </summary>
        /// <param name="periodString">A case insensitive string representing a period as a set of concatenated elements each conforming to a [number][period type] pattern.
        /// The period type can be "y" for years or "m" of months. Examples are "2y", "3m" or "4y5m".</param>
        /// <returns></returns>
        public static double PeriodToYears(string periodString)
        {
            string[] split = periodString.Split(new Char[] { 'Y', 'M' });

            if (split.Length != 2 && split[1] != "")
            {
                throw new ArgumentException("Invalid period '" + periodString + "'");
            }

            _ = double.TryParse(split[0], out double years);

            switch (periodString.Substring(split[0].Length, 1))
            {
                case "Y":
                    break;

                case "M":
                    years /= 12;
                    break;

                default:
                    throw new ArgumentException("Invalid period '" + periodString + "'");
            }

            return years;
        }
    }
}
