using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IAHelpScript : MonoBehaviour
{

    [SerializeField] List<Transform> pos;
    // Start is called before the first frame update
    void Start()
    {

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
        }
    }

    void GetDirection(Vector3 direction)
    {
        //Vector3 pos = transform.position;
        Vector3 dir = (direction - transform.position);
        if (dir.x > -3.5f && dir.x < -3f)
        {
            Debug.Log(pos[0]);
        }
        else if (dir.x > 3f && dir.x < 3.5f)
        {
            Debug.Log(pos[1]);
        }
        else if (dir.z > 2.5f && dir.z < 2.85f)
        {
            Debug.Log(pos[2]);
        }
        else if (dir.z > -4f && dir.z < -3.5f)
        {
            Debug.Log(pos[3]);
        }
        Debug.Log(dir);
    }

}
