namespace Vectis.DataModel
{
    /// <summary>
    /// A cost rate to be applied to area schedule items
    /// <para>Associated with a scheme by having a <see cref="VectisBase.PartitionKey"/> equal to the scheme's Id.</para>
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Appraisal Cost Rate")]
    public class AppraisalCostRate : AppraisalCostRevenueRate
    {
    }
}
