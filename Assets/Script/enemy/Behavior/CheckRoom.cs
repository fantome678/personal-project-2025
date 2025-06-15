using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CheckRoom : Node
{
    NavMeshAgent agent;
    ViewEnemy viewEnemy;
    EnemyIA enemy;
    bool traversingLink = false;
    Coroutine traversalCoroutine;
    public CheckRoom(NavMeshAgent _Agent, ViewEnemy _ViewEnemy, EnemyIA _Enemy)
    {
        agent = _Agent;
        viewEnemy = _ViewEnemy;
        enemy = _Enemy;
    }

    public override NodeState Evaluate()
    {

        if (enemy.seeSomething == EnemyIA.StateSee.retreat)
        {
            ClearData("seePoint");
            ClearData("last");
            state = NodeState.FAILURE;
            return state;
        }

        if (viewEnemy.OnSee(EnemyIA.skillIA.script.transform) && enemy.seeSomething != EnemyIA.StateSee.retreat)
        {
            enemy.seeSomething = EnemyIA.StateSee.see;
            ClearData("seePoint");
            ClearData("target");
            ClearData("last");

            state = NodeState.FAILURE;
            return state;
        }

        if (enemy.seeSomething == EnemyIA.StateSee.look)
        {
            Vector3 posSave = (Vector3)GetData("seePoint");
            Vector3 randomOffset = Random.insideUnitSphere;

            if (agent.remainingDistance < 0.06f)
            {
                enemy.timerCheckRoom += Time.deltaTime;
                if (enemy.timerCheckRoom2 < 3)
                {
                    if (enemy.timerCheckRoom > 2.5f)
                    {
                        randomOffset.y = 0.0f;
                        randomOffset *= 0.5f;

                        NavMeshHit hit;
                        agent.transform.LookAt(agent.transform.position + randomOffset);

                        if (NavMesh.SamplePosition(agent.transform.position + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                        {
                            agent.SetDestination(agent.transform.position + randomOffset);
                        }
                        enemy.timerCheckRoom2++;
                        enemy.timerCheckRoom = 0;
                    }
                }
                else
                {
                    ClearData("seePoint");
                    enemy.timerCheckRoom = 0;
                    enemy.seeSomething = EnemyIA.StateSee.none;
                    enemy.timerCheckRoom2 = 0;
                    state = NodeState.FAILURE;
                    return state;
                }
            }
        }

        /* if (agent.isOnOffMeshLink && !traversingLink)
         {
             traversingLink = true;
             OffMeshLinkData data = agent.currentOffMeshLinkData;

             traversalCoroutine = TaskRunner.Instance.StartCoroutine(TraverseCurvedLink(data, agent));
         }*/

        state = NodeState.RUNNING;
        return state;
    }

    private IEnumerator TraverseCurvedLink(OffMeshLinkData link, NavMeshAgent agent)
    {
        agent.isStopped = true;

        Vector3 start = link.startPos;
        Vector3 end = link.endPos;
        Vector3 control = (start + end) / 2 + Vector3.up * 3f; // Déplacement arrondi

        float t = 0;
        float duration = 1.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 a = Vector3.Lerp(start, control, t);
            Vector3 b = Vector3.Lerp(control, end, t);
            Vector3 pos = Vector3.Lerp(a, b, t);
            agent.transform.position = pos;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        traversingLink = false;
    }

}
