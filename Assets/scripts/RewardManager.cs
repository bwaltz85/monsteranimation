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

    private List<Reward> currentRewards;

    void Start()
    {
        rewardPanel.SetActive(false); // Hide on startup
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

    // ========================== Reward Pools ==========================
    Reward RandomStatBuff()
    {
        string[] buffs = { "+1 Attack", "+1 Defense", "+1 Speed", "+5 Max HP", "+10% Crit Chance", "+1 Status Resistance" };
        string selected = buffs[UnityEngine.Random.Range(0, buffs.Length)];

        return new Reward
        {
            description = "Gain " + selected,
            applyEffect = () => Debug.Log("Applied stat buff: " + selected) // Replace with real logic
        };
    }

    Reward RandomNewMove()
    {
        string[] moves = {
            "Quick Attack", "Thunder Strike", "Fire Blast",
            "Weaken", "Intimidate", "Curse",
            "Power Boost", "Strength Enhance", "Mighty Force",
            "Heal Wounds", "Rejuvenate", "Full Recovery"
        };

        string move = moves[UnityEngine.Random.Range(0, moves.Length)];

        return new Reward
        {
            description = "Learn a new move: " + move,
            applyEffect = () => Debug.Log("Learned move: " + move) // Replace with move-learn logic
        };
    }

    Reward RandomHealing()
    {
        string[] heals = { "Heal 25% HP", "Heal 50% HP", "Heal Full HP", "Shield Next Turn" };
        string selected = heals[UnityEngine.Random.Range(0, heals.Length)];

        return new Reward
        {
            description = selected,
            applyEffect = () => Debug.Log("Applied healing: " + selected) // Replace with healing logic
        };
    }
}

[System.Serializable]
public class Reward
{
    public string description;
    public Action applyEffect;
}
