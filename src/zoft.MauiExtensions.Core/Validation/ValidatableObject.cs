using System.Collections.Generic;
using System.Linq;
using zoft.MauiExtensions.Core.Models;

namespace zoft.MauiExtensions.Core.Validation
{
    /// <summary>
    /// Base class for objects that support validation via <see cref="IValidatable"/> interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidatableObject<T> : ObservableObject, IValidatable<T>
    {
        /// <summary>
        /// List of valiations associated with this object
        /// </summary>
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        private List<string> _errors = new List<string>();
        /// <summary>
        /// List of the current active errors
        /// </summary>
        public List<string> Errors
        {
            get => _errors;
            set => Set(ref _errors, value);
        }

        /// <summary>
        /// If True, cleans the validation errors when the value of the object changes
        /// </summary>
        public bool CleanOnChange { get; }

        private T _value;
        /// <summary>
        /// Current value of the object
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (Set(ref _value, value) && CleanOnChange)
                {
                    IsValid = true;
                }
            }
        }

        private bool _isValid = true;
        /// <summary>
        /// Current validity state of the object
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set => Set(ref _isValid, value);
        }

        /// <summary>
        /// base constructor of a validatable object
        /// </summary>
        /// <param name="cleanOnChange"></param>
        public ValidatableObject(bool cleanOnChange = true)
        {
            CleanOnChange = cleanOnChange;
        }

        /// <summary>
        /// Executes the validations associated with this object
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                                                    .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = Errors.Count == 0;

            return this.IsValid;
        }

        /// <summary>
        /// String representation of the current objects value
        /// </summary>
        public override string ToString()
        {
            return Value != null ? $"{Value}" : null;
        }

        /// <summary>
        /// Manualy raise the property vchanged event of the Value property of this object
        /// </summary>
        public void RaisePropertyChanged()
        {
            OnPropertyChanged(nameof(Value));
        }
    }
}
