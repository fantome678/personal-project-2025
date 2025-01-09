using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool o; 
    ParticleSystem particule;
    // Start is called before the first frame update
    void Start()
    {
        o = true;
        particule = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        TriggerGun(o);
    }

    public void TriggerGun(bool isTrigger)
    {
        if (isTrigger)
        {
            particule.Play();
        }
        else
        {
            particule.Stop();
        }
    }

}
