using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    public LayerMask groundLayer;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private Transform groundChecker;
    
    Vector2 direction = Vector3.down;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine("c_changeDirection");
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<FallingBehaviour>().isFalling())
        {
            StopCoroutine("c_changeDirection");
            return;
        }
        
        if (Physics2D.Raycast(groundChecker.position, Vector2.down, 0.1f, groundLayer))
            direction = Vector3.up;

        transform.Translate(speed * Time.deltaTime * direction);
    }

    IEnumerator c_changeDirection()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        direction *= -1;
        StartCoroutine("c_changeDirection");
    }
}
