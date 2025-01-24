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

        state = NodeState.SUCCESS;
        return state;
    }
}
