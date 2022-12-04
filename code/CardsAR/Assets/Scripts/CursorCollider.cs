using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CardCollider"))
        {
            other.GetComponent<CursorTracker>().UnderCursor = true;

        }

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
            Vector3 p = other.ClosestPoint(gameObject.transform.position);
            Debug.DrawLine(other.bounds.center,
                           gameObject.transform.position, Color.green);
            Debug.DrawLine(gameObject.transform.position,
                           p, Color.cyan);

            other.GetComponent<CursorTracker>().CollisionPoint = p;

        }

    }
}
