using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// A revenue cashflow.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Appraisal Revenue Group")]
    public class AppraisalRevenueGroup : AppraisalCashflowGroup<AppraisalRevenue>
    {
    }
}
