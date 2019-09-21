namespace TextReverser
{
    using System;

    public static class TextReverser
    {
        public static string ReverseText(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
