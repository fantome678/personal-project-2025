using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScriptOnlyOne : MonoBehaviour
{
    [SerializeField] DoorScript doorOpen;
    [SerializeField] DoorScript doorClose;
    [SerializeField] StateActionPlayerScript scriptIa;
    // Start is called before the first frame update
    void Start()
    {
        scriptIa = FindAnyObjectByType<StateActionPlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("doorScript");
            if (doorClose != null)
            {
                doorClose.isOpen = !doorClose.isOpen;
            }
            if (doorOpen != null)
            {
                doorOpen.isOpen = !doorOpen.isOpen;
            }
            scriptIa.ScriptFunction();
            Destroy(gameObject);
        }
    }
}
