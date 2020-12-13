using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IHittable
{
    //TODO: this class looks too much like Shooter

    [SerializeField] private Animator myAnimator;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Transform barrelPoint;
    [SerializeField] private Transform gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void WakeUp()
    {
        myAnimator.SetBool("IsShooting", true);
        gunAnimator.SetBool("IsShooting", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            WakeUp();
        }
    }

    private void StartShooting()
    {

    }

    public void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        /*if (isAlive)
        {
             hp -= 1;
             if(hp <= 0)
            {
                Die();
                ragdollHandler.EnableRagdoll();
                ragdollHandler.AddForceAt(hitForce, hitPosition);
                gun.isKinematic = false;
                gun.AddForceAtPosition(hitForce, hitPosition, ForceMode.Impulse);

            }

        }*/
    }

    public void ReleaseProjectile()
    {
        Debug.Log("Shoot");
        Projectile projectile = Instantiate(projectilePreFab);
        Vector3 projectilePosition = barrelPoint.transform.position;
        projectile.transform.position = projectilePosition;
        projectile.transform.forward = -Vector3.forward;
        SoundManager.PlayOneShotSoundAt(SoundNames.BlasterShot, projectilePosition);
    }
}
