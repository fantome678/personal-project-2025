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
            PlayerScript tempPlayerScript = GetComponentInParent<PlayerScript>();
            if (tempPlayerScript.couldown == false && tempPlayerScript.GetIDObject() == IdObject.Smoke && tempPlayerScript.isHide == false)
            {
                GameObject temp = Instantiate(prefab, GetComponentInParent<Transform>().position, transform.rotation);

                temp.GetComponent<Rigidbody>().AddRelativeForce(0, 50, launchVelocity);
                tempPlayerScript.listSmoke.Add(temp);
                tempPlayerScript.couldown = true;
            }
        }
    }
}
