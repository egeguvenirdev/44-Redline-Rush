
using UnityEngine;

public static class Formatter
{

    public static string FormatFloatToReadableString(float value)
    {
        float number = value;
        if (number < 1000)
        {
            return ((int)number).ToString();
        }
        string result = number.ToString();

        if (result.Contains(","))
        {
            result = result.Substring(0, 4);
            result = result.Replace(",", string.Empty);
        }
        else
        {
            result = result.Substring(0, 3);
        }

        do
        {
            number /= 1000;
        }
        while (number >= 1000);
        number = Mathf.CeilToInt(number);
        if (value >= 1000000000000000)
        {
            result = result + "Q";
        }
        else if (value >= 1000000000000)
        {
            result = result + "T";
        }
        else if (value >= 1000000000)
        {
            result = result + "B";
        }
        else if (value >= 1000000)
        {
            result = result + "M";
        }
        else if (value >= 1000)
        {
            result = result + "K";
        }

        if (((int)number).ToString().Length > 0 && ((int)number).ToString().Length < 3)
        {
            result = result.Insert(((int)number).ToString().Length, ".");
        }
        return result;
    }
}
