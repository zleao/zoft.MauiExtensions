using Microsoft.UI.Dispatching;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace zoft.MauiExtensions.Core.Services
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <inheritdoc/>
    public abstract class MainThreadService : IMainThreadService
    {
        /// <summary>
        /// MainThread Dispatcher
        /// </summary>
        protected abstract DispatcherQueue MainThreadDispatcher { get; }

        /// <inheritdoc/>
        public bool IsMainThread => MainThread.IsMainThread;

        /// <inheritdoc/>
        public void BeginInvokeOnMainThread(Action action)
        {
            if (IsMainThread)
            {
                action();
            }
            else
            {
                PlatformBeginInvokeOnMainThread(action);
            }
        }

        /// <inheritdoc/>
        public Task InvokeOnMainThreadAsync(Action action)
        {
            if (IsMainThread)
            {
                action();
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();

            BeginInvokeOnMainThread(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<T> InvokeOnMainThreadAsync<T>(Func<T> func)
        {
            if (IsMainThread)
            {
                return Task.FromResult(func());
            }

            var tcs = new TaskCompletionSource<T>();

            BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var result = func();
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task InvokeOnMainThreadAsync(Func<Task> funcTask)
        {
            if (IsMainThread)
            {
                return funcTask();
            }

            var tcs = new TaskCompletionSource<object>();

            BeginInvokeOnMainThread(
                async () =>
                {
                    try
                    {
                        await funcTask().ConfigureAwait(false);
                        tcs.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask)
        {
            if (IsMainThread)
            {
                return funcTask();
            }

            var tcs = new TaskCompletionSource<T>();

            BeginInvokeOnMainThread(
                async () =>
                {
                    try
                    {
                        var ret = await funcTask().ConfigureAwait(false);
                        tcs.SetResult(ret);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public async Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync()
        {
            SynchronizationContext ret = null;
            await InvokeOnMainThreadAsync(() =>
                ret = SynchronizationContext.Current).ConfigureAwait(false);
            return ret;
        }

        private void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (!MainThreadDispatcher.TryEnqueue(DispatcherQueuePriority.Normal, () => action()))
            {
                throw new InvalidOperationException("Unable to queue on the main thread.");
            }
        }
    }
}
