using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IHittable
{
    private Collider[] overlappingColliders;
   // [SerializeField] private CapsuleCollider capsuleCollider;
    private Vector3 deflectCapsuleHalfHeight;
    private float deflectCapsuleRadius;
   [SerializeField]  private Animator animator;

    private Transform myTransform;
    private Rigidbody rigidbody;

    private void Start()
    {
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
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
                //LEARN FROM THIS:
                // rigidbody.rotation =
                // myTransform.LookAt(projectile.transform.position);

                /*  Vector3 myYlessPosition = myTransform.position;
                  Vector3 projectileYlessPosition = projectile.transform.position;
                  myYlessPosition.y = 0;
                  projectileYlessPosition.y = 0;

                  Quaternion rotation = Quaternion.LookRotation(projectileYlessPosition - myYlessPosition);*/

                Vector3 direction = projectile.transform.position - myTransform.position;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rigidbody.rotation = rotation;


            }

        }

        string trigger = "Deflect";
        trigger += Random.Range(0, 2).ToString();
        animator.SetTrigger(trigger);
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
