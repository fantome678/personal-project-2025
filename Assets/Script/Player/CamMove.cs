using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public float maxYAngle = 80f;
    public float sensitivity = 10f;
    public float currentRotationY;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotationY = Mathf.Clamp(currentRotationY, -maxYAngle, maxYAngle);
       // transform.rotation = Quaternion.Euler(currentRotationY, transform.rotation.x, 0);
    }

}