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
                if (other.gameObject.GetComponentInParent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.lookHideOut)
                {
                    EnemyIA.skillIA.timerIACanSeeHideOut[0] += Time.deltaTime;

                    if (EnemyIA.skillIA.timerIACanSeeHideOut[0] > 6)
                    {
                        agent.carving = false;
                        if (door.transform.localRotation.y == 0)
                        {
                            door.transform.Rotate(0, 115, 0);
                        }
                    }
                    else if (EnemyIA.skillIA.timerIACanSeeHideOut[1] > 8)
                    {
                        if (door.transform.localRotation.y != 0)
                        {
                            door.transform.Rotate(0, -115, 0);
                        }
                    }
                    EnemyIA.skillIA.timerIACanSeeHideOut[1] = 0;
                }

                if (other.gameObject.GetComponentInParent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.see)
                {
                    agent.carving = false;
                    if (door.transform.localRotation.y == 0)
                    {
                        door.transform.Rotate(0, 115, 0);
                    }
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
                    EnemyIA.skillIA.timerIACanSeeHideOut[0] = 0;
                    EnemyIA.skillIA.timerIACanSeeHideOut[1] = 0;
                }
                agent.carving = true;
            }
        }
    }
}