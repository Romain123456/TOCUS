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
        if(collision.tag == "Ennemi" && parentTourScript.tourTarget == null)
        {
            parentTourScript.tourTarget = collision.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ennemi" && parentTourScript.tourTarget == null)
        {
            parentTourScript.tourTarget = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ennemi" && parentTourScript.tourTarget != null)
        {
            parentTourScript.tourTarget = null;
        }
    }
}
