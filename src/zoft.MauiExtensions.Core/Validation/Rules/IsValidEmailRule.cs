namespace zoft.MauiExtensions.Core.Validation.Rules;

/// <summary>
/// Validation rule for emails
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="zoft.MauiExtensions.Core.Validation.IValidationRule{T}" />
public class IsValidEmailRule<T> : IValidationRule<T>
{
    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    /// <value>
    /// The validation message.
    /// </value>
    public string ValidationMessage { get; set; }

    /// <summary>
    /// Run this validation rule logic.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool Check(T value)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress($"{value}");
            return addr.Address == $"{value}";
        }
        catch
        {
            return false;
        }
    }
}
