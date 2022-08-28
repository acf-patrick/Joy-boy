using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;
    }

    IEnumerator c_disable(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    void disable(float delay)
    {
        StartCoroutine(c_disable(delay));
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject enemy = target.gameObject;

            EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour)
                enemyBehaviour.bulletHit(gameObject);

            FallingBehaviour fallingBehaviour = enemy.GetComponent<FallingBehaviour>();
            if (fallingBehaviour)
                fallingBehaviour.fall();
            
            // Bullet side
            speed = 1f;
            animator.Play("Explode");
            disable(0.25f);
        }
    }
}
