using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WalletAnalyzer.TemporaryTesting;
using WebScraper;
using WebScraper.Parsers;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class Program
    {
        private static ILogger<Program> _logger;

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.LogCritical("Unhandled exception occured\n" + (Exception)e.ExceptionObject);
            throw new Exception("Global Exception Handler");
        }

        static async Task Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            _logger = serviceProvider.GetService<ILogger<Program>>();

            var config = serviceProvider.GetService<IOptions<AppSettingsOptions>>().Value;

            var sleepTimeMs = config.Blockchains.Etherscan.SleepTimeBetweenScrapesInMs;
            var appendPeriodInMs = config.Output.AppendPeriodInSeconds;
            var configEtherscan = config.Blockchains.Etherscan;
            var configDexUrl = configEtherscan.DexTable.Url;

            foreach (var token in config.Blockchains.Etherscan.TokensToScrape)
            {
                var dexCollector = serviceProvider.GetService<IDexCollector>();
                var url = $"{configEtherscan.DomainName}/{configDexUrl.Path}&{configDexUrl.TokenVarName}={token.Hash}";
                var tokenNameUrl = $"{configEtherscan.DomainName}/{configDexUrl.TokenNamePath}/{token.Hash}";
                await dexCollector.Start(url, tokenNameUrl, token.Hash, sleepTimeMs, appendPeriodInMs, token.RowsAmount);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var config = SetupConfiguration();

            services
                .AddAutoMapper(typeof(Program))
                .Configure<AppSettingsOptions>(
                    config.GetSection("AppSettings"))
                .Configure<ApiOptions>(
                    config.GetSection("AppSettings").GetSection("Blockchains").GetSection("Etherscan").GetSection("Api"))
                .Configure<OutputOptions>(
                    config.GetSection("AppSettings").GetSection("Output"));

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            services
                .AddTransient<IDexOutput, DexGoogleSheetsOutput>()
                .AddTransient<IParserCommon, ParserCommon>()

                .AddTransient<IWebScraper, WebScraper.WebScraper>()
                //.AddTransient<IWebScraper, MockWebScraper>()

                .AddTransient<IDexTableParser, EtherscanDexParser>()
                .AddTransient<IDexScraper, EtherscanDexScraper>()
                .AddTransient<IDexCollector, DexCollector>()
                .AddTransient<IDexScraperFactory, DexScraperFactory>()
                .AddSingleton<IEtherscanApiServices, EtherscanApiServices>();
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile($"local-appsettings.json", false)
                .AddJsonFile($"appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
        }
    }
}
