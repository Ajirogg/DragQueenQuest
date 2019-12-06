using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    CharacterController cc;

    public float speed = 6f;
    public float gForce = 20f;
    public float jumpSpeed = 10f;

   public bool simpleMove = true;

    private float xSpeed;
    private float ySpeed;
    private float movementSpeed;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float side = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(side, 0, 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if (cc.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        if (Input.GetKey("a"))
            simpleMove = !simpleMove;

        moveDirection.y -= gForce * Time.fixedDeltaTime;

        if(simpleMove)
            cc.SimpleMove(moveDirection * Time.fixedDeltaTime);
        else
            cc.Move(moveDirection * Time.fixedDeltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        Debug.Log(Time.deltaTime);
    }
}