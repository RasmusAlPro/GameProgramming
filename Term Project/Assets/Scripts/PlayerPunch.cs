using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchInput : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("punch");
        }
    }
}