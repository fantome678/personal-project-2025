using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActionPlayerScript : MonoBehaviour
{
    [SerializeField] PlayerScript script;

    public float[] timerIACanSeeHideOut;
    [SerializeField] private float timerGeneral;
    [SerializeField] private float timerBeforeGivePlayerPos;
    [SerializeField] private float timerBeforeHideOut;
    [SerializeField] private bool IAHasPosplayer;
    [SerializeField] private bool IACanSeeHideOut;
    [SerializeField] private bool IAPredictYourMove;

    public bool GoPointSee;
    public bool isPursuit;
    public bool isPredict;
    // Start is called before the first frame update
    void Start()
    {
        script = FindAnyObjectByType<PlayerScript>();
        timerGeneral = 0;
        timerIACanSeeHideOut[0] = 0;
        timerIACanSeeHideOut[1] = 0;
        IACanSeeHideOut = false;
        timerBeforeHideOut = 0;
        IAHasPosplayer = false;
        isPursuit = false;
        isPredict = false;
    }

    // Update is called once per frame
    void Update()
    {
        timerGeneral += Time.deltaTime;

        StateInformationPlayer();
        StateIACanSeeHideOut();
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

