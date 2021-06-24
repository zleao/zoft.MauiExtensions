using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace zoft.MauiExtensions.Core.Validation
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get => _errors;
            set => Set(ref _errors, value);
        }

        public bool CleanOnChange { get; }

        private T _value;
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
        public bool IsValid
        {
            get => _isValid;
            set => Set(ref _isValid, value);
        }

        public ValidatableObject(bool cleanOnChange = true)
        {
            CleanOnChange = cleanOnChange;
        }

        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                                                    .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = Errors.Count == 0;

            return this.IsValid;
        }

        public override string ToString()
        {
            return Value != null ? $"{Value}" : null;
        }

        #region INotifyPropertyChanged

        public void RaisePropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<TValue>(ref TValue field, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<TValue>.Default.Equals(field, newValue))
            {
                field = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                return true;
            }

            return false;
        }

        #endregion
    }
}
