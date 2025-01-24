using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IAHelpScript : MonoBehaviour
{

    [SerializeField] List<Transform> pos;
    public bool playerIsEnter;

    public Vector3 posToSpawnAI;

    // Start is called before the first frame update
    void Start()
    {
        playerIsEnter = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetDirection(other.transform.position);
            playerIsEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsEnter = false;
        }
    }

    void GetDirection(Vector3 direction)
    {
        //Vector3 pos = transform.position;

        //posToSpawnAI = Vector3.zero;

        Vector3 distance = (direction - transform.position);
        float dir = Vector3.Angle(transform.position, distance);
       // Debug.Log("dir" + dir);
        if (dir < 90)
        {
            posToSpawnAI = pos[1].position;
            Debug.Log("down");
        }
        else 
        {
            posToSpawnAI = pos[0].position;
            Debug.Log("up");
        }
        // posToSpawnAI = Vector3.zero;
    }

}
