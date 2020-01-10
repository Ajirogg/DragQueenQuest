using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;
    public Vector3 offset;

    private void Update()
    {
        Vector3 newPosition = Target.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition + offset, FollowSpeed * Time.deltaTime);
    }
}
