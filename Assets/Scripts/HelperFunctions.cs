public static class HelperFunctions
{
 public static string ReturnSign(float number)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        if (number > 0)
        {
            return "+";
        }
        else
        {
            return "-";
        }
    }
}
