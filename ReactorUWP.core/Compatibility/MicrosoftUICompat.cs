// ---------------------------------------------------------------------------------------------
// ReactorUWP.core - Microsoft.UI.* Compatibility Layer
// 为 UWP 项目提供 Microsoft.UI.* 命名空间别名，以便与 WinUI 3 代码兼容
// ---------------------------------------------------------------------------------------------

namespace Microsoft.UI.Text
{
    // 字体权重静态类 - 转发到 Windows.UI.Text.FontWeights
    public static class FontWeights
    {
        public static global::Windows.UI.Text.FontWeight Thin => global::Windows.UI.Text.FontWeights.Thin;
        public static global::Windows.UI.Text.FontWeight ExtraLight => global::Windows.UI.Text.FontWeights.ExtraLight;
        public static global::Windows.UI.Text.FontWeight Light => global::Windows.UI.Text.FontWeights.Light;
        public static global::Windows.UI.Text.FontWeight SemiLight => global::Windows.UI.Text.FontWeights.SemiLight;
        public static global::Windows.UI.Text.FontWeight Normal => global::Windows.UI.Text.FontWeights.Normal;
        public static global::Windows.UI.Text.FontWeight Medium => global::Windows.UI.Text.FontWeights.Medium;
        public static global::Windows.UI.Text.FontWeight SemiBold => global::Windows.UI.Text.FontWeights.SemiBold;
        public static global::Windows.UI.Text.FontWeight Bold => global::Windows.UI.Text.FontWeights.Bold;
        public static global::Windows.UI.Text.FontWeight ExtraBold => global::Windows.UI.Text.FontWeights.ExtraBold;
        public static global::Windows.UI.Text.FontWeight Black => global::Windows.UI.Text.FontWeights.Black;
        public static global::Windows.UI.Text.FontWeight ExtraBlack => global::Windows.UI.Text.FontWeights.ExtraBlack;
    }
    
    // TextSetOptions 枚举 - 与 Windows.UI.Text.TextSetOptions 值完全对应
    public enum TextSetOptions
    {
        None = 0,
        FormatRtf = 1,
        ApplyRtfDocumentDefaults = 2,
        Unhide = 8,
        CheckTextLimit = 16
    }
    
    // TextGetOptions 枚举 - 与 Windows.UI.Text.TextGetOptions 值完全对应
    public enum TextGetOptions
    {
        None = 0,
        AdjustCrlf = 1,
        UseCrlf = 2,
        UseObjectText = 4,
        AllowFinalEop = 8,
        NoHidden = 32,
        IncludeNumbering = 64,
        FormatRtf = 8192,
        UseLf = 16777216
    }
}

namespace Microsoft.UI.Xaml
{
    // TextWrapping 枚举 - 与 Windows.UI.Xaml.TextWrapping 值完全对应
    public enum TextWrapping
    {
        NoWrap = 1,
        Wrap = 2,
        WrapWholeWords = 3
    }
}

// 扩展方法，提供与 Windows.UI.Text 的互操作
namespace Microsoft.UI.Reactor
{
    using Windows.UI.Text;
    
    /// <summary>扩展方法，允许 Microsoft.UI.Text 枚举与 Windows.UI.Text API 互操作</summary>
    public static class TextCompatibilityExtensions
    {
        /// <summary>将 Microsoft.UI.Text.TextSetOptions 转换为 Windows.UI.Text.TextSetOptions</summary>
        public static Windows.UI.Text.TextSetOptions ToWindowsUI(this Microsoft.UI.Text.TextSetOptions options)
        {
            return (Windows.UI.Text.TextSetOptions)(int)options;
        }
        
        /// <summary>将 Microsoft.UI.Text.TextGetOptions 转换为 Windows.UI.Text.TextGetOptions</summary>
        public static Windows.UI.Text.TextGetOptions ToWindowsUI(this Microsoft.UI.Text.TextGetOptions options)
        {
            return (Windows.UI.Text.TextGetOptions)(int)options;
        }
        
        /// <summary>为 ITextDocument 添加接受 Microsoft.UI.Text.TextSetOptions 的 SetText 重载</summary>
        public static void SetText(this ITextDocument document, Microsoft.UI.Text.TextSetOptions options, string value)
        {
            document.SetText(options.ToWindowsUI(), value);
        }
        
        /// <summary>为 ITextDocument 添加接受 Microsoft.UI.Text.TextGetOptions 的 GetText 重载</summary>
        public static void GetText(this ITextDocument document, Microsoft.UI.Text.TextGetOptions options, out string value)
        {
            document.GetText(options.ToWindowsUI(), out value);
        }
    }
}

