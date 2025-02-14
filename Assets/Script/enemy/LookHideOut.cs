using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LookHideOut : Node
{
    NavMeshAgent _Agent;
    List<UnityEngine.GameObject> _list;
    public LookHideOut(NavMeshAgent agent, List<UnityEngine.GameObject> list)
    {
        _Agent = agent;
       _list = list;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log(EnemyIA.seeSomething);

        if (EnemyIA.seeSomething == EnemyIA.StateSee.none)
        {
            if (EnemyIA.skillIA.isPursuit)//EnemyIA.isPursuit)
            {
                int rand;
                
                if (_list.Count > 0)
                {
                    Debug.Log("fsf");
                    rand = Random.Range(0, _list.Count);
                    //Debug.Log(_list[rand].GetComponent<HideOutScript>().pos.transform.localPosition);
                    _Agent.SetDestination(_list[rand].GetComponent<HideOutScript>().pos.transform.position);
                    // _Agent.SetDestination(_list[rand].transform.position + _list[rand].GetComponent<HideOutScript>().pos.transform.localPosition);
                    parent.parent.SetData("HideOut", _list[rand].transform);
                    EnemyIA.seeSomething = EnemyIA.StateSee.lookHideOut;
                   // EnemyIA.isPursuit = false;
                    EnemyIA.skillIA.isPursuit = false;
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }

        }


        state = NodeState.SUCCESS;
        return state;
    }
}
