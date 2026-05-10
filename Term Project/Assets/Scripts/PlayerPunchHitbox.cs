using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchHitbox : MonoBehaviour
{
    private bool canPunch = false;

    private void OnTriggerEnter(Collider other)
    {   
        Debug.Log("Enter");
        TryHit(other);
    }

    private void OnTriggerStay(Collider other)
    {   
        Debug.Log("Stay");
        TryHit(other);
    }

    private void TryHit(Collider other)
    {   
        if (!canPunch) return;
        
        Debug.Log("truePunch");
        if (other.transform.root == transform.root)
            return;

        GuardDeath guard = other.GetComponentInParent<GuardDeath>();

        if (guard != null)
        {
            guard.Die();
        }
        BossDeath boss = other.GetComponentInParent<BossDeath>();
        if (boss != null)
        {
            boss.Die();
            return;
        }
    }
    public void StartPunch()
    {
        canPunch = true;
    }

    public void EndPunch()
    {
        canPunch = false;
    }
}
