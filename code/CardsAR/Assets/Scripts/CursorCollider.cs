using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SelectableCard")
        {

            PlayerManager.Instance.StartHighlightingCard(other.gameObject);
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SelectableCard")
        {
           
             PlayerManager.Instance.StopHighlightingcard(other.gameObject);
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
 

    }
}
