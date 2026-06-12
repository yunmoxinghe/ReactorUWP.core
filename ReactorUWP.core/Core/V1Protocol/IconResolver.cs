using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Reactor.Core.Internal;
using Windows.UI.Xaml.Controls;
using WinUI = Windows.UI.Xaml.Controls;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor.Core.V1Protocol;

// Shared V1 projection helpers for Reactor icon descriptors and symbol strings.
internal static class IconResolver
{
    /// <summary>Descriptor-accessible bridge to <see cref="ResolveIcon"/>
    /// for icon-bearing descriptor controls. Static so it can be invoked from a
    /// descriptor lambda without a Reconciler instance.</summary>
    internal static WinUI.IconElement? ResolveIconForDescriptor(IconData? iconData)
        => ResolveIcon(iconData, null);

    internal static WinUI.IconElement? ResolveIcon(IconData? iconData, string? iconSymbol)
    {
        if (iconData is not null)
        {
            return iconData switch
            {
                SymbolIconData sym => ResolveIconString(sym.Symbol) ?? new WinUI.SymbolIcon(Symbol.Placeholder),
                FontIconData fi => CreateFontIcon(fi),
                BitmapIconData bi => new WinUI.BitmapIcon { UriSource = bi.Source, ShowAsMonochrome = bi.ShowAsMonochrome },
                PathIconData pi => CreatePathIcon(pi),
                ImageIconData ii => new MUXC.ImageIcon { Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(ii.Source) },
                _ => null,
            };
        }
        if (iconSymbol is not null) return ResolveIconString(iconSymbol);
        return null;
    }

    // Handles both Symbol enum names ("Home", "Edit") and raw Segoe Fluent
    // glyphs (""). A Symbol enum mismatch used to collapse to
    // Symbol.Placeholder, which rendered as a diamond — fall through to a
    // FontIcon with SymbolThemeFontFamily so glyph strings render correctly.
    internal static WinUI.IconElement? ResolveIconString(string iconSymbol)
    {
        if (string.IsNullOrEmpty(iconSymbol)) return null;
        if (Enum.TryParse<Symbol>(iconSymbol, ignoreCase: true, out var symbol))
            return new WinUI.SymbolIcon(symbol);
        // Treat as a Segoe Fluent / MDL2 glyph codepoint.
        return new WinUI.FontIcon
        {
            Glyph = iconSymbol,
            FontFamily = Windows.UI.Xaml.Application.Current?.Resources["SymbolThemeFontFamily"] as Windows.UI.Xaml.Media.FontFamily
                         ?? new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
        };
    }

    // IconSource counterpart for controls (TabView, etc.) that take an
    // IconSource instead of IconElement. Same glyph-fallback semantics as
    // ResolveIconString.
    internal static WinUI.IconSource? ResolveIconSource(string? iconSymbol)
    {
        if (string.IsNullOrEmpty(iconSymbol)) return null;
        if (Enum.TryParse<Symbol>(iconSymbol, ignoreCase: true, out var symbol))
            return new WinUI.SymbolIconSource { Symbol = symbol };
        return new WinUI.FontIconSource
        {
            Glyph = iconSymbol,
            FontFamily = Windows.UI.Xaml.Application.Current?.Resources["SymbolThemeFontFamily"] as Windows.UI.Xaml.Media.FontFamily
                         ?? new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
        };
    }

    /// <summary>
    /// Strongly-typed <see cref="IconData"/> → <see cref="WinUI.IconSource"/>
    /// projection. Used by controls that expose an <c>IconSource</c> slot
    /// (TitleBar, TabView, etc.). Returns null on unknown subtypes so the
    /// caller can fall through to the string-glyph overload.
    /// </summary>
    internal static WinUI.IconSource? ResolveIconSource(IconData? iconData) => iconData switch
    {
        null => null,
        SymbolIconData sym => ResolveIconSource(sym.Symbol),
        FontIconData fi => new WinUI.FontIconSource
        {
            Glyph = fi.Glyph,
            FontFamily = fi.FontFamily is null ? null! : WinRTCache.GetFontFamily(fi.FontFamily),
            FontSize = fi.FontSize ?? double.NaN,
        },
        BitmapIconData bi => new WinUI.BitmapIconSource { UriSource = bi.Source, ShowAsMonochrome = bi.ShowAsMonochrome },
        PathIconData pi => CreatePathIconSource(pi),
        // ImageIconSource exists only in Microsoft.UI.Xaml.Controls and cannot be cast to Windows.UI.Xaml.Controls.IconSource
        // Return null for UWP compatibility - ImageIcon should be used via ResolveIcon instead
        ImageIconData ii => null,
        _ => null,
    };

