namespace zoft.MauiExtensions.Core.Validation.Rules
{
    /// <summary>
    /// Validation rule for pair matching
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatchPairValidationRule<T> : IValidationRule<ValidatablePair<T>>
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
        public bool Check(ValidatablePair<T> value)
		{
			return value.Item1.Value.Equals(value.Item2.Value);
		}
	}
}