﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Transform tr;
    public Rigidbody2D rd2D;
    BoxCollider2D boxCollider;
    public LayerMask layer = default;
    GameObject floor;
    Animator anim;
    public bool isOnLadder = false;
    public float Speed = 0;
    public float JumpHeight = 2;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rd2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        floor = GameObject.Find("FloorHolder");
    }

    bool IsOnGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.2f, layer);
        if (hit.collider != null) { anim.SetBool("IsOnGround", true); return true; }
        anim.SetBool("IsOnGround", false);
        return false;
    }

    void Update()
    {
        if (!isOnLadder)
            Move(Input.GetAxis("Horizontal"));
        else
            MoveVertical(Input.GetAxis("Vertical"));

        if (IsOnGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                {
                    Jump(false);
                }
            }

            if (Input.GetButtonDown("Crouch"))
            {
                {
                    Crouch();
                }
            }
            if (Input.GetButtonUp("Crouch"))
            {
                anim.SetBool("IsCrouching", false);
            }
        }
    }

    private void Crouch()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, layer);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Ladder")
            {
                anim.SetBool("IsOnLadder", true);
               
                
                rd2D.bodyType = RigidbodyType2D.Kinematic;
                Vector3 newPosition = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y - 2, hit.collider.transform.position.z);
                transform.position = newPosition;
                isOnLadder = true;
            }   else
            {
                anim.SetBool("IsCrouching", true);
            }

        }
    }

    public void Jump(bool forced)
    {
        Vector3 Velocity = new Vector3(rd2D.velocity.x, rd2D.velocity.y + JumpHeight, 0);
        rd2D.velocity = Velocity;
        if (!forced) anim.SetTrigger("DidJump");
    }
    private void Move(float value)
    {
        if (value != 0) {
            Vector3 Velocity = new Vector3(Speed * Input.GetAxis("Horizontal") * Time.deltaTime, rd2D.velocity.y, 0);
            rd2D.velocity = Velocity;
            anim.SetBool("IsMoving", true);

            if (value < 0)
            {
                tr.localRotation = new Quaternion(0, 180, 0, 0);
            }
            else tr.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else anim.SetBool("IsMoving", false);
    }

    private void MoveVertical(float value)
    {
        if (value != 0)
        {
            Vector3 Velocity = new Vector3(0, Speed * Input.GetAxis("Vertical") * Time.deltaTime, 0);
            rd2D.velocity = Velocity;
            anim.SetBool("IsMoving", true);
        }
        else anim.SetBool("IsMoving", false);
    }

    private void FixedUpdate()
    {
        
    }
}
