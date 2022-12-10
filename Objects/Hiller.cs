using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiller : MonoBehaviour
{
    [SerializeField] private bool isdestr = true;
    [SerializeField] private float healthUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SendMessage("PlayerHill", healthUp);
            if (isdestr) Destroy(gameObject);
        }
    }
    
}
