using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Reactor.Core;

namespace Microsoft.UI.Reactor.Hosting;

/// <summary>
/// UWP stub for OverlayHostWiring. Dev overlay features are simplified in UWP.
/// </summary>
internal sealed class OverlayHostWiring : IDisposable
{
    public Grid? WrapperRoot { get; private set; }

    public OverlayHostWiring(CoreDispatcher dispatcher)
    {
        // Simplified version for UWP
    }

    public void ApplyFlagState()
    {
        // No-op in UWP
    }

    public UIElement SetContentViaWrapper(UIElement? content)
    {
        // In UWP, we don't use the overlay wrapper - just return content directly
        return content ?? new Grid();
    }

    public bool TryShowErrorInWrapper(Panel errorPanel)
    {
        // Return false to let the host handle error display directly
        return false;
    }

    public void DetachContent()
    {
        // No-op in UWP
    }

    public void ScheduleHighlightFlush(Reconciler reconciler)
    {
        // No-op in UWP
    }

    public void Dispose()
    {
        // No-op in UWP
    }
}
