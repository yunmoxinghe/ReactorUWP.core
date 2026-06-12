# ReactorUWP.Core

**为 UWP 带来纯 C# 声明式 UI 开发体验**

基于 [Microsoft.UI.Reactor](https://github.com/microsoft/microsoft-ui-reactor) 架构，为 UWP/WinUI 2 实现的实验性声明式 UI 框架。

## 🎯 项目目标

Microsoft.UI.Reactor 是微软官方的 WinUI 3 声明式 UI 框架（实验性），但它**不支持 UWP**。ReactorUWP.Core 旨在将 Reactor 架构移植到 UWP 平台，并**使用完全相同的命名空间**，实现代码级别的兼容。

这意味着你可以：
- ✅ 编写一次代码，在共享项目中使用 `using Microsoft.UI.Reactor;`
- ✅ UWP 项目引用 ReactorUWP.Core（提供 `Microsoft.UI.Reactor` 命名空间）
- ✅ WinUI 3 项目引用官方 NuGet 包（同样是 `Microsoft.UI.Reactor` 命名空间）
- ✅ **无需任何条件编译或命名空间别名**，真正的一次编写，双平台运行

核心特性：
- ✅ **无需 XAML** - 所有 UI 都在 C# 代码中声明
- ✅ **React 风格 Hooks** - `UseState`, `UseEffect` 状态管理
- ✅ **声明式渲染** - UI 是状态的函数
- ✅ **原生 WinUI 2 控件** - 渲染为真实的 UWP 控件
- ✅ **API 完全兼容** - `Border(child).Background("#000000")` 等 API 与官方 Reactor 一致

## 🏗️ 架构特性

### 三层架构设计

```
共享项目 (FluentNotepads.Shared)
└── using Microsoft.UI.Reactor;  // 统一命名空间！
    └── EditingPage.cs
        └── Border(Empty()).Background("#000000")

UWP 项目 (FluentNotepads.UWP)
└── 引用 ReactorUWP.Core 类库
    └── 提供 Microsoft.UI.Reactor 命名空间实现

WinUI 3 项目 (FluentNotepads.WinUI3)
└── 引用官方 NuGet: Microsoft.UI.Reactor
    └── 官方提供 Microsoft.UI.Reactor 命名空间
```

**关键优势**: 无需 `#if UWP` 条件编译，无需命名空间别名，真正的"写一次，到处运行"。

### 核心概念

```csharp
using Microsoft.UI.Reactor;  // 同一个命名空间！

// 这段代码在 UWP 和 WinUI 3 中完全相同
public class EditingPage : Component
{
    public override Element Render()
    {
        var (count, setCount) = UseState(0);
        
        return Border(
            R.Empty()
        ).Background("#000000");
    }
}
```

### 核心架构

**命名空间兼容性**: 使用 `Microsoft.UI.Reactor` 命名空间（与官方 Reactor 相同），实现代码级别的兼容。

**三态协调器（Reconciler）**: 基于 Virtual DOM 差分算法
- **Mount** - 创建新控件（可从对象池租用）
- **Update** - 原地修补属性变化
- **Unmount** - 清理并归还到对象池

**Tag-Based 事件分发**: 事件处理器挂载时绑定一次，通过 Tag 始终指向最新闭包，避免重复绑定导致的内存泄漏。

**不可变元素树**: 使用 C# 10 `record` 实现值语义和结构相等，配合早期跳过优化。

## 💻 技术规格

- **目标框架**: .NET 10.0 (Windows 10.0.26100.0)
- **最低平台**: Windows 10.0.17763.0  
- **可空引用类型**: 启用
- **AOT 兼容**: 是
- **运行时编组**: 禁用（性能优化）

## 🚧 当前状态

### ✅ 已实现
- [x] **命名空间兼容**: 使用 `Microsoft.UI.Reactor` 而非自定义命名空间
- [x] Element 记录基础架构
- [x] Component 基类
- [x] RenderContext（Hook 状态管理）
- [x] UseState Hook
- [x] UseEffect Hook  
- [x] Reconciler 三态分发（Mount/Update/Unmount）
- [x] BorderElement 及其工厂方法
- [x] `.Background()` 扩展方法
- [x] 十六进制颜色解析（`#000000`, `#AARRGGBB`）
- [x] ReactorHost（组件宿主）
- [x] Empty() 工厂方法

### 🔨 待实现
- [ ] VStack, HStack（垂直和水平堆栈）
- [ ] Grid（网格布局）
- [ ] TextBlock, Button, TextBox 等基础控件
- [ ] ScrollViewer（滚动容器）
- [ ] 事件处理（Tag-based 分发）
- [ ] 子元素协调器（LIS 键匹配算法）
- [ ] 元素池（对象池优化）
- [ ] 更多 Hooks：UseReducer, UseMemo, UseCallback, UseRef
- [ ] Context 系统
- [ ] 导航系统

## 📖 文档

查看 `/docs` 目录获取：
- 架构设计文档
- API 参考
- 使用示例
- 与 Microsoft.UI.Reactor 的对比

## 🤝 与 Microsoft.UI.Reactor 的关系

|  | Microsoft.UI.Reactor | ReactorUWP.Core |
|--|--|--|
| 平台 | WinUI 3（桌面） | UWP/WinUI 2 |
| 状态 | 官方实验性 | 第三方移植 |
| 命名空间 | `Microsoft.UI.Reactor` | `Microsoft.UI.Reactor` ✅ 相同！ |
| 架构 | 原版设计 | 基于原版移植 |
| API | `Border(child).Background(...)` | `Border(child).Background(...)` ✅ 相同！ |
| 目标 | WinUI 3 应用 | UWP 应用，实现共享代码 |

## ⚠️ 注意事项

1. **实验性项目** - 这是学习和原型项目，不建议用于生产环境
2. **API 可能变化** - 在稳定之前 API 可能有破坏性变更
3. **非官方** - 这不是微软官方项目

## 🔧 开发环境

- Visual Studio 2026 或更高版本
- .NET 10.0 SDK
- Windows 10 SDK (10.0.26100.0)

## 📦 构建

```bash
dotnet build
```

## 📚 学习资源

- [Microsoft.UI.Reactor 官方文档](https://microsoft.github.io/microsoft-ui-reactor/)
- [React Hooks 文档](https://react.dev/reference/react)
- [Virtual DOM 协调算法](https://react.dev/learn/preserving-and-resetting-state)

## 📄 许可证

MIT（待定）

## 🙏 致谢

本项目的架构设计和核心思想来自 Microsoft.UI.Reactor 团队的杰出工作。
