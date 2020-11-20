using System;
using System.Collections.Generic;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A summary of pricing results. Non monetary values (i.e. percentages and ratios) are all marked as <see cref="UnderwritableAttribute"/>.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Evaluation Result Summary")]
    public class EvaluationResultsSummary : EvaluationDatasetBase
    {
        private decimal gDV;
        /// <summary>
        /// Gross development value.
        /// </summary>
        [MessagePack.Key(10)]
        public decimal GDV { get => gDV; set => Setter(ref gDV, value); }


        private decimal unleveragedGDC;
        /// <summary>
        /// Unleveraged gross development cost.
        /// </summary>
        [MessagePack.Key(11)]
        public decimal UnleveragedGDC { get => unleveragedGDC; set => Setter(ref unleveragedGDC, value); }


        private decimal unleveragedProfit;
        /// <summary>
        /// Unleveraged profit.
        /// </summary>
        [MessagePack.Key(12)]
        public decimal UnleveragedProfit { get => unleveragedProfit; set => Setter(ref unleveragedProfit, value); }


        private decimal unleveragedProfitOnCost;
        /// <summary>
        /// Unleveraged profit on gross development cost. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(13)]
        [Underwritable("PoC ex debt", "PoC excluding debt", 0, false, NumericInputMagnitude.Percent, "N", "%")]
        public decimal UnleveragedProfitOnCost { get => unleveragedProfitOnCost; set => Setter(ref unleveragedProfitOnCost, value); }


        private decimal unleveragedReturnMultiple;
        /// <summary>
        /// Unleverage return multiple. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(14)]
        [Underwritable("Mult ex debt", "Return Multiple excluding debt", 1, false, NumericInputMagnitude.Normal, "N", "x")]
        public decimal UnleveragedReturnMultiple { get => unleveragedReturnMultiple; set => Setter(ref unleveragedReturnMultiple, value); }


        private decimal unleveragedIRR;
        /// <summary>
        /// Unleveraged internal rate of return. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(15)]
        [Underwritable("IRR ex debt", "IRR excluding debt", 2, false, NumericInputMagnitude.Percent, "N", "%")]
        public decimal UnleveragedIRR { get => unleveragedIRR; set => Setter(ref unleveragedIRR, value); }


        private decimal leveragedGDC;
        /// <summary>
        /// Leveraged gross development cost.
        /// </summary>
        [MessagePack.Key(16)]
        public decimal LeveragedGDC { get => leveragedGDC; set => Setter(ref leveragedGDC, value); }


        private decimal leveragedProfit;
        /// <summary>
        /// Leveraged profit.
        /// </summary>
        [MessagePack.Key(17)]
        public decimal LeveragedProfit { get => leveragedProfit; set => Setter(ref leveragedProfit, value); }


        private decimal leveragedProfitOnCost;
        /// <summary>
        /// Leveraged profit on gross development cost. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(18)]
        [Underwritable("PoC inc debt", "PoC including debt", 3, false, NumericInputMagnitude.Percent, "N", "%")]
        public decimal LeveragedProfitOnCost { get => leveragedProfitOnCost; set => Setter(ref leveragedProfitOnCost, value); }


        private decimal leveragedReturnMultiple;
        /// <summary>
        /// Leveraged return multiple. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(19)]
        [Underwritable("Mult inc debt", "Return Multiple including debt",  4, false, NumericInputMagnitude.Normal, "N", "x")]
        public decimal LeveragedReturnMultiple { get => leveragedReturnMultiple; set => Setter(ref leveragedReturnMultiple, value); }


        private decimal leveragedIRR;
        /// <summary>
        /// Leveraged internal rate of return. <see cref="UnderwritableAttribute"/>
        /// </summary>
        [MessagePack.Key(20)]
        [Underwritable("IRR inc debt", "IRR including debt", 5, false, NumericInputMagnitude.Percent, "N", "%")]
        public decimal LeveragedIRR { get => leveragedIRR; set => Setter(ref leveragedIRR, value); }


        private decimal equityCommitment;
        /// <summary>
        /// Required equity commitment.
        /// </summary>
        [MessagePack.Key(21)]
        public decimal EquityCommitment { get => equityCommitment; set => Setter(ref equityCommitment, value); }


        private decimal peakDebtDraw;
        /// <summary>
        /// Peak total debt drawn amount.
        /// </summary>
        [MessagePack.Key(22)]
        public decimal PeakDebtDraw { get => peakDebtDraw; set => Setter(ref peakDebtDraw, value); }


        private decimal totalDebtCost;
        /// <summary>
        /// Cost of total debt.
        /// </summary>
        [MessagePack.Key(23)]
        public decimal TotalDebtCost { get => totalDebtCost; set => Setter(ref totalDebtCost, value); }


        private decimal debtInterest;
        /// <summary>
        /// Total debt interest.
        /// </summary>
        [MessagePack.Key(24)]
        public decimal DebtInterest { get => debtInterest; set => Setter(ref debtInterest, value); }


        private decimal debtNonUtilizationFees;
        /// <summary>
        /// Total debt non-utilization fees.
        /// </summary>
        [MessagePack.Key(25)]
        public decimal DebtNonUtilizationFees { get => debtNonUtilizationFees; set => Setter(ref debtNonUtilizationFees, value); }


        private decimal debtEntryExitFees;
        /// <summary>
        /// Total debt entry and exit fees.
        /// </summary>
        [MessagePack.Key(26)]
        public decimal DebtEntryExitFees { get => debtEntryExitFees; set => Setter(ref debtEntryExitFees, value); }


        private decimal debtLTC;
        /// <summary>
        /// Total debt loan to cost ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(27)]
        [Underwritable("Debt LTC", "Total Debt Loan to Cost", 6, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal DebtLTC { get => debtLTC; set => Setter(ref debtLTC, value); }


        private decimal debtLTV;
        /// <summary>
        /// Total debt loan to gross development value ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(28)]
        [Underwritable("Debt LTGDV", "Total Debt Loan to GDV", 7, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal DebtLTV { get => debtLTV; set => Setter(ref debtLTV, value); }


        private decimal seniorDebtCommitment;
        /// <summary>
        /// Senior debt commitment amount.
        /// </summary>
        [MessagePack.Key(29)]
        public decimal SeniorDebtCommitment { get => seniorDebtCommitment; set => Setter(ref seniorDebtCommitment, value); }


        private decimal seniorDebtLTC;
        /// <summary>
        /// Senior debt loan to cost ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(30)]
        [Underwritable("Senior LTC", "Senior Debt Loan to Cost", 8, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal SeniorDebtLTC { get => seniorDebtLTC; set => Setter(ref seniorDebtLTC, value); }


        private decimal seniorDebtLTV;
        /// <summary>
        /// Senior debt loan to GDV ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(31)]
        [Underwritable("Senior LTGDV", "Senior Debt Loan to GDV", 9, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal SeniorDebtLTV { get => seniorDebtLTV; set => Setter(ref seniorDebtLTV, value); }


        private decimal seniorDebtPeakDraw;
        /// <summary>
        /// Senior debt peak draw amount.
        /// </summary>
        [MessagePack.Key(32)]
        public decimal SeniorDebtPeakDraw { get => seniorDebtPeakDraw; set => Setter(ref seniorDebtPeakDraw, value); }


        private decimal seniorDebtIRR;
        /// <summary>
        /// Senior debt internal rate of return. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(33)]
        [Underwritable("Senior IRR", "Senior Debt IRR", 10, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal SeniorDebtIRR { get => seniorDebtIRR; set => Setter(ref seniorDebtIRR, value); }


        private decimal mezzanineDebtCommitment;
        /// <summary>
        /// Mezzanine debt commitment amount.
        /// </summary>
        [MessagePack.Key(34)]
        public decimal MezzanineDebtCommitment { get => mezzanineDebtCommitment; set => Setter(ref mezzanineDebtCommitment, value); }


        private decimal mezzanineDebtLTC;
        /// <summary>
        /// Mezzanine debt loan to cost ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(35)]
        [Underwritable("Mezz LTC", "Mezzanine Debt Loan to Cost", 11, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal MezzanineDebtLTC { get => mezzanineDebtLTC; set => Setter(ref mezzanineDebtLTC, value); }


        private decimal mezzanineDebtLTV;
        /// <summary>
        /// Mezzanine debt loan to GDV ratio. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(36)]
        [Underwritable("Mezz LTGDV", "Mezzanine Debt Loan to GDV", 12, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal MezzanineDebtLTV { get => mezzanineDebtLTV; set => Setter(ref mezzanineDebtLTV, value); }


        private decimal mezzanineDebtPeakDraw;
        /// <summary>
        /// Mezzanine debt peak draw amount.
        /// </summary>
        [MessagePack.Key(37)]
        public decimal MezzanineDebtPeakDraw { get => mezzanineDebtPeakDraw; set => Setter(ref mezzanineDebtPeakDraw, value); }


        //[MessagePack.Key(28)]
        private decimal mezzanineDebtIRR;
        /// <summary>
        /// Mezzanine debt internal rate of return. <see cref="UnderwritableAttribute"/>.
        /// </summary>
        [MessagePack.Key(38)]
        [Underwritable("Mezz IRR", "Mezzanine Debt IRR", 13, true, NumericInputMagnitude.Percent, "N", "%")]
        public decimal MezzanineDebtIRR { get => mezzanineDebtIRR; set => Setter(ref mezzanineDebtIRR, value); }



        /// <summary>
        /// Gets the aggregate <see cref="Enumerations.UnderwritingResultClassification"/> given all conditions. This is the worst green/amber/red result observed.
        /// </summary>
        /// <param name="conditions">A set of <see cref="UnderwritingCondition"/>s.</param>
        /// <returns></returns>
        public UnderwritingResultClassification GetAggregateUnderwritingResult(IEnumerable<UnderwritingCondition> conditions)
        {
            if (!GetUnderwritingResults(conditions).Any())
            {
                return UnderwritingResultClassification.Pass;
            }

            return (UnderwritingResultClassification)Convert.ToInt32(GetUnderwritingResults(conditions).Select(u => u.ResultClassification).Max());
        }



        /// <summary>
        /// Gets the individual <see cref="UnderwritingResult"/>s for the conditions supplied.
        /// </summary>
        /// <param name="conditions">A set of <see cref="UnderwritingCondition"/>s.</param>
        /// <returns></returns>
        public IEnumerable<UnderwritingResult> GetUnderwritingResults(IEnumerable<UnderwritingCondition> conditions)
        {
            var result = new LinkedList<UnderwritingResult>();

            foreach (var cond in conditions.OrderBy(c => c.Order))
            {
                var prop = GetType().GetProperties().Where(p => p.Name.ToLower() == cond.PropertyName.ToLower()).FirstOrDefault();

                var ur = new UnderwritingResult()
                {
                    ShortDescription = cond.ShortDescription,
                    LongDescription = cond.LongDescription,
                    Value = (decimal)prop.GetValue(this),
                    Format = cond.Format,
                    PreferLowerNumbers = cond.LowValuesPass,
                    GreenToAmberThreshold = cond.PassToBorderlineThreshold,
                    AmberToRedThreshold = cond.BorderlineToFailThreshold
                };

                if (cond.LowValuesPass)
                {
                    if (ur.Value < cond.PassToBorderlineThreshold)
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Pass;
                    }
                    else if (ur.Value < cond.BorderlineToFailThreshold)
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Borderline;
                    }
                    else
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Fail;
                    }
                }
                else
                {
                    if (ur.Value > cond.PassToBorderlineThreshold)
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Pass;
                    }
                    else if (ur.Value > cond.BorderlineToFailThreshold)
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Borderline;
                    }
                    else
                    {
                        ur.ResultClassification = UnderwritingResultClassification.Fail;
                    }
                }

                result.AddLast(ur);
            }

            return result;
        }


        /// <summary>
        /// Gets all <see cref="UnderwritingCondition"/>s for the properties of <see cref="EvaluationChartingResults"/>.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<UnderwritingCondition> GetAllUnderwritableProperties()
        {
            var result = new List<UnderwritingCondition>();

            var properties = typeof(EvaluationResultsSummary).GetProperties().Where(prop => prop.IsDefined(typeof(UnderwritableAttribute), false));

            foreach (var prop in properties)
            {
                var attrs = prop.GetCustomAttributes(true);

                foreach (var attr in attrs)
                {
                    if (attr is UnderwritableAttribute u)
                    {
                        result.Add(new UnderwritingCondition()
                        {
                            PropertyName = prop.Name,
                            Order = u.DefaultOrder,
                            LowValuesPass = u.PreferLowNumbers,
                            Format = u.Format,
                            PassToBorderlineThreshold = 0,
                            BorderlineToFailThreshold = 0
                        });
                    }
                }
            }

            return result;
        }
    }
}
