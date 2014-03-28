using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Highway.Data;
using Survey.Core;
using Survey.Core.Domain;
using Survey.Core.Queries;

namespace Survey.ViewModels
{
    public class HomeEditViewModel
    {
        private readonly IRepository _repository;

        public HomeEditViewModel(IRepository repository, Questionnaire questionnaire)
        {
            _repository = repository;
            Questionnaire = questionnaire;

            RequiredQuestionIds = new List<string>();
            AnsweredQuestionIds = new List<string>();

            Load();
        }

        private void Load()
        {
            Categories = LoadCategories();
        }

        public Questionnaire Questionnaire { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        public IList<string> RequiredQuestionIds { get; set; }

        public IList<string> AnsweredQuestionIds { get; set; }

        public string QuestionnaireName
        {
            get { return string.Format("{0}", Questionnaire.Survey.Name); }
        }

        public int DisplayLogicDepth { get; set; }

        private IList<CategoryViewModel> LoadCategories()
        {
            var categoryViewModels = new List<CategoryViewModel>();
            var categories = Questionnaire.Survey.Categories
                                          .Where(x => x.Questions.Count != 0)
                                          .OrderBy(x => x.DisplayOrder).ToArray();

            for (int i = 0; i < categories.Count(); i++)
            {
                var category = categories[i];

                var item = new CategoryViewModel
                    {
                        Id = category.CategoryId,
                        SurveyId = category.SurveyId,
                        Name = category.Name,
                        Description = category.Description,
                        Code = category.CategoryCode,
                        Questions = LoadQuestionsFromCategory(category)
                    };
                categoryViewModels.Add(item);
            }

            return categoryViewModels;
        }

        /// <summary>
        /// Load response value for question type date, number and text.
        /// </summary>
        /// <param name="question">The question that needs to be enabled or disabled</param>
        /// <returns>Response Value</returns>
        private string LoadSingleResponseValue(Question question)
        {
            string value = string.Empty;

            if (question.Responses.ToList().Any(x => x.QuestionId == question.QuestionId && x.QuestionnaireId == Questionnaire.QuestionnaireId))
            {
                var response = question.Responses.ToList().Single(x => x.QuestionId == question.QuestionId && x.QuestionnaireId == Questionnaire.QuestionnaireId);
                value = response.Value;
            }

            return value;
        }

        private IList<QuestionViewModel> LoadQuestionsFromCategory(Category category)
        {
            var questionViewModels = new List<QuestionViewModel>();
            var questions = category.Questions.Where(x => x.Status == QuestionStatus.Active).OrderBy(x => x.DisplayOrder).ToArray();

            for (int i = 0; i < questions.Count(); i++)
            {
                var question = questions[i];

                var qvm = new QuestionViewModel
                    {
                        Id = question.QuestionId,
                        Question = question.DisplayText,
                        QuestionCode = question.QuestionCode,
                        QuestionType = question.InputType
                    };

                if (question.InputType.ToUpper() == "DATE" || question.InputType.ToUpper() == "NUMBER" || question.InputType.ToUpper() == "TEXT")
                {
                    qvm.Value = LoadSingleResponseValue(question);
                }

                qvm.DisplayLogic = question.DisplayLogic;
                qvm.IsRequired = question.IsRequired;
                qvm.IsKey = question.IsKeyQuestion;
                qvm.Choices = LoadQuestionChoices(question);
                qvm.Status = DetermineQuestionStatus(question);//question.Status;

                questionViewModels.Add(qvm);

                if (qvm.Status != QuestionStatus.Disabled && qvm.IsRequired)
                {
                    RequiredQuestionIds.Add(qvm.Id.ToString(CultureInfo.InvariantCulture));
                }

                if (qvm.DisplayLogic.IsNotNullOrEmpty() && qvm.DisplayLogic.Contains("!="))
                {
                    qvm.Status = QuestionStatus.Disabled;
                    continue;
                }


                var responses =
                    question.Responses.Where(
                        response =>
                        response.QuestionnaireId == Questionnaire.QuestionnaireId && response.QuestionId == question.QuestionId);

                foreach (var response in responses)
                {
                    if (response.IsSelected == "_NA_")
                    {
                        qvm.Status = QuestionStatus.Disabled;
                        if (RequiredQuestionIds.Contains(qvm.Id.ToString(CultureInfo.InvariantCulture)))
                        {
                            RequiredQuestionIds.Remove(qvm.Id.ToString(CultureInfo.InvariantCulture));
                        }
                    }

                    if (!string.IsNullOrEmpty(response.Value))
                    {
                        if (response.Value.Contains("Other") && response.IsSelected == "True")
                        {
                            RequiredQuestionIds.Add(response.QuestionId.ToString(CultureInfo.InvariantCulture) + "_Comment");
                        }
                    }
                }
                AddToAnsnweredIds(question);
            }

            return questionViewModels;
        }

        private string DetermineQuestionStatus(Question question)
        {
            var returnValue = QuestionStatus.Active;

            if (question.DisplayLogic.IsNotNullOrEmpty())
            {
                var displayLogic = question.DisplayLogic;
                var questionCode = question.DisplayLogic.Split(' ')[0];

                var query = new QuestionByQuestionCode(questionCode);
                var parentCode = _repository.Find(query);

                if (parentCode != null)
                {
                    var parentResponses =
                        Questionnaire.Responses.Where(
                            x =>
                            x.QuestionnaireId == Questionnaire.QuestionnaireId && x.QuestionId == parentCode.QuestionId)
                            .Where(x => x.IsSelected == "True" || x.IsSelected == "_NA_");

                    foreach (var response in parentResponses)
                    {
                        var result = DisplayLogicEvaluator.Evaluate(questionCode, displayLogic, response.Value);
                        returnValue = result ? QuestionStatus.Disabled : QuestionStatus.Active;
                    }
                }
                else
                {
                    returnValue = QuestionStatus.Active;
                }
            }

            return returnValue;

        }

        private void AddToAnsnweredIds(Question question)
        {
            switch (question.InputType)
            {
                case QuestionInputType.Date:
                case QuestionInputType.Number:
                case QuestionInputType.Text:
                    {
                        var responses = question.Responses.Where(response => !string.IsNullOrEmpty(response.Value) && response.QuestionnaireId == Questionnaire.QuestionnaireId);

                        foreach (var response in responses)
                            if (!AnsweredQuestionIds.Contains(response.QuestionId.ToString(CultureInfo.InvariantCulture)))
                            {
                                AnsweredQuestionIds.Add(response.QuestionId.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                    }
                    break;

                default:
                    {
                        var responses = question.Responses.Where(response => response.IsSelected == "True" && response.QuestionnaireId == Questionnaire.QuestionnaireId);
                        foreach (var response in responses)
                        {
                            if (!AnsweredQuestionIds.Contains(response.QuestionId.ToString(CultureInfo.InvariantCulture)))
                            {
                                AnsweredQuestionIds.Add(response.QuestionId.ToString(CultureInfo.InvariantCulture));
                                //break;
                            }
                            /* Add comment box id to answer question id array */
                            if (string.IsNullOrEmpty(response.Comment) == false && response.Value == "Other")
                            {
                                if (AnsweredQuestionIds.Contains(response.QuestionId.ToString(CultureInfo.InvariantCulture) + "_Comment") == false)
                                {
                                    AnsweredQuestionIds.Add(response.QuestionId.ToString(CultureInfo.InvariantCulture) + "_Comment");
                                }
                            }
                        }

                        break;
                    }
            }
        }

        private IList<QuestionChoice> LoadQuestionChoices(Question question)
        {
            var questionChoices = new List<QuestionChoice>();
            var choices = question.AnswerChoices.OrderBy(x => x.AnswerChoiceId).ToList();

            List<Response> responses = question.Responses.ToList();
            /* Take care of checkbox, radiobutton, and dropdownlist type response */
            foreach (var choice in choices)
            {
                var result = responses.Where(x => x.QuestionId == choice.QuestionId && x.Value == choice.DisplayText && x.QuestionnaireId == Questionnaire.QuestionnaireId)
                                      .Select(x => new { x.IsSelected, x.Comment }).FirstOrDefault();

                var qc = new QuestionChoice();

                if (result != null)
                {
                    qc.Comment = result.Comment;
                    qc.SelectedInd = result.IsSelected;
                }
                qc.IsRequired = choice.Question.IsRequired;
                qc.DisplayText = choice.DisplayText;
                qc.Identifier = choice.AnswerChoiceId;

                questionChoices.Add(qc);
            }

            return questionChoices;
        }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }

        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string Value { get; set; }
        public string QuestionCode { get; set; }
        public string DisplayLogic { get; set; }
        public bool IsKey { get; set; }
        public bool IsRequired { get; set; }
        public string QuestionComment { get; set; }
        public string Status { get; set; }

        public IList<QuestionChoice> Choices { get; set; }
    }

    public class QuestionChoice
    {
        public int Identifier { get; set; }
        public string Comment { get; set; }
        public string SelectedInd { get; set; }
        public string DisplayText { get; set; }
        public bool IsRequired { get; set; }
    }

    public class DisplayLogic
    {
        public string QuestionCode { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}