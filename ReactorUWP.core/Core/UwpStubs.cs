// UWP 移植版本 - 仅 WinUI 3 专属功能的最小化存根
// 用于满足编译需求，这些类型在 UWP 中不提供实际功能
// 
// 重要：WinUI 2.x 的所有类型（NumberBox, InfoBar, TabView 等的枚举）
// 已经在 Microsoft.UI.Xaml NuGet 包 (v2.8.7) 中提供，不在此重复定义！

#if UWP_BUILD

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Core;
using CoreDispatcher = Windows.UI.Core.CoreDispatcher;

// ════════════════════════════════════════════════════════════════════════
//  内部辅助类 - ReferenceEqualityComparer
// ════════════════════════════════════════════════════════════════════════

internal class ReferenceEqualityComparer : IEqualityComparer<object>
{
    public static readonly ReferenceEqualityComparer Instance = new();
    
    public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);
    public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
}

// ════════════════════════════════════════════════════════════════════════
//  Microsoft.UI.Reactor 命名空间存根 (Hosting 相关类型)
// ════════════════════════════════════════════════════════════════════════

namespace Microsoft.UI.Reactor
{
    public class ReactorWindow
    {
        public Window? NativeWindow { get; set; }
        public (double X, double Y) Position { get; set; }
        public WindowState State { get; set; } = new();
        public bool IsActive { get; set; }
        public uint Dpi { get; set; } = 96;
        
        public event EventHandler<WindowDipPositionChangedEventArgs>? PositionChanged;
        public event EventHandler<WindowZOrderChangedEventArgs>? ZOrderChanged;
        public event EventHandler? StateChanged;
        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler? DpiChanged;
        
        public void RegisterAspectRatioOverride(double? aspectRatio) { }
        public void BeginDragMove() { }
        public Action RegisterClosingGuard(Func<bool> guard) => () => { };
        
        // PersistedScope存根 - 用于hot reload时保留组件状态
        public object? PersistedScope { get; set; }
    }

    public class WindowState
    {
        public string? Title { get; set; }
        public static readonly WindowState Normal = new();
    }

    public record WindowKey(string Id);

    public record WindowSpec(string Title)
    {
        public WindowKey Key => new(Title);
    }

    public class DisplayInfo
    {
        public double ScaleFactor { get; set; } = 1.0;
    }

    public class ReactorTrayIcon
    {
    }

    public record TrayIconSpec(string Title)
    {
        public string Key => Title;
    }
    
    public class ReactorApp
    {
        public static ReactorWindow? CurrentWindow => null;
        public static CoreDispatcher? UIDispatcher => Window.Current?.Dispatcher;
        public static bool DevtoolsEnabled => false;
        public static IReactorHost? ActiveHostInternal => null;
        
        // 多窗口管理
        public static IReadOnlyDictionary<WindowKey, ReactorWindow> Windows => 
            new Dictionary<WindowKey, ReactorWindow>();
        
        public static ReactorWindow? FindWindow(WindowKey key) => null;
        public static void OpenWindow(WindowSpec spec) { }
        
        // TrayIcon 管理
        public static ReactorTrayIcon? FindTrayIcon(string key) => null;
        public static void OpenTrayIcon(TrayIconSpec spec) { }
    }
    
    public interface IReactorHost
    {
        ReactorWindow? OwningWindow { get; }
    }
    
    public class WindowDipPositionChangedEventArgs : EventArgs
    {
        public (double X, double Y) Position { get; set; }
    }
    
    public class WindowZOrderChangedEventArgs : EventArgs
    {
        public bool MovedToTop { get; set; }
        public bool IsCovered { get; set; }
    }
    
    public class ReactorDisplay
    {
        public static DisplayInfo[] GetAll() => Array.Empty<DisplayInfo>();
        public static DisplayInfo[] Displays => Array.Empty<DisplayInfo>();
        public static event EventHandler? DisplayLayoutChanged;
    }
}

