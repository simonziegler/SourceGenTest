using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Vectis user information. This class is populated directly from the Azure B2C authentication
    /// system and therefore doesn't require validation or hints.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("User's Information")]
    public class UserInfo : VectisBase
    {
        /// <summary>
        /// The value assigned to all user info partition keys.
        /// </summary>
        public const string UserInfoPartitionKeyValue = "UserInfo";


        /// <summary>
        /// Email address, required.
        /// </summary>
        [MessagePack.Key(5)]
        [Required]
        public string Email { get; set; } = "";


        /// <summary>
        /// Given name, required.
        /// </summary>
        [MessagePack.Key(6)]
        [Required]
        public string GivenName { get; set; } = "";


        /// <summary>
        /// Surname, required.
        /// </summary>
        [MessagePack.Key(7)]
        [Required]
        public string FamilyName { get; set; } = "";


        /// <summary>
        /// Unique id of the entity to which the user belongs. See <see cref="Entity"/>.
        /// </summary>
        [MessagePack.Key(8)]
        [Required]
        public string EntityId { get; set; }


        /// <summary>
        /// The user's assigned role.
        /// </summary>
        [MessagePack.Key(9)]
        [Required]
        public UserRole Role { get; set; }


        /// <summary>
        /// Calculated full name "[Given Name] [Surname]".
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public string FullName => $"{GivenName} {FamilyName}";


        public UserInfo() => PartitionKey = UserInfoPartitionKeyValue;
    }
}
