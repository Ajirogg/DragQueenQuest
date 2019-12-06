using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    [SerializeField, Range(1, 50)]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float jumpStrength = 10.0f;

    private float inputHorizontal = 0.0f;

    private bool lookRight = true;
    private bool lookUp = false;
    private bool WaitlookUp = false;
    private int countCollision = 0;

    

    private Vector3 camTarget = Vector3.zero;
    [SerializeField] private float WaitTimeBeforeLookUp = 2.0f;

    [SerializeField] private Vector3 camLookRight = Vector3.zero;

    [SerializeField] private Vector3 camLookLeft = Vector3.zero;

    [SerializeField] private Vector3 camLookUp = Vector3.zero;

    private Vector3 inputSpeed;

    [Header("Scene Objects")]
    [SerializeField] Camera cam;

    [SerializeField] private GameObject steadyStick = null;

    [SerializeField] private GameObject body = null;
    

    private Rigidbody playerRB = null;

    private bool Jump = true;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        camTarget = camLookRight;
    }

    // Update is called once per frame
    void Update()
    {

        Deplacement();
        CameraMovement();

        if (Input.GetButtonDown("Jump"))
        {
            if (countCollision > 0 && Jump)
            {
                StartCoroutine(IsAllowedToJump());
                if (Mathf.Abs(inputHorizontal) > 0)
                {
                    playerRB.AddForce(new Vector3(0, 1, 0) * jumpStrength, ForceMode.Impulse);
                }
                else
                    playerRB.AddForce(new Vector3(0, 1.2f, 0) * jumpStrength, ForceMode.Impulse);
            }
        }

        if (Input.GetButtonDown("LookUp"))
        {
            WaitlookUp = true;
            StartCoroutine(WaitLookUp());
        }

        if (Input.GetButtonUp("LookUp"))
        {
            WaitlookUp = false;
            lookUp = false;
        }

        

        //Diminution de la hitbox et de la vitesse;
        if (Input.GetButton("Crouch"))
        {           
            body.transform.localScale = new Vector3(1, 1, 1);
            playerRB.transform.localScale = new Vector3(1, 0.5f, 1);
        }

        //Reattribution de la hitbox et de la vitesse;
        if (Input.GetButtonUp("Crouch"))
        {
            body.transform.localScale = new Vector3(1, 1, 1);
            playerRB.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Deplacement()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        if (countCollision > 0)
            inputSpeed = transform.right * inputHorizontal * moveSpeed;
        else
            //if ((inputHorizontal > 0 && inputSpeed.x < 0) || (inputHorizontal < 0 && inputSpeed.x > 0) || inputSpeed.x == 0)
            if ((inputHorizontal > 0 && inputSpeed.x < 0) || (inputHorizontal < 0 && inputSpeed.x > 0) || inputSpeed.x == 0)
                inputSpeed = (transform.right * inputHorizontal).normalized * moveSpeed / 3;
        

        inputSpeed = new Vector3(inputSpeed.x, playerRB.velocity.y, inputSpeed.z);


        playerRB.velocity = inputSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        countCollision++;
    }

    private void OnTriggerExit(Collider other)
    {
        countCollision--;
    }

    //Smooth travelling when the player turn arround
    private void CameraMovement()
    {
        if ((inputSpeed.x < 0 && lookRight) || (inputSpeed.x > 0 && !lookRight))
        {
            lookRight = !lookRight;

            if (lookRight)
            {
                camTarget = camLookRight;
                body.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                camTarget = camLookLeft;
                body.transform.localRotation = new Quaternion(0, 180, 0, 0);
            }
        }
        if (lookUp)
        {
            if (cam.transform.localPosition != camTarget + camLookUp)
            {
                if (countCollision == 0)
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget + camLookUp, 0.025f);
                else
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget + camLookUp, 0.1f);
            }
        } else
        {
            if (cam.transform.localPosition != camTarget)
            {
                if (countCollision == 0)
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.025f);
                else
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.05f);
            }
        }


        
    }

    private void CamLookVertical()
    {
        if(lookUp)
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget + camLookUp, 0.05f);
        else
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTarget, 0.05f);
    }

    //Disable the jump to avoid multiple jumps at once
    private IEnumerator IsAllowedToJump()
    {
        Jump = false;
        yield return new WaitForSeconds(0.5f);
        Jump = true;
    }

    private IEnumerator WaitLookUp()
    {
        yield return new WaitForSeconds(WaitTimeBeforeLookUp);
        if (WaitlookUp)
            lookUp = true;
        else
            lookUp = false;
    }

    private void LateUpdate()
    {
        steadyStick.transform.position = playerRB.transform.position;
    }
}
