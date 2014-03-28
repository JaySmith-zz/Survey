using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;
using Survey.Core.Domain;

namespace Survey.ViewModels
{
    public class HomeCopyViewModel
    {
        public Questionnaire SourceQuestionnaire { get; set; }
        public int SourceQuestionnaireId { get; set; }

        [Required(ErrorMessage = "* Target Questionnaire required!")]
        public int TargetQuestionId { get; set; }

        [DisplayName("Questionnaire")]
        public SelectList AvailableQuestionnaires { get; set; }
    }
}