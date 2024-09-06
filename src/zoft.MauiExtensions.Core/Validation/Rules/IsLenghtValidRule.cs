namespace zoft.MauiExtensions.Core.Validation.Rules;

/// <summary>
/// Validation rule for text length
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="zoft.MauiExtensions.Core.Validation.IValidationRule{T}" />
public class IsLenghtValidRule<T> : IValidationRule<T>
{
    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    /// <value>
    /// The validation message.
    /// </value>
    public string ValidationMessage { get; set; }
    /// <summary>
    /// Gets or sets the minimum lenght.
    /// </summary>
    /// <value>
    /// The minimun lenght.
    /// </value>
    public int MinimumLenght { get; set; }
    /// <summary>
    /// Gets or sets the maximum lenght.
    /// </summary>
    /// <value>
    /// The maximun lenght.
    /// </value>
    public int MaximumLenght { get; set; }

    /// <summary>
    /// Run this validation rule logic.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool Check(T value)
    {
        if (value == null)
        {
            return false;
        }

        var str = value as string;
        return (str.Length > MinimumLenght && str.Length <= MaximumLenght);
    }
}
