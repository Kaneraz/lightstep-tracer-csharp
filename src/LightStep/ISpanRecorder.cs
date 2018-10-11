﻿using System.Collections.Generic;

namespace LightStep
{
    /// <summary>
    ///     Records spans generated by the tracer. Used to batch/send spans to a collector.
    /// </summary>
    public interface ISpanRecorder
    {
        /// <summary>
        ///     Saves a span.
        /// </summary>
        /// <param name="span"></param>
        void RecordSpan(SpanData span);

        /// <summary>
        ///     Gets the current record of spans.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SpanData> GetSpanBuffer();

        /// <summary>
        ///     Clears the span record.
        /// </summary>
        void ClearSpanBuffer();
    }
}