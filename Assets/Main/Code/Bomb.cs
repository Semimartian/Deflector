using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IHittable, IExplodable
{

    private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] float explosionUpwardModifier;
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
                explodable.Explode(explosionPosition, explosionForce, explosionRadius, explosionUpwardModifier);
            }
        }
        SoundManager.PlayOneShotSoundAt(SoundNames.Explosion, explosionPosition);
        Destroy(gameObject);

    }

    public void Explode(Vector3 explosionPosition, float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        Explode();
    }

    public void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        Explode();

    }
}
