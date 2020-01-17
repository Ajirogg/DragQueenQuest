using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
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

    bool isJumping;


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

        PlayerMovement();
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

       /* if (transform.position.z != 0)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = 0;
            transform.position = newPosition;
        }*/


        jumpInput();
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

    private void jumpInput()
    {
        if (Input.GetKeyDown(jumpkey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }

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
    }






/*using System.Collections;
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
    */



/*private void OnDrawGizmos() //Sphere preview
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 1, 0f), sphereRadius);
    }*/
}