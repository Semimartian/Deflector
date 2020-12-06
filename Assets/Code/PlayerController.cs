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
    private bool isRunning = false;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;
    [SerializeField] private float accelerationPerSecond;
    [SerializeField] private float deaccelerationPerSecond;

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

        if (Input.GetKeyDown(KeyCode.W))
        {
            isRunning = true;
            animator.SetBool("IsRunning",true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isRunning = false;
            animator.SetBool("IsRunning", false);

        }
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        if (isRunning)
        {
            currentSpeed += accelerationPerSecond * deltaTime;

        }
        else
        {
            currentSpeed -= deaccelerationPerSecond * deltaTime;
        }

        if (currentSpeed < 0 )
        {
            currentSpeed = 0;
        }
        else if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        if (currentSpeed != 0)
        {
            Vector3 newPosition = rigidbody.position +
                 ((currentSpeed * deltaTime) * myTransform.forward);
            rigidbody.MovePosition(newPosition);
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
