using System;
using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class User : ModelBase
    {
        public User()
        {
            Questionnaires = new HashSet<Questionnaire>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastLogon { get; set; }
        

       public virtual ICollection<Questionnaire> Questionnaires { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}