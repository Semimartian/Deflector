using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IHittable
{
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Transform barrelPoint;
    [SerializeField] private Animator animator;
    private bool isShooting = false;
    private bool isAlive =true;
    private sbyte hp = 4;
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
        projectile.transform.position = barrelPoint.transform.position;
        projectile.transform.rotation = transform.rotation;
        isShooting = false;
    }

    public void Hit()
    {
        if (isAlive)
        {
            hp -= 1;
            if(hp <= 0)
            {
                Die();
            }
           
        }
      
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");
    }
}
