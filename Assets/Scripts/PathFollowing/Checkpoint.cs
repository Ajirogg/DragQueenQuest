using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject previousCheckpoint;
    [SerializeField] private GameObject nextCheckpoint;


    void Start()
    {
        
    }

    public GameObject GetPreviousCheckpoint()
    {
        return previousCheckpoint;
    }

    public GameObject GetNextCheckpoint()
    {
        return nextCheckpoint;
    }
}
