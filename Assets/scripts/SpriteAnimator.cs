using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Trigger the attack animation when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Trigger the "AttackTrigger" in the Animator
            animator.SetTrigger("AttackTrigger");
        }
    }
}
