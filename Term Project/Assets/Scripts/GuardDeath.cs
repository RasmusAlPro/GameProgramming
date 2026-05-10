using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardDeath : MonoBehaviour
{
    public Animator animator;

    private bool dead = false;

    public void Die()
    {
        if (dead) return;
        dead = true;

        AIController ai = GetComponent<AIController>();
        if (ai != null)
            ai.enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        Destroy(gameObject, 3f);
    }
}