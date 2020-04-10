using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // Movement
    [SerializeField] private float movementSpeed = 10f;

    // Slope
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    // Jump 
    [SerializeField] private KeyCode jumpkey;    
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private AnimationCurve jumpFallOff;
    bool isJumping;    

    //Camera
    private bool lookRight = true;
    private Vector3 camTarget;
    [SerializeField] private Vector3 camLookRight;
    [SerializeField] private Vector3 camLookLeft;


    [Header("Scene Objects")]
    [SerializeField] Camera cam;
    [SerializeField] private GameObject steadyStick = null;
    [SerializeField] private GameObject body = null;


    //PathFollow
    public bool needNewCheckpoints = false;
    public string checkpointName;


    private CharacterController charController;

    private GameObject nextTargetPosition;
    private GameObject lastCheckptReach;
    private GameObject previousTargetPosition;

    private Vector3 nextPosition;
    private Vector3 prevPosition;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        camTarget = camLookRight;
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


        //Stop the bouncing on slopes
        if ((horiz != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        jumpInput();
        CameraMovement();
        PlayerOrientation();
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


    //Smooth travelling when the player turn arround
    private void CameraMovement()
    {
        if ((Input.GetAxis("Horizontal") < 0 && lookRight) || (Input.GetAxis("Horizontal") > 0 && !lookRight))
        {
            lookRight = !lookRight;

            if (lookRight)
            {
                camTarget = camLookRight;

            }
            else
            {
                camTarget = camLookLeft;
            }
        }


        if (cam.transform.localPosition != camTarget)
        {
            if (!charController.isGrounded)
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.025f);
            else
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.05f);
        }
    }

    private void PlayerOrientation()
    {

        Vector3 targetPrev = new Vector3(previousTargetPosition.transform.position.x, body.transform.position.y, previousTargetPosition.transform.position.z);
        Vector3 targetCheckpt = new Vector3(lastCheckptReach.transform.position.x, body.transform.position.y, lastCheckptReach.transform.position.z);
        Vector3 targetNext = new Vector3(nextTargetPosition.transform.position.x, body.transform.position.y, nextTargetPosition.transform.position.z);

        if (BetweenActualAndLastCheckpoint())
        {
            nextPosition = targetCheckpt;
            prevPosition = targetPrev;

            if (lookRight)
            {
                body.transform.LookAt(targetCheckpt);
            } else
            {
                body.transform.LookAt(targetPrev);
            }
        } else
        {
            nextPosition = targetNext;
            prevPosition = targetCheckpt;
            if (lookRight)
            {
                body.transform.LookAt(targetNext);
            }
            else
            {
                body.transform.LookAt(targetCheckpt);
            }
        }
    }

    private bool BetweenActualAndLastCheckpoint()
    {
        Vector3 prevCheckptPos = previousTargetPosition.transform.position;
        Vector3 playerPos = transform.position;
        Vector3 lastCheckptReachPos = lastCheckptReach.transform.position;

        
        return Vector3.Dot((lastCheckptReachPos - prevCheckptPos).normalized, (playerPos - lastCheckptReachPos).normalized) < 0f && Vector3.Dot((prevCheckptPos - lastCheckptReachPos).normalized, (playerPos - prevCheckptPos).normalized) < 0f;
    }

    public void SetNewCheckpoints(GameObject prevCheckpt, GameObject lastCheckpointReach, GameObject nextCheckpt)
    {
        previousTargetPosition = prevCheckpt;
        lastCheckptReach = lastCheckpointReach;
        nextTargetPosition = nextCheckpt;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Checkpoint")
        {
            Debug.Log(other.gameObject.name);
            needNewCheckpoints = true;
            checkpointName = other.gameObject.name;
        }
    }
}
