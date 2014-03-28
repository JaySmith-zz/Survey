using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class QuestionnaireResponse
    {
        /// <summary>
        /// Questionnaire ID
        /// </summary>
        public int QuestionnaireId { get; set; }

        /// <summary>
        /// Survey ID
        /// </summary>
        public int SurveyId { get; set; }

        /// <summary>
        /// Current Category
        /// </summary>
        public string CurrentCategory { get; set; }

        /// <summary>
        /// Collection of question's responses
        /// </summary>
        public ICollection<Response> Responses { get; set; }
    }
}