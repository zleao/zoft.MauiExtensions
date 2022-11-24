namespace zoft.MauiExtensions.Core.Services
{
    /// <inheritdoc/>
    public sealed class MainThreadService : IMainThreadService
    {

        /// <inheritdoc/>
        public bool IsMainThread => MainThread.IsMainThread;

        /// <inheritdoc/>
        public void BeginInvokeOnMainThread(Action action) => MainThread.BeginInvokeOnMainThread(action);

        /// <inheritdoc/>
        public Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync() => MainThread.GetMainThreadSynchronizationContextAsync();

        /// <inheritdoc/>
        public Task InvokeOnMainThreadAsync(Action action) => MainThread.InvokeOnMainThreadAsync(action);

        /// <inheritdoc/>
        public Task<T> InvokeOnMainThreadAsync<T>(Func<T> func) => MainThread.InvokeOnMainThreadAsync(func);

        /// <inheritdoc/>
        public Task InvokeOnMainThreadAsync(Func<Task> funcTask) => MainThread.InvokeOnMainThreadAsync(funcTask);

        /// <inheritdoc/>
        public Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask) => MainThread.InvokeOnMainThreadAsync(funcTask);
    }
}
