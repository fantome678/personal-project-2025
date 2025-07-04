using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isFire;
    float ammo;
    [SerializeField] GameObject colliderFlame;
    ParticleSystem particule;
    // Start is called before the first frame update
    void Start()
    {
        isFire = false;
        ammo = 200;
        particule = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        TriggerGun(isFire);
        /*if (flameParticles != null)
        {
            ParticleSystem.MainModule mainModule = flameParticles.main;

            // La direction des particules peut �tre contr�l�e via la vitesse du jet
            mainModule.startRotation = Mathf.Atan2(transform.forward.y, transform.forward.x);

            // Si tu veux que les particules continuent � avancer dans la direction de tir
            var velocityOverLifetime = flameParticles.velocityOverLifetime;
            velocityOverLifetime.x = 1f;  // Remplacer la valeur selon la direction
        }*/
    }

    public void TriggerGun(bool isTrigger)
    {
        if (isTrigger && ammo > 0)
        {
            particule.Play();
            colliderFlame.SetActive(true);
            ammo -= Time.deltaTime * 10;
        }
        else
        {
            ammo = (int)ammo;
            particule.Stop();
            colliderFlame.SetActive(false);
        }
    }

    public int GetAmmo()
    {
        return (int)ammo;
    }
}
