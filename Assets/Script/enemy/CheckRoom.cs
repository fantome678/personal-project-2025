using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CheckRoom : Node
{
    NavMeshAgent _agent;
    ViewEnemy _ViewEnemy;

    public CheckRoom(NavMeshAgent agent, ViewEnemy viewEnemy)
    {
        _agent = agent;
        _ViewEnemy = viewEnemy;
    }

    public override NodeState Evaluate()
    {

        if (_ViewEnemy.OnSee(EnemyIA.skillIA.script.transform))
        {
            EnemyIA.seeSomething = EnemyIA.StateSee.see;
            ClearData("seePoint");
            ClearData("target");
            ClearData("last");

            state = NodeState.FAILURE;
            return state;
        }

        if (EnemyIA.seeSomething == EnemyIA.StateSee.look)
        {
            Vector3 posSave = (Vector3)GetData("seePoint");

            Debug.Log("in look");
            Vector3 randomOffset = Random.insideUnitSphere;
            EnemyIA.timerCheckRoom += Time.deltaTime;
            if (EnemyIA.timerCheckRoom > 2)
            {
                EnemyIA.timerCheckRoom = 0; // Réinitialisation du timer

                randomOffset.y = 0.0f; // Fixe la composante Y pour éviter de se déplacer verticalement
                randomOffset *= 5; // Multiplie pour avoir un décalage suffisant

                NavMeshHit hit;
                // Si le point aléatoire est accessible (trouvé sur le NavMesh)
                if (NavMesh.SamplePosition(posSave + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                {
                    // On déplace l'ennemi vers la position cible
                    _agent.transform.LookAt(posSave + randomOffset);
                    _agent.SetDestination(posSave + randomOffset);
                }
                else
                {
                    // Si la position n'est pas valide, on essaie de trouver une autre position
                    // (c'est pour éviter un blocage si la position aléatoire est inaccessible)
                    randomOffset = Random.insideUnitSphere; // Recalcule un autre offset
                    randomOffset.y = 0.0f;
                    randomOffset *= 5;

                    if (NavMesh.SamplePosition(posSave + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        _agent.transform.LookAt(posSave + randomOffset);
                        _agent.SetDestination(posSave + randomOffset);
                    }
                }
            }

            if (EnemyIA.timerCheckRoom > 2) // Si le temps total passé à chercher un mouvement est trop long
            {
                EnemyIA.seeSomething = EnemyIA.StateSee.none; // Changement d'état pour éviter de bloquer
                ClearData("seePoint"); // Nettoie la donnée
            }

        }
        state = NodeState.RUNNING;
        return state;
    }
}
