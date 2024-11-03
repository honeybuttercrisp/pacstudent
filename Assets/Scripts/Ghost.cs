using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int points = 300;
    [SerializeField] private Animator animator;
    public enum GhostState { Normal, Scared, Recovering, Dead }
    public GhostState currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = GhostState.Normal;
        animator.SetBool("IsWalking", true);
    }

    protected virtual void Slay()
    {
        Object.FindFirstObjectByType<GameManager>().PacStudentSlain();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PacStudent"))
        {
            if (currentState == GhostState.Normal)
            {
                // In Normal state, ghost "slays" PacStudent
                Slay();
            }
            else if (currentState == GhostState.Scared || currentState == GhostState.Recovering)
            {
                // In Scared or Recovering state, ghost can be "eaten"
                GetEaten();
            }
        }
    }

    private void GetEaten()
    {
        Object.FindFirstObjectByType<GameManager>().GhostEaten(this);
        SetDead();
        StartCoroutine(RespawnAfterDelay(5f));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetNormal();
    }

    public void SetScared()
    {
        currentState = GhostState.Scared;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsScared", true);
    }

    public void SetRecovering()
    {
        currentState = GhostState.Recovering;
        animator.SetBool("IsScared", false);
        animator.SetBool("IsRecovering", true);
    }

    public void SetDead()
    {
        currentState = GhostState.Dead;
        animator.SetBool("IsScared", false);
        animator.SetBool("IsRecovering", false);
        animator.SetBool("IsDead", true);
    }

    public void SetNormal()
    {
        currentState = GhostState.Normal;
        animator.SetBool("IsDead", false);
        animator.SetBool("IsRecovering", false);
        animator.SetBool("IsWalking", true);
    }
}
