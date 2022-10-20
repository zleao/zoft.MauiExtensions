namespace zoft.MauiExtensions.Core.Services
{
    /// <summary>
    /// Main Thread helpers.
    /// </summary>
    public interface IMainThreadService
    {
        /// <summary>
        /// Gets if it is the current main UI thread.
        /// </summary>
        bool IsMainThread { get; }

        /// <summary>
        /// Invokes an action on the main thread of the application.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        void BeginInvokeOnMainThread(Action action);

        /// <summary>
        /// Gets the main thread synchonization context
        /// </summary>
        /// <returns>The syncronization context for the main thread</returns>
        Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync();

        /// <summary>
        /// Invoke the main thread aysnc
        /// </summary>
        /// <param name="action">Action to invoke</param>
        /// <returns>A task that can be awaited</returns>
        Task InvokeOnMainThreadAsync(Action action);

        /// <summary>
        /// Invoke the main thread aysnc
        /// </summary>
        /// <typeparam name="T">Returning type</typeparam>
        /// <param name="func">A function to execute</param>
        /// <returns>A task that can be awaited</returns>
        Task<T> InvokeOnMainThreadAsync<T>(Func<T> func);

        /// <summary>
        /// Invoke the main thread aysnc
        /// </summary>
        /// <param name="funcTask">A function task to execute</param>
        /// <returns>A task that can be awaited</returns>
        Task InvokeOnMainThreadAsync(Func<Task> funcTask);

        /// <summary>
        /// Invoke the main thread aysnc
        /// </summary>
        /// <typeparam name="T">Returning type</typeparam>
        /// <param name="funcTask">A function task to execute</param>
        /// <returns>A task that can be awaited</returns>
        Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask);
    }
}
