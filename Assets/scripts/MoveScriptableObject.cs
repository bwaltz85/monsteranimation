using UnityEngine;

public enum MoveType
{
    Damage,
    Heal,
    Buff,
    Debuff
}

[CreateAssetMenu(fileName = "NewMove", menuName = "Battle/Move")]
public class MoveScriptableObject : ScriptableObject
{
    public string moveName;
    public MoveType moveType;
    public int power;            // Damage, heal amount, or stat change
    [TextArea]
    public string description;   // Optional: shown in UI or tooltip
}
