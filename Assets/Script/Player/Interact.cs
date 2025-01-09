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
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("gsg");
            if (other.gameObject.GetComponent<PlayerScript>().ButtonFunction())
            {
                doorScript.GetComponent<DoorScript>().isOpen = !doorScript.GetComponent<DoorScript>().isOpen;
            }
        }
    }
}
