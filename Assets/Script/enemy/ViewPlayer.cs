
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;
using System.Linq;

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
        var invalidStates = new[] { EnemyIA.StateSee.see, EnemyIA.StateSee.research, EnemyIA.StateSee.retreat };

        // Si l'ennemi n'est pas dans un �tat o� il doit r�agir � la vue
        if (!invalidStates.Contains(EnemyIA.seeSomething))
        {
            // Si l'ennemi voit le joueur
            if (_Enemy.OnSee(_Player.transform))
            {
                // Si aucune cible n'est d�j� d�finie
                if (GetData("target") == null)
                {
                    // Nettoyer les anciennes donn�es (�viter les effets de bord)
                    ClearData("seePoint");
                    ClearData("last");

                    // D�finir le joueur comme cible de l'ennemi
                    parent.parent.SetData("target", _Player.transform);

                    // Passer l'�tat de l'ennemi � "voir"
                    EnemyIA.seeSomething = EnemyIA.StateSee.see;

                    // Log pour le d�bogage ou l'information
                    // Debug.Log("L'ennemi a vu le joueur pour la premi�re fois");

                    // Retourner un �tat de succ�s (l'IA a r�ussi � voir la cible)
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            // Si l'ennemi ne voit toujours pas le joueur ou une autre condition �choue
            state = NodeState.FAILURE;
            return state;
        }
        else if (EnemyIA.seeSomething == EnemyIA.StateSee.see)
        {
            // L'ennemi a d�j� vu le joueur, mais il n'a pas encore une position de recherche
            if (GetData("last") == null)
            {
                // Nettoyer les anciennes donn�es de cible
                ClearData("target");

                // Sauvegarder la position actuelle du joueur
                parent.parent.SetData("last", _Player.transform.position);

                // Passer l'�tat de l'ennemi � "recherche"
                EnemyIA.seeSomething = EnemyIA.StateSee.research;

                // Log pour d�bogage ou pour une trace du comportement de l'IA
                // Debug.Log("L'ennemi commence � rechercher le joueur");

                // Retourner un �tat de succ�s (l'IA a commenc� � chercher le joueur)
                state = NodeState.SUCCESS;
                return state;
            }
        }

        // Si aucune des conditions ci-dessus n'est satisfaite, on retourne un �tat de succ�s g�n�rique
        state = NodeState.SUCCESS;
        return state;
    }
}
