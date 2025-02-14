using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIState : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindAnyObjectByType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        InitCanvas();
    }


    void InitCanvas()
    {
        switch (playerScript.GetIDObject())
        {
            case IdObject.Smoke:
                if (playerScript.couldown)
                {
                    textMeshProUGUI.text = "Time before launch " + (int)playerScript.timerCouldown;
                }
                else
                {
                    textMeshProUGUI.text = "Ready to launch";
                }
                break;
            case IdObject.Flame:
                textMeshProUGUI.text = "ammo Flame " + playerScript.GetComponentInChildren<Gun>().GetAmmo();
                break;
            case IdObject.Detector:
                textMeshProUGUI.text = "";
                break;
        }
    }
}
