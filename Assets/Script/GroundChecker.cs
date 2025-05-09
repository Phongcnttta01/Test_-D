using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask layerMask;
    public bool IsGround;

    void Update()
    {
        Vector3 spherePosition = transform.position + Vector3.down * groundDistance;
        IsGround = Physics.CheckSphere(spherePosition, groundDistance, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * groundDistance;
        Gizmos.DrawSphere(end, groundDistance);
    }
}
