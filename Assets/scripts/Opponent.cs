using UnityEngine;

public class Opponent : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int attackPower = 10;

    [Header("Moves")]
    public MoveScriptableObject[] availableMoves;  // Set in Inspector
    public MoveScriptableObject[] opponentMoves;   // Filled at runtime

    public void AssignRandomMoves()
    {
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogError("No available moves assigned to Opponent.");
            return;
        }

        opponentMoves = new MoveScriptableObject[4];
        bool hasDamageMove = false;

        while (!hasDamageMove)
        {
            for (int i = 0; i < opponentMoves.Length; i++)
            {
                opponentMoves[i] = availableMoves[Random.Range(0, availableMoves.Length)];

                if (opponentMoves[i].moveType == MoveType.Damage)
                {
                    hasDamageMove = true;
                }
            }
        }
    }

    public MoveScriptableObject SelectRandomMove()
    {
        if (opponentMoves == null || opponentMoves.Length == 0)
        {
            Debug.LogWarning("Opponent has no moves assigned.");
            return null;
        }

        int index = Random.Range(0, opponentMoves.Length);
        return opponentMoves[index];
    }

    public void ExecuteMove(MoveScriptableObject move, Player player)
    {
        if (move == null) return;

        switch (move.moveType)
        {
            case MoveType.Damage:
                player.TakeDamage(move.power);
                break;

            case MoveType.Heal:
                health += move.power;
                health = Mathf.Min(health, 100);
                break;

            case MoveType.Buff:
                attackPower += move.power;
                break;

            case MoveType.Debuff:
                player.attackPower -= move.power;
                player.attackPower = Mathf.Max(player.attackPower, 0);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);
    }

    public void ApplyDebuff(int power)
    {
        attackPower -= power;
        attackPower = Mathf.Max(attackPower, 0);
    }
}
