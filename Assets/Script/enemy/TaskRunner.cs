using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskRunner : MonoBehaviour
{

    public static TaskRunner Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

}
