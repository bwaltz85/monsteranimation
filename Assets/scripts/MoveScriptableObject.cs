using UnityEngine;

public enum MoveType
{
    Damage,  // Damage type move
    Heal,    // Heal type move
    Buff,    // Buff type move
    Debuff   // Debuff type move
}

[CreateAssetMenu(fileName = "New Move", menuName = "Move")]
public class MoveScriptableObject : ScriptableObject
{
    public string moveName;
    public MoveType moveType;
    public int power;  // Power for damage, healing, buffs, or debuffs

    // Executes the move based on its type
    public void ExecuteMove(Player player, Opponent opponent)
    {
        switch (moveType)
        {
            case MoveType.Damage:
                opponent.health.ApplyChange(-power);  // Apply damage (negative power)
                break;
            case MoveType.Heal:
                player.health.ApplyChange(power);  // Apply healing (positive power)
                break;
            case MoveType.Buff:
                player.attackPower += power;  // Apply buff (increasing stats)
                break;
            case MoveType.Debuff:
                opponent.attackPower -= power;  // Apply debuff (decreasing stats)
                break;
            default:
                break;
        }
    }
}
