using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace zoft.MauiExtensions.Core.Validation
{
    /// <summary>
    /// Enables 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidatablePair<T> : IValidatable<ValidatablePair<T>>
    {
        /// <inheritdoc/>
        public List<IValidationRule<ValidatablePair<T>>> Validations { get; } = new List<IValidationRule<ValidatablePair<T>>>();

        /// <inheritdoc/>
        public bool IsValid { get; set; } = true;

        /// <inheritdoc/>
        public List<string> Errors { get; set; } = new List<string>();

        /// <inheritdoc/>
        public ValidatableObject<T> Item1 { get; set; } = new ValidatableObject<T>();

        /// <inheritdoc/>
        public ValidatableObject<T> Item2 { get; set; } = new ValidatableObject<T>();

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public void RaisePropertyChanged()
        {
            Item1?.RaisePropertyChanged();
            Item2?.RaisePropertyChanged();
            
            PropertyChanged?.Invoke(this, null);
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            var item1IsValid = Item1.Validate();
            var item2IsValid = Item2.Validate();
            if (item1IsValid && item2IsValid)
            {
                Errors.Clear();

                IEnumerable<string> errors = Validations.Where(v => !v.Check(this))
                    .Select(v => v.ValidationMessage);

                Errors = errors.ToList();
                Item2.Errors = Errors;
                Item2.IsValid = Errors.Count == 0;
            }

            IsValid = Item1.Errors.Count == 0 && Item2.Errors.Count == 0;

            return this.IsValid;
        }
    }
}
