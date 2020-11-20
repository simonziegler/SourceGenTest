using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// An actual cost drawdown amount. This is correlated to a <see cref="CostScheduleItem"/>.
    /// The <see cref="CostScheduleItem.Date"/> is not necessarily the same as this object's <see cref="DrawdownDate"/>.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cost Schedule Actual Amount")]
    public class CostScheduleActualAmount : SchemeBase
    {
        /// <summary>
        /// The id of this amount's reference budget amount.
        /// </summary>
        [MessagePack.Key(10)]
        public string CostScheduleItemId { get; set; }


        private DateTime quantitySurveyorSiteVisitDate;
        /// <summary>
        /// The QS's site visit date.
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "QS Visit Date", Prompt = "Enter the QS's site visit date")]
        public DateTime QuantitySurveyorSiteVisitDate { get => quantitySurveyorSiteVisitDate; set => Setter(ref quantitySurveyorSiteVisitDate, value); }


        private DateTime quantitySurveyorReportDate;
        /// <summary>
        /// The QS's report date.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "QS Report Date", Prompt = "Enter the QS's report date")]
        public DateTime QuantitySurveyorReportDate { get => quantitySurveyorReportDate; set => Setter(ref quantitySurveyorReportDate, value); }


        private DateTime drawdownDate;
        /// <summary>
        /// The cost's drawdown date.
        /// </summary>
        [MessagePack.Key(13)]
        [Display(Name = "Drawdown Date", Prompt = "Enter the drawdown date")]
        public DateTime DrawdownDate { get => drawdownDate; set => Setter(ref drawdownDate, value); }


        private decimal amount;
        /// <summary>
        /// The cost's amount.
        /// </summary>
        [MessagePack.Key(14)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Cost Date", Prompt = "Enter the date for the cost")]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }


        private bool allocatedPostBudgetCloseOut;
        /// <summary>
        /// If true, this amount was allocated to a budget item after that item had
        /// been referenced as being closed out. This value should be set automatically
        /// using a rule that it copies the value of <see cref="CostScheduleItem.ClosedOut"/> 
        /// as it stood at the time when this object is created.
        /// </summary>
        [MessagePack.Key(15)]
        public bool AllocatedPostBudgetCloseOut { get => allocatedPostBudgetCloseOut; set => Setter(ref allocatedPostBudgetCloseOut, value); }
    }
}
