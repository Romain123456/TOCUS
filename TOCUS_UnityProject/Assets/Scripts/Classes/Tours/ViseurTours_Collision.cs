using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViseurTours_Collision : MonoBehaviour
{
    private Tours parentTourScript;

    private void Start()
    {
        parentTourScript = this.transform.parent.parent.GetComponent<Tours>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ennemi" && !parentTourScript.tirLaunch)
        {
            parentTourScript.tourTarget = collision.transform;
            StartCoroutine(parentTourScript.TourTirOnTriggerCooldown(parentTourScript.cadenceTir));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ennemi")
        {
            parentTourScript.tourTarget = collision.transform;
            if (!parentTourScript.tirLaunch && !parentTourScript.tirLaunch)
            {
                StartCoroutine(parentTourScript.TourTirOnTriggerCooldown(parentTourScript.cadenceTir));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ennemi")
        {
            parentTourScript.tourTarget = null;
        }
    }
}
