using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.AI;


public class Retreat : Node
{
    NavMeshAgent agent;
    UnityEngine.Vector3 posToRetreat;
    EnemyIA enemy;

    public Retreat(NavMeshAgent _agent, UnityEngine.Vector3 _posToRetreat, EnemyIA _Enemy)
    {
        agent = _agent;
        posToRetreat = _posToRetreat;
        enemy = _Enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemy.seeSomething == EnemyIA.StateSee.retreat)
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
