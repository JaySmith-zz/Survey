using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Survey.Core.Domain;

namespace Survey.Core
{
    public static class Globals
    {

        static Globals()
        {
        }

        public static string DatabaseConnectionString
        {
            get { return Properties.Settings.Default.DatabaseConnectionString; }
        }

        public static int DefaultSurveyId
        {
            get { return Properties.Settings.Default.DefaultSurveyId; }
        }

        public static int DisplayLogicDepth
        {
            get { return Properties.Settings.Default.DisplayLogicDepth;  }
        }

        public static List<SelectListItem> QuestionnaireStatuses()
        {
            var items = new List<SelectListItem>();
            foreach (var fieldInfo in typeof(QuestionnaireStatus).GetFields())
            {
                var value = fieldInfo.GetValue(null) as string;
                items.Add(new SelectListItem {Value = value, Text = value});
            }

            return items;
        }
    }
}