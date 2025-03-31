using UnityEngine;
using UnityEngine.UI;  // For working with UI elements (like buttons)
using TMPro;  // For TextMeshPro

public class GameManager : MonoBehaviour
{
    // Reference to the move buttons in the UI
    public Button[] moveButtons;   // Array of 4 buttons for the moves
    public TextMeshProUGUI playerHPText;   // TextMeshPro for displaying player's HP
    public TextMeshProUGUI opponentHPText; // TextMeshPro for displaying opponent's HP
    public TextMeshProUGUI battleMessageText; // TextMeshPro for displaying battle messages

    // References to the player's and opponent's HP
    public int playerHP = 100;
    public int opponentHP = 100;

    // Array to store the player's moves
    public MoveScriptableObject[] playerMoves = new MoveScriptableObject[4];

    // You can reference a list of all possible moves here for random selection
    public MoveScriptableObject[] allPossibleMoves;

    // Called when the game starts
    void Start()
    {
        // Update the move buttons and HP UI initially
        UpdateMoveButtons(playerMoves);
        UpdateHPUI();
    }

    // Method to update the buttons with the player's current moves
    public void UpdateMoveButtons(MoveScriptableObject[] moves)
    {
        // Loop through each move button and update its label with the move name
        for (int i = 0; i < moveButtons.Length; i++)
        {
            if (i < moves.Length)
            {
                TextMeshProUGUI buttonText = moveButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = moves[i].moveName;  // Set the move name as the button label
                }

                // Add listener to the button to execute the corresponding move when clicked
                int moveIndex = i;  // Local copy to use inside the lambda
                moveButtons[i].onClick.RemoveAllListeners();  // Remove existing listeners
                moveButtons[i].onClick.AddListener(() => OnMoveButtonPressed(moveIndex));
            }
        }
    }

    // Method to handle the move button press
    public void OnMoveButtonPressed(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < playerMoves.Length)
        {
            // Perform the move and update the battle
            MoveScriptableObject selectedMove = playerMoves[moveIndex];
            Battle(selectedMove);
        }
    }

    // Simulate a battle action (can be expanded to perform more detailed battle logic)
    public void Battle(MoveScriptableObject move)
    {
        if (move.moveType == MoveType.Damage)
        {
            opponentHP -= move.power; // Apply damage to the opponent
            battleMessageText.text = "You used " + move.moveName + "!";
        }
        else if (move.moveType == MoveType.Heal)
        {
            playerHP += move.power; // Heal the player
            battleMessageText.text = "You used " + move.moveName + "!";
        }

        // Update the HP UI after each move
        UpdateHPUI();

        // Check if the battle is over
        if (opponentHP <= 0)
        {
            battleMessageText.text = "You defeated the opponent!";
            // Optionally, give rewards here
        }
        else if (playerHP <= 0)
        {
            battleMessageText.text = "You lost the battle!";
            // Game over logic
        }
    }

    // Method to update the HP UI for both player and opponent
    public void UpdateHPUI()
    {
        playerHPText.text = "HP: " + playerHP.ToString(); // Update player HP
        opponentHPText.text = "HP: " + opponentHP.ToString(); // Update opponent HP
    }

    // Method to assign random moves to the player
    public void AssignRandomMoves()
    {
        bool damageMoveAssigned = false;

        for (int i = 0; i < playerMoves.Length; i++)
        {
            int randomIndex = Random.Range(0, allPossibleMoves.Length);
            playerMoves[i] = allPossibleMoves[randomIndex];

            // Ensure that there is at least one damage move
            if (playerMoves[i].moveType == MoveType.Damage)
            {
                damageMoveAssigned = true;
            }
        }

        // If no damage move is assigned, make sure to force one damage move
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

        // Update the move buttons with the new moves
        UpdateMoveButtons(playerMoves);
    }
}