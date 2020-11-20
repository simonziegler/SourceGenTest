using System;
using System.Collections.Generic;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A base class for revision versions with a locking mechanism
    /// </summary>
    public abstract class RevisionVersion : SchemeBase
    {
        /// <summary>
        /// The parent revision's id.
        /// </summary>
        [MessagePack.Key(5)]
        public string RevisionId { get; set; }


        /// <summary>
        /// The id of the base version from which this one was forked. Unassigned if this is the first version.
        /// </summary>
        [MessagePack.Key(6)]
        public string ForkedBaseVersionId { get; set; }


        /// <summary>
        /// The date/time at which this object was locked.
        /// </summary>
        [MessagePack.Key(7)]
        public DateTime LockedDateTime { get; set; }


        /// <summary>
        /// The id of the user who created this object.
        /// </summary>
        [MessagePack.Key(8)]
        public string LockedByUserId { get; set; }


        /// <summary>
        /// A calculated version number.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public int VersionNumber => AssociatedVersions?.OrderBy(version => version.CreatedDateTime).ToList().IndexOf(this) + 1 ??  0;


        /// <summary>
        /// True if this version has locked by having been replaced by another.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsLocked => !string.IsNullOrWhiteSpace(LockedByUserId);


        /// <summary>
        /// True if this is the scheme's default version. The default version is the latest version of the default (latest) revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsRevisionDefaultVersion
        {
            get
            {
                var versionNumber = VersionNumber;

                if (versionNumber == 0)
                {
                    return false;
                }

                return versionNumber == AssociatedVersions.Count;
            }
        }


        /// <summary>
        /// True if this is the scheme's default version. The default version is the latest version of the default (latest) revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsSchemeDefaultVersion
        {
            get
            {
                if (!GroupedDataset.GetItem<Revision>(RevisionId).IsDefaultRevision)
                {
                    return false;
                }

                return IsRevisionDefaultVersion;
            }
        }


        /// <summary>
        /// True if this is the latest version of the active revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsActiveVersion
        {
            get
            {
                if (!GroupedDataset.GetItem<Revision>(RevisionId).IsActiveRevision)
                {
                    return false;
                }

                var versionNumber = VersionNumber;

                if (versionNumber == 0)
                {
                    return false;
                }

                return versionNumber == AssociatedVersions.Count;
            }
        }


        /// <summary>
        /// Returns all versions associated with this version's revision. Return null if GroupedDatasets is null.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public abstract List<RevisionVersion> AssociatedVersions { get; }


        /// <summary>
        /// Returns all objects that are children of this version.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public abstract List<VectisBase> ChildObjects { get; }
    }
}
