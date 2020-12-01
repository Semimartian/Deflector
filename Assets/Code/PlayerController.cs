using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IHittable
{
    private Collider[] overlappingColliders;
   // [SerializeField] private CapsuleCollider capsuleCollider;
    private Vector3 deflectCapsuleHalfHeight;
    private float deflectCapsuleRadius;

    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
        overlappingColliders = new Collider[32];

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        deflectCapsuleHalfHeight = Vector3.up * (capsuleCollider.height / 2);
        deflectCapsuleRadius = capsuleCollider.radius;
        capsuleCollider.enabled = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryDeflect();
        }
    }
     
    private void TryDeflect()
    {
        Vector3 position = myTransform.position;
        Vector3 top = position + deflectCapsuleHalfHeight;
        Vector3 bottom = position - deflectCapsuleHalfHeight;

        int colliderCount = 
            Physics.OverlapCapsuleNonAlloc(bottom, top, deflectCapsuleRadius, overlappingColliders);
        if(colliderCount >= overlappingColliders.Length)
        {
            Debug.LogError("NOT GOOD");
        }
        for (int i = 0; i < colliderCount; i++)
        {
            Projectile projectile = overlappingColliders[i].GetComponentInParent<Projectile>();
            if (projectile != null)
            {
                projectile.Deflect();
            }

        }
    }

    public void Hit()
    {
        Debug.Log("I'M HIT!");

    }
    /* private void OnCollisionEnter(Collision collision)
     {
         Projectile projectile = collision.gameObject.GetComponentInParent<Projectile>();
         if (projectile != null)
         {
             projectile.Hit();
         }
     }*/


}
