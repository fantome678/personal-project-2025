using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CheckRoom : Node
{
    NavMeshAgent _agent;
    ViewEnemy _ViewEnemy;
    

    public CheckRoom(NavMeshAgent agent, ViewEnemy viewEnemy)
    {
        _agent = agent;
        _ViewEnemy = viewEnemy;
    }

    public override NodeState Evaluate()
    {
        Collider[] collider = Physics.OverlapSphere(_agent.transform.position, EnemyIA.FOV, ~3);
        if (collider.Length > 0)
        {
            if (_ViewEnemy.OnSee(collider[0].transform))
            {
                EnemyIA.seeSomething = EnemyIA.StateSee.none;
                ClearData("seePoint");
                ClearData("target");
                ClearData("last");

                state = NodeState.FAILURE;
                return state;
            }
        }
        if (EnemyIA.seeSomething == EnemyIA.StateSee.look)
        {
            Vector3 posSave = (Vector3)GetData("seePoint");

            Debug.Log("in look");
            Vector3 randomOffset = Random.insideUnitSphere;
            EnemyIA.timerCheckRoom += Time.deltaTime;
            if (EnemyIA.timerCheckRoom > 2)
            {
                EnemyIA.timerCheckRoom = 0;
                EnemyIA.counter++;
                if (EnemyIA.counter < 4)
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
                        _agent.transform.LookAt(posSave + randomOffset);
                        _agent.SetDestination(posSave + randomOffset);
                    }
                }
                else
                {
                    EnemyIA.seeSomething = EnemyIA.StateSee.none;
                    EnemyIA.timerCheckRoom = 0;
                    EnemyIA.counter = 0;
                    ClearData("seePoint");
                }
            }


        }
        state = NodeState.RUNNING;
        return state;
    }
}
