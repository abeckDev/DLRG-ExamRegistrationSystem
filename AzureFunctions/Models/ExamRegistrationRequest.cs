using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Models
{
    public class ExamRegistrationRequest
    {
        public ExamRegistrationRequest(string Name, string Surname, DateTime Birthday, Landesverband landesverband)
        {
           this.Birthday = Birthday;
           this.Name = Name;
           this.Surname = Surname;
           this.Landesverband = landesverband;
        }

        public ExamRegistrationRequest(System.Collections.Specialized.NameValueCollection? reqQueryParameterCollection)
        {

            //save query parameters to string variables
            var name = reqQueryParameterCollection["name"];
            var surname = reqQueryParameterCollection["surname"];
            //save birthday from query parameters to DateTime variable
            var birthday = System.DateTime.Parse(reqQueryParameterCollection["birthday"]);
            //Save Landesverband from query parameters to Landesverband variable as enum Landesverband
            var landesverband = (Landesverband)System.Enum.Parse(typeof(Landesverband), reqQueryParameterCollection["landesverband"]);

            //Check if values above are null
            if (name == null || surname == null)
            {
                throw new ArgumentNullException("One or more query parameters are null");
            }

            //Initialize ExamRegistrationRequest
            this.Birthday = birthday;
            this.Name = name;
            this.Surname = surname;
            this.Landesverband = landesverband;
        }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public Landesverband Landesverband { get; set; }

    }
}
