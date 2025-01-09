using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToPlayerEnemy : Node
{
    private NavMeshAgent _Pos;
    private ViewEnemy _ViewEnemy;
    public ToPlayerEnemy(NavMeshAgent pos, ViewEnemy script)
    {
        _Pos = pos;
        _ViewEnemy = script;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (EnemyIA.seeSomething == EnemyIA.StateSee.see)
        {
            if (target != null)
            {
                if (Vector3.Distance(_Pos.transform.position, target.position) > 0.01f)
                {
                    EnemyIA.skillIA.ResetTimer();

                    _Pos.SetDestination(target.position);
                    

                    EnemyIA.pointOnSeePos = new Vector3(target.position.x, target.position.y, target.position.z);
                }
            }
            if (_Pos.speed < 10f)
            {
                _Pos.speed += 1.002f * Time.deltaTime;
                Debug.Log("see" + _Pos.speed);
            }
        }
        else if (EnemyIA.seeSomething == EnemyIA.StateSee.research)
        {
            if (_Pos.speed > EnemyIA.speed * 2)
            {
                _Pos.speed -= 1.0015f * Time.deltaTime;
                Debug.Log("reseach" + _Pos.speed);
            }
            if (_ViewEnemy.OnSee((Transform)GetData("target")) == true)
            {
                EnemyIA.seeSomething = EnemyIA.StateSee.none;
                ClearData("seePoint");
                ClearData("last");
                state = NodeState.FAILURE;
                return state;
            }

            Vector3 pos = (Vector3)GetData("last");
            Vector3 randomOffset = Random.insideUnitSphere;
            // Debug.Log("Pos de la derniere position du dictionnaire : "+pos);

            // if he saw me in the research, return to begin Node
            EnemyIA.timerCheckRoom += Time.deltaTime;
            // Debug.Log("fssffgs" + pos);
            if (EnemyIA.timerCheckRoom > 1)
            {
                EnemyIA.timerCheckRoom = 0;
                EnemyIA.counter++;
                if (EnemyIA.counter < 4)
                {
                    randomOffset.y = 0.0f;
                    randomOffset *= 5;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(pos + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        Debug.Log("fssffgs" + randomOffset);
                        // agentTransform.transform.LookAt(pos.position + randomOffset);
                        _Pos.SetDestination(pos + randomOffset);

                    }
                }
                else
                {
                    EnemyIA.seeSomething = EnemyIA.StateSee.none;
                    EnemyIA.timerCheckRoom = 0;
                    EnemyIA.counter = 0;
                    ClearData("target");
                    ClearData("last");
                    _Pos.speed = EnemyIA.speed;
                }
            }

        }
        state = NodeState.RUNNING;
        return state;
    }
}
