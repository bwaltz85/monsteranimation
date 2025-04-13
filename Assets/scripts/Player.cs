using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 1000;
    public int attackPower = 10;
    public MoveScriptableObject[] availableMoves;
    public MoveScriptableObject[] playerMoves;

    private Animator animator;
    public HitEffect hitEffect;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AssignMoves()
    {
        playerMoves = MoveAssigner.AssignUniqueTypeMoves(availableMoves);

        for (int i = 0; i < playerMoves.Length; i++)
        {
            Debug.Log($"[ASSIGNED] Player Move Slot {i}: {playerMoves[i].moveName} ({playerMoves[i].moveType})");
        }
    }

    public void ExecuteMove(MoveScriptableObject move, Opponent opponent)
    {
        StartCoroutine(AttackSequence(move, opponent));
    }

    private IEnumerator AttackSequence(MoveScriptableObject move, Opponent opponent)
    {
        if (move.moveType == MoveType.Damage && animator != null)
            animator.SetTrigger("Attack");

        if (move.moveType == MoveType.Damage && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("attack");

        yield return new WaitForSeconds(0.4f);

        switch (move.moveType)
        {
            case MoveType.Damage:
                opponent.TakeDamage(move.power);
                break;
            case MoveType.Heal:
                health += move.power;
                break;
            case MoveType.Buff:
                attackPower += move.power;
                break;
            case MoveType.Debuff:
                opponent.ApplyDebuff(move.power);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);

        if (hitEffect != null)
            hitEffect.PlayHitEffect();
    }
}
