using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorProxy : MonoBehaviour
{
    public ExampleCharacterController CharacterController;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("ForwardSpeed", CharacterController.NormalizedForwardSpeed);
        _animator.SetBool("IsGrounded", CharacterController.IsGrounded);
        _animator.SetBool("JumpRequested", CharacterController.JumpRequested);

    }
}

