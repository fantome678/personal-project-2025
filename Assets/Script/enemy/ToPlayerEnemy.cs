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
        

        if (EnemyIA.seeSomething == EnemyIA.StateSee.see)
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
                        EnemyIA.seeSomething = EnemyIA.StateSee.research;
                        _Pos.SetDestination(target.transform.position);

                        ClearData("seePoint");
                        parent.parent.SetData("last", target.transform.position);
                        EnemyIA.ResetToPlayerEnemy();
                    }
                }

                /* if (Vector3.Distance(_Pos.transform.position, target.position) > 0.01f)
                 {
                     EnemyIA.skillIA.ResetTimer();

                     _Pos.SetDestination(target.position);


                     EnemyIA.pointOnSeePos = new Vector3(target.position.x, target.position.y, target.position.z);
                 }*/
            }

            if (_Pos.speed < 8.2f)
            {
                _Pos.speed += 1.015f * Time.deltaTime;
                //  Debug.Log("see" + _Pos.speed);
            }

            if (Vector3.Distance(_Pos.transform.position, target.transform.position) > 4 && EnemyIA.skillIA.isPredict && !EnemyIA.skillIA.script.OnSee(_Pos.transform))
            {
                // Debug.Log(EnemyIA.skillIA.IsPursuitState());
                if (EnemyIA.skillIA.IsPursuitState() != Vector3.zero)
                {
                    Vector3 pos = EnemyIA.skillIA.IsPursuitState();
                    EnemyIA.TPPlayer(pos);
                    parent.parent.SetData("last", target.transform.position);
                    _Pos.SetDestination(target.transform.position);
                }
                EnemyIA.skillIA.isPredict = false;

            }

          /*  if (Vector3.Distance(_Pos.transform.position, EnemyIA.skillIA.script.transform.position) > 0.01f)
            {
                if (_ViewEnemy.OnSee(EnemyIA.skillIA.script.transform))
                {
                    EnemyIA.skillIA.ResetTimer();

                    _Pos.SetDestination(EnemyIA.skillIA.script.transform.position);

                }
                else
                {
                    EnemyIA.seeSomething = EnemyIA.StateSee.research;
                    _Pos.SetDestination(EnemyIA.skillIA.script.transform.position);

                    ClearData("seePoint");
                    parent.parent.SetData("last", EnemyIA.skillIA.script.transform.position);
                    EnemyIA.ResetToPlayerEnemy();
                }
            }*/


        }
        else if (EnemyIA.seeSomething == EnemyIA.StateSee.research)
        {
            Vector3 target = (Vector3)GetData("last");
            if (_Pos.speed > EnemyIA.speed * 2)
            {
                _Pos.speed -= 1.0015f * Time.deltaTime;
                // Debug.Log("reseach" + _Pos.speed);
            }

            if (_ViewEnemy.OnSee(EnemyIA.skillIA.script.transform) == true)
            {
                EnemyIA.seeSomething = EnemyIA.StateSee.none;
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
        // Vector3 pos = (Vector3)GetData("last");
        Vector3 pos = _pos;
        Vector3 randomOffset = Random.insideUnitSphere;
       //  Debug.Log("Pos de la derniere position du dictionnaire : "+pos);

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
                    _Pos.transform.LookAt(pos + randomOffset);

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

}
