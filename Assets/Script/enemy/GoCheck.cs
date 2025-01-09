using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class GoCheck : Node
{
    NavMeshAgent _agent;
    ViewEnemy _Script;
    GameObject _Player;
    public GoCheck(NavMeshAgent agent, ViewEnemy script, GameObject player)
    {
        _agent = agent;
        _Script = script;
        _Player = player;
    }

    public override NodeState Evaluate()
    {

        if (_Script.OnSee(_Player.transform))
        {
            EnemyIA.seeSomething = EnemyIA.StateSee.none;
            ClearData("seePoint");
            ClearData("target");
            ClearData("last");

            state = NodeState.FAILURE;
            return state;
        }

        if (EnemyIA.seeSomething == EnemyIA.StateSee.find)
        {

            if (GetData("seePoint") != null)
            {
                Debug.Log("in find");
                _agent.SetDestination((Vector3)GetData("seePoint"));

                if (_agent.remainingDistance < 0.06f)
                {
                    EnemyIA.seeSomething = EnemyIA.StateSee.look;
                }
            }

        }

        state = NodeState.RUNNING;
        return state;
    }
}
