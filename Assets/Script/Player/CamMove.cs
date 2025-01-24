using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
   [SerializeField] Transform Transform;
    public float sensitivity = 10f;
    public float camVertical = 0;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * sensitivity;
        float inputY = Input.GetAxis("Mouse Y") * sensitivity;

        camVertical -= inputY;
        camVertical = Mathf.Clamp(camVertical, -90f, 90f);
        transform.localEulerAngles = Vector3.right * camVertical;

        Transform.Rotate(Vector3.up * inputX);
    }

}