using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Transform myTransform;


    private void Start()
    {
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        myTransform.position =
            target.position + offset;
    }
}
