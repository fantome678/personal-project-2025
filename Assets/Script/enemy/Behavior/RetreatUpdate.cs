using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatUpdate : Node
{
    NavMeshAgent agent;
    EnemyIA enemy;
    public RetreatUpdate(NavMeshAgent _agent, EnemyIA _Enemy)
    {
        agent = _agent;

        enemy = _Enemy;

    }

    public override NodeState Evaluate()
    {
        if (enemy.seeSomething == EnemyIA.StateSee.retreat)
        {
            if (agent.remainingDistance < 0.06f)
            {
                agent.speed = EnemyIA.speed;
                enemy.seeSomething = EnemyIA.StateSee.looked;
               // parent.parent.SetData("seePoint", EnemyIA.skillIA.script.transform.position);
                agent.SetDestination(EnemyIA.skillIA.script.transform.position);
                
                state = NodeState.FAILURE;
                return state;

            }
        }
      
        state = NodeState.RUNNING;
        return state;
    }
}
