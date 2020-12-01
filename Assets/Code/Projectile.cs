using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rigidbody;
    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        myTransform = transform;
    }

    private void FixedUpdate()
    {

        /*Vector3 velocity = myTransform.forward * speed * Time.fixedDeltaTime;
        rigidbody.velocity = velocity;*/
        Vector3 movement = myTransform.forward * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Hit()
    {
        Destroy(gameObject);
    }

    internal void Deflect()
    {
        //This method is dumb
        Vector3 lookAtPosition = (-myTransform.forward) + myTransform.position;
        myTransform.LookAt(lookAtPosition);
    }


    private void OnTriggerEnter(Collider other)
    {
        /* Projectile projectile = other.gameObject.GetComponentInParent<Projectile>();
         if (projectile != null)
         {
             projectile.Hit();
         }*/
        bool hitSomething = false;
        IHittable hittable = other.gameObject.GetComponentInParent<IHittable>();

        if (hittable != null)
        {
            hittable.Hit();
            hitSomething = true;
        }
        else if(other.gameObject.layer == 0)
        {
            hitSomething = true; 
        }

        if (hitSomething)
        {
            Hit();
        }
    }
}
