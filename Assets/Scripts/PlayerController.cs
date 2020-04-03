using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 10;
    private bool isJumping = false;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;


    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = transform.right * inputHorizontal * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        if(inputHorizontal != 0 && IsOnSlope())
        {
           // arrêter le bounce
        }


    }

    private bool IsOnSlope()
    {
        if (isJumping)
        {
            return false;
        }

        RaycastHit hit;
        float height = GetComponent<CapsuleCollider>().height;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, height/2 * slopeForceRayLength))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }
}
