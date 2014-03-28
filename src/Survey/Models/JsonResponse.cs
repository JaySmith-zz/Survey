namespace Survey.Models
{
    public class JsonResponse
    {
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }
        public string IsSelected { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }
}