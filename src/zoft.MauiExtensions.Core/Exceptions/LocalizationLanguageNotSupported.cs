using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Core.Exceptions;

/// <summary>Exception that is thrown when someone tries to use a unsupported culture name in the <see cref="ILocalizationService">LocalizationService</see></summary>
public class LocalizationLanguageNotSupported : Exception
{
    /// <summary>Initializes a new instance of the <see cref="LocalizationLanguageNotSupported" /> class.</summary>
    public LocalizationLanguageNotSupported() : base()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="LocalizationLanguageNotSupported" /> class.</summary>
    /// <param name="message">The message that describes the error.</param>
    public LocalizationLanguageNotSupported(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizationLanguageNotSupported"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public LocalizationLanguageNotSupported(string message, Exception innerException) : base(message, innerException)
    {
    }
}
