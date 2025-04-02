using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public bool isPlayerTurn { get; private set; } = true;

    public event Action OnPlayerTurnStart;
    public event Action OnOpponentTurnStart;

    private bool battleActive = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartBattle()
    {
        isPlayerTurn = true;
        battleActive = true;
        OnPlayerTurnStart?.Invoke();
    }

    public void EndPlayerTurn()
    {
        if (!battleActive) return;
        isPlayerTurn = false;
        OnOpponentTurnStart?.Invoke();
    }

    public void EndOpponentTurn()
    {
        if (!battleActive) return;
        isPlayerTurn = true;
        OnPlayerTurnStart?.Invoke();
    }

    public void EndBattle()
    {
        battleActive = false;
    }
}
