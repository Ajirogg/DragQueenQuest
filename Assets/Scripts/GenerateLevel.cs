using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{

    [SerializeField] private GameObject[] checkpoints;
    [Space]
    [SerializeField] private GameObject pointBeforeSpawn;
    [SerializeField] private GameObject pointAfterEnd;

    private GameObject spawnPoint;

    [SerializeField] private GameObject player;
    private PlayerController2D playerController;

    private int lastIndex;

    // Start is called before the first frame update
    void Start()
    {
        lastIndex = checkpoints.Length - 1;
        spawnPoint = checkpoints[0];
        player.transform.position = spawnPoint.transform.position;
        playerController = player.GetComponentInChildren<PlayerController2D>();
        playerController.SetNewCheckpoints(pointBeforeSpawn, spawnPoint, checkpoints[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.needNewCheckpoints)
        {
            int index = findCheckpoint(playerController.checkpointName);
            SetNewCheckpoint(index);

            playerController.needNewCheckpoints = false;
            
        }
    }

    private int findCheckpoint(string name)
    {
        for(int i = 0; i < checkpoints.Length; i++)
        {
            if(checkpoints[i].name == name)
            {
                return i;
            }
        }

        return -1;
    }

    private void SetNewCheckpoint(int index)
    {
        if (index == -1)
        {
            Debug.LogError("CHECKPOINT NOT FIND");
        }
        else
        {
            if (index == 0)
            {
                playerController.SetNewCheckpoints(pointBeforeSpawn, spawnPoint, checkpoints[1]);
            }
            else if (index == lastIndex)
            {
                playerController.SetNewCheckpoints(checkpoints[index - 1], checkpoints[index], pointAfterEnd);
            }
            else
            {
                playerController.SetNewCheckpoints(checkpoints[index - 1], checkpoints[index], checkpoints[index + 1]);
            }
        }
    }
}
