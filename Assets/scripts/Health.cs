using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    // Initialize health (e.g., in Start)
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Apply damage (negative change) or healing (positive change)
    public void ApplyChange(int amount)
    {
        currentHealth += amount;

        // Clamp the value to ensure current health doesn't go below 0 or above max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
