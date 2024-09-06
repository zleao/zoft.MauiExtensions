using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using zoft.MauiExtensions.Core.Attributes;
using zoft.MauiExtensions.Core.Extensions;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Core.Validation;
using zoft.MauiExtensions.Core.WeakSubscription;

namespace zoft.MauiExtensions.Core.ViewModels
{
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
        /// <exception cref="NullReferenceException">IMvxLogProvider</exception>
        protected CoreViewModel()
        {
            InitializePropertyDependencies(GetType());

            InitializeMethodDependencies(GetType());

            InitializePropertyChanged();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreViewModel" /> class.
        /// </summary>
        /// <param name="mainThreadService">Instance of the <see cref="IMainThreadService"/></param>
        protected CoreViewModel(IMainThreadService mainThreadService)
            : this()
        {
            MainThreadService = mainThreadService;
        }

        #endregion

        #region Dependency Management

        /// <summary>
        /// Helper list used to prevent PropertyCHanged propagation in cases where an action is ExecuteWithoutConditionalDependsOn 
        /// </summary>
        private readonly Dictionary<string, int> _dependsOnConditionalCount = new();

        /// <summary>
        /// PropertyChanged subscription of this instance
        /// </summary>
        private NotifyPropertyChangedEventSubscription _propertyChangedSubscription;

        /// <summary>
        /// List of all the properties that have DependsOn attribute configured
        /// </summary>
        private readonly Dictionary<string, IList<DependencyInfo>> _propertyDependencies = new();

        /// <summary>
        /// List of all the notifiable collection properties that have the PropagateCollectionChange attribute configured, with the respective CollectionChanged subscription
        /// </summary>
        private readonly Dictionary<string, CollectionSubscriptionInfo> _notifiableCollectionsPropertyDependencies = new();

        /// <summary>
        /// List of the found dependencies (and respective PropertyChangeEventSubscription) that correspond to an IValidatable object.
        /// Each subscription will trigger a PropertyChanged of the respective PropertyName
        /// </summary>
        private readonly Dictionary<string, ValidatableCollectionInfo> _validatablePropertyDependencies = new();

        /// <summary>
        /// List of all the methods that have DependsOn attribute configured
        /// </summary>
        private readonly Dictionary<string, IList<MethodInfo>> _methodDependencies = new();

        /// <summary>
        /// Gets a value indicating whether this instance should react to property changed events
        /// </summary>
        protected bool HasDependencies => _propertyDependencies.Count > 0 ||
                                           _notifiableCollectionsPropertyDependencies.Count > 0 ||
                                           _methodDependencies.Count > 0;

        /// <summary>
        /// Maps all the properties that have DependsOn and/or PropagateCollectionChange attributes configured
        /// </summary>
        private void InitializePropertyDependencies(Type type)
        {
            foreach (var property in type.GetProperties(true))
            {
                // Store the property dependencies that where configured in the view model
                var attributes = property.GetCustomAttributes<DependsOnAttribute>(true);
                var dependsOnAttributes = attributes as DependsOnAttribute[] ?? attributes.ToArray();
                if (dependsOnAttributes?.Length > 0)
                {
                    lock (_propertyDependencies)
                    {
                        foreach (var attribute in dependsOnAttributes)
                        {
                            // Store the property dependency with the mapping for the dependent property
                            if (!_propertyDependencies.ContainsKey(attribute.Name))
                            {
                                _propertyDependencies.Add(attribute.Name, new List<DependencyInfo>());
                            }
                            _propertyDependencies[attribute.Name].Add(new DependencyInfo(property, attribute.IsConditional));

                            // If the property dependency corresponds to a validatable object, 
                            // we need to update the list of _validatablePropertyDependencies
                            // We only need to add one entry per property name
                            if(!_validatablePropertyDependencies.ContainsKey(attribute.Name))
                            {
                                // Check if the property dependency is a validatable object
                                var dependencyProperty = type.GetProperty(attribute.Name);
                                if (dependencyProperty != null && typeof(IValidatable).IsAssignableFrom(dependencyProperty.PropertyType))
                                {
                                    // At this point we know the dependency property is a IValidatableObject 
                                    // and it was not added the list of _validatablePropertyDependencies
                                    var validatableObject = dependencyProperty.GetValue(this, null) as IValidatable;
                                    _validatablePropertyDependencies.Add(attribute.Name, new ValidatableCollectionInfo { ValidatableObject = validatableObject });
                                }
                            }
                        }
                    }
                }

                // Store the properties that are signaled PropagateCollectionChanged (if applicable)
                if (typeof(INotifyCollectionChanged).IsAssignableFrom(property.PropertyType))
                {
                    var attribute = property.GetCustomAttribute<PropagateCollectionChangeAttribute>(true);
                    if (attribute != null)
                    {
                        lock (_notifiableCollectionsPropertyDependencies)
                        {
                            var collection = property.GetValue(this, null) as INotifyCollectionChanged;
                            _notifiableCollectionsPropertyDependencies.Add(property.Name, new CollectionSubscriptionInfo { Collection = collection });
                        }
                    }
                }
            }

            if (type != typeof(CoreViewModel))
            {
                var newType = type.GetTypeInfo().BaseType;
                if (newType != null)
                    InitializePropertyDependencies(newType);
            }
        }

        /// <summary>
        /// Maps all the methods that have DependsOn attribute configured
        /// </summary>
        private void InitializeMethodDependencies(Type type)
        {
            //foreach (var method in type.GetTypeInfo().DeclaredMethods.Where(m => m.ReturnType.Equals(typeof(void)) && m.GetParameters().Length == 0))
            foreach (var method in type.GetTypeInfo().DeclaredMethods.Where(m => m.GetParameters().Length == 0))
            {
                var attributes = method.GetCustomAttributes<DependsOnAttribute>(true);
                var dependsOnAttributes = attributes as DependsOnAttribute[] ?? attributes.ToArray();
                if (dependsOnAttributes?.Length > 0)
                {
                    foreach (var attribute in dependsOnAttributes)
                    {
                        lock (_methodDependencies)
                        {
                            if (!_methodDependencies.ContainsKey(attribute.Name))
                            {
                                _methodDependencies.Add(attribute.Name, new List<MethodInfo>());
                            }
                            _methodDependencies[attribute.Name].Add(method);
                        }
                    }
                }
            }

            if (type != typeof(CoreViewModel))
            {
                var newType = type.GetTypeInfo().BaseType;
                if (newType != null)
                    InitializeMethodDependencies(newType);
            }
        }

        /// <summary>
        /// Initializes the listeners for property changed events
        /// </summary>
        private void InitializePropertyChanged()
        {
            if (HasDependencies)
            {
                _propertyChangedSubscription = this.WeakSubscribe(OnPropertyChanged);

                foreach (var item in _notifiableCollectionsPropertyDependencies)
                {
                    if (item.Value.Collection != null)
                    {
                        item.Value.CollectionChangedSubscription = item.Value.Collection.WeakSubscribe(OnCollectionChanged);
                    }
                }

                foreach (var item in _validatablePropertyDependencies)
                {
                    if(item.Value.ValidatableObject != null)
                    {
                        item.Value.PropertyChangedSubscription = item.Value.ValidatableObject.WeakSubscribe(OnValidatableObjectChanged);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the collection changed events for the notifiable collections marked with the PropagateCollectionChange attribute
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var propertyDependency in _notifiableCollectionsPropertyDependencies.Where(nc => ReferenceEquals(sender, nc.Value.Collection)))
            {
                RaiseDependenciesPropertyChanged(propertyDependency.Key);
            }
        }

        /// <summary>
        /// Handles the property changed events for the validatable objects referenced as dependency properties
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnValidatableObjectChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyDependency = _validatablePropertyDependencies.FirstOrDefault(nc => ReferenceEquals(sender, nc.Value.ValidatableObject));
            RaiseDependenciesPropertyChanged(propertyDependency.Key);
        }

        /// <summary>
        /// Called when a property raises the <see cref="PropertyChangedEventHandler"/>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null)
                return;

            UpdateCollectionPropertyValue(e.PropertyName);
            UpdateValidatableObjectPropertyValue(e.PropertyName);
            RaiseDependenciesPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Updates the collection property dependency subscription.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void UpdateCollectionPropertyValue(string propertyName)
        {
            if (_notifiableCollectionsPropertyDependencies.TryGetValue(propertyName, out var collectionSubscriptionInfo))
            {
                var senderCollection = this.GetPropertyValue(propertyName) as INotifyCollectionChanged;
                if (!ReferenceEquals(collectionSubscriptionInfo.Collection, senderCollection))
                {
                    //Remove previous subscription
                    collectionSubscriptionInfo.CollectionChangedSubscription?.Dispose();
                    collectionSubscriptionInfo.CollectionChangedSubscription = null;

                    //Add new subscription
                    if (senderCollection != null)
                    {
                        collectionSubscriptionInfo.CollectionChangedSubscription = senderCollection.WeakSubscribe(OnCollectionChanged);
                    }

                    //Update collection reference
                    collectionSubscriptionInfo.Collection = senderCollection;
                }
            }
        }

        /// <summary>
        /// Updates the validatable object property dependency subscription.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void UpdateValidatableObjectPropertyValue(string propertyName)
        {
            if(_validatablePropertyDependencies.TryGetValue(propertyName, out var validatableSubscriptionInfo))
            {
                var senderValidatable = this.GetPropertyValue(propertyName) as IValidatable;
                if (!ReferenceEquals(validatableSubscriptionInfo.ValidatableObject, senderValidatable))
                {
                    //Remove previous subscription
                    validatableSubscriptionInfo.PropertyChangedSubscription?.Dispose();
                    validatableSubscriptionInfo.PropertyChangedSubscription = null;

                    //Add new subscription
                    if (senderValidatable != null)
                    {
                        validatableSubscriptionInfo.PropertyChangedSubscription = senderValidatable.WeakSubscribe(OnValidatableObjectChanged);
                    }

                    //Update collection reference
                    validatableSubscriptionInfo.ValidatableObject = senderValidatable;
                }
            }
        }

        /// <summary>
        /// Raises the dependencies property changed.
        /// </summary>
        /// <param name="dependencyName">Name of the dependency.</param>
        public void RaiseDependenciesPropertyChanged(string dependencyName)
        {
            //Extra protection for null/empty values
            if (dependencyName.IsNullOrWhiteSpace())
            {
                return;
            }

            // Ensure this method runs in the main thread
            if (MainThreadService != null)
            {
                if(!MainThreadService.IsMainThread)
                {
                    MainThreadService.BeginInvokeOnMainThread(() => RaiseDependenciesPropertyChanged(dependencyName));
                    return;
                }
            }
            else
            {
                if (!MainThread.IsMainThread)
                {
                    MainThread.BeginInvokeOnMainThread(() => RaiseDependenciesPropertyChanged(dependencyName));
                    return;
                }
            }

            //Prevents the conditional DependsOn from firing, if the execution was made
            //to prevent propagation (ExecuteWithoutConditionalDependsOn)
            if (_dependsOnConditionalCount.ContainsKey(dependencyName) &&
                _dependsOnConditionalCount[dependencyName] > 0)
            {
                return;
            }

            lock (_propertyDependencies)
            {
                if (_propertyDependencies.TryGetValue(dependencyName, out var properties))
                {
                    foreach (var property in properties)
                    {
                        if (typeof(Command).IsAssignableFrom(property.Info.PropertyType))
                        {
                            var mauiCommand = property.Info.GetValue(this, null) as Command;
                            mauiCommand?.ChangeCanExecute();
                        }
                        else if (typeof(IRelayCommand).IsAssignableFrom(property.Info.PropertyType))
                        {
                            var asyncCommand = property.Info.GetValue(this, null) as IRelayCommand;
                            asyncCommand?.NotifyCanExecuteChanged();
                        }

                        else if (typeof(IValidatable).IsAssignableFrom(property.Info.PropertyType))
                        {
                            var validatable = property.Info.GetValue(this, null) as IValidatable;
                            validatable.RaisePropertyChanged();
                        }
                        else
                        {
                            OnPropertyChanged(property.Info.Name);
                        }
                    }
                }
            }

            lock (_methodDependencies)
            {
                if (_methodDependencies.TryGetValue(dependencyName, out var methods))
                {
                    foreach (var method in methods)
                    {
                        method.Invoke(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the property changed handlers.
        /// </summary>
        private void RemovePropertyChangedHandlers()
        {
            if (_propertyChangedSubscription != null)
            {
                _propertyChangedSubscription.Dispose();
                _propertyChangedSubscription = null;
            }
        }

        /// <summary>
        /// Removes the collection changed handlers.
        /// </summary>
        private void RemoveCollectionChangedHandlers()
        {
            foreach (var item in _notifiableCollectionsPropertyDependencies)
            {
                try
                {
                    item.Value?.CollectionChangedSubscription?.Dispose();
                }
                catch (InvalidOperationException)
                {
                    // This error might occur during dispose.
                }
            }

            _notifiableCollectionsPropertyDependencies.Clear();
        }

        /// <summary>
        /// Executes the action, preventing the propagation of DependsOn
        /// that are marked with the 'IsConditional' flag for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="action">The action.</param>
        protected void ExecuteWithoutConditionalDependsOn(string propertyName, Action action)
        {
            if (MainThreadService != null)
            {
                MainThreadService.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (!_dependsOnConditionalCount.ContainsKey(propertyName))
                            _dependsOnConditionalCount.Add(propertyName, 1);
                        else
                            _dependsOnConditionalCount[propertyName]++;

                        action.Invoke();

                        _dependsOnConditionalCount[propertyName] = Math.Max(0, _dependsOnConditionalCount[propertyName] - 1);
                    }
                    catch
                    {
                        _dependsOnConditionalCount.Remove(propertyName);
                    }
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (!_dependsOnConditionalCount.ContainsKey(propertyName))
                            _dependsOnConditionalCount.Add(propertyName, 1);
                        else
                            _dependsOnConditionalCount[propertyName]++;

                        action.Invoke();

                        _dependsOnConditionalCount[propertyName] = Math.Max(0, _dependsOnConditionalCount[propertyName] - 1);
                    }
                    catch
                    {
                        _dependsOnConditionalCount.Remove(propertyName);
                    }
                });
            }
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
            if (action == null)
                throw new ArgumentNullException(nameof(action));

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
            if (action == null)
                throw new ArgumentNullException(nameof(action));

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
        protected virtual void DisposeManagedResources()
        {
            RemovePropertyChangedHandlers();
            RemoveCollectionChangedHandlers();
        }

        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources() { }

        #endregion
    }
}
