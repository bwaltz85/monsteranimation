using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI opponentHPText;
    public TextMeshProUGUI battleMessageText;

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
                int index = i; // capture index for the lambda
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => gameManager.OnPlayerMoveSelected(index));
                moveButtons[i].gameObject.SetActive(true);
            }
            else
            {
                moveButtons[i].gameObject.SetActive(false);
            }
        }
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
}
