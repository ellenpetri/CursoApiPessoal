﻿using System.Collections.Concurrent;

namespace ApiCatalogo.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfig;
    readonly ConcurrentDictionary<string, CustomerLogger> loggers = new();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig)
    {

       loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    public void Dispose() => loggers.Clear();
}