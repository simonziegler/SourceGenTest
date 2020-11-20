using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cashflow group.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class AppraisalCashflowGroup<T> : ProjectBase where T : AppraisalCashflow
    {
        /// <summary>
        /// The parent project task's id.
        /// </summary>
        [MessagePack.Key(10)]
        public string ProjectTaskId { get; set; }


        private string name = "";
        /// <summary>
        /// The group name.
        /// </summary>
        [MessagePack.Key(11)]
        [Required, MinLength(1)]
        [Display(Name = "Group Name", Prompt = "Enter a name for the cashflow group")]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description = "";
        /// <summary>
        /// Description, optional.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Description", Prompt = "A description is optional")]
        public string Description { get => description; set => Setter(ref description, value); }


        /// <summary>
        /// Sum of <see cref="AppraisalCashflow.Amount"/> in <see cref="Cashflows"/>.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal Amount => Cashflows.Sum(c => c.Amount);


        /// <summary>
        /// A list of the cashflow group's cashflows.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ICollection<T> Cashflows => GroupedDataset?.GetItems<T>().Where(c => c.CashflowGroupId == Id).ToList();

        ///// <summary>
        ///// Gets the index of the supplied cashflow in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow being sought.</param>
        ///// <returns>Zero based index.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public int GetCashflowIndex(Cashflow cashflow)
        //{
        //    try
        //    {
        //        return GetCashflowIndex(cashflow.Id);
        //    }
        //    catch
        //    {
        //        throw new ArgumentException($"Cashflow '{cashflow.Name}' not found in Cashflow Group '{Name}'");
        //    }
        //}


        ///// <summary>
        ///// Gets the index of the cashflow with the supplied id in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflowId">The id of the cashflow being sought.</param>
        ///// <returns>Zero based index.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public int GetCashflowIndex(string cashflowId)
        //{
        //    int i = 0;

        //    var en = Cashflows.GetEnumerator();

        //    while (en.MoveNext())
        //    {
        //        if (en.Current.Id == cashflowId)
        //        {
        //            return i;
        //        }

        //        i++;
        //    }

        //    throw new ArgumentException($"Cashflow not found with id {cashflowId} in Cashflow Group '{Name}'");
        //}


        ///// <summary>
        ///// Determines if the supplied cashflow is the first in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow being tested.</param>
        ///// <returns>True if first.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public bool IsCashflowFirst(Cashflow cashflow)
        //{
        //    return GetCashflowIndex(cashflow) == 0;
        //}


        ///// <summary>
        ///// Determines if the cashflow with the supplied id is the first in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflowId">The id of the cashflow being tested.</param>
        ///// <returns>True if last.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public bool IsCashflowFirst(string cashflowId)
        //{
        //    return GetCashflowIndex(cashflowId) == 0;
        //}


        ///// <summary>
        ///// Determines if the supplied cashflow is the last in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow being tested.</param>
        ///// <returns>True if last.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public bool IsCashflowLast(Cashflow cashflow)
        //{
        //    return GetCashflowIndex(cashflow) == Cashflows.Count - 1;
        //}


        ///// <summary>
        ///// Determines if the cashflow with the supplied id is the last in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflowId">The id of the cashflow being tested.</param>
        ///// <returns>True if last.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow isn't found.</exception>
        //public bool IsCashflowLast(string cashflowId)
        //{
        //    return GetCashflowIndex(cashflowId) == Cashflows.Count - 1;
        //}


        ///// <summary>
        ///// Moves the supplied cashflow up in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow to be moved.</param>
        //public void MoveCashflowUp(Cashflow cashflow)
        //{
        //    int targetIndex = GetCashflowIndex(cashflow);

        //    if (targetIndex == 0)
        //    {
        //        return;
        //    }

        //    LinkedList<Cashflow> newCashflows = new LinkedList<Cashflow>();
        //    var en = Cashflows.GetEnumerator();
        //    int i = 0;

        //    while (en.MoveNext())
        //    {
        //        if (i == targetIndex - 1)
        //        {
        //            newCashflows.AddLast(cashflow);
        //            newCashflows.AddLast(en.Current);
        //        }
        //        else if (en.Current.Id != cashflow.Id)
        //        {
        //            newCashflows.AddLast(en.Current);
        //        }

        //        i++;
        //    }

        //    Cashflows = newCashflows;
        //}


        ///// <summary>
        ///// Moves the supplied cashflow down in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow to be moved.</param>
        //public void MoveCashflowDown(Cashflow cashflow)
        //{
        //    int targetIndex = GetCashflowIndex(cashflow);

        //    if (targetIndex == Cashflows.Count - 1)
        //    {
        //        return;
        //    }

        //    LinkedList<Cashflow> newCashflows = new LinkedList<Cashflow>();
        //    var en = Cashflows.GetEnumerator();
        //    int i = 0;

        //    while (en.MoveNext())
        //    {
        //        if (i == targetIndex + 1)
        //        {
        //            newCashflows.AddLast(en.Current);
        //            newCashflows.AddLast(cashflow);
        //        }
        //        else if (en.Current.Id != cashflow.Id)
        //        {
        //            newCashflows.AddLast(en.Current);
        //        }

        //        i++;
        //    }

        //    Cashflows = newCashflows;
        //}


        ///// <summary>
        ///// Adds a new cashflow to the end of <see cref="Cashflows"/>.
        ///// </summary>
        ///// <returns>The new cashflow.</returns>
        //public Cashflow AddCashflow()
        //{
        //    return Cashflows.AddLast(new Cashflow()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Name = "New Cashflow",
        //        Cost = 0,
        //        Revenue = 0,
        //        CostRevenueApplication = CostRevenueApplication.CostAndRevenue,
        //    }).Value;
        //}


        ///// <summary>
        ///// Deletes a cashflow from <see cref="Cashflows"/>
        ///// </summary>
        ///// <param name="cashflowId">The id of the cashflow to be deleted.</param>
        //public void DeleteCashflow(string cashflowId)
        //{
        //    var newList = new LinkedList<Cashflow>();

        //    foreach (var item in Cashflows)
        //    {
        //        if (item.Id != cashflowId)
        //        {
        //            newList.AddLast(item);
        //        }
        //    }

        //    Cashflows = newList;
        //}


        ///// <summary>
        ///// Upserts a cashflow in <see cref="Cashflows"/>.
        ///// </summary>
        ///// <param name="cashflow">The cashflow to upsert.</param>
        ///// <returns>The upserted cashflow.</returns>
        //public Cashflow UpsertCashflow(Cashflow cashflow)
        //{
        //    var newList = new LinkedList<Cashflow>();
        //    var found = false;

        //    foreach (var item in Cashflows)
        //    {
        //        if (item.Id == cashflow.Id)
        //        {
        //            newList.AddLast(cashflow);
        //            found = true;
        //        }
        //        else
        //        {
        //            newList.AddLast(item);
        //        }
        //    }

        //    if (!found)
        //    {
        //        newList.AddLast(cashflow);
        //    }

        //    Cashflows = newList;

        //    return cashflow;
        //}


        ///// <inheritdoc/>
        //public override void AddChangeEventHandler(EventHandler handler)
        //{
        //    base.AddChangeEventHandler(handler);

        //    foreach (var item in Cashflows)
        //    {
        //        item.AddChangeEventHandler(handler);
        //    }
        //}
    }
}
