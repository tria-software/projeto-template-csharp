namespace ProjetoTemplate.Domain.Helpers
{
    public static class StringExtensions
    {
        public static string ToLowerAndRemoveSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.ToLower().Replace(" ", "");
        }
    }
}
