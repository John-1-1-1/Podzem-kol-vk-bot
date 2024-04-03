using System.Collections.ObjectModel;
using VkNet.Model;

namespace VkBotWorker.Services.UnitStateHandler;

public class ExploreWorld {
    
    private static IList<string> GetInlineButtons(Message message) {

        IList<string> inlineButtons = new Collection<string>();
        if (message.Keyboard == null) 
            return inlineButtons;
        
        foreach (var buttons in message.Keyboard.Buttons) {
            if (buttons == null) 
                continue;
            

            foreach (var button in buttons) {
                if (button == null) 
                    continue;
                
                inlineButtons.Add(button.Action.Label);
            }
        }

        return inlineButtons;
    }
    static public MessagesSendParams Execute(Message message,IList<string> textButtons, 
        long peerId, PlayerInfo playerInfo) {

        var inlineButtons = GetInlineButtons(message: message);

        if (inlineButtons.IndexOf("Освежевать") != -1) {
            return new MessagesSendParams() { Message = "Освежевать", PeerId = peerId, RandomId = 0 };
        }

        if (message.Text.IndexOf("Вся рудa полностью дoбыта") != -1) {
            return new MessagesSendParams() { Message = "Покинуть", PeerId = peerId, RandomId = 0 };
        }
        
        if (playerInfo.buttle) {
            playerInfo.buttle = false;
            return new MessagesSendParams() { Message = "Отдых", PeerId = peerId, RandomId = 0 };
        }
        
        
        if (textButtons.IndexOf("Исследовать уровень") != -1) {
            return new MessagesSendParams() { Message = "Исследовать уровень", PeerId = peerId, RandomId = 0 };
        }

        if (textButtons.IndexOf("Обыскать") != -1) {
            if (message.Text.IndexOf("нечего", StringComparison.Ordinal) != -1) {
                return new MessagesSendParams() {
                    Message = "Обыскать", PeerId = peerId, RandomId = 0
                };
            }
        }

        if (textButtons.IndexOf("Продолжить") != -1) {
            return new MessagesSendParams() {
                Message = "Продолжить", PeerId = peerId, RandomId = 0
            };
        }

        if (textButtons.IndexOf("Запад") != -1) {
            return new MessagesSendParams() {
                Message = "Запад", PeerId = peerId, RandomId = 0
            };
        }

        if (textButtons.IndexOf("Закинуть удочку") != -1) {
            if (message.Text.IndexOf("Нaживки осталоcь: 0", StringComparison.Ordinal) != -1 ||
                message.Text.IndexOf("Наживка в лодке закончилась", StringComparison.Ordinal) != -1) {
                return new MessagesSendParams() {
                    Message = "Прервать рыбалку", PeerId = peerId, RandomId = 0
                };
            }
            
            return new MessagesSendParams() { 
                Message = "Закинуть удочку", PeerId = peerId, RandomId = 0
            };
            
        }

        if (textButtons.IndexOf("Подсечь") != -1) {
            return new MessagesSendParams() {
                Message = "Подсечь", PeerId = peerId, RandomId = 0
            };
            
        }

        if (textButtons.IndexOf("Левая") != -1) {
            return new MessagesSendParams() { Message = "Левая", PeerId = peerId, RandomId = 0 };
        }
        if (textButtons.IndexOf("Открыть правую") != -1) {
            return new MessagesSendParams() {
                Message = "Открыть правую", PeerId = peerId, RandomId = 0
            };
        }
        if (textButtons.IndexOf("Открыть левую") != -1) { 
            return new MessagesSendParams() { 
                Message = "Открыть левую", PeerId = peerId, RandomId = 0
            };
        }

        if (textButtons.IndexOf("Обыскать") != -1) {
           return new MessagesSendParams() {
               Message = "Обыскать", PeerId = peerId, RandomId = 0 
           };
        }

        if (textButtons.IndexOf("Открыть") != -1) {
            return new MessagesSendParams() {
                Message = "Открыть", PeerId = peerId, RandomId = 0 
            };
        }
        
        if (textButtons.IndexOf("Покинуть источник") != -1) {
            return new MessagesSendParams() {
                Message = "Покинуть источник", PeerId = peerId, RandomId = 0 
            };
        }
        
        if (textButtons.IndexOf("Обыскать зал") != -1) {
            return new MessagesSendParams() {
                Message = "Обыскать зал", PeerId = peerId, RandomId = 0 
            };
        }
        
        if (textButtons.IndexOf("Стащить золото") != -1) {
            return new MessagesSendParams() {
                Message = "Стащить золото", PeerId = peerId, RandomId = 0 
            };
        }

        if (textButtons.IndexOf("Прервать поиск") != -1 &&
            message.Text.IndexOf("не осталось") != -1) {
            return new MessagesSendParams() {
                Message = "Прервать поиск", PeerId = peerId, RandomId = 0 
            };
        }
       
        if (message.Text.IndexOf("Рyны на камне") != -1) {
            return new MessagesSendParams() {
                Message = "Уйти", PeerId = peerId, RandomId = 0 
            };
        }
        
        return new MessagesSendParams();
    }
}