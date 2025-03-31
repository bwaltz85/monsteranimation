using UnityEngine;

public class MoveManager : MonoBehaviour
{
    // Array to store all possible moves (ScriptableObject references)
    public MoveScriptableObject[] allPossibleMoves;

    // Player's 4 random moves
    public MoveScriptableObject[] playerMoves = new MoveScriptableObject[4];

    // Reference to GameManager (for updating UI or performing actions based on moves)
    public GameManager gameManager;

    void Start()
    {
        // Assign random moves to the player at the start of the game
        AssignRandomMoves();
    }

    // Assign 4 random moves to the player, ensuring that at least one is a damage move
    void AssignRandomMoves()
    {
        bool damageMoveAssigned = false;

        for (int i = 0; i < playerMoves.Length; i++)
        {
            int randomIndex = Random.Range(0, allPossibleMoves.Length);
            playerMoves[i] = allPossibleMoves[randomIndex];

            // If this move is damage type, mark it
            if (playerMoves[i].moveType == MoveType.Damage)
            {
                damageMoveAssigned = true;
            }
        }

        // If no damage move is assigned, force a damage move to be one of the player's moves
        if (!damageMoveAssigned)
        {
            int randomIndex = Random.Range(0, allPossibleMoves.Length);
            for (int i = 0; i < playerMoves.Length; i++)
            {
                if (playerMoves[i].moveType != MoveType.Damage)
                {
                    playerMoves[i] = allPossibleMoves[randomIndex];
                    break;
                }
            }
        }

        // Update the UI with the assigned moves (you can modify this to fit your UI system)
        gameManager.UpdateMoveButtons(playerMoves);
    }

    // Example of how a move could be used (damage, heal, etc.)
    public void PerformMove(int moveIndex)
    {
        MoveScriptableObject move = playerMoves[moveIndex];

        switch (move.moveType)
        {
            case MoveType.Damage:
                // Perform damage to opponent
                break;
            case MoveType.Heal:
                // Perform healing to player
                break;
            case MoveType.Buff:
                // Apply buff to player
                break;
            case MoveType.Debuff:
                // Apply debuff to opponent
                break;
        }
    }
}