using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSmoke : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public float launchVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            if (GetComponentInParent<PlayerScript>().couldown == false && GetComponentInParent<PlayerScript>().flame.activeSelf == false)
            {
                GameObject temp = Instantiate(prefab, GetComponentInParent<Transform>().position, transform.rotation);

                temp.GetComponent<Rigidbody>().AddRelativeForce(0, 50, launchVelocity);
                GetComponentInParent<PlayerScript>().listSmoke.Add(temp);
                GetComponentInParent<PlayerScript>().couldown = true;
            }
        }
    }
}
