using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject stonePrefab;

    public LayerMask playerLayer;

    [SerializeField]
    private Transform playerChecker;

    float range = 6f;

    // Default : left
    int direction = -1;

    bool carryingStone = true;

    Vector3 initialPosition;

    Rigidbody2D body;
    Animator animator;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<FallingBehaviour>().isFalling())
            return;

        float speed = carryingStone ? 2.5f : 4f;
        Vector3 offset = new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
        transform.Translate(offset);
        if (
            direction < 0 && transform.position.x <= initialPosition.x - range || 
            direction > 0 && transform.position.x >= initialPosition.x + range
        )
            changeDirection();
        dropStone();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            Player.kill();
    }

    void changeDirection()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void dropStone()
    {
        if (carryingStone)
            if (Physics2D.Raycast(playerChecker.position, Vector2.down, Mathf.Infinity, playerLayer))
            {
                Instantiate(stonePrefab, playerChecker.position, Quaternion.identity);
                carryingStone = false;
                animator.Play("Fly");
            }
    }

}
