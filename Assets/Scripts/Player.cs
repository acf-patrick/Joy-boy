using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float jumpPower = 5f;

    [SerializeField]
    private Transform onGroundChecker;

    public GameObject bulletPrefab;

    public LayerMask groundLayer;

    private bool jumping = false;
    private bool onGround = false;

    /* Components */

    private Rigidbody2D body;
    private Animator animator;

    public static void kill()
    {
        GameObject player = GameObject.Find("Player");
        if (player)
        {
            print ("Dead");
        }
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            shot();
            
        checkIfOnGround();
    }

    void FixedUpdate()
    {
        handleWalk();
        handleJump();
    }

    // Shot bullet
    void shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        float scaleX = transform.localScale.x;
        bullet.GetComponent<Bullet>().speed *= scaleX / Mathf.Abs(scaleX);
    }

    void handleWalk()
    {
        int offset = (int)Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(offset * speed, body.velocity.y);
        
        // Switch animation
        animator.SetInteger("Direction", offset);

        // Fix player direction
        if (offset != 0)
            transform.localScale = new Vector3(offset, 1f, 1f);
    }

    void checkIfOnGround()
    {
        if (Physics2D.Raycast(onGroundChecker.position, Vector2.down, 0.1f, groundLayer))
        {
            onGround = true;
            if (jumping)
            {
                jumping = false;
                animator.SetBool("Jumping", false);
            }
        }
        else
            onGround = false;
    }

    void handleJump()
    {
        if (onGround && Input.GetKey(KeyCode.Space))
        {
            jumping = true;
            animator.SetBool("Jumping", true);
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
    }
}
