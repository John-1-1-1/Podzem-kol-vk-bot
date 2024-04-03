using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VkNet.Model;

namespace VkBotWorker.Services.UnitStateHandler;

public class AttackEnemy {

    public List<ActionUnit> AttackUnit;

    private List<ActionUnit> HealUnit;
    
    public AttackEnemy() {
        AttackUnit = new List<ActionUnit>() {
            new ActionUnit(){ Name = "Блок щитом"},
            new ActionUnit(){ Name = "|Рассечение|"},
            new ActionUnit(){ Name = "|Сила теней|"},
            new ActionUnit(){ Name = "Атака"},
        };

        HealUnit = new List<ActionUnit>() {
            new ActionUnit() { Name = "|Слабое исцеление|"},
            new ActionUnit() { Name = "Зелье" }
        };
    }

    public void ResetAttackUnits() {
        foreach (var unit in AttackUnit) {
            unit.Active = unit.ActiveInBattle;
        }
        
        foreach (var unit in HealUnit) {
            unit.Active = unit.ActiveInBattle;
        }
    }
    
    public MessagesSendParams Execute(Message message,IList<string> textButtons, 
        long peerId, PlayerInfo playerInfo) {

        if (message.Text.IndexOf("\u2764") != -1) {
            ResetAttackUnits();
        }

        if (message.Text.IndexOf("\u2764") == -1 && message.Text.IndexOf("\ud83d\udee1") == -1) {
            return new MessagesSendParams();
        }

        var inlineButtons = GetInlineButtons(message);

        foreach (var unit in AttackUnit) {
            if (textButtons.IndexOf(unit.Name) == -1) {
                unit.Active = false;
            }
        }
        
        foreach (var button in inlineButtons) {
            if (button.IndexOf("зелье") != -1) {
                return new MessagesSendParams()
                    { Message = button, PeerId = peerId, RandomId = 0};
            }
        }
        
        if (message.Text.IndexOf("\u2764") != -1 && playerInfo.LowHp()) {
            
            for (int i = 0; i < HealUnit.Count; i ++) {
                if (HealUnit[i].Active) {
                    HealUnit[i].Active = false;
                    
                    return new MessagesSendParams()
                        { Message = HealUnit[i].Name, PeerId = peerId, RandomId = 0 };
                }
            }
        }
        
        for (int i = 0; i < AttackUnit.Count; i ++) {
            if (AttackUnit[i].Active) {
                AttackUnit[i].Active = false;
                return new MessagesSendParams()
                    { Message = AttackUnit[i].Name, PeerId = peerId, RandomId = 0 };
            }
        }
       

        return new MessagesSendParams();
    }

 
    
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
}

public class ActionUnit {
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public bool ActiveInBattle { get; set; } = true;
}