using System;
using Windows.Foundation;
using Windows.UI.Core;

namespace Microsoft.UI.Reactor.Core
{
    /// <summary>
    /// Extension methods for CoreDispatcher to provide WinUI 3-like TryEnqueue API for UWP.
    /// </summary>
    internal static class CoreDispatcherExtensions
    {
        /// <summary>
        /// Attempts to enqueue a callback on the dispatcher queue with normal priority.
        /// Returns true if the callback was successfully enqueued.
        /// </summary>
        public static bool TryEnqueue(this CoreDispatcher dispatcher, Action callback)
        {
            if (dispatcher == null || callback == null) return false;
            try
            {
                _ = dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(callback));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to enqueue a callback on the dispatcher queue with the specified priority.
        /// Returns true if the callback was successfully enqueued.
        /// </summary>
        public static bool TryEnqueue(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Action callback)
        {
            if (dispatcher == null || callback == null) return false;
            try
            {
                _ = dispatcher.RunAsync(priority, new DispatchedHandler(callback));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}