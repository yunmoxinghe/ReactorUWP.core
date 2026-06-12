using Microsoft.Extensions.Logging;
using Windows.UI.Xaml;

namespace Microsoft.UI.Reactor.Hosting;

/// <summary>
/// UWP stub for BackdropApplier. SystemBackdrop is WinUI 3 only.
/// In UWP, backdrop operations are no-ops with debug logging.
/// </summary>
internal sealed class BackdropApplier
{
    private readonly Window? _window;
    private readonly ILogger? _logger;

    public BackdropApplier(Window? window, ILogger? logger = null)
    {
        _window = window;
        _logger = logger;
    }

    public void Apply(object backdropChoice)
    {
        // SystemBackdrop (Mica, Acrylic, etc.) is WinUI 3 only
        // In UWP, this is a no-op
        _logger?.LogDebug("BackdropApplier: Backdrop modifiers are not supported in UWP (WinUI 2)");
    }

    public void Reset()
    {
        // No-op in UWP
    }
}
