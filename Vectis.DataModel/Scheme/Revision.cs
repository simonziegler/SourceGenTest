using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// Base class for all revisions.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class Revision : SchemeBase
    {
        private string name = "";
        /// <summary>
        /// The item's name.
        /// </summary>
        [MessagePack.Key(5)]
        [Required, MinLength(1)]
        [Display(Name = "Item Name", Prompt = "Enter a name for the area schedule item")]
        public string Name { get => name; set => Setter(ref name, value); }


        /// <summary>
        /// The id of the base revision from which this one was forked. Unassigned if this is the first version.
        /// </summary>
        [MessagePack.Key(6)]
        public string ForkedBaseRevisionId { get; set; }


        private string description = "";
        /// <summary>
        /// The item's description.
        /// </summary>
        [MessagePack.Key(7)]
        [Required, MinLength(1)]
        [Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        public string Description { get => description; set => Setter(ref description, value); }


        private RevisionDeploymentStatus revisionDeploymentStatus = RevisionDeploymentStatus.WorkInProgress;
        /// <summary>
        /// The revision's deployment status.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Deployment Status", Prompt = "The revision's deployment status")]
        public RevisionDeploymentStatus RevisionDeploymentStatus { get => revisionDeploymentStatus; set => Setter(ref revisionDeploymentStatus, value); }


        /// <summary>
        /// A calculated revision number.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public int RevisionNumber => AssociatedRevisions?.OrderBy(revision => revision.CreatedDateTime).ToList().IndexOf(this) + 1 ?? 0;


        /// <summary>
        /// True if this is the scheme's default (latest) revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public virtual bool IsDefaultRevision
        {
            get
            {
                var revisionNumber = RevisionNumber;

                if (revisionNumber == 0)
                {
                    return false;
                }

                return revisionNumber == AssociatedRevisions.Count;
            }
        }


        /// <summary>
        /// True if this is the scheme's active. A revision is active if either it is referenced as such within the <see cref="Scheme"/> or if
        /// there are no valid references the default revision is active.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsActiveRevision
        {
            get
            {
                var activeRevisionId = ActiveRevisionId ?? "";

                if (activeRevisionId == Id)
                {
                    return true;
                }

                return IsDefaultRevision;
            }
        }


        /// <summary>
        /// Returns the default version within the current revision.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public RevisionVersion DefaultRevisionVersion =>
            GroupedDataset?
            .GetItems<RevisionVersion>()
            .Where(item => item.RevisionId == Id && item.IsRevisionDefaultVersion)
            .FirstOrDefault();


        /// <summary>
        /// Returns all versions associated with this version's revision.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected abstract ICollection<Revision> AssociatedRevisions { get; }


        /// <summary>
        /// Returns the active revision Id from the <see cref="Scheme"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected abstract string ActiveRevisionId { get; }
    }
}
