using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agebull.Common.Logging
{

    /// <summary>
    /// A provider of <see cref="T:Microsoft.Extensions.Logging.Console.TextLogger" /> instances.
    /// </summary>
    [ProviderAlias("Text")]
    public class TextLoggerProvider : ILoggerProvider, IDisposable, ISupportExternalScope
    {
        private readonly IDisposable _optionsReloadToken;
        readonly IOptionsMonitor<TextLoggerOption> _options;
        IExternalScopeProvider _scopeProvider;

        static TextLogger _logger;
        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.Logging.Console.TextLoggerProvider" />.
        /// </summary>
        public TextLoggerProvider(IOptionsMonitor<TextLoggerOption> options)
        {
            _options = options;
            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
            if (_logger != null)
                _logger.ScopeProvider = _scopeProvider;
        }
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return _logger ??= new TextLogger(_options.CurrentValue);
        }
        private void ReloadLoggerOptions(TextLoggerOption options)
        {
            options.Initialize();
            if (_logger != null)
                _logger.Option = options;
        }
        /// <inheritdoc />
        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _logger?.Dispose();
        }
    }


}