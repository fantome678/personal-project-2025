using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interract : MonoBehaviour
{
    bool isEnter;
    bool accessCard;
    [SerializeField] GameObject doorScript;

    // Start is called before the first frame update
    void Start()
    {
        isEnter = false;
        accessCard = false;

        LoadMaterialButton();
    }

    void LoadMaterialButton()
    {
        switch (doorScript.GetComponent<DoorScript>().doorStage)
        {
            case DoorScript.DoorStage.none:
                this.GetComponent<MeshRenderer>().material.color = Color.white;
                break;

            case DoorScript.DoorStage.zero:
                this.GetComponent<MeshRenderer>().material.color = Color.red;
                break;

                case DoorScript.DoorStage.one:
                this.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;

                case DoorScript.DoorStage.two:
                this.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEnter && accessCard == true)
        {
            doorScript.GetComponent<DoorScript>().isOpen = !doorScript.GetComponent<DoorScript>().isOpen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = true;
            if (other.gameObject.GetComponent<PlayerScript>())
            {
                if (other.gameObject.GetComponent<PlayerScript>().KeySecurity >= doorScript.GetComponent<DoorScript>().doorStage)
                {
                    accessCard = true;
                    Debug.Log("dqf");
                }
            }
            else
            {
                Debug.Log("not found");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
            accessCard = false;
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
