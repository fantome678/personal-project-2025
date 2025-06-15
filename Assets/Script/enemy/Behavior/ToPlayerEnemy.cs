using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToPlayerEnemy : Node
{
    private NavMeshAgent _Pos;
    private ViewEnemy _ViewEnemy;
    private EnemyIA enemyIA;  // Référence vers l'instance EnemyIA
    public ToPlayerEnemy(NavMeshAgent pos, ViewEnemy script, EnemyIA _EnemyIA)
    {
        _Pos = pos;
        _ViewEnemy = script;
        enemyIA = _EnemyIA;  // Stocke la référence
    }

    public override NodeState Evaluate()
    {
        if (enemyIA.seeSomething == EnemyIA.StateSee.retreat)
        {
            UnityEngine.Debug.Log("fuit");
            ClearData("seePoint");
            ClearData("last");
         //   _Pos.SetDestination(EnemyIA.skillIA.PointToRetreat.transform.position);
            state = NodeState.FAILURE;
            return state;
        }
        if (enemyIA.seeSomething == EnemyIA.StateSee.see)
        {
            Transform target = (Transform)GetData("target");
            if (target != null)
            {
                if (Vector3.Distance(_Pos.transform.position, target.transform.position) > 0.01f)
                {
                    if (_ViewEnemy.OnSee(target.transform))
                    {
                        EnemyIA.skillIA.ResetTimer();
                        _Pos.SetDestination(target.transform.position);
                    }
                    else
                    {
                        enemyIA.seeSomething = EnemyIA.StateSee.research; // Modifie directement seeSomething de EnemyIA
                        _Pos.SetDestination(target.transform.position);
                        ClearData("seePoint");
                        parent.parent.SetData("last", target.transform.position);
                        enemyIA.ResetToPlayerEnemy();
                    }
                }
            }

            /*  float distance = Vector3.Distance(_Pos.transform.position, target.position);
              _Pos.speed = Mathf.Lerp(EnemyIA.speed, EnemyIA.speed * 2, distance / 10f);*/

            /* if (_Pos.speed < 8.2f)
             {
                 _Pos.speed += 1.015f * Time.deltaTime;
             }*/
            _Pos.speed = Mathf.MoveTowards(_Pos.speed, 8.2f, 1.015f * Time.deltaTime);


            if (Vector3.Distance(_Pos.transform.position, target.transform.position) > 4 && enemyIA.dataIA.isPredict && !EnemyIA.skillIA.script.OnSee(_Pos.transform))
            {
                if (EnemyIA.skillIA.IsPursuitState() != Vector3.zero)
                {
                    Vector3 pos = EnemyIA.skillIA.IsPursuitState();
                    EnemyIA.TPPlayer(pos);
                    parent.parent.SetData("last", target.transform.position);
                    _Pos.SetDestination(target.transform.position);
                }
                enemyIA.dataIA.isPredict = false;
            }
        }
        else if (enemyIA.seeSomething == EnemyIA.StateSee.research)
        {
            Debug.Log("recherche");
            Vector3 target = (Vector3)GetData("last");
            if (_Pos.speed > EnemyIA.speed * 2)
            {
                _Pos.speed -= 1.0015f * Time.deltaTime;
            }

            if (_ViewEnemy.OnSee(EnemyIA.skillIA.script.transform) == true)
            {
                enemyIA.seeSomething = EnemyIA.StateSee.none;
                ClearData("seePoint");
                ClearData("last");
                state = NodeState.FAILURE;
                return state;
            }
            if (target != null)
            {
                Research(target);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void Research(Vector3 _pos)
    {
        /* Vector3 pos = _pos;
         Vector3 randomOffset = Random.insideUnitSphere;

         enemyIA.timerCheckRoom += Time.deltaTime;

         if (enemyIA.timerCheckRoom > 1)
         {
             enemyIA.timerCheckRoom = 0;
             enemyIA.counter++;
             if (enemyIA.counter < 4)
             {
                 randomOffset.y = 0.0f;
                 randomOffset *= 5;

                 NavMeshHit hit;
                 if (NavMesh.SamplePosition(pos + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                 {
                     _Pos.SetDestination(pos + randomOffset);
                     _Pos.transform.LookAt(pos + randomOffset);
                 }
             }
             else
             {
                 enemyIA.seeSomething = EnemyIA.StateSee.none;  // Modifie directement seeSomething
                 enemyIA.timerCheckRoom = 0;
                 enemyIA.counter = 0;
                 ClearData("target");
                 ClearData("last");
                 _Pos.speed = EnemyIA.speed;
             }
         }*/

        if (_Pos.remainingDistance < 1.0f)
        {
            if (enemyIA.counter < 4)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 5;
                randomDirection.y = 0;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(_pos + randomDirection, out hit, 2f, NavMesh.AllAreas))
                {
                    _Pos.SetDestination(hit.position);
                }

                enemyIA.counter++;
            }
            else
            {
                enemyIA.seeSomething = EnemyIA.StateSee.none;
                enemyIA.counter = 0;
                ClearData("target");
                ClearData("last");
            }
        }

    }
}
