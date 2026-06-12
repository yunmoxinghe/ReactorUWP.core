# ReactorUWP.core 迁移进度总结

## 📊 总体进度
**从 560 个错误 → 294 个错误**  
**完成率：47.5%** ✅

## ✅ 已完成的主要工作

### 1. 创建 UWP 存根系统
- ✅ Hosting 命名空间（ReactorWindow, WindowState, DisplayInfo, ReactorTrayIcon 等）
- ✅ Docking 命名空间（DockManager, DockNode, DockLayoutOperation 等）
- ✅ Charting.Accessibility 命名空间
- ✅ WinUI 3 专属控件存根（TitleBar, ScrollView, ItemsView, SelectorBar 等）
- ✅ WinUI 3 枚举和事件参数

### 2. 命名空间修复
- ✅ `Windows.System.CoreDispatcher` → `Windows.UI.Core.CoreDispatcher`
- ✅ `CoreDispatcher.GetForCurrentThread()` → `CoreWindow.GetForCurrentThread()?.Dispatcher`
- ✅ `Microsoft.UI.Colors` → `Windows.UI.Colors`
- ✅ `Microsoft.UI.ColorHelper` → `Windows.UI.ColorHelper`
- ✅ WinUI 2.x 控件（ItemsRepeater, NumberBox, Layout）使用 MUXC 别名

### 3. 创建扩展方法
- ✅ CoreDispatcherExtensions：为 UWP CoreDispatcher 提供 TryEnqueue API

### 4. 批量文件修复
- ✅ 核心 Reconciler 文件（Mount, Update, KeyedItemsBinding）
- ✅ 20+ 个 Descriptor 文件添加 MUXC using
- ✅ Element.cs、ElementExtensions.cs、ControlEventPayloads.cs
- ✅ LazyStack 相关文件
- ✅ 修复了 9 个文件中的 CoreDispatcher 引用

## ❌ 剩余的 294 个错误

### 关键发现：NavigationView 控件命名空间问题

**问题根源**：
- `NavigationViewItem.MenuItems` 属性**只存在于** WinUI 2 (`Microsoft.UI.Xaml.Controls`)
- UWP 原生的 `Windows.UI.Xaml.Controls.NavigationViewItem` **没有** `MenuItems` 属性
- 代码中使用了 `WinUI.NavigationViewItem`（指向 UWP 原生），但调用了只有 MUXC 才有的 API

**解决方案**：NavigationView 相关代码必须使用 MUXC (WinUI 2)

### 主要错误类型

#### 1. NavigationView 相关 (大量)
```csharp
// ❌ 错误：使用 WinUI (Windows.UI.Xaml.Controls)
WinUI.NavigationViewItem.MenuItems.Add(...)

// ✅ 正确：使用 MUXC (Microsoft.UI.Xaml.Controls)
MUXC.NavigationViewItem.MenuItems.Add(...)
```

#### 2. VirtualList 文件 (2个)
- `VirtualListElement.cs(126)`: `StackLayout` 需要 MUXC 前缀
- `VirtualListComponent.cs(77)`: `ItemsRepeater` 需要 MUXC 前缀

#### 3. ReswResourceProvider.cs (4个)
缺少 `using System.IO;`（Path, File, IOException）

#### 4. ResizeGrip.cs (4个)
- `ProtectedCursor`, `InputSystemCursor`, `InputSystemCursorShape`（WinUI 3 专属 API，需要存根或条件编译）
- `Microsoft.UI.Colors` → `Windows.UI.Colors`

#### 5. Colors 引用 (多个文件)
- CellRenderers.cs
- PropertyGridComponent.cs
- ResizeGrip.cs

#### 6. Dsl.cs 引用问题 (7个)
引用了被排除的 WinUI 3 专属 Descriptor：
- ScrollViewDescriptorHandler
- TitleBarDescriptorHandler
- SelectorBarDescriptorHandler
- AnnotatedScrollBarDescriptorHandler
- MapControlDescriptorHandler
- ItemContainerDescriptorHandler
- ItemsViewDescriptor

#### 7. AutoSuggestElement.cs (1个)
`ReactorApp` 类不存在（Hosting 命名空间被条件编译排除）

## 🔧 下一步行动计划

### 优先级 1：NavigationView 命名空间修复
```powershell
# 在所有使用 NavigationViewItem.MenuItems 的地方
# 将 WinUI.NavigationViewItem 替换为 MUXC.NavigationViewItem
```

### 优先级 2：快速修复
1. VirtualList* 文件添加 MUXC using 或修改类型引用
2. ReswResourceProvider.cs 添加 `using System.IO;`
3. 批量替换剩余的 `Microsoft.UI.Colors` → `Windows.UI.Colors`

### 优先级 3：条件编译
1. ResizeGrip.cs 使用条件编译排除 WinUI 3 专属 API
2. Dsl.cs 中引用已排除 Descriptor 的代码使用 `#if !UWP_BUILD`
3. AutoSuggestElement 处理 ReactorApp 引用

## 📝 命名空间使用规则

### UWP 项目中的命名空间使用

| 控件类型 | 使用命名空间 | 别名 |
|---------|------------|------|
| UWP 原生控件（Button, TextBlock等） | `Windows.UI.Xaml.Controls` | `WinUI` |
| WinUI 2 控件（NavigationView, ItemsRepeater等） | `Microsoft.UI.Xaml.Controls` | `MUXC` |
| 布局（StackLayout, Layout等） | `Microsoft.UI.Xaml.Controls` | `MUXC` |
| 颜色和辅助类 | `Windows.UI` | 无 |
| CoreDispatcher | `Windows.UI.Core` | 无 |

### 典型文件头
```csharp
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MUXC = Microsoft.UI.Xaml.Controls;
using WinUI = Windows.UI.Xaml.Controls;  // 仅当需要区分时
```

## 🎯 预计完成

- 修复 NavigationView 问题：预计减少 200+ 个错误
- 快速修复：预计减少 20+ 个错误
- 条件编译：预计减少 30+ 个错误
- 剩余零散错误：需要逐个排查

**预计最终可减少到 50 个以下的错误**，主要是需要创建 UWP 兼容实现的特殊 API。

## 📚 参考资料

- [UWP NavigationView API](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview)
- [WinUI 2 NavigationView API](https://docs.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.navigationview)
- [CoreDispatcher API](https://learn.microsoft.com/en-us/uwp/api/windows.ui.core.coredispatcher)
- [UWP Colors](https://learn.microsoft.com/en-us/uwp/api/windows.ui.colors)
