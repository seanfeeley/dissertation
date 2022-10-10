using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public Vector3 _targetPosition = new Vector3(0,0,0);
    private bool _animationRunning = false;

    public void SetDestination(Vector3 posistion)
    {
        _targetPosition = posistion;
        _animationRunning = true;
    }
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animationRunning) return;

        float speed = 0.8f;
        float step = speed * Time.deltaTime;
        Vector3 direction = (_targetPosition - gameObject.transform.position).normalized;
        gameObject.transform.Translate(Vector3.forward * step, Space.Self);
        float angle = Vector3.SignedAngle(gameObject.transform.forward, direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 8f);
        if (Vector3.Distance(gameObject.transform.position, _targetPosition) < 0.1f) _animationRunning = false;
    }
}
