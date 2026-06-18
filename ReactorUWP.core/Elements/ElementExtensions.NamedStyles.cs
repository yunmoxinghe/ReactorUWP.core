using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Reactor.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor;

// Named-style fluent helpers. Spec 039 §17.
//
// Promotes frequently-used WinUI named styles or enum-property values to
// fluent helpers so the common case never requires .ApplyStyle("…") or a
// verbose init-property assignment.
public static partial class ElementExtensions
{
    // ── §17.1 Button style fluents ─────────────────────────────────────

    /// <summary>Applies the AccentButtonStyle — accent-color primary-button look.
    /// Code-level implementation that sets properties directly for UWP compatibility.</summary>
    public static ButtonElement AccentButton(this ButtonElement el) =>
        el.Set(btn =>
        {
            // Get system accent color
            var accentColor = (Windows.UI.Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            btn.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    // Lighter accent on hover
                    var lighterAccent = LightenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(lighterAccent);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            };
            
            btn.PointerPressed += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    // Darker accent on press
                    var darkerAccent = DarkenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(darkerAccent);
                }
            };
            
            btn.PointerReleased += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var lighterAccent = LightenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(lighterAccent);
                }
            };
        });

    /// <inheritdoc cref="AccentButton(ButtonElement)" />
    public static DropDownButtonElement AccentButton(this DropDownButtonElement el) =>
        el.Set(btn =>
        {
            var accentColor = (Windows.UI.Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            btn.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var lighterAccent = LightenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(lighterAccent);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            };
        });

    /// <inheritdoc cref="AccentButton(ButtonElement)" />
    public static SplitButtonElement AccentButton(this SplitButtonElement el) =>
        el.Set(btn =>
        {
            var accentColor = (Windows.UI.Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            btn.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var lighterAccent = LightenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(lighterAccent);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            };
        });

    /// <inheritdoc cref="AccentButton(ButtonElement)" />
    public static ToggleSplitButtonElement AccentButton(this ToggleSplitButtonElement el) =>
        el.Set(btn =>
        {
            var accentColor = (Windows.UI.Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            btn.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var lighterAccent = LightenColor(accentColor, 0.1f);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(lighterAccent);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(accentColor);
            };
        });
    
    // Helper methods for color manipulation
    private static Windows.UI.Color LightenColor(Windows.UI.Color color, float amount)
    {
        var r = (byte)Math.Min(255, color.R + (255 - color.R) * amount);
        var g = (byte)Math.Min(255, color.G + (255 - color.G) * amount);
        var b = (byte)Math.Min(255, color.B + (255 - color.B) * amount);
        return Windows.UI.Color.FromArgb(color.A, r, g, b);
    }
    
    private static Windows.UI.Color DarkenColor(Windows.UI.Color color, float amount)
    {
        var r = (byte)(color.R * (1 - amount));
        var g = (byte)(color.G * (1 - amount));
        var b = (byte)(color.B * (1 - amount));
        return Windows.UI.Color.FromArgb(color.A, r, g, b);
    }

    /// <summary>Applies the SubtleButtonStyle — chromeless transparent-background button look.
    /// 
    /// ⚠️ INCOMPLETE IMPLEMENTATION - 不完整的实现 ⚠️
    /// 
    /// This is a TEMPORARY code-level workaround for UWP that approximates WinUI 3's SubtleButtonStyle.
    /// The implementation has visual differences from the authentic WinUI 3 Gallery SubtleButton.
    /// 
    /// Known issues / 已知问题:
    /// 1. Missing native button press animation/scale effect (缺少原生按钮按下动画/缩放效果)
    /// 2. Colors are approximations, may not match WinUI 3 exactly (颜色值是近似值，可能与 WinUI 3 不完全匹配)
    /// 3. Does not use VisualStateManager, relies on manual event handling (未使用 VisualStateManager)
    /// 4. UWP lacks BackgroundSizing property, visual appearance differs from WinUI 3
    ///    (UWP 缺少 BackgroundSizing 属性，视觉外观与 WinUI 3 不同)
    /// 
    /// TODO: Investigate proper way to replicate WinUI 3 SubtleButton behavior in UWP
    /// TODO: Find how to preserve default button animations while overriding colors
    /// </summary>
    public static ButtonElement SubtleButton(this ButtonElement el) =>
        el.Set(btn =>
        {
            // Transparent background by default
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            // Register pointer events for hover/press effects
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    // Subtle fill on hover (approximation)
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(15, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            };
            
            btn.PointerPressed += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    // More visible fill on press (approximation)
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(24, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(24, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
            
            btn.PointerReleased += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    // Back to hover state
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(15, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
        });

    /// <inheritdoc cref="SubtleButton(ButtonElement)" />
    public static DropDownButtonElement SubtleButton(this DropDownButtonElement el) =>
        el.Set(btn =>
        {
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(15, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            };
        });

    /// <inheritdoc cref="SubtleButton(ButtonElement)" />
    public static SplitButtonElement SubtleButton(this SplitButtonElement el) =>
        el.Set(btn =>
        {
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(15, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            };
        });

    /// <inheritdoc cref="SubtleButton(ButtonElement)" />
    public static ToggleSplitButtonElement SubtleButton(this ToggleSplitButtonElement el) =>
        el.Set(btn =>
        {
            btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            
            btn.PointerEntered += (s, e) =>
            {
                if (btn.IsEnabled)
                {
                    var color = Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Light
                        ? Windows.UI.Color.FromArgb(15, 0, 0, 0)
                        : Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                }
            };
            
            btn.PointerExited += (s, e) =>
            {
                if (btn.IsEnabled)
                    btn.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            };
        });

    // ── §17.2 TextLink ─────────────────────────────────────────────────

    /// <summary>Applies a text link style (chromeless inline-link rendering).
    /// Use for the "Learn more" pattern inside body text.
    /// UWP-compatible implementation that manually sets properties since TextBlockButtonStyle
    /// doesn't exist in WinUI 2.</summary>
    public static HyperlinkButtonElement TextLink(this HyperlinkButtonElement el) =>
        el.Set(btn =>
        {
            btn.Background = null;
            btn.BorderBrush = null;
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            btn.Padding = new Windows.UI.Xaml.Thickness(0);
            btn.MinWidth = 0;
            btn.MinHeight = 0;
        });

    /// <inheritdoc cref="TextLink(HyperlinkButtonElement)" />
    public static ButtonElement TextLink(this ButtonElement el) =>
        el.Set(btn =>
        {
            btn.Background = null;
            btn.BorderBrush = null;
            btn.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            btn.Padding = new Windows.UI.Xaml.Thickness(0);
            btn.MinWidth = 0;
            btn.MinHeight = 0;
        });

    // ── §17.3 InputScope fluents ───────────────────────────────────────
    // Promotes the most-common InputScope values to fluent helpers. The
    // generic .InputScope(InputScopeNameValue) escape hatch lives below for
    // the long tail. Applied via .Set() so we don't need an init property.

    /// <summary>Sets <c>InputScope = Number</c>. Drives the soft-keyboard
    /// layout and IME hints on platforms that respect it.</summary>
    public static TextBoxElement NumericInput(this TextBoxElement el) =>
        el.InputScope(InputScopeNameValue.Number);

    /// <summary>Sets <c>InputScope = EmailSmtpAddress</c>.</summary>
    public static TextBoxElement EmailInput(this TextBoxElement el) =>
        el.InputScope(InputScopeNameValue.EmailSmtpAddress);

    /// <summary>Sets <c>InputScope = Url</c>.</summary>
    public static TextBoxElement UrlInput(this TextBoxElement el) =>
        el.InputScope(InputScopeNameValue.Url);

    /// <summary>Sets <c>InputScope = TelephoneNumber</c>.</summary>
    public static TextBoxElement PhoneInput(this TextBoxElement el) =>
        el.InputScope(InputScopeNameValue.TelephoneNumber);

    /// <summary>Sets <c>InputScope = Search</c>.</summary>
    public static TextBoxElement SearchInput(this TextBoxElement el) =>
        el.InputScope(InputScopeNameValue.Search);

    /// <summary>Sets a specific <see cref="InputScopeNameValue"/>. Escape hatch
    /// for input scopes outside the named helpers above (e.g. <c>Chat</c>,
    /// <c>FormulaNumber</c>, <c>AlphanumericFullWidth</c>).</summary>
    public static TextBoxElement InputScope(this TextBoxElement el, InputScopeNameValue scope) =>
        el.Set(tb =>
        {
            var s = new InputScope();
            s.Names.Add(new InputScopeName(scope));
            tb.InputScope = s;
        });

    // ── §17.4 InfoBar severity fluents ─────────────────────────────────

    /// <summary>Sets severity to <see cref="MUXC.InfoBarSeverity.Informational"/>
    /// — neutral blue/grey skin, info icon.</summary>
    public static InfoBarElement Informational(this InfoBarElement el) =>
        el with { Severity = MUXC.InfoBarSeverity.Informational };

    /// <summary>Sets severity to <see cref="MUXC.InfoBarSeverity.Success"/> —
    /// green skin, check icon.</summary>
    public static InfoBarElement Success(this InfoBarElement el) =>
        el with { Severity = MUXC.InfoBarSeverity.Success };

    /// <summary>Sets severity to <see cref="MUXC.InfoBarSeverity.Warning"/> —
    /// yellow skin, warning icon.</summary>
    public static InfoBarElement Warning(this InfoBarElement el) =>
        el with { Severity = MUXC.InfoBarSeverity.Warning };

    /// <summary>Sets severity to <see cref="MUXC.InfoBarSeverity.Error"/> — red
    /// skin, error icon.</summary>
    public static InfoBarElement Error(this InfoBarElement el) =>
        el with { Severity = MUXC.InfoBarSeverity.Error };
}
