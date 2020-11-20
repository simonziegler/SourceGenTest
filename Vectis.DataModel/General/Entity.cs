using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A Vectis entity.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Legal Entity")]
    public class Entity : VectisBase
    {
        /// <summary>
        /// The value assigned to all entity partition keys.
        /// </summary>
        public const string EntityPartitionKeyValue = "Entity";


        private string name;
        /// <summary>
        /// The entity/company name.
        /// </summary>
        [MessagePack.Key(5)]
        [Required, MinLength(1)]
        [Display(Name = "Entity Name", Prompt = "The entity's name")]
        public string Name { get => name; set => Setter(ref name, value); }


        private Address address;
        /// <summary>
        /// The address.
        /// </summary>
        [MessagePack.Key(6)]
        [ValidateComplexType]
        public Address Address { get => address; set => Setter(ref address, value); }


        private string email;
        /// <summary>
        /// Email address, required.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Email", Prompt = "The entity central email address")]
        [Required]
        [EmailAddress]
        [CustomValidation(typeof(Entity), nameof(ValidateEmail))]
        public string Email { get => email; set => Setter(ref email, value); }


        private Phone phone = new();
        /// <summary>
        /// Phone number, required.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Phone", Prompt = "The entity main phone number")]
        [Required]
        [ValidateComplexType]
        public Phone Phone { get => phone; set => Setter(ref phone, value); }


        /// <summary>
        /// True if the entity details have been saved.
        /// </summary>
        [MessagePack.Key(9)]
        public bool HasBeenSaved { get; set; }



        public Entity() => PartitionKey = EntityPartitionKeyValue;


        /// <summary>
        /// Validates the email address with <see cref="DataValidation.ValidateEmailAsync(string)"/>.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public static ValidationResult ValidateEmail(string email, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            Entity thisEntity = (Entity)validationContext.ObjectInstance;

            //Task.Run(async () =>
            //{
            //    var ei = await DataValidation.ValidateEmailAsync(email);

            //    if (ei.Validity == DataValidation.EmailValidity.Valid)
            //    {
            //        thisEntity.Email = ei.Email;
            //    }

            //    result = ei.Result;
            //}).Wait();

            return result;
        }
    }
}
