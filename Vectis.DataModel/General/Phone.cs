using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Class representing a phone number.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Telephone Number")]
    public class Phone : VectisBase
    {
        private string localFormat;
        /// <summary>
        /// The phone number formatted without the international dialing prefix.
        /// </summary>
        [MessagePack.Key(5)]
        [Display(Name = "Phone Number", Prompt = "Local format without international dialing prefix")]
        [Required, MinLength(1)]
        [Phone]
        [CustomValidation(typeof(Phone), nameof(ValidateLocalFormat))]
        public string LocalFormat { get => localFormat; set => Setter(ref localFormat, value); }


        private string internationalFormat;
        /// <summary>
        /// The phone number formatted with the international dialing prefix. 
        /// </summary>
        [MessagePack.Key(6)]
        [Display(Name = "International Number", Prompt = "International format with international dialing prefix")]
        public string InternationalFormat { get => internationalFormat; set => Setter(ref internationalFormat, value); }


        private string countryPrefix;
        /// <summary>
        /// The country code prefix, eg "+44" for the UK.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Country Prefix", Prompt = "Numeric international dialing prefix")]
        public string CountryPrefix { get => countryPrefix; set => Setter(ref countryPrefix, value); }


        private string countryCode = "GB";
        /// <summary>
        /// The country code, eg "GB" for the UK. 
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Country Code", Prompt = "Two letter international country code")]
        [Required, MinLength(1)]
        public string CountryCode { get => countryCode; set => Setter(ref countryCode, value); }


        /// <summary>
        /// Validates a local format phone number using <see cref="DataValidation.ValidatePhoneAsync(string, string)"/> .
        /// </summary>
        /// <param name="localFormat"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public static ValidationResult ValidateLocalFormat(string localFormat, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            Phone thisEntity = (Phone)validationContext.ObjectInstance;

            //Task.Run(async () =>
            //{
            //    var pi = await DataValidation.ValidatePhoneAsync(thisEntity.CountryCode, localFormat);

            //    if (pi.IsValid)
            //    {
            //        thisEntity.LocalFormat = pi.LocalFormat;
            //        thisEntity.InternationalFormat = pi.InternationalFormat;
            //        thisEntity.CountryPrefix = pi.CountryPrefix;
            //        thisEntity.CountryCode = pi.CountryCode;
            //    }

            //    result = pi.Result;
            //}).Wait();

            return result;
        }
    }
}
