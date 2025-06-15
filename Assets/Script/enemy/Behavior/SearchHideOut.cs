using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SearchHideOut : Node
{
    NavMeshAgent agent;
    ViewEnemy script;
    GameObject player;
    EnemyIA enemyIA;
    public SearchHideOut(NavMeshAgent _agent, ViewEnemy _script, GameObject _player, EnemyIA _EnemyIA)
    {
        agent = _agent;
        script = _script;
        player = _player;
        enemyIA = _EnemyIA;
    }

    public override NodeState Evaluate()
    {
        if (script.OnSee(player.transform))
        {
            enemyIA.seeSomething = EnemyIA.StateSee.none;
            Debug.Log("see");
            ClearData("last");
            ClearData("target");
            ClearData("HideOut");
            state = NodeState.FAILURE;
            return state;
        }
        if (enemyIA.seeSomething == EnemyIA.StateSee.lookHideOut)
        {
            ClearData("last");
            ClearData("target");

            if (agent.remainingDistance == 0)
            {

                Vector3 target = ((Transform)GetData("HideOut")).forward;

                agent.transform.eulerAngles = new Vector3(0, ((Transform)GetData("HideOut")).eulerAngles.y + 180.0f, 0);

                enemyIA.dataIA.timerIACanSeeHideOut += Time.deltaTime;

                if (enemyIA.dataIA.timerIACanSeeHideOut > 8)
                {
                    enemyIA.seeSomething = EnemyIA.StateSee.none;
                    ClearData("last");
                    ClearData("target");
                    ClearData("HideOut");
                    state = NodeState.FAILURE;
                    return state;
                }
            }

        }
        state = NodeState.RUNNING;
        return state;
    }
}
