using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using System.Collections.Generic;

public class PatrollerTask : Node
{
    private NavMeshAgent agentTransform;
    private GameObject player;
    EnemyIA enumEnemy;
    int index = 0;

    public PatrollerTask(NavMeshAgent agent, GameObject _player, EnemyIA _enumEnemy)
    {
        player = _player;
        agentTransform = agent;
        enumEnemy = _enumEnemy;
    }

    public override NodeState Evaluate()
    {
        if (enumEnemy.seeSomething == EnemyIA.StateSee.retreat)
        {
            ClearData("seePoint");
            ClearData("last");
            state = NodeState.FAILURE;
            return state;
        }

        if (enumEnemy.seeSomething == EnemyIA.StateSee.none && EnemyIA.skillIA != null)
        {
            ClearData("target");

            if (!enumEnemy.dataIA.GoPointSee)
            {
                if (agentTransform.remainingDistance < 0.1f)
                {
                    if (Random.Range(0, 10) > 8)
                    {
                        index = GetTacticalPatrolPointNearPlayer();
                        Debug.Log("fq");
                    }
                    else
                    {
                        index = Random.Range(0, EnemyIA.skillIA.pointPatroler.Count);
                    }
                    agentTransform.SetDestination(EnemyIA.skillIA.pointPatroler[index].position);
                    agentTransform.speed = EnemyIA.speed;
                }
            }
           /* else
            {
                if (EnemyIA.posSearch.x == 0 && EnemyIA.posSearch.y == 0 && EnemyIA.posSearch.z == 0)
                {
                    EnemyIA.posSearch = EnemyIA.skillIA.GetPlayer().position;
                    agentTransform.SetDestination(EnemyIA.posSearch);
                    agentTransform.speed = EnemyIA.speed;
                }

               if (agentTransform.remainingDistance == 0)
                {
                    Vector3 posSave = EnemyIA.posSearch;
                    Vector3 randomOffset = Random.insideUnitSphere;
                    EnemyIA.timerSearchPlayer += Time.deltaTime;
                    if (EnemyIA.timerSearchPlayer > 1.2f)
                    {
                        EnemyIA.timerSearchPlayer = 0;
                        EnemyIA.counterSearch++;
                        if (EnemyIA.counterSearch < 3)
                        {
                            randomOffset.y = 0.0f;
                            randomOffset *= 5;

                            NavMeshHit hit;
                            if (NavMesh.SamplePosition(posSave + randomOffset,
                                out hit,
                                1.0f,
                            NavMesh.AllAreas
                            ))
                            {
                                agentTransform.transform.LookAt(posSave + randomOffset);
                                agentTransform.SetDestination(posSave + randomOffset);
                            }
                        }
                        else
                        {
                            EnemyIA.posSearch = new Vector3(0, 0, 0);
                            EnemyIA.seeSomething = EnemyIA.StateSee.none;
                            EnemyIA.timerSearchPlayer = 0;
                            EnemyIA.counterSearch = 0;
                            EnemyIA.skillIA.GoPointSee = false;
                        }
                    }
                }

            }*/
        }
        state = NodeState.RUNNING;
        return state;
    }

    private int GetTacticalPatrolPointNearPlayer()
    {
        Transform playerTransform = player.transform;

        float minDist = float.MaxValue;
        int chosenIndex = 0;

        for (int i = 0; i < EnemyIA.skillIA.pointPatroler.Count; i++)
        {
            float distToPlayer = Vector3.Distance(EnemyIA.skillIA.pointPatroler[i].position, playerTransform.position);

            
            float score = distToPlayer + Random.Range(0f, 3f);

            if (score < minDist)
            {
                minDist = score;
                chosenIndex = i;
            }
        }

        return chosenIndex;
    }
}