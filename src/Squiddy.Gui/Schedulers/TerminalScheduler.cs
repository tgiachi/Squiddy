﻿using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Terminal.Gui;

namespace Squiddy.Gui.Schedulers;

public class TerminalScheduler : LocalScheduler
{
    public static readonly TerminalScheduler Default = new ();
    TerminalScheduler() { }

    public override IDisposable Schedule<TState>(
        TState state, TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action
    )
    {
        IDisposable PostOnMainLoop()
        {
            var composite = new CompositeDisposable(2);
            var cancellation = new CancellationDisposable();
            Application.MainLoop.Invoke(
                () =>
                {
                    if (!cancellation.Token.IsCancellationRequested)
                        composite.Add(action(this, state));
                }
            );
            composite.Add(cancellation);
            return composite;
        }

        IDisposable PostOnMainLoopAsTimeout()
        {
            var composite = new CompositeDisposable(2);
            var timeout = Application.MainLoop.AddTimeout(
                dueTime,
                args =>
                {
                    composite.Add(action(this, state));
                    return false;
                }
            );
            composite.Add(Disposable.Create(() => Application.MainLoop.RemoveTimeout(timeout)));
            return composite;
        }

        return dueTime == TimeSpan.Zero
            ? PostOnMainLoop()
            : PostOnMainLoopAsTimeout();
    }
}
