using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWeapon : MonoBehaviour
{
    private bool canKill = false;
    private bool hasHit = false;

    public GameOverManager gameOverManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;
            gameOverManager.GameOver();
        }
    }

    public void StartAttack()
    {
        canKill = true;
        hasHit = false;
    }

    public void EndAttack()
    {
        canKill = false;
    }
}
