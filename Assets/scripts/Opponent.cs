using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Health health;  // Reference to the opponent's health component
    public int attackPower;  // The opponent's attack power
    public int defensePower; // The opponent's defense power

    void Start()
    {
        health = GetComponent<Health>();  // Assuming Health is a component attached to the opponent
    }

    // Method to apply damage to the opponent
    public void TakeDamage(int damage)
    {
        // Reduce opponent's health by the damage amount
        health.ApplyChange(damage);  // Apply damage (negative amount)
    }

    // Method to apply a debuff to the opponent's stats (e.g., reduce attack power)
    public void ApplyDebuff(int debuffPower)
    {
        attackPower -= debuffPower;

        // Optionally, clamp the values to prevent going below 0
        attackPower = Mathf.Max(attackPower, 0);
    }

    // Other opponent-related functionality...
}