// Hosting 命名空间也需要这些类型（用于内部引用）
namespace Microsoft.UI.Reactor.Hosting
{
    public class ReactorWindow : Microsoft.UI.Reactor.ReactorWindow
    {
        public void Update(Microsoft.UI.Reactor.WindowSpec spec) { }
    }

    public class WindowState : Microsoft.UI.Reactor.WindowState
    {
    }
    
    public class HotReloadService
    {
        public static bool WithinUpdatePass => false;
        public static bool IsHotReloadLive => false;
    }
    
    public class ReactorHotReloadCopier
    {
        public static T? CreateInstance<T>() where T : new() => new T();
        public static T? CreateInstance<T>(Func<T?> factory) => factory();
        public static object? CreateInstance(Type type)
        {
            try { return Activator.CreateInstance(type); }
            catch { return null; }
        }
        
        public static bool TryMigrate<T>(T oldInstance, T newInstance, Func<T, T, bool> copier) => false;
        public static bool TryMigrate<T>(T oldInstance, T newInstance) => false;
        public static bool TryMigrate<T>(T oldInstance, T newInstance, HashSet<object> visited) where T : class => false;
        public static bool TryMigrate(object oldInstance, object newInstance, HashSet<object> visited) => false;
    }
}

// ════════════════════════════════════════════════════════════════════════
//  Docking 命名空间存根
// ════════════════════════════════════════════════════════════════════════

namespace Microsoft.UI.Reactor.Docking
{
    public record DockManager
    {
        public Action<DockLayoutChangingEventArgs>? OnLayoutChanging { get; init; }
        public Action<DockLayoutChangedEventArgs>? OnLayoutChanged { get; init; }
        public Action<DockDocumentClosingEventArgs>? OnDocumentClosing { get; init; }
        public Action<DockDocumentClosedEventArgs>? OnDocumentClosed { get; init; }
        public Action<DockToolWindowHidingEventArgs>? OnToolWindowHiding { get; init; }
        public Action<DockToolWindowHiddenEventArgs>? OnToolWindowHidden { get; init; }
        public Action<DockToolWindowClosingEventArgs>? OnToolWindowClosing { get; init; }
        public Action<DockToolWindowClosedEventArgs>? OnToolWindowClosed { get; init; }
        public Action<DockContentFloatingEventArgs>? OnContentFloating { get; init; }
        public Action<DockContentFloatedEventArgs>? OnContentFloated { get; init; }
        public Action<DockContentDockingEventArgs>? OnContentDocking { get; init; }
        public Action<DockContentDockedEventArgs>? OnContentDocked { get; init; }
        public Action<DockActiveContentChangedEventArgs>? OnActiveContentChanged { get; init; }
        public Action<DockFloatingWindowCreatedEventArgs>? OnFloatingWindowCreated { get; init; }
        public Action<DockFloatingWindowClosedEventArgs>? OnFloatingWindowClosed { get; init; }
        public Action<DockTarget?>? OnDropTargetHovered { get; init; }
        public Action<DockTarget>? OnDropTargetConfirmed { get; init; }
        public Action? OnDropTargetsDismissed { get; init; }
        public Action? OnLiveLayoutChanged { get; init; }
        public Action? OnSplitterDragCompleted { get; init; }
    }

    public record DockNode(string Id);

    public class DockPaneInfo
    {
    }
    
    public class DockLayoutOperation
    {
    }
    
    // Docking 事件参数类型
    public class DockLayoutChangingEventArgs : EventArgs { }
    public class DockLayoutChangedEventArgs : EventArgs { }
    public class DockDocumentClosingEventArgs : EventArgs { }
    public class DockDocumentClosedEventArgs : EventArgs { }
    public class DockToolWindowHidingEventArgs : EventArgs { }
    public class DockToolWindowHiddenEventArgs : EventArgs { }
    public class DockToolWindowClosingEventArgs : EventArgs { }
    public class DockToolWindowClosedEventArgs : EventArgs { }
    public class DockContentFloatingEventArgs : EventArgs { }
    public class DockContentFloatedEventArgs : EventArgs { }
    public class DockContentDockingEventArgs : EventArgs { }
    public class DockContentDockedEventArgs : EventArgs { }
    public class DockActiveContentChangedEventArgs : EventArgs { }
    public class DockFloatingWindowCreatedEventArgs : EventArgs { }
    public class DockFloatingWindowClosedEventArgs : EventArgs { }
    
