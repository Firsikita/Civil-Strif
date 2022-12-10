using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private Animator an;
    public ParticleSystem leafParticle;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.position.x - col.transform.position.x > 0)
        {
            PlayLeft();
        }
        else
        {
            PlayRight();
        }
    }

    private void PlayRight()
    {
        an.Play("MovingGrassR");
    }
    private void PlayLeft()
    {
        an.Play("MovingGrassL");
    }
    private void Damage(float damage)
    {
        Destroy(gameObject);
        Instantiate(leafParticle, transform.position, Quaternion.identity);
    }
}
