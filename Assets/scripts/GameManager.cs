using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public BattleUIManager uiManager;
    public TurnManager turnManager; // ✅ Reference to TurnManager instance

    private bool isPlayerTurn = true;
    private bool battleEnded = false;

    void Start()
    {
        player.AssignRandomMoves();
        opponent.AssignRandomMoves();

        uiManager.UpdateMoveButtons();
        uiManager.UpdateHPUI();

        uiManager.DisplayMessage("Battle Start! Your turn.");
    }

    public void OnPlayerMoveSelected(int moveIndex)
    {
        if (!isPlayerTurn || battleEnded) return;

        MoveScriptableObject selectedMove = player.playerMoves[moveIndex];
        player.ExecuteMove(selectedMove, opponent);
        uiManager.UpdateHPUI();

        if (opponent.health <= 0)
        {
            EndBattle(true);
            return;
        }

        isPlayerTurn = false;
        Invoke(nameof(OpponentTurn), 1.5f); // Short delay for pacing

        if (turnManager != null)
        {
            turnManager.EndPlayerTurn(); // ✅ Call instance method safely
        }
        else
        {
            Debug.LogWarning("TurnManager not assigned in GameManager!");
        }
    }

    void OpponentTurn()
    {
        if (battleEnded) return;

        MoveScriptableObject opponentMove = opponent.SelectRandomMove();
        opponent.ExecuteMove(opponentMove, player);
        uiManager.UpdateHPUI();
        uiManager.DisplayMessage($"Opponent used {opponentMove.moveName}!");

        if (player.health <= 0)
        {
            EndBattle(false);
            return;
        }

        isPlayerTurn = true;
        uiManager.DisplayMessage("Your turn.");
    }

    void EndBattle(bool playerWon)
    {
        battleEnded = true;

        if (playerWon)
            uiManager.DisplayMessage("You win!");
        else
            uiManager.DisplayMessage("You lose.");
    }
}
