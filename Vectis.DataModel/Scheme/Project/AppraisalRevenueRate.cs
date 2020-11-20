namespace Vectis.DataModel
{
    /// <summary>
    /// A revenue rate to be applied to area schedule items
    /// <para>Associated with a scheme by having a <see cref="VectisBase.PartitionKey"/> equal to the scheme's Id.</para>
    /// </summary>
    [TypeDiscriminator("Appraisal Revenue Rate")]
    [MessagePack.MessagePackObject]
    public class AppraisalRevenueRate : AppraisalCostRevenueRate
    {
    }
}
