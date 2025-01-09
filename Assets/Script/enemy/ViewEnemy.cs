using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewEnemy : MonoBehaviour
{
    [HideInInspector] public float dis;
    [SerializeField] public float detectAngle;
    Vector3 posToTarget;
    bool see;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(dis);
        see = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool OnSee(Transform _Player)
    {
        if (Vector3.Distance(transform.position, _Player.position) < dis)
        {
            Vector3 disPlayerEnemy = _Player.position - transform.position;
            float angle = Vector3.Angle(transform.forward, disPlayerEnemy);
            if (angle < detectAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, disPlayerEnemy, out hit)) // visible physique
                {
                     Debug.DrawLine(transform.position, hit.point);
                    // Debug.Log("hit: " + hit.collider.gameObject.name);
                    // Debug.Log("player: " + _Player.gameObject.name);
                    // Debug.Log(hit.collider.gameObject);
                    if (hit.collider.gameObject.tag == _Player.gameObject.tag)
                    {
                        // Debug.Log("see Player");
                        // transform.LookAt(hit.collider.transform);
                        see = true;
                        return true;
                    }
                }
                see = false;
                return false;
            }

        }
        return false;
    }

    public void Head(int i)
    {
        if (i == -1)
        {
            transform.Rotate(new Vector3(0, -25, 0));
        }
        else if (i == 0)
        {
            transform.Rotate(new Vector3(0, 0, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, 25, 0));
        }
    }
    private void OnDrawGizmos()
    {
        if (see)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        //UnityEngine.Gizmos.DrawSphere(transform.position, dis);
        Vector3 directionLimiteRight = Quaternion.Euler(0.0f, detectAngle, 0.0f) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + directionLimiteRight * detectAngle);
        Vector3 directionLimiteLeft = Quaternion.Euler(0.0f, -detectAngle, 0.0f) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + directionLimiteLeft * detectAngle);

        Gizmos.DrawWireSphere(transform.position, detectAngle);
    }
}
