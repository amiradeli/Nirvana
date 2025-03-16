using Microsoft.Extensions.Logging;
using Refit;
using WorkItemApp.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace WorkItemApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug()
					  .SetMinimumLevel(LogLevel.Debug);
#endif

		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				Converters = { new JsonStringEnumConverter() }
			})
		};

		builder.Services.AddRefitClient<IWorkItemService>(refitSettings)
			.ConfigureHttpClient(c => 
			{
				c.BaseAddress = new Uri("http://localhost:5142");
				Console.WriteLine($"Configured API URL: {c.BaseAddress}");
			});

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<AddWorkItemPage>();
		builder.Services.AddLogging(logging =>
		{
			logging.AddDebug();
			logging.SetMinimumLevel(LogLevel.Debug);
		});

		var app = builder.Build();
		Console.WriteLine("MAUI App built successfully");
		return app;
	}
}