    public class DockTarget
    {
    }
    
    namespace Native
    {
        public class NativeDockPaneInfo { }
        public class NativeDockNodeInfo { }
        public class NativeDockTargetInfo { }
        public class NativeDockLayoutInfo { }
    }
}

// ════════════════════════════════════════════════════════════════════════
//  Charting 命名空间存根
// ════════════════════════════════════════════════════════════════════════

namespace Microsoft.UI.Reactor.Charting
{
    public record ChartElement() : Core.Element;
    
    public class ChartData
    {
    }
    
    namespace Accessibility
    {
        public interface IChartAccessibilityData
        {
            string? Name { get; }
            string? Description { get; }
            System.Collections.Generic.IReadOnlyList<ChartSeriesData> Series { get; }
        }
        
        public class ChartSeriesData
        {
        }
        
        public class ChartAccessibility
        {
        }
        
        public class ChartPalette
        {
            public static double ContrastRatio(D3.D3Color a, D3.D3Color b) => 1.0;
            public static double MinColorblindDeltaE(D3.D3Color a, D3.D3Color b) => 1.0;
            public static (ChartPalette Palette, string[] Warnings) Harden(D3.D3Color[] colors) 
                => (new ChartPalette(), Array.Empty<string>());
                
            public D3.D3Color[] Colors => Array.Empty<D3.D3Color>();
        }
        
        public class ChartSummarizer
        {
            public static string Summarize(IChartAccessibilityData data) => string.Empty;
        }
        
        public class ChartScannerHint
        {
            public Core.CanvasElement? InnerCanvas { get; set; }
        }
    }
    
    namespace D3
    {
        public record struct D3Color(byte R, byte G, byte B)
        {
            public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";
        }
    }
}

// ════════════════════════════════════════════════════════════════════════
//  WinUI 3 专属控件类型存根 (UWP 中不存在)
// ════════════════════════════════════════════════════════════════════════

namespace Windows.UI.Xaml.Controls
{
    // WinUI 3 新控件
    public class TitleBar : Control { }
    public class ScrollView : FrameworkElement { }
    public class ItemsView : Control { }
    public class ItemContainer : Control { }
    public class SelectorBar : Control { }
    public class AnnotatedScrollBar : Control { }
    public class MapControl : Control { }
    
    // WinUI 3 ScrollView 相关枚举
    public enum ScrollingContentOrientation { Vertical, Horizontal, Both, None }
    public enum ScrollingScrollBarVisibility { Auto, Visible, Hidden }
    public enum ScrollingScrollMode { Enabled, Disabled, Auto }
    public enum ScrollingZoomMode { Enabled, Disabled }
    
    // PipsPager WrapMode (可能在 WinUI 2.x 晚期版本中，在这里定义以防万一)
    public enum PipsPagerWrapMode { None, Wrap }
    
    // WinUI 3 事件参数
    public class SelectorBarSelectionChangedEventArgs : EventArgs { }
    public class ItemsViewItemInvokedEventArgs : EventArgs { }
    public class ItemsViewSelectionChangedEventArgs : EventArgs { }
    public class CoreWebView2InitializedEventArgs : EventArgs { }
}

// ════════════════════════════════════════════════════════════════════════
//  Microsoft.UI.Xaml.Controls 命名空间存根 (仅 WinUI 3 专属)
//  注意：WinUI 2.x 的类型已经在 Microsoft.UI.Xaml NuGet 包中！
// ════════════════════════════════════════════════════════════════════════

namespace Microsoft.UI.Xaml.Controls
{
    // ItemsView 相关枚举 (WinUI 3 专属)
    public enum ItemsViewSelectionMode
    {
        None,
        Single,
        Multiple
    }
}

#endif
