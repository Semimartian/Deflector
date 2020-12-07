using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IHittable,IExplodable
{
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Transform barrelPoint;
    [SerializeField] private Animator animator;
    private bool isShooting = false;
    private bool isAwake = false;
    private bool isAlive =true;
    public bool IsAlive
    {
        get { return isAlive; }
    }
    //private sbyte hp = 4;
    [SerializeField] private bool lookAtPlayer;
    [SerializeField] private float minShootInterval;
    [SerializeField] private float maxShootInterval;

    private Transform myTransform;
    [SerializeField] private Collider collider;
    [SerializeField] private float shootingDistanceFromPlayer;
    [SerializeField] private RagdollHandler ragdollHandler;
    [SerializeField] private Transform[] heldItems;
    private void Start()
    {
        ragdollHandler.DisableRagdoll();
        myTransform = transform;

        if (GameManager.allowAutomaticShooting)
        {
            Invoke("ShootRoutine", Random.Range(1f, 4f));

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Input.GetMouseButtonDown(1))
            {
                TryShoot();
            }
        }
    }

    private void FixedUpdate()
    {
        if (lookAtPlayer && isAlive)
        {
            Vector3 direction = GameManager.playerPosition - myTransform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            myTransform.rotation = rotation;
        }   
    }

    private void TryShoot()
    {
        if (!isShooting)
        {
            animator.SetTrigger("Shoot");
            isShooting = true;
        }
    }

    public void ReleaseProjectile()
    {
        Projectile projectile = Instantiate(projectilePreFab);
        Vector3 projectilePosition = barrelPoint.transform.position;
        projectile.transform.position = projectilePosition;
        projectile.transform.rotation = transform.rotation;
        SoundManager.PlayOneShotSoundAt(SoundNames.BlasterShot, projectilePosition);
        isShooting = false;
    }

    public void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        if (isAlive)
        {
           /* hp -= 1;
            if(hp <= 0)*/
            {
                Die();
                ragdollHandler.EnableRagdoll();
                ragdollHandler.AddForceAt(hitForce, hitPosition);
            }

        }
    }

    public void Explode(Vector3 explosionPosition, float explosionForce, float explosionRadius)
    {
        Die();
        ragdollHandler.EnableRagdoll();
        ragdollHandler.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
    }


    private void Die()
    {
        isAlive = false;
        //animator.SetTrigger("Die");
        collider.enabled = false;
        SoundManager.PlayOneShotSoundAt(SoundNames.Wilhelm, myTransform.position);

        GameManager.CheckWaveState();
    }

    private void ShootRoutine()
    {
        if (isAwake)
        {
            /*float distanceFromPlayer = Vector3.Distance(myTransform.position, GameManager.playerPosition);
            if (distanceFromPlayer < shootingDistanceFromPlayer)*/
            {
                TryShoot();
            }
        }

        Invoke("ShootRoutine", Random.Range(minShootInterval, maxShootInterval));
    } 

    public void Awaken()
    {
        isAwake = true;
    }

    private void ReleaseHeldItems()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            heldItems[i].SetParent(null);
        }
    }

}
