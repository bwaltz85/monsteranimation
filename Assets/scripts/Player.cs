using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int attackPower = 10;

    [Header("Moves")]
    public MoveScriptableObject[] availableMoves; // Set in Inspector
    public MoveScriptableObject[] playerMoves;    // Filled at runtime

    public void AssignRandomMoves()
    {
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogError("No available moves assigned to Player.");
            return;
        }

        playerMoves = new MoveScriptableObject[4];
        bool hasDamageMove = false;

        while (!hasDamageMove)
        {
            for (int i = 0; i < playerMoves.Length; i++)
            {
                playerMoves[i] = availableMoves[Random.Range(0, availableMoves.Length)];

                if (playerMoves[i].moveType == MoveType.Damage)
                {
                    hasDamageMove = true;
                }
            }
        }
    }

    public void ExecuteMove(MoveScriptableObject move, Opponent opponent)
    {
        switch (move.moveType)
        {
            case MoveType.Damage:
                opponent.TakeDamage(move.power);
                break;

            case MoveType.Heal:
                health += move.power;
                health = Mathf.Min(health, 100); // Optional: Clamp to max health
                break;

            case MoveType.Buff:
                attackPower += move.power;
                break;

            case MoveType.Debuff:
                opponent.ApplyDebuff(move.power);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);
    }
}
