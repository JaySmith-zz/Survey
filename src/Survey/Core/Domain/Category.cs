using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class Category : ModelBase
    {
        public Category()
        {
            Questions = new List<Question>();
            Questionnaires = new List<Questionnaire>();
        }

        public int CategoryId { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int DisplayOrder { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }

        public string CategoryCode
        {
            get
            {
                //return Name.Replace(" ", string.Empty);
                return "CAT_" + CategoryId.ToString().PadLeft(5, '0');
            }
        }

        
    }

    public class CategoryStatus
    {
        public const string Active = "Active";
        public const string InActive = "InActive";
    }
}