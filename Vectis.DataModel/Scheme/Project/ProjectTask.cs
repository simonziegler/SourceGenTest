using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A project task.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Project Task")]
    public class ProjectTask : ProjectBase
    {
        private string name = "";
        /// <summary>
        /// The task name.
        /// </summary>
        [MessagePack.Key(10)]
        [Required, MinLength(1)]
        [Display(Name = "Task Name", Prompt = "Enter a name for the task")]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description = "";
        /// <summary>
        /// Optional description.
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "Description", Prompt = "A description is optional")]
        public string Description { get => description; set => Setter(ref description, value); }


        private ProjectTaskLink link = new();
        /// <summary>
        /// Link to the prior project task.
        /// </summary>
        [MessagePack.Key(12)]
        [ValidateComplexType]
        public ProjectTaskLink Link { get => link; set => Setter(ref link, value); }


        private DateTime startDate = DateTime.Today;
        /// <summary>
        /// Start date for the task if not linked.
        /// </summary>
        [MessagePack.Key(13)]
        [Required]
        [Display(Name = "Start Date", Prompt = "Task start date (this task is not linked)")]
        public DateTime StartDate { get => startDate; set => Setter(ref startDate, value); }


        private int periodMonths;
        /// <summary>
        /// Length of the task in months.
        /// </summary>
        [MessagePack.Key(14)]
        [Required]
        [Display(Name = "Period", Prompt = "Length of the task in months")]
        public int PeriodMonths { get => periodMonths; set => Setter(ref periodMonths, value); }


        private decimal vatChargeRate = 0m;
        /// <summary>
        /// The rate at which VAT is charged on task costs.
        /// </summary>
        [MessagePack.Key(15)]
        [Range(0, 1, ErrorMessage = "VAT rate must be from 0% to 100%")]
        [Display(Name = "Charged VAT Rate", Prompt = "The charged VAT rate to apply to the cashflow")]
        public decimal VatChargeRate { get => vatChargeRate; set => Setter(ref vatChargeRate, value); }


        private decimal vatReclaimRate = 0m;
        /// <summary>
        /// The rate at which VAT is reclaimed subsequent to payment.
        /// </summary>
        [MessagePack.Key(16)]
        [Range(0, 1, ErrorMessage = "VAT rate must be from 0% to 100%")]
        [Display(Name = "Reclaimed VAT Rate", Prompt = "The reclaimed VAT rate to apply to the cashflow")]
        public decimal VatReclaimRate { get => vatReclaimRate; set => Setter(ref vatReclaimRate, value); }


        /// <summary>
        /// Sum of <see cref="AppraisalCashflowGroup{T}.Amount"/> in <see cref="AppraisalCostGroups"/>. Applied as
        /// project task cost if there is no cost schedule being applied, otherwise the evaluation will
        /// calculate revenue from budgets and actuals.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal AppraisalCost => AppraisalCostGroups.Sum(c => c.Amount);


        /// <summary>
        /// Sum of <see cref="AppraisalCashflowGroup{T}.Amount"/> in <see cref="AppraisalRevenueGroups"/>. Applied as
        /// project task revenue if there is no revenue schedule being applied, otherwise the evaluation will
        /// calculate revenue from budgets and actuals.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal AppraisalRevenue => AppraisalRevenueGroups.Sum(c => c.Amount);


        /// <summary>
        /// The actual start date for this task taking links into account.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public DateTime ActualStartDate => Link.AssessedStartDate(GroupedDataset, this);


        /// <summary>
        /// The actual end date for this task taking links into account.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public DateTime ActualEndDate => ActualStartDate.AddMonths(PeriodMonths);


        /// <summary>
        /// The prior linked task.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ProjectTask PreviousTask => (Link.LinkType != LinkType.None) ? GroupedDataset?.GetItem<ProjectTask>(Link.PreviousProjectTaskId) : default;


        /// <summary>
        /// A list of the project task's cost groups.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ICollection<AppraisalCostGroup> AppraisalCostGroups => GroupedDataset?.GetItems<AppraisalCostGroup>().Where(c => c.ProjectTaskId == Id).ToList();


        /// <summary>
        /// A list of the project task's revenue groups.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ICollection<AppraisalRevenueGroup> AppraisalRevenueGroups => GroupedDataset?.GetItems<AppraisalRevenueGroup>().Where(c => c.ProjectTaskId == Id).ToList();


        ///// <summary>
        ///// Gets the index of the supplied cashflow group in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group being sought.</param>
        ///// <returns>Zero based index.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public int GetCashflowGroupIndex(CashflowGroup cashflowGroup)
        //{
        //    try
        //    {
        //        return GetCashflowGroupIndex(cashflowGroup.Id);
        //    }
        //    catch
        //    {
        //        throw new ArgumentException($"Cashflow Group '{cashflowGroup.Name}' not found in Project Task '{Name}'");
        //    }
        //}


        ///// <summary>
        ///// Gets the index of the cashflow group with the supplied id in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroupId">The id of the cashflow group being sought.</param>
        ///// <returns>Zero based index.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public int GetCashflowGroupIndex(string cashflowGroupId)
        //{
        //    int i = 0;

        //    var en = CashflowGroups.GetEnumerator();

        //    while (en.MoveNext())
        //    {
        //        if (en.Current.Id == cashflowGroupId)
        //        {
        //            return i;
        //        }

        //        i++;
        //    }

        //    throw new ArgumentException($"Cashflow Group not found with id {cashflowGroupId} in Project Taks '{Name}'");
        //}


        ///// <summary>
        ///// Determines if the supplied cashflow group is the first in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group being tested.</param>
        ///// <returns>True if first.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public bool IsCashflowGroupFirst(CashflowGroup cashflowGroup)
        //{
        //    return GetCashflowGroupIndex(cashflowGroup) == 0;
        //}


        ///// <summary>
        ///// Determines if the cashflow group with the supplied id is the first in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroupId">The id of the cashflow group being tested.</param>
        ///// <returns>True if last.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public bool IsCashflowGroupFirst(string cashflowGroupId)
        //{
        //    return GetCashflowGroupIndex(cashflowGroupId) == 0;
        //}


        ///// <summary>
        ///// Determines if the supplied cashflow group is the last in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group being tested.</param>
        ///// <returns>True if first.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public bool IsCashflowGroupLast(CashflowGroup cashflowGroup)
        //{
        //    return GetCashflowGroupIndex(cashflowGroup) == CashflowGroups.Count - 1;
        //}


        ///// <summary>
        ///// Determines if the cashflow group with the supplied id is the first in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroupId">The id of the cashflow group being tested.</param>
        ///// <returns>True if last.</returns>
        ///// <exception cref="ArgumentException">Thrown if the cashflow group isn't found.</exception>
        //public bool IsCashflowGroupLast(string cashflowGroupId)
        //{
        //    return GetCashflowGroupIndex(cashflowGroupId) == CashflowGroups.Count - 1;
        //}


        ///// <summary>
        ///// Moves the supplied cashflow group up in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group to be moved.</param>
        //public void MoveCashflowGroupUp(CashflowGroup cashflowGroup)
        //{
        //    int targetIndex = GetCashflowGroupIndex(cashflowGroup);

        //    if (targetIndex == 0)
        //    {
        //        return;
        //    }

        //    LinkedList<CashflowGroup> newCashflowGroups = new LinkedList<CashflowGroup>();
        //    var en = CashflowGroups.GetEnumerator();
        //    int i = 0;

        //    while (en.MoveNext())
        //    {
        //        if (i == targetIndex - 1)
        //        {
        //            newCashflowGroups.AddLast(cashflowGroup);
        //            newCashflowGroups.AddLast(en.Current);
        //        }
        //        else if (en.Current.Id != cashflowGroup.Id)
        //        {
        //            newCashflowGroups.AddLast(en.Current);
        //        }

        //        i++;
        //    }

        //    CashflowGroups = newCashflowGroups;
        //}


        ///// <summary>
        ///// Moves the supplied cashflow group down in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group to be moved.</param>
        //public void MoveCashflowGroupDown(CashflowGroup cashflowGroup)
        //{
        //    int targetIndex = GetCashflowGroupIndex(cashflowGroup);

        //    if (targetIndex == CashflowGroups.Count - 1)
        //    {
        //        return;
        //    }

        //    LinkedList<CashflowGroup> newCashflowGroups = new LinkedList<CashflowGroup>();
        //    var en = CashflowGroups.GetEnumerator();
        //    int i = 0;

        //    while (en.MoveNext())
        //    {
        //        if (i == targetIndex + 1)
        //        {
        //            newCashflowGroups.AddLast(en.Current);
        //            newCashflowGroups.AddLast(cashflowGroup);
        //        }
        //        else if (en.Current.Id != cashflowGroup.Id)
        //        {
        //            newCashflowGroups.AddLast(en.Current);
        //        }

        //        i++;
        //    }

        //    CashflowGroups = newCashflowGroups;
        //}


        ///// <summary>
        ///// Adds a new cashflow group to the end of <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <returns>The new cashflow group.</returns>
        //public CashflowGroup AddCashflowGroup()
        //{
        //    return CashflowGroups.AddLast(new CashflowGroup()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Name = "New Cashflow Group",
        //        Description = ""
        //    }).Value;
        //}


        ///// <summary>
        ///// Deletes a cashflow group from <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroupId">The id of the cashflow group to be deleted.</param>
        //public void DeleteCashflowGroup(string cashflowGroupId)
        //{
        //    var newList = new LinkedList<CashflowGroup>();

        //    foreach (var item in CashflowGroups)
        //    {
        //        if (item.Id != cashflowGroupId)
        //        {
        //            newList.AddLast(item);
        //        }
        //    }

        //    CashflowGroups = newList;
        //}


        ///// <summary>
        ///// Upserts a cashflow group in <see cref="CashflowGroups"/>.
        ///// </summary>
        ///// <param name="cashflowGroup">The cashflow group to upsert.</para
        ///// <returns>The upserted cashflow group.</returns>
        //public CashflowGroup UpsertCashflowGroup(CashflowGroup cashflowGroup)
        //{
        //    var newList = new LinkedList<CashflowGroup>();
        //    var found = false;

        //    foreach (var item in CashflowGroups)
        //    {
        //        if (item.Id == cashflowGroup.Id)
        //        {
        //            newList.AddLast(cashflowGroup);
        //            found = true;
        //        }
        //        else
        //        {
        //            newList.AddLast(item);
        //        }
        //    }

        //    if (!found)
        //    {
        //        newList.AddLast(cashflowGroup);
        //    }

        //    CashflowGroups = newList;

        //    return cashflowGroup;
        //}


        ///// <summary>
        ///// Sets <see cref="AllTasks"/>.
        ///// </summary>
        ///// <param name="tasks">The complete task list.</param>
        //internal void SetAllTasks(ICollection<ProjectTask> tasks)
        //{
        //    AllTasks = new Dictionary<string, ProjectTask>();

        //    foreach (var item in tasks)
        //    {
        //        AllTasks.Add(item.Id, item);
        //    }
        //}
    }
}
