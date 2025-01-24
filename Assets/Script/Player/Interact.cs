using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interract : MonoBehaviour
{
    [SerializeField] GameObject doorScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("gsg");
            if (other.GetComponentInParent<PlayerScript>().ButtonFunction())
            {
                doorScript.GetComponent<DoorScript>().isOpen = !doorScript.GetComponent<DoorScript>().isOpen;
            }
        }
    }
}
