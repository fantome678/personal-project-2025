
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class ViewPlayer : Node
{
    private Transform _Player;
    private NavMeshAgent _Pos;
    private ViewEnemy _Enemy;
    // private float dis;
    // private float detectedAngle;

    public ViewPlayer(Transform player, NavMeshAgent pos, ViewEnemy enemy)
    {
        _Player = player;
        _Pos = pos;
        _Enemy = enemy;
        //  UnityEngine.Debug.Log("f25");
    }

    public override NodeState Evaluate()
    {

        /*if (Vector3.Distance(_Player.position, _Pos.transform.position) < dis)
        {
            parent.parent.SetData("target", _Player.position);
            state = NodeState.SUCCESS;
            return state;
        }*/

        // Collider[] collider = Physics.OverlapSphere(_Pos.transform.position, EnemyIA.FOV, ~3);
        if (EnemyIA.seeSomething != EnemyIA.StateSee.see && EnemyIA.seeSomething != EnemyIA.StateSee.research)
        {
            // if (collider.Length > 0)
            // {
            if (_Enemy.OnSee(_Player.transform) /*&& (EnemyIA.seeSomething != EnemyIA.StateSee.see && EnemyIA.seeSomething != EnemyIA.StateSee.research)*/)
            {
                if (GetData("target") == null)
                {
                    //Debug.Log("inView");
                    ClearData("seePoint");
                    ClearData("last");
                    parent.parent.SetData("target", _Player.transform);
                    EnemyIA.seeSomething = EnemyIA.StateSee.see;
                    state = NodeState.SUCCESS;
                    return state;
                }
                // }
                // state = NodeState.FAILURE;
                // return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
        else if (EnemyIA.seeSomething == EnemyIA.StateSee.see)
        {
            if (GetData("last") == null)
            {
                // if (collider.Length > 0)
                // {
                if (!_Enemy.OnSee(_Player.transform))
                {
                    //Debug.Log("lostView");
                    EnemyIA.seeSomething = EnemyIA.StateSee.research;
                    parent.parent.SetData("last", _Player.transform.position);
                    ClearData("seePoint");
                    EnemyIA.ResetToPlayerEnemy();
                    state = NodeState.SUCCESS;
                    return state;
                }
                // }
                //  state = NodeState.FAILURE;
                //  return state;
            }
        }

        // EnemyIA.seeSomething = EnemyIA.StateSee.none;
        state = NodeState.SUCCESS;
        return state;
    }
}
