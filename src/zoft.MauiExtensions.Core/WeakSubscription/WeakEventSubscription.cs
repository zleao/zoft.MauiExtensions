using System;
using System.Reflection;

namespace zoft.MauiExtensions.Core.WeakSubscription
{
    /// <summary>
    /// Base class for event weak subscriptions
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public class WeakEventSubscription<TSource, TEventArgs> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;
        private readonly MethodInfo _eventHandlerMethodInfo;
        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;
        private bool _subscribed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventSubscription{TSource, TEventArgs}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceEventName">Name of the source event.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        public WeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventSubscription{TSource, TEventArgs}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceEventInfo">The source event information.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        /// <exception cref="System.ArgumentNullException">
        /// sourceEventInfo - missing source event info in WeakEventSubscription
        /// </exception>
        protected WeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            _sourceEventInfo = sourceEventInfo ?? throw new ArgumentNullException(nameof(sourceEventInfo), "missing source event info in WeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            _ourEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        /// <summary>
        /// Creates the event handler.
        /// </summary>
        /// <returns></returns>
        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        /// <summary>
        /// Gets the target object.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        /// <summary>
        /// This is the method that will handle the event of source
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event agrs instance containing the event data.</param>
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
            {
                return;
            }

            if (_sourceReference.TryGetTarget(out TSource source))
            {
                _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
            {
                throw new InvalidOperationException("Should not call _subscribed twice");
            }

            if (_sourceReference.TryGetTarget(out TSource source))
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }

    /// <summary>
    /// Base class for typed event weak subscriptions
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public class WeakEventSubscription<TSource> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventSubscription{TSource}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceEventName">Name of the source event.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        public WeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventSubscription{TSource}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceEventInfo">The source event information.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        /// <exception cref="System.ArgumentNullException">
        /// sourceEventInfo - missing source event info in WeakEventSubscription
        /// </exception>
        protected WeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler targetEventHandler)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            _sourceEventInfo = sourceEventInfo ?? throw new ArgumentNullException(nameof(sourceEventInfo), "missing source event info in WeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            _ourEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        /// <summary>
        /// Gets the target object.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        /// <summary>
        /// Creates the event handler.
        /// </summary>
        /// <returns></returns>
        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }

        /// <summary>
        ///This is the method that will handle the event of source.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        protected void OnSourceEvent(object sender, EventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
            {
                return;
            }

            if (_sourceReference.TryGetTarget(out TSource source))
            {
                _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
            {
                throw new InvalidOperationException("Should not call _subscribed twice");
            }

            if (_sourceReference.TryGetTarget(out TSource source))
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }
}