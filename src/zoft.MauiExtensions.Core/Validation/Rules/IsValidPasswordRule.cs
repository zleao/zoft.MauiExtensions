using System.Text.RegularExpressions;

namespace zoft.MauiExtensions.Core.Validation.Rules
{
    /// <summary>
    /// Validation rule for passwords
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="zoft.MauiExtensions.Core.Validation.IValidationRule{T}" />
    public class IsValidPasswordRule<T> : IValidationRule<T>
    {
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        public string ValidationMessage { get; set; }
        /// <summary>
        /// Gets or sets the regex password.
        /// </summary>
        /// <value>
        /// The regex password.
        /// </value>
        public Regex RegexPassword { get; set; } = new Regex("(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}");

        /// <summary>
        /// Run this validation rule logic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Check(T value)
        {
            return RegexPassword.IsMatch($"{value}");
        }
    }
}
