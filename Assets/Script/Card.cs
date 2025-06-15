using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    [SerializeField] public DoorScript.DoorStage KeyCard;
    // Start is called before the first frame update
    void Start()
    {
        switch (KeyCard)
        {
            case DoorScript.DoorStage.zero:
                GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                break;

            case DoorScript.DoorStage.one:
                GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                break;

            case DoorScript.DoorStage.two:
                GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 20, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerScript>().KeySecurity += 1;
            Destroy(gameObject);
        }
    }
}
