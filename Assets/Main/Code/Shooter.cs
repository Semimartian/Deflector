using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IHittable,IExplodable
{
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Transform gunAnchor;
    [SerializeField] private Rigidbody gun;
    [SerializeField] private Transform barrelPoint;
    [SerializeField] private Animator animator;
    private bool isShooting = false;
    private bool isAwake = false;
    private bool isAlive =true;
    public bool IsAlive
    {
        get { return isAlive; }
    }
    [SerializeField] private bool lookAtPlayer;
    [SerializeField] private float minShootInterval;
    [SerializeField] private float maxShootInterval;

    private Transform myTransform;
    [SerializeField] private Collider collider;
    [SerializeField] private RagdollHandler ragdollHandler;

    //private sbyte hp = 4;
    //[SerializeField] private float shootingDistanceFromPlayer;
    //[SerializeField] private Transform[] heldItems;

    private void Start()
    {
        ragdollHandler.DisableRagdoll();
        myTransform = transform;

        if (GameManager.allowAutomaticShooting)
        {
            Invoke("ShootRoutine", Random.Range(1f, 4f));

        }

        gun.transform.SetParent(null);
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
            gun.position = gunAnchor.position;

            Vector3 playerPosition = GameManager.playerPosition;
            Vector3 gunDirection = playerPosition - gun.position;
            gun.rotation = Quaternion.LookRotation(gunDirection);

            Vector3 myDirection = playerPosition - myTransform.position;
            myDirection.y = 0;
            myTransform.rotation = Quaternion.LookRotation(myDirection);
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
        projectile.transform.rotation = gun.rotation;
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
                gun.isKinematic = false;
                gun.AddForceAtPosition(hitForce, hitPosition, ForceMode.Impulse);

            }

        }
    }

    public void Explode(Vector3 explosionPosition, float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        Die();
        ragdollHandler.EnableRagdoll();
        ragdollHandler.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, explosionUpwardModifier);

        gun.isKinematic = false;
        gun.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, explosionUpwardModifier);


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



}
