using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D_CC : MonoBehaviour
{

    private CharacterController charaControl;
    private float inputH = 0f;
    private float gravForce = 40000f;
    private float moveSpeed = 10f;
    private float jumpSpeed = 40f;

    ///TEST AREA\\\

    private bool canJump = false;


    private Vector3 moveDirection = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        charaControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        inputH = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(inputH, 0, 0) * moveSpeed;

        if (charaControl.isGrounded)
        {
            canJump = true;
            gravForce = 40000000f;
        }




        if (charaControl.isGrounded && Input.GetButtonDown("Jump"))
        {
            canJump = false;
            gravForce = 40f;
            moveDirection.y = jumpSpeed;
            Debug.Log("JUMP");
        }

        moveDirection.y -= gravForce * Time.deltaTime * 2f;

        charaControl.Move(moveDirection * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