    /// <summary>
    /// MUXC (WinUI 2) IconSource projection for controls like TeachingTip, TabView, InfoBar
    /// that require Microsoft.UI.Xaml.Controls.IconSource types.
    /// </summary>
    internal static MUXC.IconSource? ResolveMuxcIconSource(IconData? iconData) => iconData switch
    {
        null => null,
        SymbolIconData sym => ResolveMuxcIconSource(sym.Symbol),
        FontIconData fi => new MUXC.FontIconSource
        {
            Glyph = fi.Glyph,
            FontFamily = fi.FontFamily is null ? null! : WinRTCache.GetFontFamily(fi.FontFamily),
            FontSize = fi.FontSize ?? double.NaN,
        },
        BitmapIconData bi => new MUXC.BitmapIconSource { UriSource = bi.Source, ShowAsMonochrome = bi.ShowAsMonochrome },
        PathIconData pi => CreateMuxcPathIconSource(pi),
        ImageIconData ii => new MUXC.ImageIconSource { ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(ii.Source) },
        _ => null,
    };

    /// <summary>
    /// MUXC IconSource counterpart for string-based symbol/glyph projection.
    /// </summary>
    internal static MUXC.IconSource? ResolveMuxcIconSource(string? iconSymbol)
    {
        if (string.IsNullOrEmpty(iconSymbol)) return null;
        if (Enum.TryParse<Symbol>(iconSymbol, ignoreCase: true, out var symbol))
            return new MUXC.SymbolIconSource { Symbol = symbol };
        return new MUXC.FontIconSource
        {
            Glyph = iconSymbol,
            FontFamily = Windows.UI.Xaml.Application.Current?.Resources["SymbolThemeFontFamily"] as Windows.UI.Xaml.Media.FontFamily
                         ?? new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
        };
    }

    internal static MUXC.PathIconSource? CreateMuxcPathIconSource(PathIconData pi)
    {
        var src = new MUXC.PathIconSource();
        if (Windows.UI.Xaml.Markup.XamlReader.Load(
            $"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{pi.Data}</Geometry>")
            is Windows.UI.Xaml.Media.Geometry geo)
        {
            src.Data = geo;
        }
        return src;
    }

    internal static WinUI.PathIconSource? CreatePathIconSource(PathIconData pi)
    {
        var src = new WinUI.PathIconSource();
        if (Windows.UI.Xaml.Markup.XamlReader.Load(
            $"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{pi.Data}</Geometry>")
            is Windows.UI.Xaml.Media.Geometry geo)
        {
            src.Data = geo;
        }
        return src;
    }

    internal static WinUI.FontIcon CreateFontIcon(FontIconData fi)
    {
        var icon = new WinUI.FontIcon { Glyph = fi.Glyph };
        if (fi.FontFamily is not null) icon.FontFamily = WinRTCache.GetFontFamily(fi.FontFamily);
        if (fi.FontSize.HasValue) icon.FontSize = fi.FontSize.Value;
        return icon;
    }

    internal static WinUI.PathIcon CreatePathIcon(PathIconData pi)
    {
        var icon = new WinUI.PathIcon();
        if (Windows.UI.Xaml.Markup.XamlReader.Load(
            $"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{pi.Data}</Geometry>")
            is Windows.UI.Xaml.Media.Geometry geo)
        {
            icon.Data = geo;
        }
        return icon;
    }

    internal static Symbol ParseSymbol(string name)
    {
        if (Enum.TryParse<Symbol>(name, ignoreCase: true, out var symbol)) return symbol;
        return Symbol.Placeholder;
    }
}
