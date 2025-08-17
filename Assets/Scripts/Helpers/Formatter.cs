
using UnityEngine;

public static class Formatter
{
    public static string FormatFloatToReadableString(float value)
    {
        if(value < 1000)
            return value.ToString("0");

        if (value < 1_000_000)
            return (value / 1_000f).ToString("0.#") + "K";

        if (value < 1_000_000_000)
            return (value / 1_000_000f).ToString("0.#") + "M";

        if (value < 1_000_000_000_000)
            return (value / 1_000_000_000f).ToString("0.#") + "B";

        if (value < 1_000_000_000_000_000)
            return (value / 1_000_000_000_000f).ToString("0.#") + "T";

            return (value / 1_000_000_000_000_000f).ToString("0.#") + "Q";
    }
}
