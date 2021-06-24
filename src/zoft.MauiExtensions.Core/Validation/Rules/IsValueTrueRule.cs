namespace zoft.MauiExtensions.Core.Validation.Rules
{
    /// <summary>
    /// Validation rule for true value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="zoft.MauiExtensions.Core.Validation.IValidationRule{T}" />
    public class IsValueTrueRule<T> : IValidationRule<T>
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
            return bool.Parse($"{value}");
        }
    }
}
