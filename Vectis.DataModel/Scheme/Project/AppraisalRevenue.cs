using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cost cashflow.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Appraisal Revenue Amount")]
    public class AppraisalRevenue : AppraisalCashflow
    {
    }
}
