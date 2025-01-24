using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{
    public float dis;
    public float timer;
    bool onGround;
    public bool isTouch;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        onGround = false;
        isTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround)
        {
            timer += Time.deltaTime;
            
        }
    }

    public Transform GetTransform(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) < dis && isTouch == false)
        {
            return transform;
        }


        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, dis);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Corridor") || collision.gameObject.CompareTag("Room"))
        {
            onGround = true;
        }
    }

}
