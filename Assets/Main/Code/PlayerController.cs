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

    [SerializeField] private Transform mouseRayMarker;
    [SerializeField] private Camera camera;
    //[SerializeField] private float groundY;

    private int hits = 0;
    [SerializeField] private UIText hitsText;

    private void Start()
    {
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        overlappingColliders = new Collider[32];

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        deflectCapsuleHalfHeight = Vector3.up * (capsuleCollider.height / 2);
        deflectCapsuleRadius = capsuleCollider.radius;
        capsuleCollider.enabled = false;

        UpdateUI();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRunning)
        {
            TryDeflect();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartRunning();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            StopRunning();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SoundManager.PlayOneShotSoundAt(SoundNames.Explosion, myTransform.position);

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SoundManager.PlayOneShotSoundAt(SoundNames.OldExplosion, myTransform.position);

        }
    }

    public void StartRunning()
    {
        isRunning = true;
        animator.SetBool("IsRunning", true);
        rigidbody.rotation = Quaternion.identity;

    }

    public void StopRunning()
    {
        isRunning = false;
        animator.SetBool("IsRunning", false);
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

        Vector3 lookAtPosition = MouseToGroundPlane(Input.mousePosition);
        Vector3 direction = //projectile.transform.position - myTransform.position;
               lookAtPosition - position; 
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rigidbody.rotation = rotation;

        string trigger = "Deflect";
        trigger += Random.Range(0, 2).ToString();
        animator.SetTrigger(trigger);
        SoundManager.PlayOneShotSoundAt(SoundNames.LightSaberSwing, position);


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

                //LEARN FROM THIS:
                // rigidbody.rotation =
                // myTransform.LookAt(projectile.transform.position);

                /*  Vector3 myYlessPosition = myTransform.position;
                  Vector3 projectileYlessPosition = projectile.transform.position;
                  myYlessPosition.y = 0;
                  projectileYlessPosition.y = 0;

                  Quaternion rotation = Quaternion.LookRotation(projectileYlessPosition - myYlessPosition);*/

                projectile.Deflect(lookAtPosition);

            }
        }
    }


    private void UpdateUI()
    {
        hitsText.UpdateText("HITS: " + hits.ToString());
    }

    private Vector3 MouseToGroundPlane(Vector3 mousePosition)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;
        float groundY = 0;
        if  (Physics.Raycast(ray, out raycastHit))
        {
            groundY = raycastHit.point.y;
        }

        float rayLength = (ray.origin.y - groundY) / ray.direction.y;

        Debug.DrawLine(ray.origin, ray.origin - (ray.direction * rayLength), Color.red);

        Vector3 results = ray.origin - (ray.direction * rayLength);
        mouseRayMarker.position = results;
        return results;
    }

    public void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        Debug.Log("I'M HIT!");
        hits += 1;
        UpdateUI();
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
