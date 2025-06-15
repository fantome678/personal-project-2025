using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideOutScript : MonoBehaviour
{
    [SerializeField] public NavMeshObstacle agent;
    [SerializeField] GameObject door;
    [SerializeField] public GameObject pos;
    StateActionPlayerScript skillIA;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshObstacle>();
        skillIA = GameObject.FindAnyObjectByType<StateActionPlayerScript>();
       // door.transform.Rotate(0, -115, 0);
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerScript>().InteractFunction(door, transform);
        }
        if (other.GetComponentInParent<EnemyIA>())
        {
            if (other.GetComponentInParent<EnemyIA>().CompareTag("IA"))
            {
                other.GetComponentInParent<EnemyIA>().dataIA.timerIACanSeeHideOut += Time.deltaTime;
                if (other.gameObject.GetComponentInParent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.lookHideOut)
                {
                    if (other.GetComponentInParent<EnemyIA>().dataIA.timerIACanSeeHideOut > 6)
                    {
                        agent.carving = false;
                        if (door.transform.localRotation.y == 0)
                        {
                            door.transform.Rotate(0, 115, 0);
                        }
                    }
                    else if (other.GetComponentInParent<EnemyIA>().dataIA.timerIACanSeeHideOut > 8 && other.gameObject.GetComponentInParent<EnemyIA>().GetStateSee() != EnemyIA.StateSee.see)
                    {
                        skillIA.waiting = false;
                        if (door.transform.localRotation.y != 0)
                        {
                            door.transform.Rotate(0, -115, 0);
                        }
                        skillIA.timerIACanSeeHideOutGeneral = 0;
                    }
                }

                if (other.gameObject.GetComponentInParent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.see)
                {
                    agent.carving = false;
                    if (door.transform.localRotation.y == 0)
                    {
                        door.transform.Rotate(0, 115, 0);
                    }
                    other.gameObject.GetComponentInParent<EnemyIA>().agent.SetDestination(other.gameObject.GetComponentInParent<EnemyIA>().Player.transform.position);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (door.transform.localRotation.y != 0)
            {
                door.transform.Rotate(0, -115, 0);
            }
            agent.carving = true;
        }
        if (other.GetComponentInParent<EnemyIA>())
        {
            if (other.GetComponentInParent<EnemyIA>().CompareTag("IA"))
            {
                if (door.transform.localRotation.y != 0)
                {
                    door.transform.Rotate(0, -115, 0);
                    other.GetComponentInParent<EnemyIA>().dataIA.timerIACanSeeHideOut = 0;
                }
                agent.carving = true;
                other.GetComponentInParent<EnemyIA>().dataIA.timerIACanSeeHideOut = 0;
            }
        }
    }
}