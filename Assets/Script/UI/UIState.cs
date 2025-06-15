using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIState : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    public TextMeshProUGUI textMeshProUGUI;
    [SerializeField] List<Image> keys;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindAnyObjectByType<PlayerScript>();
        for (int i = 0; i < keys.Count; i++) 
        {
            keys[i].enabled = false;
        }
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
                textMeshProUGUI.text = "Fuel " + playerScript.GetComponentInChildren<Gun>().GetAmmo();
                break;
            case IdObject.Detector:
                textMeshProUGUI.text = "";
                break;
        }
        if (playerScript.KeySecurity > 0)
        {
            keys[(int)playerScript.KeySecurity - 1].enabled = true;
        }

    }
}
