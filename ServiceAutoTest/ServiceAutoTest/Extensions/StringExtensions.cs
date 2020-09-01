using System.Text;


public static class StringExtensions
{
    public static byte[] ToByteArray(this string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }
}

