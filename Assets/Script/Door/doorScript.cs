using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorScript : MonoBehaviour
{
    public enum DoorStage
    {
        none,
        zero,
        one, 
        two
    }

    [SerializeField] float speed;
    [SerializeField] GameObject triggerPursuit;
    [SerializeField] GameObject triggerScript;
    [SerializeField] public DoorStage doorStage;
    public bool isOpen;
    Vector3 posBegin;
    Vector3 posEnd;
    bool UniqueOpen;
    public float dist;
    // Start is called before the first frame update
    void Start()
    {
        posBegin = transform.position;
        // Debug.Log(posBegin);
        posEnd = new Vector3(transform.position.x, transform.position.y + dist, transform.position.z);
        // Debug.Log(posEnd);
        if (isOpen)
        {
            transform.position = posEnd;
        }
        else
        {
            transform.position = posBegin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //if (doorStage == DoorStage.none)
      //  {
            DoorAction();
      //  }
    }

    void DoorAction()
    {
        if (UniqueOpen == false)
        {
            if (isOpen)
            {
                if (transform.position.y < posEnd.y)
                {
                    transform.Translate(0, speed * Time.deltaTime, 0);
                    Debug.Log("speed");
                }

                if (triggerPursuit != null)
                {
                    triggerPursuit.SetActive(true);
                }
            }
            else
            {
                if (transform.position.y > posBegin.y)
                {
                    transform.Translate(0, -speed * Time.deltaTime, 0);
                }

                if (triggerPursuit != null)
                {
                    triggerPursuit.SetActive(false);
                }
            }
        }
    }
}
