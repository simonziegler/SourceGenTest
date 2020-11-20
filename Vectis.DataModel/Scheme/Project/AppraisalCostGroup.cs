using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cost cashflow.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Appraisal Cost Group")]
    public class AppraisalCostGroup : AppraisalCashflowGroup<AppraisalCost>
    {
    }
}
