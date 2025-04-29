using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public bool isPlayerTurn { get; private set; } = true;
    public bool BattleActive { get; private set; } = true;

    public event Action OnPlayerTurnStart;
    public event Action OnOpponentTurnStart;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartBattle()
    {
        Debug.Log("TurnManager: Starting battle");
        isPlayerTurn = true;
        BattleActive = true;
        OnPlayerTurnStart?.Invoke();
    }

    public void EndPlayerTurn()
    {
        Debug.Log("TurnManager: Ending player turn. BattleActive: " + BattleActive);
        if (!BattleActive) return;
        isPlayerTurn = false;
        OnOpponentTurnStart?.Invoke();
    }

    public void EndOpponentTurn()
    {
        Debug.Log("TurnManager: Ending opponent turn. BattleActive: " + BattleActive);
        if (!BattleActive) return;
        isPlayerTurn = true;
        OnPlayerTurnStart?.Invoke();
    }

    public void EndBattle()
    {
        Debug.Log("TurnManager: Ending battle");
        BattleActive = false;
    }
}