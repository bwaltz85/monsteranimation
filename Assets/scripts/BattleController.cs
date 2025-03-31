using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController : MonoBehaviour
{
    // UI references for the move buttons, HP texts, and message display
    public Button[] moveButtons;   // Buttons for the player's moves
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI opponentHPText;
    public TextMeshProUGUI battleMessageText;

    // Player and opponent stats
    public int playerHP = 100;
    public int opponentHP = 100;

    // The current player moves
    public MoveScriptableObject[] playerMoves = new MoveScriptableObject[4];

    // Reference to GameManager for battle control
    public GameManager gameManager;

    // This will manage the player's turn and perform battle logic
    void Start()
    {
        // Initially set up the UI with player and opponent HP
        UpdateHPUI();
        // Enable all buttons at the start
        SetMoveButtonsActive(true);
    }

    // Method to handle the player's move selection from a button press
    public void OnMoveButtonPressed(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < playerMoves.Length)
        {
            // Perform the player's selected move
            MoveScriptableObject selectedMove = playerMoves[moveIndex];
            Battle(selectedMove);

            // After the player's move, the opponent will have a turn
            OpponentTurn();
        }
    }

    // Perform the battle logic for each move
    void Battle(MoveScriptableObject move)
    {
        // Display move message
        battleMessageText.text = "You used " + move.moveName + "!";

        // Handle different types of moves
        if (move.moveType == MoveType.Damage)
        {
            // Damage opponent
            opponentHP -= move.power;
        }
        else if (move.moveType == MoveType.Heal)
        {
            // Heal the player
            playerHP += move.power;
        }

        // Update the HP UI after each move
        UpdateHPUI();

        // Check if the battle is over
        if (opponentHP <= 0)
        {
            battleMessageText.text = "You defeated the opponent!";
            SetMoveButtonsActive(false); // Disable move buttons after victory
            // Optionally: Trigger some reward logic here
        }
        else if (playerHP <= 0)
        {
            battleMessageText.text = "You lost the battle!";
            SetMoveButtonsActive(false); // Disable move buttons after defeat
            // Game over logic
        }
    }

    // Simulate opponent's turn with random move (or logic to define the opponent's action)
    void OpponentTurn()
    {
        // For simplicity, we'll just apply a random damage move from the opponent
        MoveScriptableObject opponentMove = GetRandomOpponentMove();

        battleMessageText.text = "Opponent used " + opponentMove.moveName + "!";

        // If the move is damage, apply to player
        if (opponentMove.moveType == MoveType.Damage)
        {
            playerHP -= opponentMove.power;
        }

        // Update the HP UI after the opponent's turn
        UpdateHPUI();

        // Check if the battle is over after the opponent's turn
        if (playerHP <= 0)
        {
            battleMessageText.text = "You lost the battle!";
            SetMoveButtonsActive(false); // Disable move buttons after defeat
        }
    }

    // Update the player's and opponent's HP UI
    void UpdateHPUI()
    {
        playerHPText.text = "HP: " + playerHP.ToString();
        opponentHPText.text = "HP: " + opponentHP.ToString();
    }

    // Disable or enable all move buttons
    void SetMoveButtonsActive(bool isActive)
    {
        for (int i = 0; i < moveButtons.Length; i++)
        {
            moveButtons[i].gameObject.SetActive(isActive); // Set the button's GameObject active or inactive
        }
    }

    // For simplicity, randomly select an opponent move (you can expand this logic)
    MoveScriptableObject GetRandomOpponentMove()
    {
        // For this example, just pick a random move from the player's moves
        int randomIndex = Random.Range(0, playerMoves.Length);
        return playerMoves[randomIndex];
    }
}