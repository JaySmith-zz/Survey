using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using Survey.Core.Domain;

namespace Survey.Core
{
    public class QuestionnaireExcelExporter
    {
        public QuestionnaireExcelExporter(Questionnaire questionnaire)
        {
            _questionnaire = questionnaire;
        }

        private Questionnaire _questionnaire { get; set; }

        public MemoryStream Export()
        {
            var workBook = new XLWorkbook();

            var orderedCategories = _questionnaire.Survey.Categories.Where(x => x.Status == CategoryStatus.Active).OrderBy(x => x.DisplayOrder).ToList();

            foreach (var category in orderedCategories)
            {
                if (category.Questions.Count == 0)
                {
                    continue;
                }

                var categoryName = category.Name.Length < 30 ? category.Name : category.Name.Substring(0, 30);

                // removing illegal characters
                categoryName = categoryName.Replace('\\', ' ');
                categoryName = categoryName.Replace('/', ' ');
                categoryName = categoryName.Replace('?', ' ');
                categoryName = categoryName.Replace('*', ' ');
                categoryName = categoryName.Replace('[', ' ');
                categoryName = categoryName.Replace(']', ' ');

                if (workBook.Worksheets.Any(x => x.Name == categoryName))
                {
                    categoryName = categoryName.Substring(0, categoryName.Length - 1) + workBook.Worksheets.Count(x => x.Name == categoryName) + 1;
                }

                var sheet = workBook.Worksheets.Add(categoryName);
                sheet.Range("A1:S500").Style.Protection.SetLocked(false);
                sheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                sheet.Style.Alignment.WrapText = true;

                // Add header row
                var headerStyle = sheet.Cell(1, 1).Style;
                headerStyle.Font.Bold = true;
                headerStyle.Alignment.WrapText = true;
                headerStyle.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                headerStyle.Font.SetBold(true);
                headerStyle.Protection.SetLocked(true);

                sheet.Cell(1, 1).Value = category.Name;
                sheet.Cell(1, 1).Style.Font.FontSize = 14;
                sheet.Cell(1, 1).Style.Font.Bold = true;
                sheet.Range("A1:D1").Merge();
                sheet.Row(1).Height = 23;

                sheet.Cell(3, 1).Value = "Required";
                sheet.Cell(3, 1).Style = headerStyle;
                sheet.Column(1).Width = 10;

                sheet.Cell(3, 2).Value = "Directions";
                sheet.Cell(3, 2).Style = headerStyle;
                sheet.Column(2).Width = 30;

                sheet.Cell(3, 3).Value = "Question";
                sheet.Cell(3, 3).Style = headerStyle;
                sheet.Column(3).Width = 60;

                sheet.Cell(3, 4).Value = "Possible Answers";
                sheet.Cell(3, 4).Style = headerStyle;
                sheet.Column(4).Width = 30;

                sheet.Cell(3, 5).Value = "Answer";
                sheet.Cell(3, 5).Style = headerStyle;
                sheet.Column(5).Width = 30;
                sheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                sheet.Cell(3, 6).Value = "Comment";
                sheet.Cell(3, 6).Style = headerStyle;
                sheet.Column(6).Width = 30;
                sheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                var questions = category.Questions.Where(x => x.Status == QuestionStatus.Active).OrderBy(x => x.DisplayOrder).ToArray();
                for (int i = 0; i < questions.Length; i++)
                {
                    var currentRowIndex = i + 4;
                    var question = questions[i];

                    sheet.Cell(currentRowIndex, 1).Value = question.IsRequired ? "Yes" : "No";
                    sheet.Cell(currentRowIndex, 1).Style.Protection.SetLocked(true);
                    sheet.Cell(currentRowIndex, 2).Value = GetDirectionsForInputType(question.InputType);
                    sheet.Cell(currentRowIndex, 2).Style.Protection.SetLocked(true);
                    sheet.Cell(currentRowIndex, 3).Value = question.DisplayText;
                    sheet.Cell(currentRowIndex, 3).Style.Protection.SetLocked(true);
                    sheet.Cell(currentRowIndex, 4).Value = GetPossibleAnswerString(question.AnswerChoices);
                    sheet.Cell(currentRowIndex, 4).Style.Protection.SetLocked(true);

                    var responses = _questionnaire.Responses.Where(x => x.QuestionId == question.QuestionId).ToList();
                    sheet.Cell(currentRowIndex, 5).Value = GetAnswerResponseString(responses);
                    sheet.Cell(currentRowIndex, 6).Value = GetComment(responses);
                }

                var protection = sheet.Protect("HRSurvey_Protection");
                protection.SelectLockedCells = true;
                protection.SelectUnlockedCells = true;
            }

            var stream = new MemoryStream();
            workBook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private string GetComment(IEnumerable<Response> responses)
        {
            string comment = null;

            foreach (var response in responses)
            {
                if (response.IsSelected == null || response.Value == null || response.IsSelected.ToLower() != "true")
                    continue;

                if (response.Value.ToUpper().StartsWith("OTHER"))
                {
                    comment = response.Comment;
                }
            }

            return comment;
        }

        private string GetDirectionsForInputType(string inputType)
        {
            var message = string.Empty;

            switch (inputType)
            {
                case QuestionInputType.Checkbox:
                    message = "Select all that apply";
                    break;

                case QuestionInputType.RadioButton:
                case QuestionInputType.Dropdown:
                    message = "Select one answer";
                    break;

                case QuestionInputType.Date:
                    message = "Enter a date (mm/dd/yyyy)";
                    break;

                case QuestionInputType.Text:
                    message = "Enter text (max 100 characters)";
                    break;

                case QuestionInputType.Number:
                    message = "Enter a number";
                    break;
            }

            return message;
        }

        private string GetPossibleAnswerString(IEnumerable<AnswerChoice> items)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in items.OrderBy(x => x.AnswerChoiceId))
            {
                stringBuilder.Append(item.DisplayText + "\r\n");
            }

            return stringBuilder.ToString();
        }

        private string GetAnswerResponseString(IEnumerable<Response> items)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in items)
            {
                if (item.IsSelected == "True")
                {
                    stringBuilder.Append(item.Value + "\r\n");
                }
            }

            return stringBuilder.ToString();
        }
    }
}