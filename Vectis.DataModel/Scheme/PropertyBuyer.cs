using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Details for a property buyer.
    /// </summary>
    [TypeDiscriminator("Property Buyer")]
    [MessagePack.MessagePackObject]
    public class PropertyBuyer : SchemeBase
    {
        private Salutation salutation;
        /// <summary>
        /// The buyer's salutation.
        /// </summary>
        [MessagePack.Key(10)]
        public Salutation Salutation { get => salutation; set => Setter(ref salutation, value); }


        private string givenName;
        /// <summary>
        /// Given name.
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "Given Name", Prompt = "Buyer's given name")]
        public string GivenName { get => givenName; set => Setter(ref givenName, value); }


        private string familyName;
        /// <summary>
        /// Family name.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Family Name", Prompt = "Buyer's family name")]
        public string FamilyName { get => familyName; set => Setter(ref familyName, value); }


        private Address address;
        /// <summary>
        /// Address.
        /// </summary>
        [MessagePack.Key(13)]
        public Address Address { get => address; set => Setter(ref address, value); }


        private Phone phone;
        /// <summary>
        /// Phone number.
        /// </summary>
        [MessagePack.Key(14)]
        public Phone Phone { get => phone; set => Setter(ref phone, value); }


        private string email;
        /// <summary>
        /// Email address.
        /// </summary>
        [MessagePack.Key(15)]
        public string Email { get => email; set => Setter(ref email, value); }
    }
}
