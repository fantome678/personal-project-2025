using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCollidingHideOut : MonoBehaviour
{
    public bool doorIsOpen;
    // Start is called before the first frame update
    void Start()
    {
        doorIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorIsOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorIsOpen = false;
        }
    }
}
