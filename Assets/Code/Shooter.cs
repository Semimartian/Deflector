using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Transform barrelPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Projectile projectile = Instantiate(projectilePreFab);
        projectile.transform.position = barrelPoint.transform.position;
        projectile.transform.rotation = transform.rotation;
    }
}
