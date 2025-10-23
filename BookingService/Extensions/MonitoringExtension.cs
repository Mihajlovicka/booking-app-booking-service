
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using OpenTelemetry.Metrics;
namespace BookingService.Extensions;

public static class MonitoringExtensions
{
    public static WebApplicationBuilder AddMonitoring(
        this WebApplicationBuilder builder
    )
    {
        var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "unknown-service";
        var collectorEndpoint = Environment.GetEnvironmentVariable("OTEL_COLLECTOR") ?? "";
        var lokiEndpoint = Environment.GetEnvironmentVariable("LOKI") ?? "";

            
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);

            configuration.WriteTo.GrafanaLoki(
                uri: lokiEndpoint,
                labels: new[]
                {
                    new LokiLabel { Key = "app", Value = serviceName }
                },
                propertiesAsLabels: new[] { "app" , "RequestPath", "StatusCode" }
            );
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = _ => true; // record all requests, even 404s
                    })
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(collectorEndpoint);
                    opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                })
            ) .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddMeter("custom_metrics_" + serviceName)
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("System.Net.Http")
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .SetExemplarFilter(ExemplarFilterType.AlwaysOn) 
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(collectorEndpoint);
                    });
            });


        return builder;
    }
}