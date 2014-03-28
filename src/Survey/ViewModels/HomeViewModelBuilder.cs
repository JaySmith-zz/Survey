using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Survey.Core;
using Survey.Core.Domain;

namespace Survey.ViewModels
{
    public interface IHomeViewModelBuilder
    {
        IEnumerable<HomeIndexViewModel> GetHomeIndexModel(IEnumerable<Questionnaire> questionnaires);
    }

    public class HomeViewModelBuilder : IHomeViewModelBuilder
    {

        public IEnumerable<HomeIndexViewModel> GetHomeIndexModel(IEnumerable<Questionnaire> questionnaires)
         {
             var items = new List<HomeIndexViewModel>();

             foreach (var item in questionnaires)
             {
                 var homeIndexItem = new HomeIndexViewModel
                 {
                     Status = item.Status,
                     StatusSelectListItems = BuildStatusDropDown(item.Status),
                     SurveyName = item.Survey.Name,
                     Id = item.QuestionnaireId,
                     Owner = item.User.FullName,
                     QuestionnareName = item.QuestionniareName,
                     PersonnelAreaNumber = item.PersonnelAreaNumber,
                     ModifiedDate = item.Modified,
                     ModifiedBy = "NA",
                     UserId = item.User.Id
                 };

                 items.Add(homeIndexItem);
             }

             return items;
         }

        private List<SelectListItem> BuildStatusDropDown(string selectedStatus)
        {
            var items = Globals.QuestionnaireStatuses();
            items.Single(x => x.Text == selectedStatus).Selected = true;

            return items;
        }
    }
}