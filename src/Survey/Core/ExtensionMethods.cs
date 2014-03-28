namespace Survey.Core
{
    public static class ExtensionMethods
    {
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }
    }
}