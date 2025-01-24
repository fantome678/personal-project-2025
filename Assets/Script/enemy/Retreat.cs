using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.AI;


public class Retreat : Node
{
    NavMeshAgent agent;
    UnityEngine.Vector3 posToRetreat;

    public Retreat(NavMeshAgent _agent, UnityEngine.Vector3 _posToRetreat)
    {
        agent = _agent;
        posToRetreat = _posToRetreat;
    }

    public override NodeState Evaluate()
    {
        if (EnemyIA.seeSomething == EnemyIA.StateSee.retreat)
        {
            agent.SetDestination(posToRetreat);
            agent.speed = EnemyIA.speed * 2;
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
