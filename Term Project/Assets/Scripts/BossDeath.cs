using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossDeath : MonoBehaviour
{
    public Animator animator;
    public GameObject winScreen;
    public GameObject player;

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
            agent.isStopped = true;

        if (animator != null)
            animator.SetTrigger("die");

        Invoke(nameof(ShowWinScreen), 2f);
    }

    void ShowWinScreen()
    {
        winScreen.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (player != null)
        {
            player.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}