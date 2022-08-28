  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    Animator animator;
    Rigidbody2D body;

    int jumpCount = 0;

    // default : Left
    int direction = -1;

    bool animationRunning;
    bool lastFrameReached;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(c_jump());
    }

    void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        if (animationRunning && lastFrameReached)
        {
            animationRunning = false;

            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
        }
    }

    void animationFinished()
    {
        lastFrameReached = true;

        if (direction < 0)
            animator.Play("IdleLeft");
        else
            animator.Play("IdleRight");

        if (Random.Range(0, 2) == 1)
           direction *= -1;
    }

    IEnumerator c_jump()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));

        animationRunning = true;
        lastFrameReached = false;

        if (direction < 0)
            animator.Play("JumpLeft");
        else 
            animator.Play("JumpRight");

        StartCoroutine(c_jump());
    }
}