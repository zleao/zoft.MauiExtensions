namespace zoft.MauiExtensions.Core.Validation;

/// <summary>
/// Rule to be aplied to a <see cref="IValidatable">validatable object</see>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IValidationRule<T>
{
    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    /// <value>
    /// The validation message.
    /// </value>
    string ValidationMessage { get; set; }

    /// <summary>
    /// Run this validation rule logic.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    bool Check(T value);
}
