namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extensions ofr decimal type
/// </summary>
public static class DecimalExtensions
{
    /// <summary>
    /// Rounds a decimal value with a precision defined by argument.
    /// </summary>
    /// <param name="value">The decimal value to round</param>
    /// <param name="precision">Decimal places precision (default = 2).</param>
    /// <returns></returns>
    public static decimal RoundCurrency(this decimal value, int precision = 2)
    {
        value *= (decimal)(Math.Pow(10, precision));

        if (value - (long)value >= 0.5m)
        {
            value = (long)value + 1;
        }
        else
        {
            value = (long)value;
        }
        return value / (decimal)(Math.Pow(10, precision));
    }
}
