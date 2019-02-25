namespace Yol.Punla.Utility
{
    public static class StringExtensions
    {
        public static bool HasValue(this string text)
        {
            if (!(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)))
                return true;
            else
                return false;
        }
    }
}
