using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CC : MonoBehaviour
{

    [Header("Environment")]
    [SerializeField] private float gravityScale = 1;

    [Space]
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;
    private Vector3 moveDirection;

    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpForce = 150;

    //CharaControl
    private CharacterController charController;


    [Space]
    [Header("Camera")]
    private bool lookRight = true;
    private Vector3 camTarget = new Vector3(4, 3, 2.5f);
    [SerializeField] private Vector3 camLookRight;
    [SerializeField] private Vector3 camLookLeft;
    [SerializeField] Camera cam;
    [SerializeField] private float camPosZ;
   
    [Space]
    [Header("Body model")]
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject steadycam = null;

    [Space]
    [Header("Path Following")]
    [SerializeField] private GameObject spawnPoint;
    private GameObject nextCheckpoint;
    private GameObject previousCheckpoint;
    private GameObject lastCheckpointPassed;
    private bool isInCheckpoint = false;




    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        transform.position = spawnPoint.transform.position;

        ChangeCheckpoints(spawnPoint);
    }

    // Update is called once per frame
    void Update()
    {
        // MoveAndJumpLinear();
        MoveAndJumpFollowingPath();
        CameraMovement();
        PlayerOrientation();
    }

    // Manage the movement of the drag without using checkpoint
    void MoveAndJumpLinear() {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, 0f);

        if (charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        charController.Move(moveDirection * Time.deltaTime);
    }

    // Manage the movement of the drag to follow a path
    void MoveAndJumpFollowingPath()
    {
        Vector3 moveDir = body.transform.forward * Mathf.Abs(Input.GetAxis("Horizontal")) * moveSpeed;
        moveDirection = new Vector3(moveDir.x, moveDirection.y, moveDir.z);

        if (charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        charController.Move(moveDirection * Time.deltaTime);
    }

    //Manage the travelling when change direction
    private void CameraMovement()
    {
        /* if (Input.GetAxis("Horizontal") < 0)
         {
             if (lookRight) // au moment ou l'on change de direction (tourner à gauche)
             {
                 camTarget = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, camPosZ);
                 cam.transform.localPosition = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, -cam.transform.localPosition.z);
             }

             lookRight = false;
         } else if(Input.GetAxis("Horizontal") > 0)
         {
             if (!lookRight) // au moment ou l'on change de direction (tourner à droite)
             {
                 camTarget = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, camPosZ);
                 cam.transform.localPosition = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, -cam.transform.localPosition.z);
             }
             lookRight = true;
         }*/

        if ((Input.GetAxis("Horizontal") < 0 && lookRight) || (Input.GetAxis("Horizontal") > 0 && !lookRight))
        {
            lookRight = !lookRight;

            camTarget = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, camPosZ);
            cam.transform.localPosition = new Vector3(-cam.transform.localPosition.x, cam.transform.localPosition.y, -cam.transform.localPosition.z);
        }

            float yAngle = -90;

        if (!lookRight)
        {
            yAngle += 180;
            
        }

         cam.transform.rotation = Quaternion.Euler(body.transform.eulerAngles + new Vector3(20, yAngle, 0));

        if (cam.transform.localPosition != camTarget)
        {
            if (!charController.isGrounded)
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.025f);
            else
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.05f);

        }









        /*   if ((Input.GetAxis("Horizontal") < 0 && lookRight) || (Input.GetAxis("Horizontal") > 0 && !lookRight))
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

           } */
    }

    //Refresh the checkpoints
    private void ChangeCheckpoints(GameObject checkpoint)
    {
        lastCheckpointPassed = checkpoint;
        nextCheckpoint = checkpoint.GetComponent<Checkpoint>().GetNextCheckpoint();
        previousCheckpoint = checkpoint.GetComponent<Checkpoint>().GetPreviousCheckpoint();

    }


    // When Player reach checkpoint update his chheckpoints' list
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {

            ChangeCheckpoints(other.gameObject);
            PlayerOrientation();
            isInCheckpoint = true;
        }
    }

    //Drag's out
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            isInCheckpoint = false;
        }
    }

    //Check if the drag is between the last checkpoint passed and the previous one
    private bool BetweenActualAndPreviousCheckpoint()
    {
       /* Vector3 prevCheckptPos = previousCheckpoint.transform.position;
        Vector3 playerPos = transform.position;
        Vector3 lastCheckptReachPos = lastCheckpointPassed.transform.position;*/


        Vector3 prevCheckptPos = new Vector3(previousCheckpoint.transform.position.x, 0f, previousCheckpoint.transform.position.z);
        Vector3 playerPos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 lastCheckptReachPos = new Vector3(lastCheckpointPassed.transform.position.x, 0f, lastCheckpointPassed.transform.position.z);
      


        

        return Vector3.Dot((lastCheckptReachPos - prevCheckptPos).normalized, (playerPos - lastCheckptReachPos).normalized) <= 0f && Vector3.Dot((prevCheckptPos - lastCheckptReachPos).normalized, (playerPos - prevCheckptPos).normalized) <= 0f;
    }




    // Make the drag look at her next checkpoint to reach
    private void PlayerOrientation()
    {
        Vector3 targetPrev = new Vector3(previousCheckpoint.transform.position.x, body.transform.position.y, previousCheckpoint.transform.position.z);
        Vector3 targetCheckpt = new Vector3(lastCheckpointPassed.transform.position.x, body.transform.position.y, lastCheckpointPassed.transform.position.z);
        Vector3 targetNext = new Vector3(nextCheckpoint.transform.position.x, body.transform.position.y, nextCheckpoint.transform.position.z);

        if (isInCheckpoint)
        {
            if (lookRight)
            {
                body.transform.LookAt(targetNext);
            }
            else
            {
                body.transform.LookAt(targetPrev);
                
            }
        } else
        {
            if (BetweenActualAndPreviousCheckpoint())
            {
                if (lookRight)
                {
                    body.transform.LookAt(targetCheckpt);

                }
                else
                {
                    body.transform.LookAt(targetPrev);
                }
            }
            else
            {
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
       // SmoothTravelCamera();
    }


   /* void SmoothTravelCamera()
    {
        steadycam.transform.rotation = new Quaternion(body.transform.rotation.x, body.transform.rotation.y % 180 - 90, body.transform.rotation.z, body.transform.rotation.w);
    }*/
}
