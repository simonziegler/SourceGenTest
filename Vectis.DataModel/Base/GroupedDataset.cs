using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A dataset for the given <see cref="VectisBase"/> parent object.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Grouped Dataset")]
    public class GroupedDataset : VectisBase
    {
        /// <summary>
        /// The <see cref="VectisBase"/> that is the dataset's parent.
        /// </summary>
        [MessagePack.Key(5)]
        public VectisBase Parent { get; init; }


        private IReadOnlyList<VectisBase> itemList = new ReadOnlyCollection<VectisBase>(new List<VectisBase>());
        /// <summary>
        /// List of all items that are logical children the parent.
        /// </summary>
        [MessagePack.Key(6)]
        public IReadOnlyList<VectisBase> ItemList
        {
            get => itemList;
            init => itemList = new ReadOnlyCollection<VectisBase>(value.ToList());
        }


        private ReadOnlyDictionary<string, VectisBase> itemDictionary;
        /// <summary>
        /// Dictionary of all items that are logical children the parent indexed by <see cref="VectisBase.Id"/>.
        /// </summary>
        [VectisSerializationIgnore]
        [MessagePack.IgnoreMember]
        public ReadOnlyDictionary<string, VectisBase> ItemDictionary
        {
            get
            {
                if (itemDictionary == null)
                {
                    itemDictionary = new ReadOnlyDictionary<string, VectisBase>(ItemList.ToDictionary(i => i.Id));
                }

                return itemDictionary;
            }
        }


        private ReadOnlyDictionary<Type, ReadOnlyDictionary<string, VectisBase>> typedItems;
        /// <summary>
        /// Dictionary of dictionaries. The parent dictionary is indexed by type (descendents of <see cref="VectisBase"/>.
        /// Child dictionaries are then all items of the parent dictionary's key's type, indexed by <see cref="VectisBase.Id"/>.
        /// </summary>
        [VectisSerializationIgnore]
        [MessagePack.IgnoreMember]
        public ReadOnlyDictionary<Type, ReadOnlyDictionary<string, VectisBase>> TypedItems
        {
            get
            {
                if (typedItems == null)
                {
                    Dictionary<Type, Dictionary<string, VectisBase>> dict = new();

                    foreach (var item in ItemList)
                    {
                        var types = item.GroupedDatasetTypes();

                        foreach (var type in types)
                        {
                            if (!dict.TryGetValue(type, out Dictionary<string, VectisBase> typeDict))
                            {
                                typeDict = new();
                                dict.Add(type, typeDict);
                            }

                            typeDict.Add(item.Id, item);
                        }
                    }

                    typedItems = new(dict.ToDictionary(outer => outer.Key, outer => new ReadOnlyDictionary<string, VectisBase>(outer.Value)));
                }

                return typedItems;
            }
        }


        public GroupedDataset() { }


        /// <summary>
        /// Contructor taking the <see cref="Parent"/> and (an assumed complete) enumerable of every <see cref="VectisBase"/> item
        /// belonging to that scheme. Performs a DeepCopy on the parent and 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="items"></param>
        public GroupedDataset(VectisBase parent, IEnumerable<VectisBase> items, bool freeze = false)
        {
            Parent = parent.DeepCopy();

            var list = items.DeepCopy().ToList();

            Parent.GroupedDataset = this;
            list.ForEach(item => item.GroupedDataset = this);

            if (freeze)
            {
                Freeze();
            }

            ItemList = new ReadOnlyCollection<VectisBase>(list);
        }


        /// <summary>
        /// Creates a new GroupedDataset using the parent and items from the provided copy.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="items"></param>
        public GroupedDataset(GroupedDataset groupedDataset, bool freeze = false) : this(groupedDataset.Parent, groupedDataset.ItemList, freeze) { }


        /// <summary>
        /// Gets a strongly typed item from the dataset with the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetItem<T>(string id) where T : VectisBase
        {
            var type = typeof(T);

            if (!ItemDictionary.TryGetValue(id, out VectisBase result))
            {
                return null;
            }

            if (result is T)
            {
                return result as T;
            }

            throw new ArgumentException($"VectisModel SchemeDataset.GetItem<T>() cannot return a '{result.GetType()}' as a '{type}' with Id '{id}'");
        }


        /// <summary>
        /// Gets a collection of all items of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetItems<T>()
        {
            if (!TypedItems.TryGetValue(typeof(T), out ReadOnlyDictionary<string, VectisBase> dict))
            {
                return new List<T>();
            }

            return dict.Values.Cast<T>().ToList();
        }


        /// <summary>
        /// Returns a new <see cref="GroupedDataset"/>, frozen as specified.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupedDataset Duplicate(bool freeze = false) => new(Parent, ItemList, freeze);


        /// <summary>
        /// Returns a new <see cref="GroupedDataset"/> with the indicated item removed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupedDataset Remove(string id, bool freeze = false)
        {
            var dict = new Dictionary<string, VectisBase>(ItemDictionary);

            if (dict.TryGetValue(id, out VectisBase _))
            {
                dict.Remove(id);
            }

            return new GroupedDataset(Parent, dict.Values, freeze);
        }


        /// <summary>
        /// Returns a new <see cref="GroupedDataset"/> with the indicated item upserted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupedDataset Upsert(VectisBase item, bool freeze = false)
        {
            var dict = new Dictionary<string, VectisBase>(ItemDictionary);

            if (!dict.TryAdd(item.Id, item))
            {
                dict[item.Id] = item;
            }

            return new GroupedDataset(Parent, dict.Values, freeze);
        }


        /// <summary>
        /// Overrides <see cref="VectisBase.Freeze"/>, and freezes all contained items.
        /// </summary>
        public new void Freeze()
        {
            Parent.Freeze();

            foreach (var item in ItemList)
            {
                item.Freeze();
            }

            base.Freeze();
        }
    }
}
