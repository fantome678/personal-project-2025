using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatUpdate : Node
{
    NavMeshAgent agent;
    Vector3 playerPos;
    public RetreatUpdate(NavMeshAgent _agent, Vector3 _player)
    {
        agent = _agent;
        playerPos = _player;
    }

    public override NodeState Evaluate()
    {
        if (EnemyIA.seeSomething == EnemyIA.StateSee.retreat)
        {
            if (agent.remainingDistance < 0.06f)
            {
                agent.speed = EnemyIA.speed;
                agent.SetDestination(playerPos);
                EnemyIA.seeSomething = EnemyIA.StateSee.none;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
