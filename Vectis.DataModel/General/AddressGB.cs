using Vectis.DataModel;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// An <see cref="Address"/> in the United Kingdom.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("UK Postal Address")]
    public class AddressGB : Address
    {
        private string street1;
        /// <summary>
        /// First line of the street. Required.
        /// </summary>
        [MessagePack.Key(6)]
        [Display(Name = "Street 1", Prompt = "First street line")]
        [Required, MinLength(1)]
        public string Street1 { get => street1; set => Setter(ref street1, value); }


        private string street2;
        /// <summary>
        /// Second line of the street.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Street 2", Prompt = "Second street line")]
        public string Street2 { get => street2; set => Setter(ref street2, value); }


        private string town;
        /// <summary>
        /// Town name. Required.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Town", Prompt = "Town name")]
        [Required, MinLength(1)]
        public string Town { get => town; set => Setter(ref town, value); }


        private string county;
        /// <summary>
        /// County name.
        /// </summary>
        [MessagePack.Key(9)]
        [Display(Name = "County", Prompt = "Country name")]
        public string County { get => county; set => Setter(ref county, value); }


        private string postcode;
        /// <summary>
        /// Postcode, validated against address servers. Required.
        /// </summary>
        [MessagePack.Key(10)]
        [Display(Name = "Postcode", Prompt = "UK Postcode")]
        [Required, MinLength(1)]
        //[CustomValidation(typeof(AddressGB), nameof(ValidatePostcode))]
        public string Postcode { get => postcode; set => Setter(ref postcode, value); }


        public AddressGB() : base() { }
    }
}
