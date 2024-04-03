using VkBotWorker;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var api = new VkApi();
	
api.Authorize(new ApiAuthParams
{
    AccessToken = ""
});

builder.Services.AddSingleton(api);
var host = builder.Build();
host.Run();