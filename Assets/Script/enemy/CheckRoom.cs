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
            Vector3 randomOffset = Random.insideUnitSphere;

            EnemyIA.timerCheckRoom += Time.deltaTime;
            if (EnemyIA.timerCheckRoom > 2 && EnemyIA.timerCheckRoom2 < 3)
            {
                EnemyIA.timerCheckRoom = 0; // R�initialisation du timer

                randomOffset.y = 0.0f; // Fixe la composante Y pour �viter de se d�placer verticalement
                randomOffset *= 0.5f; // Multiplie pour avoir un d�calage suffisant

                NavMeshHit hit;
                _agent.transform.LookAt(posSave + randomOffset);
                // Si le point al�atoire est accessible (trouv� sur le NavMesh)
                if (NavMesh.SamplePosition(posSave + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                {
                    // On d�place l'ennemi vers la position cible
                    _agent.SetDestination(posSave + randomOffset);
                }
                else
                {
                    // Si la position n'est pas valide, on essaie plusieurs fois avec un rayon plus large
                    bool foundNewPosition = false;
                    int y = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        randomOffset = Random.insideUnitSphere; // Recalcule un autre offset
                        randomOffset.y = 0.0f;
                        randomOffset *= 0.5f;
                        if (y > 10)
                        {
                            _agent.transform.LookAt(posSave + randomOffset);
                            y = 0;
                        }
                        // Augmente le rayon de recherche pour �viter que l'ennemi se bloque dans un petit couloir
                        if (NavMesh.SamplePosition(posSave + randomOffset, out hit, 2.0f, NavMesh.AllAreas))
                        {
                            _agent.transform.LookAt(posSave + randomOffset);
                            _agent.SetDestination(posSave + randomOffset);
                            foundNewPosition = true;
                            break;
                        }
                        y++;
                    }
                    // Si on n'a pas trouv� de nouvelle position, on pourrait r�initialiser l'agent
                    if (!foundNewPosition)
                    {
                        // L'ennemi pourrait soit s'arr�ter, ou revenir � un comportement de recherche
                        _agent.ResetPath(); // R�initialise le chemin et �vite le blocage
                    }
                }
                EnemyIA.timerCheckRoom2++;
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
}
