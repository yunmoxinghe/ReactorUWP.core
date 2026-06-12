using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using WinUI = Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor.Core.V1Protocol.Descriptor.Descriptors;

/// <summary>
/// Spec 047 §14 Phase 3 deferred specialized controls — descriptor variant of
/// the hand-coded AnnotatedScrollBar mount/update arms.
/// </summary>
internal static class AnnotatedScrollBarDescriptor
{
    public static readonly ControlDescriptor<AnnotatedScrollBarElement, WinUI.AnnotatedScrollBar> Descriptor =
        new ControlDescriptor<AnnotatedScrollBarElement, WinUI.AnnotatedScrollBar>
        {
            Children = new None<AnnotatedScrollBarElement, WinUI.AnnotatedScrollBar>(),
            GetSetters = static e => e.Setters,
        };
}

/// <summary>
/// Spec 048 §3.3 thin handler — instantiated lazily by
/// <see cref="ControlRegistry"/> when the global path needs the
/// descriptor-backed <see cref="AnnotatedScrollBarDescriptor"/>.
/// </summary>
internal sealed class AnnotatedScrollBarDescriptorHandler()
    : DescriptorHandler<AnnotatedScrollBarElement, WinUI.AnnotatedScrollBar>(AnnotatedScrollBarDescriptor.Descriptor);
