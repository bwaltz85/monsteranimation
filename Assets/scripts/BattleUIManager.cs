using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you're using TextMeshPro

public class BattleUIManager : MonoBehaviour
{
    public Button[] moveButtons;  // Array of buttons for moves
    public TextMeshProUGUI playerHPText;  // Text for displaying player's HP
    public TextMeshProUGUI opponentHPText;  // Text for displaying opponent's HP
    public GameManager gameManager;  // Reference to the GameManager
    public Player player;  // Reference to the Player
    public Opponent opponent;  // Reference to the Opponent

    // Start is called before the first frame update
    void Start()
    {
        // Assuming that you have some setup for the move buttons
        for (int i = 0; i < moveButtons.Length; i++)
        {
            int index = i; // Local copy for closure
            moveButtons[i].onClick.AddListener(() => OnMoveButtonPressed(index)); // Add listener for move buttons
        }

        // Set initial HP texts
        UpdateHPUI();
    }

    public void UpdateMoveButtons()
    {
        // Update move buttons with player moves
        for (int i = 0; i < moveButtons.Length; i++)
        {
            if (player.playerMoves != null && player.playerMoves.Length > i)
            {
                moveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = player.playerMoves[i].moveName;
            }
        }
    }

    public void OnMoveButtonPressed(int moveIndex)
    {
        if (player.playerMoves != null && player.playerMoves.Length > moveIndex)
        {
            // Execute the selected move
            MoveScriptableObject selectedMove = player.playerMoves[moveIndex];
            player.UseMove(selectedMove); // Use the selected move via the Player script

            // Update HP after move
            UpdateHPUI();
        }
    }

    public void UpdateHPUI()
    {
        // Update the HP text for both player and opponent
        playerHPText.text = "HP: " + player.health.currentHealth;
        opponentHPText.text = "HP: " + opponent.health.currentHealth;
    }
}