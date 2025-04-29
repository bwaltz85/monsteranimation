using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI opponentHPText;
    public TextMeshProUGUI battleMessageText;
    public TextMeshProUGUI turnIndicatorText;

    public Button[] moveButtons;
    public TextMeshProUGUI[] moveButtonTexts;

    [Header("References")]
    public GameManager gameManager;

    public void UpdateMoveButtons()
    {
        for (int i = 0; i < moveButtons.Length; i++)
        {
            if (i < gameManager.player.playerMoves.Length)
            {
                moveButtonTexts[i].text = gameManager.player.playerMoves[i].moveName;

                int index = i;
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => gameManager.OnPlayerMoveSelected(index));

                moveButtons[i].gameObject.SetActive(true);
            }
            else
            {
                moveButtons[i].gameObject.SetActive(false);
            }
        }

        EnableMoveButtons(); // Ensure buttons are enabled when updated
    }

    public void UpdateHPUI()
    {
        playerHPText.text = "HP: " + gameManager.player.health;
        opponentHPText.text = "HP: " + gameManager.opponent.health;
    }

    public void DisplayMessage(string message)
    {
        battleMessageText.text = message;
    }

    public void DisplayTurn(string message)
    {
        if (turnIndicatorText != null)
        {
            turnIndicatorText.text = message;
        }
    }

    public void EnableMoveButtons()
    {
        foreach (var button in moveButtons)
        {
            if (button.gameObject.activeSelf) // Only enable buttons that are visible
            {
                button.interactable = true;
            }
        }
    }

    public void DisableMoveButtons()
    {
        foreach (var button in moveButtons)
        {
            button.interactable = false;
        }
    }
}