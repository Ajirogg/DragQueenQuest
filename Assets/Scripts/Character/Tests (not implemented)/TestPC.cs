using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPC : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private KeyCode jumpkey;
    [SerializeField]
    private AnimationCurve jumpFallOff;
    [SerializeField]
    private float jumpMultiplier;
    [SerializeField]
    private float slopeForce;
    [SerializeField]
    private float slopeForceRayLength;

    private Vector3 velocity;

    [SerializeField]
    private float gravityForce = 20f;

    [SerializeField]
    private float fallMultiplier = 10f;

    [SerializeField]
    private float lowJumpMultiplier = 10f;

    bool isJumping;
    bool Jumping;


    private CharacterController charController;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(charController.isGrounded);

        PlayerMovement();

        if (velocity.y < 0)
        {
            velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetKeyDown(jumpkey))
        {
            velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }


    }

    private void PlayerMovement()
    {
        float horiz = Input.GetAxis("Horizontal") * movementSpeed;

        Vector3 rightMovement = transform.right * horiz;

        charController.SimpleMove(rightMovement);
        //charController.SimpleMove(Vector3.ClampMagnitude(rightMovement, 1.0f) * movementSpeed);


        //Stop the bouncing on slopes
        if ((horiz != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        /*if (transform.position.z != 0)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = 0;
            transform.position = newPosition;
        }*/

        Jump();


        //jumpInput();
    }

    //Detect if player is on a slope
    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    /*private void jumpInput()
    {
        if (Input.GetKeyDown(jumpkey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
        Debug.Log(isJumping); //Debug
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;

            yield return null;

        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }*/

    void Jump()
    {

        Debug.Log("Input.GetKeyDown(jumpkey) = " + Input.GetKeyDown(jumpkey));
        Debug.Log("isJumping = " + isJumping);

        if (Input.GetKeyDown(jumpkey) && !isJumping)
        {
            Debug.Log("jump && !isJumping");
            velocity = jumpMultiplier * Vector3.up;
            Jumping = true;
        }

        if (Input.GetKeyDown(jumpkey) && isJumping)
        {
            Debug.Log("jump && isJumping");
            isJumping = true;
        }
        else
        {
            Debug.Log("Default");
            isJumping = false;
        }
    }
}