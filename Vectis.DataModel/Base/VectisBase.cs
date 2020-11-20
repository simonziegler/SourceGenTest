using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vectis.DataModel
{
    /// <summary>
    /// The base class for all Vectis data model objects. Both parameters are set automatically on construction.
    /// </summary>
    
    /// Base Classes
    [MessagePack.Union(0, typeof(GroupedDataset))]

    // General Classes
    [MessagePack.Union(100, typeof(AddressGB))]
    [MessagePack.Union(101, typeof(Entity))]
    [MessagePack.Union(102, typeof(Phone))]
    [MessagePack.Union(103, typeof(UserInfo))]

    // Geospatial Classes
    [MessagePack.Union(200, typeof(Coordinate))]

    //Installation Classes
    [MessagePack.Union(300, typeof(Installation))]
    [MessagePack.Union(301, typeof(InstallationCostBudgetOriginator))]
    [MessagePack.Union(303, typeof(InstallationCostCategory))]
    [MessagePack.Union(304, typeof(InstallationRevenueBudgetOriginator))]
    [MessagePack.Union(305, typeof(InstallationRevenueCategory))]
    [MessagePack.Union(306, typeof(LenderDetails))]

    // Scheme
    [MessagePack.Union(400, typeof(PropertyBuyer))]
    [MessagePack.Union(401, typeof(Scheme))]
    [MessagePack.Union(402, typeof(SchemeCostCategory))]
    [MessagePack.Union(403, typeof(SchemeRevenueCategory))]

    // Scheme Capital Structure
    [MessagePack.Union(500, typeof(CapitalStructureRevision))]
    [MessagePack.Union(501, typeof(CapitalStructureRevisionVersion))]
    [MessagePack.Union(502, typeof(FundingSourceDrawdown))]
    [MessagePack.Union(503, typeof(FundingSourceRepayment))]
    [MessagePack.Union(504, typeof(InterestRatePair))]
    [MessagePack.Union(505, typeof(OrdinaryEquity))]
    [MessagePack.Union(506, typeof(PIKAccrualCommittedLoan))]
    [MessagePack.Union(507, typeof(PIKDrawdownCommittedLoan))]
    [MessagePack.Union(508, typeof(ZeroCashflowPreferredEquity))]

    // Scheme Cost Schedule
    [MessagePack.Union(600, typeof(CostScheduleActualAmount))]
    [MessagePack.Union(601, typeof(CostScheduleBudgetAmount))]
    [MessagePack.Union(602, typeof(CostScheduleItem))]
    [MessagePack.Union(603, typeof(CostScheduleRevision))]
    [MessagePack.Union(604, typeof(CostScheduleRevisionVersion))]
    [MessagePack.Union(605, typeof(CashflowChartRow))]

    // Scheme Evaluation
    [MessagePack.Union(700, typeof(EvaluationChartingResults))]
    [MessagePack.Union(701, typeof(EvaluationDataset))]
    [MessagePack.Union(702, typeof(EvaluationResultsSummary))]
    [MessagePack.Union(703, typeof(UnderwritingCondition))]
    [MessagePack.Union(704, typeof(UnderwritingResult))]

    // Scheme Project
    [MessagePack.Union(800, typeof(AppraisalCost))]
    [MessagePack.Union(801, typeof(AppraisalCostGroup))]
    [MessagePack.Union(802, typeof(AppraisalCostRate))]
    [MessagePack.Union(803, typeof(AppraisalRevenue))]
    [MessagePack.Union(804, typeof(AppraisalRevenueGroup))]
    [MessagePack.Union(805, typeof(AppraisalRevenueRate))]
    [MessagePack.Union(806, typeof(AreaScheduleItem))]
    [MessagePack.Union(807, typeof(ProjectRevision))]
    [MessagePack.Union(808, typeof(ProjectRevisionVersion))]
    [MessagePack.Union(809, typeof(ProjectTask))]
    [MessagePack.Union(810, typeof(ProjectTaskCostCategoryAllocation))]
    [MessagePack.Union(811, typeof(ProjectTaskLink))]
    [MessagePack.Union(812, typeof(ProjectTaskRevenueCategoryAllocation))]

    //Scheme Revenue Schedule
    [MessagePack.Union(900, typeof(RevenueScheduleActualAmount))]
    [MessagePack.Union(901, typeof(RevenueScheduleActualAreaScheduleAmount))]
    [MessagePack.Union(902, typeof(RevenueScheduleBudgetAmount))]
    [MessagePack.Union(903, typeof(RevenueScheduleItem))]
    [MessagePack.Union(904, typeof(RevenueScheduleRevision))]
    [MessagePack.Union(905, typeof(RevenueScheduleRevisionVersion))]

    [MessagePack.MessagePackObject]
    public abstract class VectisBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Marks the property to be ignored by <see cref="NewtonsoftJsonConverter{T}"/> and <see cref="SystemTextJsonConverter{T}"/>. Provided
        /// as a convenience to avoid using both [JsonIgnore] attributes. This is a protected sealed member of <see cref="ViewModelBase"/> to
        /// restrict usage to objects descending from <see cref="ViewModelBase"/>.
        /// <para>
        /// DO NOT USE IF YOU ARE NOT SERIALIZING WITH EITHER OF THE CONVERTERS NOTED ABOVE.
        /// </para>
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        protected sealed class VectisSerializationIgnoreAttribute : Attribute { }


        /// <summary>
        /// Marks the property to be ignored by <see cref="NewtonsoftJsonConverter{T}"/> and <see cref="SystemTextJsonConverter{T}"/>. Provided
        /// as a convenience to avoid using both [JsonIgnore] attributes. This is a protected sealed member of <see cref="ViewModelBase"/> to
        /// restrict usage to objects descending from <see cref="ViewModelBase"/>.
        /// <para>
        /// DO NOT USE IF YOU ARE NOT SERIALIZING WITH EITHER OF THE CONVERTERS NOTED ABOVE.
        /// </para>
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        protected sealed class VectisSerializationPropertyNameAttribute : Attribute
        {
            public readonly string PropertyName;

            public VectisSerializationPropertyNameAttribute(string propertyName) => PropertyName = propertyName;
        }


        /// <summary>
        /// The object's partition key.
        /// </summary>
        [MessagePack.Key(0)]
        [VectisSerializationPropertyName("pk")]
        public string PartitionKey { get; set; }


        /// <summary>
        /// The object's unique ID.
        /// </summary>
        [MessagePack.Key(1)]
        [VectisSerializationPropertyName("id")]
        public string Id { get; set; }


        /// <summary>
        /// A unique id to enable view logic to determine whether an object has been updated. Suggested
        /// usage is for SignalR calls to notify clients of object updates using both the Id property and the
        /// ViewVersion. If a client has a copy of the object with a matching ViewVersion then it will
        /// know not to request an updated value via a server API call.
        /// </summary>
        [MessagePack.Key(2)]
        public string ViewVersion { get; set; }


        /// <summary>
        /// The date/time at which this object was first created.
        /// </summary>
        [MessagePack.Key(3)]
        public DateTime CreatedDateTime { get; set; }


        /// <summary>
        /// The id of the user who created this object.
        /// </summary>
        [MessagePack.Key(4)]
        public string CreatedByUserId { get; set; }


        /// <summary>
        /// A grouped dataset to which this object belongs.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public GroupedDataset GroupedDataset { get; set; }


        // This is false at the point when an object is deserializing, thereby not testing for frozen status.
        // The constructor subsequently sets it to true to allow a freeze test every time Setter is called.
        private readonly bool performFreezeTest;
        private bool frozen;


        /// <summary>
        /// Determines whether the object is frozen and therefore immutable. This is a one-way
        /// latch that cannot be reset to false once true.
        /// </summary>
        [MessagePack.IgnoreMember]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool Frozen { get => frozen; set => frozen = frozen ? frozen : value; }


        /// <summary>
        /// Blocks PropertyChanged events if true.
        /// </summary>
        [MessagePack.IgnoreMember]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool BlockPropertyChanged { get; set; }


        /// <summary>
        /// A change event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        public VectisBase()
        {
            performFreezeTest = true;
            PartitionKey = UniqueIdHelper.NewId();
            Id = UniqueIdHelper.NewId();
            ViewVersion = UniqueIdHelper.NewId();
            CreatedDateTime = DateTime.UtcNow;
            CreatedByUserId = "";
        }


        public VectisBase(string userId)
            : this()
        {
            CreatedByUserId = userId;
        }


        /// <summary>
        /// Freezes the object into immutability such that calling <see cref="Setter"/> throws an exception,
        /// irrespective of whether the relevant property has changed.
        /// </summary>
        public void Freeze() => Frozen = true;


        /// <summary>
        /// All property setters in derived classes should call this rather than perform the property set themselves.
        /// Throws an exception if the object is immutable/frozen irrespective of whether the property has changed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The stored property value that may need changing.</param>
        /// <param name="value">The new property value.</param>
        /// <returns>True if the value changed.</returns>
        protected bool Setter<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (performFreezeTest && Frozen)
            {
                throw new InvalidOperationException("Cannot set a property on a frozen instance");
            }

            if (!EqualityComparer<T>.Default.Equals(field, value) && !BlockPropertyChanged)
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName!));
                return true;
            }

            return false;
        }


        /// <summary>
        /// Performs a recursive deep copy of an object, determining the freeze status of the copy.
        /// </summary>
        /// <param name="frozen">Sets the frozen property of the copied object.</param>
        /// <returns>A deep copy with the frozen parameter set as specified.</returns>
        public T DeepCopy<T>(bool frozen) where T : VectisBase
        {
            var result = this.DeepCopy();
            result.frozen = frozen;
            return result as T;
        }


        /// <summary>
        /// Performs a recursive deep copy of an object.
        /// </summary>
        /// <param name="frozen">Sets the frozen property of the copied object.</param>
        /// <returns>A deep copy with the frozen parameter set as specified.</returns>
        public T ShallowCopy<T>() where T : VectisBase
        {
            return MemberwiseClone() as T;
        }
        
        
        /// <summary>
        /// Performs a recursive deep copy of an object, determining the freeze status of the copy and determining whether to retain the ViewVersion.
        /// </summary>
        /// <param name="frozen">Sets the frozen property of the copied object.</param>
        /// <param name="retainViewVersion">Copies the ViewVersion if set to true, otherwise allows a new ViewVersion to be used.</param>
        /// <returns>A deep copy with the frozen parameter set as specified.</returns>
        public T DeepCopyNewViewVersion<T>(bool frozen) where T : VectisBase
        {
            var result = DeepCopy<T>(frozen);
            result.ViewVersion = UniqueIdHelper.NewId();
            return result;
        }


        /// <summary>
        /// Returns all the types to group current class against in <see cref="GroupedDataset"/>, including
        /// actual type and all values applied by <see cref="GroupedDatasetAdditionalTypeAttribute"/>.
        /// </summary>
        /// <returns></returns>
        public ICollection<Type> GroupedDatasetTypes()
        {
            var result = new List<Type>
            {
                GetType()
            };

            foreach (var attribute in GetType().GetCustomAttributes<GroupedDatasetAdditionalTypeAttribute>())
            {
                result.Add(attribute.AdditionalType);
            }

            return result;
        }
    }
}
