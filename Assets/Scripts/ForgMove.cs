using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : MonoBehaviour
{
    public Sprite[] sprites;

    public LayerMask groundLayer;

    [SerializeField]
    private Transform onGroundChecker;

    // int currentSprite = 1;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
