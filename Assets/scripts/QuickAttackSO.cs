using UnityEngine;

[CreateAssetMenu(fileName = "QuickAttack", menuName = "Game/Moves/QuickAttack")]
public class QuickAttack : MoveScriptableObject
{
    // This is just an example, you can define any other specific behavior in the script
    private void OnEnable()
    {
        moveName = "Quick Attack";
        moveType = MoveType.Damage;
        power = 30;
    }
}