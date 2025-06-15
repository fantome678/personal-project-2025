using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LookHideOut : Node
{
    NavMeshAgent _Agent;
    List<UnityEngine.GameObject> list;
    GameObject player;
    EnemyIA enemyIA;
    public LookHideOut(NavMeshAgent agent, List<UnityEngine.GameObject> _List, GameObject _Player, EnemyIA _EnemyIA)
    {
        _Agent = agent;
        list = _List;
        enemyIA = _EnemyIA;
        player = _Player;
    }

    public override NodeState Evaluate()
    {
        if (enemyIA.seeSomething == EnemyIA.StateSee.none)
        {
            if (enemyIA.dataIA.isPursuit)
            {
                int rand;

                if (list.Count > 0)
                {
                    float minDistance = 10f;

                    List<GameObject> findHideOuts = new List<GameObject>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        Vector3 pos = list[i].transform.position;
                        float dis = Vector3.Distance(player.transform.position, pos);

                        if (dis < minDistance)
                        {
                            findHideOuts.Add(list[i]);
                        }

                    }

                    if (findHideOuts.Count > 0)
                    {
                        rand = Random.Range(0, findHideOuts.Count);
                        _Agent.SetDestination(findHideOuts[rand].GetComponent<HideOutScript>().pos.transform.position);
                        parent.parent.SetData("HideOut", findHideOuts[rand].transform);
                        enemyIA.seeSomething = EnemyIA.StateSee.lookHideOut;
                        enemyIA.dataIA.isPursuit = false;
                        state = NodeState.SUCCESS;
                        return state;
                    }

                    rand = Random.Range(0, list.Count);
                    _Agent.SetDestination(list[rand].GetComponent<HideOutScript>().pos.transform.position);

                    parent.parent.SetData("HideOut", list[rand].transform);
                    enemyIA.seeSomething = EnemyIA.StateSee.lookHideOut;
                    enemyIA.dataIA.isPursuit = false;
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }

        }

        state = NodeState.SUCCESS;
        return state;
    }
}
