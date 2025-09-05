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
        using var timeoutCts = new CancellationTokenSource();
        var timeoutTask = Task.Delay(timeoutInMilliseconds, timeoutCts.Token);
        
        var completedTask = await Task.WhenAny(task, timeoutTask)
            .ConfigureAwait(false);

        if (completedTask == task)
        {
            timeoutCts.Cancel(); // Cancel timeout task to free resources
            return await task.ConfigureAwait(false);
        }
        
        throw new TimeoutException($"Task timed out after {timeoutInMilliseconds} milliseconds");
    }

    /// <summary>
    /// Task extension to add a timeout.
    /// </summary>
    /// <returns>The task with timeout.</returns>
    /// <param name="task">Task.</param>
    /// <param name="timeout">Timeout Duration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout) =>
        WithTimeout(task, (int)timeout.TotalMilliseconds);

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
