namespace Zoft.MauiExtensions.Core.WeakSubscription;

public sealed class WeakEventSubscription<TSource, TTarget, TDelegate, TArgs> : IDisposable
    where TSource : class
    where TTarget : class
    where TDelegate : Delegate
{
    private readonly TSource _source;
    private readonly WeakReference<TTarget> _targetRef;
    private readonly Action<TSource, TDelegate> _remove;
    private readonly Action<TSource, TTarget, TArgs> _forward; // how to call the target
    private TDelegate? _proxy;  // the actual event handler
    private int _disposed; // 0 = not disposed, 1 = disposed

    public WeakEventSubscription(
        TSource source,
        TTarget target,
        Action<TSource, TDelegate> add,
        Action<TSource, TDelegate> remove,
        Func<Action<object?, TArgs>, TDelegate> makeProxy,       // adapter to event's delegate
        Action<TSource, TTarget, TArgs> forward)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (target is null) throw new ArgumentNullException(nameof(target));
        if (add is null) throw new ArgumentNullException(nameof(add));
        if (remove is null) throw new ArgumentNullException(nameof(remove));
        if (makeProxy is null) throw new ArgumentNullException(nameof(makeProxy));
        if (forward is null) throw new ArgumentNullException(nameof(forward));

        _source = source;
        _targetRef = new(target);
        _remove = remove;
        _forward = forward;

        _proxy = makeProxy(OnEvent);  // build the correct delegate type
        add(source, _proxy);
    }

    private void OnEvent(object? sender, TArgs args)
    {
        if (Volatile.Read(ref _disposed) == 1) return;

        if (sender is TSource source && _targetRef.TryGetTarget(out var target))
            _forward(source, target, args);
        else
            Dispose(); // auto-detach when target is GC'd
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1) return;

        var proxy = Interlocked.Exchange(ref _proxy, null);
        if (proxy is null) return;

        try { _remove(_source, proxy); } catch { /* best effort */ }
        GC.SuppressFinalize(this);
    }
}

