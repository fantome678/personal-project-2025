using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interract : MonoBehaviour
{
    bool isEnter;
    [SerializeField] GameObject doorScript;

    // Start is called before the first frame update
    void Start()
    {
        isEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEnter)
        {
            doorScript.GetComponent<DoorScript>().isOpen = !doorScript.GetComponent<DoorScript>().isOpen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = true;
            Debug.Log("dqf");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("gsg");
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorScript.GetComponent<DoorScript>().isOpen = !doorScript.GetComponent<DoorScript>().isOpen;
            }
        }
    }*/
}
