using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // CHARACTER CONTROLLER
    private CharacterController charController;
    private Vector3 velocity;
    private bool isGrounded = true;

    [Header("MOVEMENT")]
    public float playerSpeed = 7f;
    public LayerMask layerGround;

    [Header("JUMP")]
    public float gravityForce = 10f;
    public float jumpHeight = 2f;

    // GROUND DETECTION PARAMETER
    public float sphereRadius = 0.2f;

    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //GRAVITY DECLARED FIRST
        velocity.y -= gravityForce * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);

        // VARS
        float moveX = Input.GetAxis("Horizontal");

        // MOVEMENT
        Movement(moveX);
     
        // CUSTOM GROUND CHECK
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, 1, 0f), sphereRadius, layerGround);

        // Keep the following, jump won't work otherwise
        if (isGrounded)
            velocity.y = 0f;

        //Debug.Log(isGrounded); //Debug

        // JUMP
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * 2f * gravityForce);
        // Mathf.Sqrt useful when you want to have more control over the jump action.

        //Debug.Log(Mathf.Sqrt(jumpHeight * 2f * gravityForce)); //Debug
    }

    private void Movement(float moveX)
    {
        Vector3 move = new Vector3(moveX, 0, 0);
        charController.Move(move * Time.deltaTime * playerSpeed);
    }
































    private void OnDrawGizmos() //Sphere preview
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 1, 0f), sphereRadius);
    }
}