using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using MUXC = Microsoft.UI.Xaml.Controls;
using WinUI = Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Reactor.Core.V1Protocol.Descriptor.Descriptors;

/// <summary>
/// Spec 047 §14 Phase 3 deferred specialized controls — descriptor variant of
/// the hand-coded AnimatedVisualPlayer mount/update arms.
/// </summary>
internal static class AnimatedVisualPlayerDescriptor
{
    public static readonly ControlDescriptor<AnimatedVisualPlayerElement, MUXC.AnimatedVisualPlayer> Descriptor =
        new ControlDescriptor<AnimatedVisualPlayerElement, MUXC.AnimatedVisualPlayer>
        {
            Children = new None<AnimatedVisualPlayerElement, MUXC.AnimatedVisualPlayer>(),
            GetSetters = static e => e.Setters,
        }
        .OneWay(
            get: static e => e.AutoPlay,
            set: static (c, v) => c.AutoPlay = v);
}

/// <summary>
/// Spec 048 §3.3 thin handler — instantiated lazily by
/// <see cref="ControlRegistry"/> when the global path needs the
/// descriptor-backed <see cref="AnimatedVisualPlayerDescriptor"/>.
/// </summary>
internal sealed class AnimatedVisualPlayerDescriptorHandler()
    : DescriptorHandler<AnimatedVisualPlayerElement, MUXC.AnimatedVisualPlayer>(AnimatedVisualPlayerDescriptor.Descriptor);
