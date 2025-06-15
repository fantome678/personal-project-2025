using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class GoSeePoint : Node
{
    NavMeshAgent agent;
    PlayerScript playerScript;
    EnemyIA enemyIA;
    public GoSeePoint(NavMeshAgent _agent, PlayerScript _playerScript, EnemyIA _enemy)
    {
        agent = _agent;
        playerScript = _playerScript;
        enemyIA = _enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemyIA.seeSomething == EnemyIA.StateSee.none || enemyIA.seeSomething == EnemyIA.StateSee.looked || enemyIA.seeSomething == EnemyIA.StateSee.lookHideOut || enemyIA.seeSomething == EnemyIA.StateSee.research)
        {
            // function with smoke

            if (playerScript.listSmoke.Count > 0)
            {
                for (int i = 0; i < playerScript.listSmoke.Count; i++)
                {
                    if (playerScript.listSmoke[i].GetComponent<SmokeScript>().GetTransform(agent.transform.position) != null)
                    {

                        ClearData("target");
                        ClearData("last");
                        enemyIA.seeSomething = EnemyIA.StateSee.look;
                        parent.parent.SetData("seePoint", playerScript.listSmoke[i].GetComponent<SmokeScript>().GetTransform(agent.transform.position).position);
                        agent.SetDestination(playerScript.listSmoke[i].transform.position);
                        playerScript.listSmoke[i].GetComponent<SmokeScript>().isTouch = true;
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }

            if (enemyIA.seeSomething == EnemyIA.StateSee.looked)
            {
                ClearData("target");
                ClearData("last");
                enemyIA.seeSomething = EnemyIA.StateSee.look;
                parent.parent.SetData("seePoint", playerScript.transform.position);
                agent.SetDestination(playerScript.transform.position);
                state = NodeState.SUCCESS;
                return state;
            }
            else if (playerScript.GetTransform(agent.transform.position) != null)
            {
                ClearData("target");
                ClearData("last");
                enemyIA.seeSomething = EnemyIA.StateSee.look;
                parent.parent.SetData("seePoint", playerScript.GetTransform(agent.transform.position).position);
                agent.SetDestination(playerScript.GetTransform(agent.transform.position).position);
                state = NodeState.SUCCESS;
                return state;
            }
            

            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
