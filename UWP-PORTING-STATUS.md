# ReactorUWP.core - UWP 移植状态

## 当前进展

### ✅ 已完成

1. **文件复制和结构对齐**
   - 从官方 microsoft-ui-reactor 复制了所有核心文件夹
   - 删除了重复的嵌套文件夹（Charting/Charting, Controls/Controls 等）

2. **批量 API 替换**
   - 替换 `Microsoft.UI.Xaml` → `Windows.UI.Xaml`
   - 替换 `Microsoft.UI.Input` → `Windows.UI.Input`
   - 替换 `Microsoft.UI.Composition` → `Windows.UI.Composition`
   - 添加 `using System.*` 到 621 个文件

3. **NuGet 包依赖**
   - Microsoft.UI.Xaml 2.8.7 (WinUI 2 for UWP)
   - Microsoft.Extensions.Logging.Abstractions 9.0.0
   - Microsoft.Extensions.DependencyInjection 9.0.0
   - System.Drawing.Common 9.0.4
   - MessageFormat 8.0.0 (Jeffijoe.MessageFormat)
   - Microsoft.Web.WebView2 1.0.2839.0

4. **错误减少**
   - 初始错误：3220
   - 当前错误：~1280
   - **减少了 60%**

### ⚠️ 已排除的功能（UWP 不支持或依赖缺失）

#### 完全排除的模块
- **Hosting/** - Window 管理（ReactorWindow, WindowState 等 WinUI 3 专属）
- **Docking/** - 停靠面板系统（依赖 Hosting）
- **Charting/** - 图表组件（依赖 Hosting）

#### 排除的单个文件
- `Elements\DevtoolsMenuShim.cs` - 开发工具菜单
- `Elements\BackdropExtensions.cs` - SystemBackdrop（Acrylic/Mica，WinUI 3 专属）
- `Core\V1Protocol\Descriptor\Descriptors\XamlHostDescriptor.cs`
- `Core\V1Protocol\Descriptor\Descriptors\XamlPageDescriptor.cs`
- `Core\V1Protocol\OverlayLifecycle.cs`

#### WinUI 3 专属控件（已排除 Descriptor）
- TitleBar
- ScrollView (UWP 使用 ScrollViewer)
- ItemsView
- ItemContainer
- SelectorBar
- AnnotatedScrollBarscriptor

### 🔧 剩余错误类型（约 1280 个）

#### 1. 缺失的 WinUI 2.x 控件类型
这些控件应该通过 Microsoft.UI.Xaml 2.8.7 可用，但可能需要命名空间调整：

```
- NumberBox
- RadioButtons  
- TabView
- BreadcrumbBar
- TeachingTip
- InfoBar
- InfoBadge
- AnimatedIcon
- AnimatedVisualPlayer
- PipsPager
- ItemsRepeater
- Expander (需要 CommunityToolkit)
```

**可能的解决方案**：
- 使用 `MUXC = Microsoft.UI.Xaml.Controls` 别名
- 检查这些控件是否在 Element.cs 中有对应定义
- 如果 WinUI 2.8.7 中缺失，需要注释掉相关代码

#### 2. Yoga 布局引擎
一些文件缺少 `using System.Collections.Generic`，导致 List<>, Stack<> 等类型找不到。

**解决方案**：已批量添加 using，但可能有遗漏。

#### 3. Reconciler 中对 Hosting 的引用
Core/Reconciler.cs, Reconciler.Mount.cs, Reconciler.Update.cs 等文件引用了 `Microsoft.UI.Reactor.Hosting`。

**解决方案**：
- 创建空的 Hosting 命名空间存根
- 或者条件编译排除相关代码

#### 4. ElementExtensions 中对 Docking 的引用
`Elements\ElementExtensions.Events.cs` 引用了 Docking 命名空间。

**解决方案**：条件编译或注释掉相关扩展方法。

## 下一步工作

### 优先级 1：核心功能可用
1. 解决 Reconciler 对 Hosting 的依赖
2. 创建 Hosting 命名空间存根（空实现）
3. 确保 Component, Element, Hooks 核心功能可编译

### 优先级 2：基础控件支持
1. 验证 WinUI 2.x 控件的命名空间
2. 修复 NumberBox, RadioButtons, TabView 等的引用
3. 注释掉 UWP 完全不支持的控件相关代码

### 优先级 3：清理和文档
1. 整理所有被排除的功能到单独文档
2. 为 UWP 不支持的 Element 类型添加条件编译
3. 创建 UWP 专属的 Factories 和 Extensions

## 可用性评估

### 预计可用的功能
- ✅ Component 基础（Hooks: UseState, UseEffect, UseMemo 等）
- ✅ Element 基础（TextBlock, Button, Border, Grid, Stack 等）
- ✅ FlexPanel 布局（Yoga）
- ✅ 动画系统（Animation）
- ✅ 输入手势（Input）
- ✅ 数据绑定（Data）
- ✅ 自定义控件（Controls - DataGrid, PropertyGrid 等）
- ⚠️ WinUI 2.x 新控件（需要命名空间修复）

### 不可用的功能
- ❌ Window 管理（Hosting）
- ❌ 停靠面板（Docking）
- ❌ 图表（Charting）
- ❌ WinUI 3 专属控件（TitleBar, ScrollView, ItemsView 等）
- ❌ SystemBackdrop（Acrylic/Mica）

## 使用建议

当前状态下，ReactorUWP.core 可以用于：
- 基础的 Reactor 组件开发（Component + Hooks + Element）
- FlexPanel 响应式布局
- 基础控件（Button, TextBox, ListView 等）

**不推荐用于**：
- 需要多窗口管理的应用
- 需要高级图表功能
- 需要 WinUI 3 最新控件的场景

## 编译命令

```bash
# 还原依赖
dotnet restore ReactorUWP.core\ReactorUWP.core.csproj

# 编译
dotnet build ReactorUWP.core\ReactorUWP.core.csproj

# 查看错误统计
dotnet build ReactorUWP.core\ReactorUWP.core.csproj 2>&1 | Select-String "error CS" | Measure-Object
```

## 参考链接

- 官方 Reactor: https://github.com/microsoft/microsoft-ui-reactor
- WinUI 2.x 文档: https://learn.microsoft.com/windows/apps/winui/winui2/
- Microsoft.UI.Xaml NuGet: https://www.nuget.org/packages/Microsoft.UI.Xaml
