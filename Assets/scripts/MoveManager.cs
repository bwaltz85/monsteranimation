using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance { get; private set; }

    public List<MoveScriptableObject> allMoves = new List<MoveScriptableObject>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllMoves();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAllMoves()
    {
        // Load all move assets in the Resources/Moves folder
        MoveScriptableObject[] loadedMoves = Resources.LoadAll<MoveScriptableObject>("Moves");
        allMoves.AddRange(loadedMoves);
    }

    public MoveScriptableObject GetRandomMove()
    {
        if (allMoves.Count == 0)
        {
            Debug.LogWarning("No moves available in MoveManager.");
            return null;
        }

        return allMoves[Random.Range(0, allMoves.Count)];
    }

    public MoveScriptableObject GetRandomMoveOfType(MoveType type)
    {
        List<MoveScriptableObject> filtered = allMoves.FindAll(m => m.moveType == type);
        if (filtered.Count == 0)
        {
            Debug.LogWarning("No moves of type: " + type);
            return null;
        }

        return filtered[Random.Range(0, filtered.Count)];
    }

    ///////////////// Reward Integration ///////////////////////

    [Header("Replacement UI")]
    public GameObject replacePromptPanel;
    public TMP_Text promptText;
    public Button[] moveButtons;

    private MoveScriptableObject moveToAdd;
    private Player playerRef;

    public void StartMoveReplacement(MoveScriptableObject newMove, Player player)
    {
        moveToAdd = newMove;
        playerRef = player;

        if (promptText != null)
            promptText.text = $"Select a move to replace with <color=yellow>{moveToAdd.moveName}</color>";

        if (replacePromptPanel != null)
            replacePromptPanel.SetActive(true);

        for (int i = 0; i < moveButtons.Length; i++)
        {
            int index = i;
            moveButtons[i].onClick.RemoveAllListeners();
            moveButtons[i].onClick.AddListener(() => ReplaceMove(index));
        }

        UpdateMoveButtonsUI(); // Call this here to make sure button labels are set
    }

    private void ReplaceMove(int index)
    {
        if (playerRef != null && moveToAdd != null)
        {
            if (index >= 0 && index < playerRef.playerMoves.Length)
            {
                playerRef.playerMoves[index] = moveToAdd;
                Debug.Log($"Replaced move at index {index} with {moveToAdd.moveName}");
            }
        }

        UpdateMoveButtonsUI();

        if (replacePromptPanel != null)
            replacePromptPanel.SetActive(false);
    }

    private void UpdateMoveButtonsUI()
    {
        if (playerRef == null || playerRef.playerMoves == null) return;

        for (int i = 0; i < moveButtons.Length; i++)
        {
            if (i < playerRef.playerMoves.Length && moveButtons[i] != null)
            {
                TMP_Text buttonText = moveButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                    buttonText.text = playerRef.playerMoves[i].moveName;
            }
        }
    }
}
