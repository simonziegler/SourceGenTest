using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// A row for a cashflow chart.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cashflow Chart Row")]
    public class CashflowChartRow
    {
        /// <summary>
        /// The datum's date.
        /// </summary>
        [MessagePack.Key(0)]
        public DateTime Date { get; set; }


        /// <summary>
        /// The year.
        /// </summary>
        [MessagePack.Key(1)]
        public int Year { get; set; }


        /// <summary>
        /// The month.
        /// </summary>
        [MessagePack.Key(2)]
        public int Month { get; set; }


        /// <summary>
        /// The day.
        /// </summary>
        [MessagePack.Key(3)]
        public int Day { get; set; }


        /// <summary>
        /// The unleverage cash balance amount.
        /// </summary>
        [MessagePack.Key(4)]
        public decimal? CashBalance { get; set; }


        /// <summary>
        /// The equity amount.
        /// </summary>
        [MessagePack.Key(5)]
        public decimal? Equity { get; set; }


        /// <summary>
        /// The mezz debt amount.
        /// </summary>
        [MessagePack.Key(6)]
        public decimal? Mezzanine { get; set; }


        /// <summary>
        /// The senior debt amount.
        /// </summary>
        [MessagePack.Key(7)]
        public decimal? Senior { get; set; }
    }
}
