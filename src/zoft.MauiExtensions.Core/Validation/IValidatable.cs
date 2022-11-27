using System.ComponentModel;

namespace zoft.MauiExtensions.Core.Validation
{
    /// <summary>
    /// Base interface definition that represents an object that can be validated and has associated errors
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IValidatable : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the errors associated with this object.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        List<string> Errors { get; set; }

        /// <summary>
        /// Executes the validation associated with this object.
        /// </summary>
        /// <returns></returns>
        bool Validate();

        /// <summary>
        /// Returns true if this object is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this object is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; set; }

        /// <summary>
        /// Raises the property changed for this object's value.
        /// </summary>
        void RaisePropertyChanged();
    }
    /// <summary>
    /// Interface the represents a typed object that can be validated and has associated errors
    /// It enables the association of validation rules
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IValidatable<T> : IValidatable
    {
        /// <summary>
        /// Gets a list of validations associated with this object.
        /// </summary>
        /// <value>
        /// The validations.
        /// </value>
        List<IValidationRule<T>> Validations { get; }
    }
}
