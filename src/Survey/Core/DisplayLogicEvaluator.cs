using Expressions;

namespace Survey.Core
{
    public class DisplayLogicEvaluator
    {
        public static bool Evaluate(string questionCode, string displayLogic, string response)
        {
            var isInverse = displayLogic.Contains("!=");

            displayLogic = displayLogic.Replace(questionCode, "a");
            
            if (isInverse)
            {
                displayLogic = displayLogic.Replace("!=", "==");
            }

            var expression = new DynamicExpression(displayLogic, ExpressionLanguage.Csharp);

            var context = new ExpressionContext();
            context.Variables.Add("a", response);

            var result = (bool)expression.Invoke(context);

            return isInverse ? !result : result;
        } 
    }
}