using Serilog.Core;
using Serilog.Events;

namespace Quotes.API.Enrichers;

public class ThreadIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                   "ThreadId", Thread.CurrentThread.ManagedThreadId));
    }
}
