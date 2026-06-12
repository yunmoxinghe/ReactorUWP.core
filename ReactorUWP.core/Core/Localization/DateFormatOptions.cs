using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.UI.Reactor.Localization;

/// <summary>
/// Options for IntlAccessor.FormatDate(). Maps to .NET DateTimeFormatInfo styles.
/// </summary>
public enum DateStyle
{
    Default,
    Short,
    Long,
    Full
}

public sealed class DateFormatOptions
{
    public DateStyle Style { get; init; }
}
