using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrike", menuName = "Game/Moves/ThunderStrike")]
public class ThunderStrike : MoveScriptableObject
{
    // This is just an example, you can define any other specific behavior in the script
    private void OnEnable()
    {
        moveName = "Thunder Strike";
        moveType = MoveType.Damage;
        power = 50;
    }
}