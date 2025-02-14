
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

        // Si l'ennemi n'est pas dans un état où il doit réagir à la vue
        if (!invalidStates.Contains(EnemyIA.seeSomething))
        {
            // Si l'ennemi voit le joueur
            if (_Enemy.OnSee(_Player.transform))
            {
                // Si aucune cible n'est déjà définie
                if (GetData("target") == null)
                {
                    // Nettoyer les anciennes données (éviter les effets de bord)
                    ClearData("seePoint");
                    ClearData("last");

                    // Définir le joueur comme cible de l'ennemi
                    parent.parent.SetData("target", _Player.transform);

                    // Passer l'état de l'ennemi à "voir"
                    EnemyIA.seeSomething = EnemyIA.StateSee.see;

                    // Log pour le débogage ou l'information
                    // Debug.Log("L'ennemi a vu le joueur pour la première fois");

                    // Retourner un état de succès (l'IA a réussi à voir la cible)
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            // Si l'ennemi ne voit toujours pas le joueur ou une autre condition échoue
            state = NodeState.FAILURE;
            return state;
        }
        else if (EnemyIA.seeSomething == EnemyIA.StateSee.see)
        {
            // L'ennemi a déjà vu le joueur, mais il n'a pas encore une position de recherche
            if (GetData("last") == null)
            {
                // Nettoyer les anciennes données de cible
                ClearData("target");

                // Sauvegarder la position actuelle du joueur
                parent.parent.SetData("last", _Player.transform.position);

                // Passer l'état de l'ennemi à "recherche"
                EnemyIA.seeSomething = EnemyIA.StateSee.research;

                // Log pour débogage ou pour une trace du comportement de l'IA
                // Debug.Log("L'ennemi commence à rechercher le joueur");

                // Retourner un état de succès (l'IA a commencé à chercher le joueur)
                state = NodeState.SUCCESS;
                return state;
            }
        }

        // Si aucune des conditions ci-dessus n'est satisfaite, on retourne un état de succès générique
        state = NodeState.SUCCESS;
        return state;
    }
}
