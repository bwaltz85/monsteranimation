using UnityEngine;

[CreateAssetMenu(fileName = "FireBlast", menuName = "Game/Moves/FireBlast")]
public class FireBlast : MoveScriptableObject
{
    // This is just an example, you can define any other specific behavior in the script
    private void OnEnable()
    {
        moveName = "Fire Blast";
        moveType = MoveType.Damage;
        power = 120;
    }
}