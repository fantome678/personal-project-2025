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
        looked,
        see,
        research,
        lookHideOut
    }



   public NavMeshAgent agent;
    ViewEnemy viewEnemy;

    [UnityEngine.SerializeField] public int index;
    public UnityEngine.GameObject Player;
    public static UnityEngine.Vector3 posSearch;
    public static StateActionPlayerScript skillIA;
    public static UnityEngine.Vector3 pointOnSee;
    public static UnityEngine.Vector3 pointOnSeePos;
    public DataIA dataIA;
    public float enterFOV;
    public float enterSpeed;
    public static float FOV;
    public static float speed;
    public float timerSearchPlayer;
    public float timerCheckRoom;
    public float timerCheckRoom2;
    public int counter;
    public int counterSearch;
    public static UnityEngine.Transform transformStatic;

    public StateSee seeSomething;

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

        yield return new UnityEngine.WaitForSeconds(1.5f);

        skillIA.listDataIA.Add(dataIA);

        /*yield return new UnityEngine.WaitForSeconds(1.5f);
         dataIA = skillIA.listDataIA[index];*/
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
                new Retreat(agent, skillIA.PointToRetreat.transform.position, this),
                new RetreatUpdate(agent, this),
            }),

            new Sequence(new List<Node>
            {
                new ViewPlayer(Player.transform, agent, viewEnemy, this),
                new ToPlayerEnemy(agent, viewEnemy, this),
            }),

            new Sequence(new List<Node>
            {
                new GoSeePoint(agent, Player.GetComponentInParent<PlayerScript>(), this),
                new CheckRoom(agent, viewEnemy, this),
            }),

           new Sequence(new List<Node>
            {
                new LookHideOut(agent, skillIA.hideOutList, Player, this),
                new SearchHideOut(agent, viewEnemy, Player, this),
            }),

            new PatrollerTask(agent, Player, this),
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
            UnityEngine.Debug.Log("touched" + index);
        }
    }

    public void GameOver()
    {
        if (GetStateSee() != StateSee.retreat)
        {
            SceneManager.LoadScene(0);
        }
        //SceneManager.LoadScene(0);
    }

    public  void ResetToPlayerEnemy()
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
