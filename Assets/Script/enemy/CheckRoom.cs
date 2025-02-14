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
                EnemyIA.timerCheckRoom = 0; // R�initialisation du timer

                randomOffset.y = 0.0f; // Fixe la composante Y pour �viter de se d�placer verticalement
                randomOffset *= 5; // Multiplie pour avoir un d�calage suffisant

                NavMeshHit hit;
                // Si le point al�atoire est accessible (trouv� sur le NavMesh)
                if (NavMesh.SamplePosition(posSave + randomOffset, out hit, 1.0f, NavMesh.AllAreas))
                {
                    // On d�place l'ennemi vers la position cible
                    _agent.transform.LookAt(posSave + randomOffset);
                    _agent.SetDestination(posSave + randomOffset);
                }
                else
                {
                    // Si la position n'est pas valide, on essaie de trouver une autre position
                    // (c'est pour �viter un blocage si la position al�atoire est inaccessible)
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

            if (EnemyIA.timerCheckRoom > 2) // Si le temps total pass� � chercher un mouvement est trop long
            {
                EnemyIA.seeSomething = EnemyIA.StateSee.none; // Changement d'�tat pour �viter de bloquer
                ClearData("seePoint"); // Nettoie la donn�e
            }

        }
        state = NodeState.RUNNING;
        return state;
    }
}
