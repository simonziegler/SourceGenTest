using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A Vectis loan.
    /// </summary>
    public abstract class Loan : FundingSource
    {
        private bool applyFacility = false;
        /// <summary>
        /// Determines whether calculations should apply the facility. Applied if True and ignored if False.
        /// </summary>
        [MessagePack.Key(15)]
        [Display(Name = "Apply Facility", Prompt = "Indicates whether to apply this facility to the property development")]
        public bool ApplyFacility { get => applyFacility; set => Setter(ref applyFacility, value); }


        private CommitmentType commitmentType = CommitmentType.LoanToCost;
        /// <summary>
        /// The loan's <see cref="CommitmentType"/>.
        /// </summary>
        [MessagePack.Key(16)]
        [Display(Name = "Commitment Type", Prompt = "Express commitment as a loan to cost ratio or a specified cash amount")]
        public CommitmentType CommitmentType
        {
            get => commitmentType;
            set
            {
                if (value != commitmentType)
                {
                    var trancheCommitmentAmount = TrancheCommitmentAmount;
                    var totalCommitmentAmount = TotalCommitmentAmount;
                    var trancheLTC = TrancheLTC;
                    var totalLTC = TotalLTC;

                    Setter(ref commitmentType, value);

                    TrancheCommitmentAmount = trancheCommitmentAmount;
                    TotalCommitmentAmount = totalCommitmentAmount;
                    TrancheLTC = trancheLTC;
                    TotalLTC = totalLTC;
                }
            }
        }


        private CommitmentStacking commitmentStacking = CommitmentStacking.Tranched;
        /// <summary>
        /// The loan's <see cref="CommitmentStacking"/>.
        /// </summary>
        [MessagePack.Key(17)]
        [Display(Name = "Commitment Stacking", Prompt = "Express mezz commitment as tranche thickness or total LTC")]
        public CommitmentStacking CommitmentStacking
        {
            get => commitmentStacking;
            set
            {
                if (value != commitmentStacking)
                {
                    var trancheCommitmentAmount = TrancheCommitmentAmount;
                    var totalCommitmentAmount = TotalCommitmentAmount;
                    var trancheLTC = TrancheLTC;
                    var totalLTC = TotalLTC;

                    Setter(ref commitmentStacking, value);

                    TrancheCommitmentAmount = trancheCommitmentAmount;
                    TotalCommitmentAmount = totalCommitmentAmount;
                    TrancheLTC = trancheLTC;
                    TotalLTC = totalLTC;
                }
            }
        }


        private decimal trancheCommitmentAmount;
        /// <summary>
        /// The tranche thickness as a cash amount. The setter is ignored unless <see cref="CommitmentType"/>
        /// is <see cref="CommitmentType.CashAmount"/> and <see cref="CommitmentStacking"/> is <see cref="CommitmentStacking.Tranched"/>.
        /// </summary>
        [MessagePack.Key(18)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Tranche Commitment Amount", Prompt = "Tranche commitment amount")]
        public decimal TrancheCommitmentAmount
        {
            get
            {
                if (CommitmentType == CommitmentType.CashAmount)
                {
                    if (CommitmentStacking == CommitmentStacking.Tranched) return trancheCommitmentAmount;
                    else return TotalCommitmentAmount - (NextSeniorLoan?.TotalCommitmentAmount ?? 0);
                }
                else
                {
                    return TrancheLTC * DevelopmentGDC;
                }
            }

            set
            {
                if (CommitmentType == CommitmentType.CashAmount && CommitmentStacking == CommitmentStacking.Tranched)
                {
                    ResetCommitments();
                    Setter(ref trancheCommitmentAmount, value);
                }
            }
        }


        private decimal totalCommitmentAmount;
        /// <summary>
        /// The total debt tranche commitment as a cash amount. The setter is ignored unless <see cref="CommitmentType"/>
        /// is <see cref="CommitmentType.CashAmount"/> and <see cref="CommitmentStacking"/> is <see cref="CommitmentStacking.Total"/>.
        /// </summary>
        [MessagePack.Key(19)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Total Commitment Amount", Prompt = "Total commitment amount")]
        public decimal TotalCommitmentAmount
        {
            get
            {
                if (CommitmentType == CommitmentType.CashAmount)
                {
                    if (CommitmentStacking == CommitmentStacking.Total) return totalCommitmentAmount;
                    else return TrancheCommitmentAmount + (NextSeniorLoan?.TotalCommitmentAmount ?? 0);
                }
                else
                {
                    return TotalLTC * DevelopmentGDC;
                }
            }

            set
            {
                if (CommitmentType == CommitmentType.CashAmount && CommitmentStacking == CommitmentStacking.Total)
                {
                    ResetCommitments();
                    Setter(ref totalCommitmentAmount, value);
                }
            }
        }


        private decimal trancheLTC;
        /// <summary>
        /// The tranche thickness as an LTC amount. The setter is ignored unless <see cref="CommitmentType"/>
        /// is <see cref="CommitmentType.LoanToCost"/> and <see cref="CommitmentStacking"/> is <see cref="CommitmentStacking.Tranched"/>.
        /// </summary>
        [MessagePack.Key(20)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Tranche Commitment LTC", Prompt = "Tranche LTC ratio")]
        public decimal TrancheLTC
        {
            get
            {
                if (CommitmentType == CommitmentType.LoanToCost)
                {
                    if (CommitmentStacking == CommitmentStacking.Tranched) return trancheLTC;
                    else return TotalLTC - (NextSeniorLoan?.TotalLTC ?? 0);
                }
                else
                {
                    return (DevelopmentGDC == 0) ? 0 : TrancheCommitmentAmount / DevelopmentGDC;
                }
            }

            set
            {
                if (CommitmentType == CommitmentType.LoanToCost && CommitmentStacking == CommitmentStacking.Tranched)
                {
                    ResetCommitments();
                    Setter(ref trancheLTC, value);
                }
            }
        }


        private decimal totalLTC;
        /// <summary>
        /// The total debt tranche commitment as an LTC amount. The setter is ignored unless <see cref="CommitmentType"/>
        /// is <see cref="CommitmentType.LoanToCost"/> and <see cref="CommitmentStacking"/> is <see cref="CommitmentStacking.Total"/>.
        /// </summary>
        [MessagePack.Key(21)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Total Commitment LTC", Prompt = "Total LTC ratio")]
        public decimal TotalLTC
        {
            get
            {
                if (CommitmentType == CommitmentType.LoanToCost)
                {
                    if (CommitmentStacking == CommitmentStacking.Total) return totalLTC;
                    else return TrancheLTC + (NextSeniorLoan?.TotalLTC ?? 0);
                }
                else
                {
                    return (DevelopmentGDC == 0) ? 0 : TotalCommitmentAmount / DevelopmentGDC;
                }
            }

            set
            {
                if (CommitmentType == CommitmentType.LoanToCost && CommitmentStacking == CommitmentStacking.Total)
                {
                    ResetCommitments();
                    Setter(ref totalLTC, value);
                }
            }
        }


        private DateTime startDate = DateTime.Today.AddDays(14).FirstDayOfMonth().AddMonths(1);
        /// <summary>
        /// The facility's start date.
        /// </summary>
        [MessagePack.Key(22)]
        [Display(Name = "Facility Start Date", Prompt = "The facility start date")]
        public DateTime StartDate { get => startDate; set => Setter(ref startDate, value); }


        private DateTime firstRollDate = DateTime.Today.AddDays(14).FirstDayOfMonth().AddMonths(1);
        /// <summary>
        /// The facility's first roll date. Can be the start date if the facility has a whole number of months.
        /// </summary>
        [MessagePack.Key(23)]
        [Display(Name = "Facility End Date", Prompt = "The facility end date")]
        public DateTime FirstRollDate { get => firstRollDate; set => Setter(ref firstRollDate, value); }


        private DateTime endDate = DateTime.Today.AddDays(14).FirstDayOfMonth().AddMonths(13);
        /// <summary>
        /// The facility's start date.
        /// </summary>
        [MessagePack.Key(24)]
        [Display(Name = "Facility End Date", Prompt = "The facility end date")]
        public DateTime EndDate { get => endDate; set => Setter(ref endDate, value); }


        private Frequency frequency = Frequency.Monthly;
        /// <summary>
        /// The facility's start date.
        /// </summary>
        [MessagePack.Key(25)]
        [Display(Name = "Facility Frequency", Prompt = "The facility coupon or accrual frequency")]
        public Frequency Frequency { get => frequency; set => Setter(ref frequency, value); }


        private Daycount daycount;
        /// <summary>
        /// Facility daycount convention.
        /// </summary>
        [MessagePack.Key(26)]
        [Display(Name = "Daycount", Prompt = "The facility daycount convention")]
        public Daycount Daycount { get => daycount; set => Setter(ref daycount, value); }


        private DaycountAdjustment calculationDateAdjustment;
        /// <summary>
        /// Facility calculation date adjustment.
        /// </summary>
        [MessagePack.Key(27)]
        [Display(Name = "Calculation Date Adjustment", Prompt = "The facility calculation date adjustment")]
        public DaycountAdjustment CalculationDateAdjustment { get => calculationDateAdjustment; set => Setter(ref calculationDateAdjustment, value); }


        private DaycountAdjustment paymentDateAdjustment;
        /// <summary>
        /// Facility payment date adjustment.
        /// </summary>
        [MessagePack.Key(28)]
        [Display(Name = "Payment Date Adjustment", Prompt = "The facility payment date adjustment")]
        public DaycountAdjustment PaymentDateAdjustment { get => paymentDateAdjustment; set => Setter(ref paymentDateAdjustment, value); }


        private decimal nonUtilizationFeeRate = 0.01m;
        /// <summary>
        /// The running non-utilization fee rate.
        /// </summary>
        [MessagePack.Key(29)]
        [Display(Name = "Non-Utilization Fee Rate", Prompt = "Facility non-utilization fee rate on undrawn funds")]
        public decimal NonUtilizationFeeRate { get => nonUtilizationFeeRate; set => Setter(ref nonUtilizationFeeRate, value); }


        private decimal entryFee = 0.01m;
        /// <summary>
        /// The one-off entry fee.
        /// </summary>
        [MessagePack.Key(30)]
        [Display(Name = "Entry Fee", Prompt = "Entry fee charged at facility draw down, to include broker fees")]
        public decimal EntryFee { get => entryFee; set => Setter(ref entryFee, value); }


        private decimal exitFee = 0.01m;
        /// <summary>
        /// The one-off exit fee.
        /// </summary>
        [MessagePack.Key(31)]
        [Display(Name = "Exit Fee", Prompt = "Exit fee charged at facility repayment")]
        public decimal ExitFee { get => exitFee; set => Setter(ref exitFee, value); }


        private void ResetCommitments()
        {
            trancheCommitmentAmount = 0;
            totalCommitmentAmount = 0;
            trancheLTC = 0;
            totalLTC = 0;
        }
    }
}
