using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatUpdate : Node
{
    NavMeshAgent agent;

    public RetreatUpdate(NavMeshAgent _agent)
    {
        agent = _agent;
    }

    public override NodeState Evaluate()
    {
        if (EnemyIA.seeSomething == EnemyIA.StateSee.retreat)
        {
            if (agent.remainingDistance < 0.06f)
            {
                agent.speed = EnemyIA.speed;
                EnemyIA.seeSomething = EnemyIA.StateSee.none;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
