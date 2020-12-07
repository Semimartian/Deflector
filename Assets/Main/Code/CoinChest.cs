using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinChest : MonoBehaviour, IHittable
{

    public void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        Break();
    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
