using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CardCollider"))
        {
            other.GetComponent<CursorTracker>().UnderCursor = false;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CardCollider"))
        {
            other.GetComponent<CursorTracker>().UnderCursor = true;

        }

    }
}
