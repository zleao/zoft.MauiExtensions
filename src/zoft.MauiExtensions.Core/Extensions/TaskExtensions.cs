// Full credits to James Montemagno (Refactored.MvvmHelpers)
// https://github.com/jamesmontemagno/mvvm-helpers/blob/master/MvvmHelpers/Utils.cs

namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extension for tasks
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Task extension to add a timeout.
    /// </summary>
    /// <returns>The task with timeout.</returns>
    /// <param name="task">Task.</param>
    /// <param name="timeoutInMilliseconds">Timeout duration in Milliseconds.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public async static Task<T> WithTimeout<T>(this Task<T> task, int timeoutInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (timeoutInMilliseconds < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeoutInMilliseconds), timeoutInMilliseconds, "Timeout must be -1 (infinite) or a non-negative value.");
        }

        var timeout = timeoutInMilliseconds == -1
            ? Timeout.InfiniteTimeSpan
            : TimeSpan.FromMilliseconds(timeoutInMilliseconds);

        try
        {
            return await task.WaitAsync(timeout).ConfigureAwait(false);
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"Task timed out after {timeoutInMilliseconds} milliseconds", ex);
        }
    }

    /// <summary>
    /// Task extension to add a timeout.
    /// </summary>
    /// <returns>The task with timeout.</returns>
    /// <param name="task">Task.</param>
    /// <param name="timeout">Timeout Duration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
    {
        ArgumentNullException.ThrowIfNull(task);

        try
        {
            return await task.WaitAsync(timeout).ConfigureAwait(false);
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"Task timed out after {timeout.TotalMilliseconds} milliseconds", ex);
        }
    }

    /// <summary>
    /// Attempts to await on the task and catches exception
    /// </summary>
    /// <param name="task">Task to execute</param>
    /// <param name="onException">What to do when method has an exception</param>
    /// <param name="continueOnCapturedContext">If the context should be captured.</param>
    public static async void SafeFireAndForget(this Task task, Action<Exception>? onException = null, bool continueOnCapturedContext = false)
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex) when (onException != null)
        {
            onException(ex);
        }
    }
}
