using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using zoft.MauiExtensions.Core.Extensions;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Core.Validation;
using zoft.MauiExtensions.Core.WeakSubscription;

namespace zoft.MauiExtensions.Core.ViewModels;

/// <summary>
/// Core view model"/>
/// </summary>
public abstract partial class CoreViewModel : ObservableObject, IDisposable
{
    /// <summary>
    /// Get the instance of the MainThreadService. <br/>
    /// </summary>
    /// <remarks>This instance can be null, depending on how the <see cref="CoreViewModel"/> was instantiated</remarks>
    public IMainThreadService MainThreadService { get; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [ObservableProperty]
    private string _title = string.Empty;

    /// <summary>
    /// Gets or sets the subtitle.
    /// </summary>
    /// <value>The subtitle.</value>
    [ObservableProperty]
    private string _subtitle = string.Empty;

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>The icon.</value>
    [ObservableProperty]
    private string _icon = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is busy.
    /// </summary>
    /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                IsNotBusy = !_isBusy;
            }
        }
    }
    private bool _isBusy;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is not busy.
    /// </summary>
    /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
    public bool IsNotBusy
    {
        get => _isNotBusy;
        set
        {
            if (SetProperty(ref _isNotBusy, value))
            {
                IsBusy = !_isNotBusy;
            }
        }
    }
    private bool _isNotBusy = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance can load more.
    /// </summary>
    /// <value><c>true</c> if this instance can load more; otherwise, <c>false</c>.</value>
    [ObservableProperty]
    private bool _canLoadMore = true;

    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    /// <value>The header.</value>
    [ObservableProperty]
    private string _header = string.Empty;

    /// <summary>
    /// Gets or sets the footer.
    /// </summary>
    /// <value>The footer.</value>
    [ObservableProperty]
    private string _footer = string.Empty;

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CoreViewModel" /> class.
    /// </summary>
    /// <param name="mainThreadService">Instance of the <see cref="IMainThreadService"/></param>
    protected CoreViewModel(IMainThreadService mainThreadService)
        : base()
    {
        MainThreadService = mainThreadService;
    }

    #endregion

    #region Busy Notification Management

    private int _busyCount;

    /// <summary>
    /// Message to be shown in the busy indicator
    /// </summary>
    [ObservableProperty]
    private string _busyMessage;

    /// <summary>
    /// Executes work asynchronously.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="workMessage">The work message.</param>
    /// <param name="isSilent">if set to <c>true</c> [is silent].</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">action</exception>
    protected virtual async Task DoWorkAsync(Func<Task> action, string workMessage = null, bool isSilent = false)
    {
        ArgumentNullException.ThrowIfNull(action);

        StartWork(workMessage, isSilent);

        try
        {
            await action.Invoke().ConfigureAwait(false);

            FinishWork(isSilent);
        }
        catch
        {
            FinishWork(isSilent);
            throw;
        }
    }

    /// <summary>
    /// Executes work asynchronously and returns the result of the action invocation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action">The action.</param>
    /// <param name="workMessage">The work message.</param>
    /// <param name="isSilent">if set to <c>true</c> [is silent].</param>
    /// <returns></returns>
    protected virtual async Task<T> DoWorkAsync<T>(Func<Task<T>> action, string workMessage = null, bool isSilent = false)
    {
        ArgumentNullException.ThrowIfNull(action);

        StartWork(workMessage, isSilent);

        try
        {
            var result = await action.Invoke().ConfigureAwait(false);

            FinishWork(isSilent);

            return result;
        }
        catch
        {
            FinishWork(isSilent);
            throw;
        }
    }

    /// <summary>
    /// Signals the IsBusy to indicate that a new work has started
    /// </summary>
    /// <param name="isSilent">if set to <c>true</c> the IsBusy will no be signaled.</param>
    protected virtual void StartWork(bool isSilent = false)
    {
        if (!isSilent)
        {
            Interlocked.Increment(ref _busyCount);
            IsBusy = _busyCount > 0;
        }
    }

    /// <summary>
    /// Signals the IsBusy to indicate that a new work has started and sets busy message.
    /// </summary>
    /// <param name="message">The busy message.</param>
    /// <param name="isSilent">if set to <c>true</c> the IsBusy will no be signaled.</param>
    public virtual void StartWork(string message, bool isSilent = false)
    {
        if (!isSilent && !message.IsNullOrWhiteSpace())
        {
            BusyMessage = message;
        }

        StartWork(isSilent);
    }

    /// <summary>
    /// Signals the IsBusy to indicate that work is finished.
    /// </summary>
    /// <param name="isSilent">if set to <c>true</c> the IsBusy will no be signaled.</param>
    public virtual void FinishWork(bool isSilent = false)
    {
        if (!isSilent)
        {
            Interlocked.Decrement(ref _busyCount);
            IsBusy = _busyCount > 0;

            if (_busyCount <= 0)
            {
                BusyMessage = null;
            }
        }
    }

    #endregion

    #region IDisposable Members

    private bool _disposed;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC.SupressFinalize to
        // take this object off the finalization queue
        // and prevent finalization code for this object
        // from executing a second time.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!_disposed)
        {
            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                DisposeManagedResources();
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
            DisposeUnmanagedResources();

            // Note disposing has been done.
            _disposed = true;
        }
    }

    /// <summary>
    /// Disposes the managed resources.
    /// </summary>
    protected virtual void DisposeManagedResources() { }

    /// <summary>
    /// Disposes the unmanaged resources.
    /// </summary>
    protected virtual void DisposeUnmanagedResources() { }

    #endregion
}
