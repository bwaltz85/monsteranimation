using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Health health;  // Reference to the player's health component
    public int attackPower;  // Player's attack power, which could be modified by buffs/debuffs
    public MoveScriptableObject[] playerMoves;  // Player's moves, which are randomly assigned
    public Opponent target;  // Reference to the opponent (target)

    void Start()
    {
        // Use FindFirstObjectByType instead of the deprecated FindObjectOfType
        target = FindFirstObjectByType<Opponent>();  // Find the first Opponent object in the scene
        AssignRandomMoves();  // Call this to assign random moves to the player at the start of the game
    }

    // Assign random moves to the player, with at least one guaranteed damage move
    public void AssignRandomMoves()
    {
        // You can add logic here to randomize the player's moves,
        // ensuring one of them is a damage-type move.

        // For this example, we assume you have a list of available moves and select randomly.
        List<MoveScriptableObject> allMoves = new List<MoveScriptableObject>();

        // Add your move options here
        allMoves.AddRange(Resources.LoadAll<MoveScriptableObject>("Moves"));

        // Randomize and pick 4 moves (at least one damage move)
        playerMoves = new MoveScriptableObject[4];

        bool hasDamageMove = false;

        // Ensure at least one damage move
        while (!hasDamageMove)
        {
            for (int i = 0; i < playerMoves.Length; i++)
            {
                playerMoves[i] = allMoves[Random.Range(0, allMoves.Count)];

                // Check if there is at least one damage move
                if (playerMoves[i].moveType == MoveType.Damage)
                {
                    hasDamageMove = true;
                }
            }
        }
    }

    // Use a move by calling the selected move from playerMoves
    public void UseMove(MoveScriptableObject selectedMove)
    {
        if (selectedMove != null)
        {
            // Apply effects based on move type
            if (selectedMove.moveType == MoveType.Damage)
            {
                target.TakeDamage(selectedMove.power);
            }
            else if (selectedMove.moveType == MoveType.Debuff)
            {
                target.ApplyDebuff(selectedMove.power);
            }
            else if (selectedMove.moveType == MoveType.Heal)
            {
                health.ApplyChange(selectedMove.power);  // Heal the player
            }
            else if (selectedMove.moveType == MoveType.Buff)
            {
                attackPower += selectedMove.power;  // Apply buff to the player's stats
            }
        }
    }

    // You could also implement additional functionality for taking damage, healing, etc.
    public void TakeDamage(int damage)
    {
        health.ApplyChange(-damage);  // Decrease health by the given damage
    }
}
