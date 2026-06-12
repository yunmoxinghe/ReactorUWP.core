#if !UWP_BUILD
// 此文件依赖 Hosting 层，在 UWP 版本中暂时排除
// TODO: 为 UWP 实现简化版的 ReactorHost

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor.Core;

/// <summary>
/// Reactor 宿主 - 将 Component 渲染到 UWP 容器中
/// </summary>
public class ReactorHost
{
    private Component? _rootComponent;
    private UIElement? _currentTree;
    private readonly Panel _container;

    /// <summary>
    /// 创建 Reactor 宿主
    /// </summary>
    /// <param name="container">承载 UI 的容器（如 Grid）</param>
    public ReactorHost(Panel container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    /// <summary>
    /// 渲染组件到容器
    /// </summary>
    public void Render(Component component)
    {
        _rootComponent = component ?? throw new ArgumentNullException(nameof(component));

        // 重置 Hook 索引
        _rootComponent.Context.ResetHookIndex();

        // 调用组件的 Render 方法获取 Element 树
        var element = _rootComponent.Render();

        // 将 Element 树转换为 UIElement 树
        var newTree = Reconciler.Render(element, _currentTree);

        // 更新容器
        if (newTree != _currentTree)
        {
            _container.Children.Clear();
            if (newTree != null)
            {
                _container.Children.Add(newTree);
            }
            _currentTree = newTree;
        }

        // 运行 Effects
        _rootComponent.Context.RunEffects();
    }

    /// <summary>
    /// 卸载组件
    /// </summary>
    public void Unmount()
    {
        if (_rootComponent != null)
        {
            _rootComponent.Context.CleanupEffects();
            _rootComponent = null;
        }

        _container.Children.Clear();
        _currentTree = null;
    }
}

#endif
