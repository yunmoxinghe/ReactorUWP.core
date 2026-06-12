using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Windows.UI.Xaml;
using MUXC = Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using WinUI = Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor.Core.V1Protocol.Descriptor.Descriptors;

/// <summary>
/// Spec 047 §14 Phase 3 (batch 11) — descriptor variant of the hand-coded
/// <c>MountBreadcrumbBar</c> / <c>UpdateBreadcrumbBar</c> arms in
/// <see cref="Reconciler"/>.
///
/// <para><b>Coverage:</b>
/// <list type="bullet">
///   <item><c>Items</c> — one-way <c>ItemsSource</c> assignment of a label
///   list. The descriptor rebuilds the label list on each pass and assigns
///   it to <c>ItemsSource</c> (mirrors the legacy mount + update arms,
///   which both unconditionally do the same).</item>
///   <item><c>ItemClicked</c> — <see cref="ControlDescriptor{TElement,TControl}.HandCodedEvent{TPayload,TDelegate}"/>.
///   The trampoline maps <c>args.Index</c> back to the live element's
///   <c>Items[idx]</c> data — matches the legacy hand-coded mapping.</item>
/// </list></para>
/// </summary>
internal static class BreadcrumbBarDescriptor
{
    private static readonly TypedEventHandler<MUXC.BreadcrumbBar, MUXC.BreadcrumbBarItemClickedEventArgs>
        ItemClickedTrampoline = (s, args) =>
        {
            var bar = (MUXC.BreadcrumbBar)s!;
            if (Reconciler.GetElementTag(bar) is not BreadcrumbBarElement el) return;
            if (args.Index >= 0 && args.Index < el.Items.Length)
                el.OnItemClicked?.Invoke(el.Items[args.Index]);
        };

    public static readonly ControlDescriptor<BreadcrumbBarElement, MUXC.BreadcrumbBar> Descriptor =
        new ControlDescriptor<BreadcrumbBarElement, MUXC.BreadcrumbBar>
        {
            Children = new None<BreadcrumbBarElement, MUXC.BreadcrumbBar>(),
            GetSetters = static e => e.Setters,
        }
        .OneWay<BreadcrumbBarItemData[]>(
            get: static e => e.Items,
            set: static (c, items) => c.ItemsSource = items.Select(i => i.Label).ToList())
        .HandCodedEvent<BreadcrumbBarEventPayload,
            TypedEventHandler<MUXC.BreadcrumbBar, MUXC.BreadcrumbBarItemClickedEventArgs>>(
            subscribe:        static (c, h) => c.ItemClicked += h,
            callbackPresent:  static e => e.OnItemClicked,
            trampoline:       ItemClickedTrampoline,
            slotIsNull:       static p => p.ItemClickedTrampoline is null,
            setSlot:          static (p, h) => p.ItemClickedTrampoline = h);
}

/// <summary>
/// Spec 048 §3.3 thin handler — instantiated lazily by
/// <see cref="ControlRegistry"/> when the global path needs the
/// descriptor-backed <see cref="BreadcrumbBarDescriptor"/>.
/// </summary>
internal sealed class BreadcrumbBarDescriptorHandler()
    : DescriptorHandler<BreadcrumbBarElement, MUXC.BreadcrumbBar>(BreadcrumbBarDescriptor.Descriptor);
