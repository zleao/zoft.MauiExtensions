using System.Text;

namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extensions for Exception type
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Gets the full description of the exception.
    /// </summary>
    /// <param name="ex">The ex.</param>
    /// <returns></returns>
    public static string GetFullDescription(this Exception ex)
    {
        StringBuilder desc = new(ex.Message);

        if (ex.InnerException != null)
        {
            desc.Append(" + ");
            desc.Append(GetFullDescription(ex.InnerException));
        }

        return desc.ToString();
    }
}
