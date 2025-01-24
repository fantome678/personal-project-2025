using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class GoSeePoint : Node
{
    NavMeshAgent agent;
    PlayerScript playerScript;
    public GoSeePoint(NavMeshAgent _agent, PlayerScript _playerScript)
    {
        agent = _agent;
        playerScript = _playerScript;
    }

    public override NodeState Evaluate()
    {
        if (EnemyIA.seeSomething == EnemyIA.StateSee.none || EnemyIA.seeSomething == EnemyIA.StateSee.lookHideOut || EnemyIA.seeSomething == EnemyIA.StateSee.research)
        {
           // function with smoke
            if (GetData("seePoint") ==  null)
            {
                if (playerScript.GetTransform(agent.transform.position) != null)
                {
                    ClearData("target");
                    ClearData("last");
                    EnemyIA.seeSomething = EnemyIA.StateSee.look;
                    parent.parent.SetData("seePoint", playerScript.GetTransform(agent.transform.position).position);
                    agent.SetDestination(playerScript.GetTransform(agent.transform.position).position);
                    state = NodeState.SUCCESS;
                    return state;
                }
                if (playerScript.listSmoke.Count > 0)
                {
                    for (int i = 0; i < playerScript.listSmoke.Count; i++)
                    {
                        if (playerScript.listSmoke[i].GetComponent<SmokeScript>().GetTransform(agent.transform.position) != null)
                        {

                            ClearData("target");
                            ClearData("last");
                            EnemyIA.seeSomething = EnemyIA.StateSee.look;
                            parent.parent.SetData("seePoint", playerScript.listSmoke[i].GetComponent<SmokeScript>().GetTransform(agent.transform.position).position);
                            playerScript.listSmoke[i].GetComponent<SmokeScript>().isTouch = true;
                            state = NodeState.SUCCESS;
                            return state;
                        }
                    }
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
