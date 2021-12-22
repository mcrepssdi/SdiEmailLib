namespace SdiEmailLib.Utilities.cs
{
    public static class StringUtil
    {
        public static bool IsEmpty(this string str)
        {
            if (str == null) return true;
            if (str.Length == 0) return true;
            return false;
        }
    }
}