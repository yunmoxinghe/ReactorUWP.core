# Button Styles for ReactorUWP.core

This directory contains XAML resource dictionaries adapted from WinUI 3 for UWP compatibility.

## Available Styles

### SubtleButtonStyle
A chromeless button with transparent background that shows subtle feedback on hover/press.

**Usage in C#:**
```csharp
using static Microsoft.UI.Reactor.Factories;

// Using the fluent helper (already defined in ElementExtensions.NamedStyles.cs)
Button("Click me", OnClick).SubtleButton()

// Or apply style directly
Button("Click me", OnClick).ApplyStyle("SubtleButtonStyle")
```

**Visual States:**
- **Normal**: Transparent background
- **PointerOver**: Subtle fill (6% opacity)
- **Pressed**: More visible fill (9% opacity)
- **Disabled**: Transparent with disabled foreground

### AccentButtonStyle
A button with accent color background for primary actions.

**Usage in C#:**
```csharp
// Using the fluent helper
Button("Save", OnSave).AccentButton()

// Or apply style directly
Button("Save", OnSave).ApplyStyle("AccentButtonStyle")
```

## Integration

The `ButtonStyles.xaml` resource dictionary is automatically included in the ReactorUWP.core project and provides theme-aware resources for:

- **Default/Dark Theme**: Dark mode colors
- **Light Theme**: Light mode colors  
- **HighContrast**: High contrast mode for accessibility

## Theme Resources

### Subtle Button Resources
- `SubtleButtonBackground` - Transparent
- `SubtleButtonBackgroundPointerOver` - Subtle fill on hover
- `SubtleButtonBackgroundPressed` - Visible fill when pressed
- `SubtleButtonBackgroundDisabled` - Transparent when disabled
- `SubtleButtonForeground*` - Text colors for each state
- `SubtleButtonBorderBrush*` - Border colors (transparent)

### Accent Button Resources
- `AccentButtonBackground` - Accent color fill
- `AccentButtonBackgroundPointerOver` - Lighter accent on hover
- `AccentButtonBackgroundPressed` - Darker accent when pressed
- `AccentButtonBackgroundDisabled` - Disabled fill
- `AccentButtonForeground*` - White text colors
- `AccentButtonBorderBrush*` - Border colors

## Implementation Notes

1. **UWP Compatibility**: These styles are adapted from WinUI 3's Button_themeresources.xaml
2. **System Resources**: Uses standard UWP system brushes for consistency
3. **Theme Awareness**: Automatically adapts to Light/Dark/HighContrast themes
4. **Fluent Helpers**: Extension methods in `ElementExtensions.NamedStyles.cs` provide convenient fluent API

## Source

Adapted from: [microsoft-ui-xaml/controls/dev/CommonStyles/Button_themeresources.xaml](https://github.com/microsoft/microsoft-ui-xaml/blob/main/controls/dev/CommonStyles/Button_themeresources.xaml)
