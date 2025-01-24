using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject trigger;
    public bool isOpen;
    Vector3 posBegin;
    Vector3 posEnd;
    // Start is called before the first frame update
    void Start()
    {
        posBegin = transform.position;
        // Debug.Log(posBegin);
        posEnd = new Vector3(transform.position.x, transform.position.y + 8.0f, transform.position.z);
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
        if (isOpen)
        {
            if (transform.position.y < posEnd.y)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);

            }
           // GetComponent<NavMeshObstacle>().carving = false;
            if (trigger != null)
            {
                trigger.SetActive(true);
            }
        }
        else
        {
            if (transform.position.y > posBegin.y)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);

            }
            //GetComponent<NavMeshObstacle>().carving = true;
            if (trigger != null)
            {
                trigger.SetActive(false);
            }
        }
    }
}
