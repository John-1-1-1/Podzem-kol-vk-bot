using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VkBotWorker.Services.UnitStateHandler;
using VkNet.Model;

namespace VkBotWorker.Services;

public class MessageHandler {

    public BotState State = BotState.Explore;

    public PlayerInfo PlayerInfo = new PlayerInfo();

    public AttackEnemy AttackEnemy = new AttackEnemy();
    
    public MessagesSendParams ExecuteAction(IEnumerable<Conversation> conversations, Message message, long peerId) {
        
        IList<string> textButtons = GetTextButton(conversations: conversations);
        
        PlayerInfo.GetHp(message);
        
        if (textButtons.IndexOf("В бой") != -1) {
            State = BotState.Attack;
            return new MessagesSendParams() {
                Message = "В бой",
                PeerId = peerId, RandomId = 0
            };
        }

        if (message.Text.IndexOf("Бой завершен") != -1) {
            State = BotState.Explore;
        }
        
        if (State == BotState.Attack) {
            PlayerInfo.buttle = true;
            return AttackEnemy.Execute(message, textButtons, peerId, PlayerInfo);
        }

        if (State == BotState.Explore) {
            return ExploreWorld.Execute(message: message, textButtons, peerId, PlayerInfo);
        }

        return new MessagesSendParams();
    }

    
    
    private IList<string> GetInlineButtons(Message message) {

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
    
    private IList<string> GetTextButton(IEnumerable<Conversation> conversations) {
        List<string> textButtons = new List<string>();
        textButtons = new List<string>();
        if (conversations == null)
            return textButtons; 
        foreach (var item in conversations) {
            if (item.CurrentKeyboard == null) { 
                continue;
            }
            foreach (var buttons in item.CurrentKeyboard.Buttons) { 
                foreach (var button in buttons) { 
                    textButtons.Add(button.Action.Label);
                    
                } 
            } 
            break;
        }

        return textButtons;
    }
}

public class PlayerInfo {
    public double CurrentHp;
    public  double FullHp;
    public bool buttle;

    public void GetHp(Message message) {

        
        var result = new Regex("[0-9]+/[0-9]+").Matches(message.Text);
        
        if (result.Count >= 1) {
            var hp = result[result.Count - 1].Value.Split("/");
            CurrentHp = int.Parse(hp[0]);
            FullHp = int.Parse(hp[1]);
        }
    }

    public bool LowHp() {
        return  CurrentHp / FullHp < 0.6;
    }
    
}

public enum BotState {
    Explore, Attack
}

