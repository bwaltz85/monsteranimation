using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardPanel;
    public Button[] rewardButtons;
    public TextMeshProUGUI[] rewardTexts;
    public MoveManager moveManager; // Set in Inspector
    public Player playerRef;        // Set in Inspector

    private List<Reward> currentRewards;

    void Start()
    {
        rewardPanel.SetActive(false);
    }

    public void ShowRewards()
    {
        rewardPanel.SetActive(true);
        currentRewards = GenerateRewards();

        for (int i = 0; i < rewardButtons.Length; i++)
        {
            int index = i;
            rewardTexts[i].text = currentRewards[i].description;
            rewardButtons[i].onClick.RemoveAllListeners();
            rewardButtons[i].onClick.AddListener(() => ChooseReward(index));
        }
    }

    void ChooseReward(int index)
    {
        currentRewards[index].applyEffect?.Invoke();
        rewardPanel.SetActive(false);
    }

    List<Reward> GenerateRewards()
    {
        List<Reward> rewards = new List<Reward>();

        while (rewards.Count < 3)
        {
            int roll = UnityEngine.Random.Range(0, 3); // 0 = Stat, 1 = Move, 2 = Heal

            switch (roll)
            {
                case 0: rewards.Add(RandomStatBuff()); break;
                case 1: rewards.Add(RandomNewMove()); break;
                case 2: rewards.Add(RandomHealing()); break;
            }
        }

        return rewards;
    }

    // ========================== Reward Options ==========================

    Reward RandomStatBuff()
    {
        string[] buffs = { "+1 Attack", "+1 Defense", "+1 Speed", "+5 Max HP" };
        string selected = buffs[UnityEngine.Random.Range(0, buffs.Length)];

        return new Reward
        {
            description = "Gain " + selected,
            applyEffect = () => Debug.Log("Applied stat buff: " + selected) // Replace with real stat buff logic
        };
    }

    Reward RandomNewMove()
    {
        MoveScriptableObject move = moveManager.GetRandomMove();

        return new Reward
        {
            description = $"Learn a new move: <color=yellow>{move.moveName}</color>",
            applyEffect = () => moveManager.StartMoveReplacement(move, playerRef)
        };
    }

    Reward RandomHealing()
    {
        string[] heals = { "Heal 25%", "Heal 50%", "Full Heal" };
        string selected = heals[UnityEngine.Random.Range(0, heals.Length)];

        return new Reward
        {
            description = selected,
            applyEffect = () => Debug.Log("Healed player: " + selected) // Replace with playerRef.Heal()
        };
    }
}

[System.Serializable]
public class Reward
{
    public string description;
    public Action applyEffect;
}
