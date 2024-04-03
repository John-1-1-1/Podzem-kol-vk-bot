using VkBotWorker.Services;
using VkNet;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace VkBotWorker;

public class Worker : BackgroundService {
    private readonly ILogger<Worker> _logger;
    private readonly VkApi _vkApi;

    private MessageHandler _messageHandler = new MessageHandler();

    
    public Worker(VkApi vkApi, ILogger<Worker> logger) {
        _logger = logger;
        _vkApi = vkApi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        
  
        var s = _vkApi.Messages.GetLongPollServer(lpVersion: 3);            
        ulong lastPts = 0; // Инициализация переменной для хранения последней серверной точки обновлений
        long peerId = -182985865;
        
        while(true)
        {
            
            var messages = _vkApi.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams { Ts = s.Ts, Pts = lastPts , LpVersion = 3});
            Thread.Sleep(10000);
            foreach (var message in messages.Messages) {
                if (message.PeerId != peerId) {
                    continue;
                }

                Console.WriteLine($"New message: {message.Text}");


                var messagesSendParams = _messageHandler.ExecuteAction(
                    _vkApi.Messages.GetConversationsById(new List<long>() { peerId }).Items,
                    message, peerId);
                    if (messagesSendParams.RandomId != null) {
                            _vkApi.Messages.Send(messagesSendParams);
                            Thread.Sleep(1000);
                }
            }
            lastPts = messages.NewPts;
        }
    }
}