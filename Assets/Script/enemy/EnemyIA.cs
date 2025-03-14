using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Unity.MLAgents;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyIA : Tree
{
    public enum StateSee
    {
        none,
        retreat,
        look,
        see,
        research,
        lookHideOut
    }



    NavMeshAgent agent;
    ViewEnemy viewEnemy;

    [UnityEngine.SerializeField] public int index;
    public UnityEngine.GameObject Player;
    public static UnityEngine.Vector3 posSearch;
    public static StateActionPlayerScript skillIA;
    public static UnityEngine.Vector3 pointOnSee;
    public static UnityEngine.Vector3 pointOnSeePos;
    public static DataIA dataIA;
    public float enterFOV;
    public float enterSpeed;
    public static float FOV;
    public static float speed;
    public static float timerSearchPlayer;
    public static float timerCheckRoom;
    public static float timerCheckRoom2;
    public static int counter;
    public static int counterSearch;
    public static UnityEngine.Transform transformStatic;
    // public static bool isPursuit;

    public static StateSee seeSomething;

    private void Awake()
    {
        posSearch = new UnityEngine.Vector3(0, 0, 0);
        dataIA = new DataIA();
        seeSomething = StateSee.none;
        FOV = enterFOV;
        speed = enterSpeed;
        timerSearchPlayer = 0f;
        timerCheckRoom = 0f;
        timerCheckRoom2 = 0f;
        counter = 0;
        counterSearch = 0;
        agent = GetComponent<NavMeshAgent>();
        GetComponentInChildren<ViewEnemy>().dis = 13;
        viewEnemy = GetComponentInChildren<ViewEnemy>();

        

        StartCoroutine(GetStateAction());

        transformStatic = transform;
    }

    IEnumerator GetStateAction()
    {
        skillIA = FindObjectOfType<StateActionPlayerScript>();
       
        yield return null;
        dataIA = skillIA.listDataIA[index];
    }

    public static void TPPlayer(UnityEngine.Vector3 _pos)
    {
        transformStatic.position = _pos;
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
         {
            new Sequence(new List<Node>
            {
                new Retreat(agent, skillIA.PointToRetreat.transform.position),
                new RetreatUpdate(agent, skillIA.PointToRetreat.transform.position),
            }),

            new Sequence(new List<Node>
            {
                new ViewPlayer(Player.transform, agent, viewEnemy),
                new ToPlayerEnemy(agent, viewEnemy),
            }),

            new Sequence(new List<Node>
            {
                new GoSeePoint(agent, Player.GetComponentInParent<PlayerScript>()),
                new CheckRoom(agent, viewEnemy),
            }),

            new Sequence(new List<Node>
            {
                new LookHideOut(agent, skillIA.hideOutList),
                new SearchHideOut(agent, viewEnemy, Player),
            }),

            new PatrollerTask(agent, Player),
         });
        return root;
    }

    public StateSee GetStateSee()
    {
        return seeSomething;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("FlameCollider"))
        {
            seeSomething = StateSee.retreat;
        }
    }

    public static void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    public static void ResetToPlayerEnemy()
    {
        timerCheckRoom = 0f;
        counter = 0;
    }

    private void OnDrawGizmos()
    {
        if (skillIA != null)
        {
            for (int i = 0; i < skillIA.pointPatroler.Count; i++)
            {
                UnityEngine.Gizmos.color = new UnityEngine.Color(1, 0, 0, 0.5f);
                UnityEngine.Gizmos.DrawCube(skillIA.pointPatroler[i].transform.position, new UnityEngine.Vector3(1, 1, 1));

            }
        }
        //UnityEngine.Gizmos.DrawCube(((UnityEngine.Transform)Node.GetData("target")).position, new UnityEngine.Vector3(1, 1, 1));
    }
}
