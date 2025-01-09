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

    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            //door.transform.Rotate(0, 115, 0);
            //PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            other.gameObject.GetComponent<PlayerScript>().InteractFunction(door, transform);
        }
        if (other.gameObject.CompareTag("IA"))
        {
           // Debug.Log(other.gameObject.GetComponent<EnemyIA>().GetStateSee());
            if (other.gameObject.GetComponent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.lookHideOut)
            {
                skillIA.timerIACanSeeHideOut[0] += Time.deltaTime;

                if (skillIA.timerIACanSeeHideOut[0] > 6)
                {
                    agent.carving = false;
                    if (door.transform.localRotation.y == 0)
                    {
                        door.transform.Rotate(0, 115, 0);
                    }
                }
                else if (skillIA.timerIACanSeeHideOut[1] > 8)
                {
                    if (door.transform.localRotation.y != 0)
                    {
                        door.transform.Rotate(0, -115, 0);
                    }
                }
                skillIA.timerIACanSeeHideOut[1] = 0;
            }
            
            if (other.gameObject.GetComponent<EnemyIA>().GetStateSee() == EnemyIA.StateSee.see)
            {
                agent.carving = false;
                if (door.transform.localRotation.y == 0)
                {
                    door.transform.Rotate(0, 115, 0);
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
        if (other.gameObject.CompareTag("IA"))
        {
            if (door.transform.localRotation.y != 0)
            {
                door.transform.Rotate(0, -115, 0);
                skillIA.timerIACanSeeHideOut[0] = 0;
                skillIA.timerIACanSeeHideOut[1] = 0;
            }
            agent.carving = true;
        }
    }
}