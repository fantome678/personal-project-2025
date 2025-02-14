using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActionPlayerScript : MonoBehaviour
{
    [SerializeField] public GameObject PointToRetreat;
    [SerializeField] public PlayerScript script;
    [SerializeField] List<GameObject> triggerList;
    [SerializeField] public List<UnityEngine.GameObject> hideOutList;

    public float[] timerIACanSeeHideOut;
    [SerializeField] private float timerGeneral;
    [SerializeField] private float timerBeforeGivePlayerPos;
    [SerializeField] private float timerBeforeHideOut;
    [SerializeField] private float timerIsPredict;
    [SerializeField] private bool IAHasPosplayer;
    [SerializeField] private bool IACanSeeHideOut;
    [SerializeField] private bool IAPredictYourMove;

    public bool GoPointSee;
    public bool isPursuit;
    public bool isPredict;

    private void Awake()
    {
        hideOutList = new List<UnityEngine.GameObject>();
        hideOutList.AddRange(UnityEngine.GameObject.FindGameObjectsWithTag("HideOut"));

        for (int i = 0; i < hideOutList.Count; i++)
        {
            hideOutList[i].GetComponent<HideOutScript>().index = i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        script = FindAnyObjectByType<PlayerScript>();
        triggerList.AddRange(GameObject.FindGameObjectsWithTag("Trigger"));
        

        timerGeneral = 0;
        timerIACanSeeHideOut[0] = 0;
        timerIACanSeeHideOut[1] = 0;
        timerBeforeHideOut = 0;
        timerIsPredict = 0;

        IACanSeeHideOut = false;
        IAHasPosplayer = false;
        isPursuit = false;
        isPredict = true;
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
        else if (IAHasPosplayer && GoPointSee == false)
        {
            timerBeforeHideOut += Time.deltaTime;
            if (timerBeforeHideOut > 60.0f)
            {
                GoPointSee = true;
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
            timerIACanSeeHideOut[1] += Time.deltaTime;
            if (timerIACanSeeHideOut[1] > 15.0f)
            {
                isPursuit = true;
                timerIACanSeeHideOut[1] = 0;
            }
        }
        else
        {
            timerIACanSeeHideOut[1] = 0;
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
            if (IAPredictYourMove && timerIsPredict < 15f && isPredict == false)
            {
                timerIsPredict += Time.deltaTime;
            }
            else
            {
                isPredict = true;
                timerIsPredict = 0;
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
        GoPointSee = false;
    }

    public Transform GetPlayer()
    {
        return script.transform;
    }
}

