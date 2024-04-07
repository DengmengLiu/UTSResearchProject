using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigbody2D;
    public Animator ar;

    private Vector2 moveDir;
    private static bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            ProcessInputs();
        }         
    }

    void FixedUpdate()
    {
        if ( canMove)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        ar.SetFloat("Horizontal", moveDir.x);
        ar.SetFloat("Vertical", moveDir.y);
        ar.SetFloat("Speed", moveDir.sqrMagnitude);
    }

    void Move()
    {
        rigbody2D.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y *moveSpeed);
    }

    public static void DisableMovement() => canMove = false;

    public static void EnableMovement() => canMove = true;
}
