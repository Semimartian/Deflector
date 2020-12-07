using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IHittable, IExplodable
{

    private float explosionRadius;
    [SerializeField] private float explosionForce;

    // Start is called before the first frame update
    void Start()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        explosionRadius = sphere.radius;
        sphere.enabled = false;
    }


    private void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);


        for (int i = 0; i < colliders.Length; i++)
        {
            IExplodable explodable = colliders[i].gameObject.GetComponentInParent<IExplodable>();

            if (explodable != null && explodable != this)
            {
                explodable.Explode(explosionPosition, explosionForce);
            }
        }
        Destroy(gameObject);

    }

    public void Hit()
    {
        Explode();
    }

    public void Explode(Vector3 explosionPosition, float explosionForce)
    {
        Explode();
    }
}
