
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;
using System.Linq;

public class ViewPlayer : Node
{
    private Transform _Player;
    private NavMeshAgent _Pos;
    private ViewEnemy _Enemy;
    EnemyIA enumEnemy;

    public ViewPlayer(Transform player, NavMeshAgent pos, ViewEnemy enemy, EnemyIA _enumEnemy)
    {
        _Player = player;
        _Pos = pos;
        _Enemy = enemy;
        enumEnemy = _enumEnemy;
    }

    public override NodeState Evaluate()
    {
        var invalidStates = new[] { EnemyIA.StateSee.see, EnemyIA.StateSee.research, EnemyIA.StateSee.retreat };

        if (!invalidStates.Contains(enumEnemy.seeSomething))
        {

            if (_Enemy.OnSee(_Player.transform))
            {

                if (GetData("target") == null)
                {

                    ClearData("seePoint");
                    ClearData("last");

                    parent.parent.SetData("target", _Player.transform);

                    enumEnemy.seeSomething = EnemyIA.StateSee.see;

                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
        else if (enumEnemy.seeSomething == EnemyIA.StateSee.see)
        {

            if (GetData("last") == null)
            {
                ClearData("target");


                parent.parent.SetData("last", _Player.transform.position);


                enumEnemy.seeSomething = EnemyIA.StateSee.research;

                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
