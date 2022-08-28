using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float jumpRepulsion = 5f;

    [SerializeField]
    protected float speed = 1f;

    protected GameObject player;
    protected Rigidbody2D body;
    protected Animator animator;

    public enum Direction
    {
        top, 
        bottom, 
        left, 
        right
    };

    [SerializeField]
    protected Transform[] collisionCheckers = new Transform[4];

    [SerializeField]
    protected LayerMask groundLayer, playerLayer;

    public enum State
    {
        Stomped,
        MovingLeft,
        MovingRight
    };
    protected State state = State.MovingLeft;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        checkCollisions();
        handleMove();
    }
    
    void handleMove()
    {
        switch (state)
        {
        case State.MovingLeft:
            body.velocity = new Vector2(-speed, body.velocity.y);
            break;
        case State.MovingRight:
            body.velocity = new Vector2(speed, body.velocity.y);
            break;
        case State.Stomped:
            body.velocity = new Vector2(speed, body.velocity.y);
            break;
        default : 
            break;
        }
    }

    void checkBoundaries()
    {
        if (state == State.Stomped)
            return;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        
        // Swap collision checkers due to scale change
        (collisionCheckers[(int)Direction.left], collisionCheckers[(int)Direction.right]) = (collisionCheckers[(int)Direction.right], collisionCheckers[(int)Direction.left]);

        if (state == State.MovingLeft)
            state = State.MovingRight;
        else if (state == State.MovingRight)
            state = State.MovingLeft;

        transform.localScale = scale;
    }

    // Called when the enemy has been stomped
    protected virtual void onStomp()
    {        
        gameObject.layer = LayerMask.NameToLayer("Dead");
        StartCoroutine(dying(3f));
    }

    protected IEnumerator dying(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void stomp()
    {
        state = State.Stomped;
        animator.Play("Stomped");
    }

    /* Called when collision occures */

    protected virtual void collisionTop(RaycastHit2D hit)
    {
        if (state != State.Stomped)
        {
            hit.rigidbody.velocity = new Vector2(hit.rigidbody.velocity.x, jumpRepulsion);
            speed = 0f;
            onStomp();
            stomp();
        }
    }

    // If object hit is the Player, we kill it
    protected virtual void collisionBottom(RaycastHit2D hit)
    {
        if (state == State.Stomped)
            return;
            
        // This object has no rigidBody component attached to
        if (!hit.rigidbody)
            return;

        GameObject objectHit = hit.rigidbody.gameObject;
        if (objectHit.name == "Player")
            Player.kill();
    }

    // If object hit is the Player, we kill it
    protected virtual void collisionLeft(RaycastHit2D hit)
    {
        if (state == State.Stomped)
            return;
            
        // This object has no rigidBody component attached to
        if (!hit.rigidbody)
            return;

        GameObject objectHit = hit.rigidbody.gameObject;
        if (objectHit.name == "Player")
            Player.kill();
    }

    // If object hit is the Player, we kill it
    protected virtual void collisionRight(RaycastHit2D hit)
    {
        if (state == State.Stomped)
            return;
            
        // This object has no rigidBody component attached to
        if (!hit.rigidbody)
            return;

        GameObject objectHit = hit.rigidbody.gameObject;
        if (objectHit.name == "Player")
            Player.kill();
    }

    /* End of definitions */

    protected void checkCollisions()
    {
        RaycastHit2D hit;
        
        hit = Physics2D.Raycast(collisionCheckers[(int)Direction.top].position, Vector2.up, 0.1f, playerLayer);
        if (hit)
            collisionTop(hit);
        
        hit = Physics2D.Raycast(collisionCheckers[(int)Direction.left].position, Vector2.left, 0.1f, playerLayer);
        if (hit)
            collisionLeft(hit);

        hit = Physics2D.Raycast(collisionCheckers[(int)Direction.right].position, Vector2.right, 0.1f, playerLayer);
        if (hit)
            collisionRight(hit);

        hit = Physics2D.Raycast(collisionCheckers[(int)Direction.bottom].position, Vector2.down, 0.1f, groundLayer);
        if (hit)
            collisionBottom(hit);
        else
            checkBoundaries();
    }

    public virtual void bulletHit(GameObject bullet)
    {
        stomp();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        gameObject.layer = LayerMask.NameToLayer("Dying");

        float bulletSpeed = bullet.GetComponent<Bullet>().speed;
        body.velocity = new Vector2(bulletSpeed / Mathf.Abs(bulletSpeed), 3f );
    }
}
