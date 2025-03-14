using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyIA;

[System.Serializable]
public class DataIA
{
    public bool GoPointSee = false;
    public bool isPursuit = false;
    public bool isPredict = true;

    public float[] timerIACanSeeHideOut;

    public StateSee seeSomething = StateSee.none;
}

public class StateActionPlayerScript : MonoBehaviour
{
    [SerializeField] public GameObject PointToRetreat;
    [SerializeField] public PlayerScript script;
    [SerializeField] List<GameObject> triggerList;

    [SerializeField] public List<UnityEngine.GameObject> hideOutList;
    [SerializeField] public List<UnityEngine.Transform> pointPatroler;

    //public float[] timerIACanSeeHideOut;
    [SerializeField] private float timerGeneral;
    [SerializeField] private float timerBeforeGivePlayerPos;
    [SerializeField] private float timerBeforeHideOut;
    [SerializeField] private float timerIsPredict;
    [SerializeField] private bool IAHasPosplayer;
    [SerializeField] private bool IACanSeeHideOut;
    [SerializeField] private bool IAPredictYourMove;
    [SerializeField] public List<DataIA> listDataIA;

    // public bool GoPointSee;
    //public bool isPursuit;
    //public bool isPredict;

    private void Awake()
    {
        hideOutList = new List<UnityEngine.GameObject>();
        hideOutList.AddRange(UnityEngine.GameObject.FindGameObjectsWithTag("HideOut"));

        for (int i = 0; i < hideOutList.Count; i++)
        {
            hideOutList[i].GetComponent<HideOutScript>().index = i;
        }
        listDataIA = new List<DataIA>();

        for (int i = 0; i < 2; i++)
        {
            DataIA temp = new DataIA();
            temp.timerIACanSeeHideOut = new float[2];
            listDataIA.Add(temp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        script = FindAnyObjectByType<PlayerScript>();
        triggerList.AddRange(GameObject.FindGameObjectsWithTag("Trigger"));


        timerGeneral = 0;
        for (int i = 0; i < listDataIA.Count; i++)
        {
            listDataIA[i].timerIACanSeeHideOut[0] = 0;
            listDataIA[i].timerIACanSeeHideOut[1] = 0;
        }
        timerBeforeHideOut = 0;
        timerIsPredict = 0;

        IACanSeeHideOut = false;
        IAHasPosplayer = false;
        //isPursuit = false;
        //isPredict = true;
    }

    // Update is called once per frame
    void Update()
    {
        timerGeneral += Time.deltaTime;

        StateInformationPlayer();
        StateIACanSeeHideOut();
        StateIACanPredictInPursuit();
    }

    void StateInformationPlayer()
    {
        if (timerGeneral > 30.0f && IAHasPosplayer == false)
        {
            IAHasPosplayer = true;
        }
        else if (IAHasPosplayer)
        {
            for (int i = 0; i < listDataIA.Count; i++)
            {
                if (listDataIA[i].GoPointSee == false)
                {
                    timerBeforeHideOut += Time.deltaTime;
                    if (timerBeforeHideOut > 60.0f)
                    {
                        listDataIA[i].GoPointSee = true;
                    }
                }
            }
        }
    }

    void StateIACanSeeHideOut()
    {
        if (!IACanSeeHideOut && script.isHide)
        {
            timerBeforeHideOut += Time.deltaTime;
            if (timerBeforeHideOut > 15.0f)
            {
                IACanSeeHideOut = true;
            }
        }

        if (IACanSeeHideOut && script.isHide)
        {
            for (int i = 0; i < listDataIA.Count; i++)
            {
                listDataIA[i].timerIACanSeeHideOut[1] += Time.deltaTime;
                if (listDataIA[i].timerIACanSeeHideOut[1] > 15.0f)
                {

                    listDataIA[i].isPursuit = true;

                    listDataIA[i].timerIACanSeeHideOut[1] = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < listDataIA.Count; i++)
            {
                listDataIA[i].timerIACanSeeHideOut[1] = 0;
            }
        }
    }

    void StateIACanPredictInPursuit()
    {
        if (timerGeneral > 30 && IAPredictYourMove == false)
        {
            IAPredictYourMove = true;
        }
        else
        {
            for (int i = 0; i < listDataIA.Count; i++)
            {
                if (IAPredictYourMove && timerIsPredict < 15f && listDataIA[i].isPredict == false)
                {
                    timerIsPredict += Time.deltaTime;
                }
                else
                {
                    listDataIA[i].isPredict = true;
                    timerIsPredict = 0;
                }
            }
        }
    }

    public Vector3 IsPursuitState()
    {
        for (int i = 0; i < triggerList.Count; i++)
        {
            if (triggerList[i].GetComponent<IAHelpScript>().playerIsEnter)
            {
                Debug.Log(triggerList[i].GetComponent<IAHelpScript>().posToSpawnAI);
                return triggerList[i].GetComponent<IAHelpScript>().posToSpawnAI;
            }
        }
        return Vector3.zero;
    }

    public void ResetTimer()
    {
        timerBeforeHideOut = 0;
        for (int i = 0; i < listDataIA.Count; i++)
        {
            listDataIA[i].GoPointSee = false;
        }
    }

    public Transform GetPlayer()
    {
        return script.transform;
    }
}

