using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Expresses the parameters of a Vectis installation.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Vectis Installation Definition")]
    public class Installation : VectisBase
    {
        /// <summary>
        /// The value assigned to all installation partition keys.
        /// </summary>
        public const string InstallationPartitionKeyValue = "Installation";


        /// <summary>
        /// A default message for when Vectis has been temporarily deactivated by Administrators.
        /// </summary>
        public const string DefaultSystemInactiveMessage = "Vectis is briefly down for maintenance - please visit soon";



        /// <summary>
        /// The "localhost" host name.
        /// </summary>
        public const string Localhost = "localhost";
        private const string Delimiter = "\n";
        

        private string name = "";
        /// <summary>
        /// The installation name - usually the client's name.
        /// </summary>
        [MessagePack.Key(5)]
        [Required, MinLength(1)]
        [Display(Name = "Client's Name", Prompt = "Client/Lender's name")]
        public string Name { get => name; set => Setter(ref name, value); }


        private string requestedContainerName = "";
        /// <summary>
        /// Azure Cosmos DB container name.
        /// </summary>
        [MessagePack.Key(6)]
        [Required, MinLength(1)]
        [Display(Name = "Container Name", Prompt = "Database container name")]
        public string RequestedContainerName { get => requestedContainerName; set => Setter(ref requestedContainerName, value); }


        private bool isActive = true;
        /// <summary>
        /// The installation is active if "True".
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Is Active", Prompt = "Sets the lender's system as active or inactive")]
        public bool IsActive { get => isActive; set => Setter(ref isActive, value); }


        private string inactiveMessage = DefaultSystemInactiveMessage;
        /// <summary>
        /// Message to display to users when the system is inactive.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Inactive Message", Prompt = "Message to display to users when the system is inactive")]
        [Required, MinLength(1)]
        public string InactiveMessage { get => inactiveMessage; set => Setter(ref inactiveMessage, value); }


        private ICollection<string> webHostingDomains = new List<string>();
        /// <summary>
        /// Domains where the installation is hosted.
        /// </summary>
        [MessagePack.Key(9)]
        [ValidateComplexType]
        [Display(Name = "Web Hosting Domains", Prompt = "One domain name per row")]
        public ICollection<string> WebHostingDomains { get => webHostingDomains; set => Setter(ref webHostingDomains, value); }


        private ICollection<string> lenderEmailDomains = new List<string>();
        /// <summary>
        /// Email domains indicating that a new user is to be granted lender status as a row delimited string.
        /// </summary>
        [MessagePack.Key(10)]
        [ValidateComplexType]
        [Display(Name = "Lender Email Domains", Prompt = "One domain name per row - all emails in the domain are lender")]
        public ICollection<string> LenderEmailDomains { get => lenderEmailDomains; set => Setter(ref lenderEmailDomains, value); }


        private ICollection<string> lenderEmails = new List<string>();
        /// <summary>
        /// Specific email addresses indicating that a new user is to be granted lender status as a row delimited string.
        /// </summary>
        [MessagePack.Key(11)]
        [ValidateComplexType]
        [Display(Name = "Lender Emails", Prompt = "One domain name per row - each email is a lender")]
        public ICollection<string> LenderEmails { get => lenderEmails; set => Setter(ref lenderEmails, value); }



        /// <summary>
        /// The full container name including the ID
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public string FullContainerName
        {
            get => $"{RequestedContainerName} {{{Id}}}";
        }


        public Installation() => PartitionKey = InstallationPartitionKeyValue;


        /// <summary>
        /// <see cref="WebHostingDomains"/> as a row delimted string.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public string WebHostingDomainsString
        {
            get => CollectionToString(WebHostingDomains);
            set => WebHostingDomains = StringToCollection(value);
        }


        /// <summary>
        /// <see cref="LenderEmailDomains"/> as a row delimited string.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public string LenderEmailDomainsString
        {
            get => CollectionToString(LenderEmailDomains);
            set => LenderEmailDomains = StringToCollection(value);
        }


        /// <summary>
        /// <see cref="LenderEmails"/> as a row delimited string.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public string LenderEmailsString
        {
            get => CollectionToString(LenderEmails);
            set => LenderEmails = StringToCollection(value);
        }



        private static string CollectionToString(ICollection<string> coll)
        {
            string result = "";
            string delim = "";

            foreach (var x in coll)
            {
                result += delim + x;
                delim = Delimiter;
            }

            return result;
        }

        private static ICollection<string> StringToCollection(string str)
        {
            var list = str.Split(Delimiter);

            var result = new List<string>();

            foreach (var x in list)
            {
                if (!string.IsNullOrWhiteSpace(x))
                {
                    result.Add(x.Trim());
                }
            }

            result.Sort();

            return result;
        }
    }
}
