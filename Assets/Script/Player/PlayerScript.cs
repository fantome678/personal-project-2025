using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public enum IdObject
{
    Smoke,
    Flame,
    Detector
}


public class PlayerScript : MonoBehaviour
{
    public DoorScript.DoorStage KeySecurity;

    public bool isHide;
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    public float speed;
    public float valueMultiplySpeed;
    int add;
    public bool couldown;
    public float timerCouldown;

    int indexSave;

    IdObject indexWeapon;

    [SerializeField] public GameObject flame;
    [SerializeField] public GameObject detector;

    bool run;
    int rayonMove;
    public int counter;

    public List<GameObject> listSmoke;

    // Start is called before the first frame update
    void Start()
    {
       // KeySecurity = DoorScript.DoorStage.none;
        indexWeapon = 0;
        indexSave = 0;
        listSmoke = new List<GameObject>();
        couldown = false;
        timerCouldown = 8.0f;
        add = 0;
        run = false;
        valueMultiplySpeed = 1;
        rayonMove = 0;
        counter = 0;
        isHide = false;
    }

    void Moveplayer()
    {

        if (!isHide)
        {
           
            if (Input.GetKey(KeyCode.LeftShift))
            {
                run = true;
                add = 4;
                valueMultiplySpeed = 1.5f;
            }
            else 
            {
                run = false;
                add = 0;
                valueMultiplySpeed = 1;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                rayonMove = 5;
            }
            else 
            {
                rayonMove = 1;
            }

            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * (speed * valueMultiplySpeed) * Time.deltaTime));
        }

    }

    void ShootPlayer()
    {
        if (!Input.GetKey(KeyCode.C))
        {
            if (isHide == false)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
                {
                    indexWeapon++;
                    if (indexWeapon > (IdObject)1)
                    {
                        indexWeapon = 0;
                        indexSave = 0;
                    }
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
                {
                    indexWeapon--;
                    if (indexWeapon < 0)
                    {
                        indexWeapon = (IdObject)1;
                        indexSave = 1;
                    }
                }
                indexWeapon = (IdObject)indexSave;
            }
        }
        else 
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                indexWeapon = IdObject.Detector;
            }
            else
            {
                indexWeapon = (IdObject)indexSave;
            }
        }

        switch (indexWeapon)
        {
            case IdObject.Smoke:
                flame.SetActive(false);
                detector.SetActive(false);
                break;
            case IdObject.Flame:
                flame.SetActive(true);
                detector.SetActive(false);
                if (Input.GetAxis("Fire1") != 0)
                {
                    flame.GetComponent<Gun>().isFire = true;
                }
                else
                {
                    flame.GetComponent<Gun>().isFire = false;
                }
                break;
            case IdObject.Detector:
                flame.SetActive(false);
                detector.SetActive(true);
                break;
            default:
                break;
        }

        if (couldown)
        {
            timerCouldown -= Time.deltaTime;
            if (timerCouldown < 0f)
            {
                couldown = false;
                timerCouldown = 8.0f;
            }
            if (timerCouldown < 2.0f)
            {
                if (listSmoke.Count > 0)
                {
                    Destroy(listSmoke[0].gameObject);
                    listSmoke.RemoveAt(0);
                }
            }
        }

    }

    public IdObject GetIDObject()
    {
        return indexWeapon;
    }
    // Update is called once per frame
    void Update()
    {

        Moveplayer();
        ShootPlayer();
    }

    // show noise do player
    public Transform GetTransform(Vector3 pos)
    {
        if (!isHide)
        {
            if (Vector3.Distance(pos, transform.position) < (rayonMove + add))
            {

                return transform;
            }
        }

        return null;
    }

    public bool GetRun()
    {
        return run;
    }

    public bool IsButtonTouch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Button"))
                {
                    return true;
                }
            }
        }


        return false;
    }

    public bool isGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Room"))
                {
                    return true;
                }
            }
        }


        return false;
    }

    private bool IsHideOut()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("downHideout"))
                {
                    return true;
                }
            }
        }


        return false;
    }

    private void HideOutState(GameObject door, Transform pos)
    {
        
        if (isHide == false)
        {
            if (IsHideOut())
            {
                isHide = true;
                door.transform.Rotate(0, -115, 0);
                GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;
            }
            else
            {
                if (door.transform.localRotation.y == 0)
                {
                    door.transform.Rotate(0, 115, 0);
                }
            }
        }
        else
        {
            
            if (IsHideOut())
            {
                isHide = false;
                GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60;
                if (door.transform.localRotation.y == 0)
                {
                    door.transform.Rotate(0, 115, 0);
                }
            }
        }
    }

    public void InteractFunction(GameObject door, Transform pos)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            HideOutState(door, pos);
        }
    }

    public void TPPlayerIn(Vector3 pos)
    {
        transform.position = pos;
    }

    public bool OnSee(Transform _pos)
    {
        if (Vector3.Distance(transform.position, _pos.position) < 20)
        {
            Vector3 disPlayerEnemy = _pos.position - transform.position;
            float angle = Vector3.Angle(transform.forward, disPlayerEnemy);
            if (angle < 80)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, disPlayerEnemy, out hit)) // visible physique
                {
                    Debug.DrawLine(transform.position, hit.point);
                    if (hit.collider.gameObject.tag == _pos.gameObject.tag)
                    {
                        return true;
                    }
                }
                return false;
            }

        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, rayonMove + add);
    }
}
