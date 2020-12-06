using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinChest : MonoBehaviour, IHittable
{
    public void Hit()
    {
        Break();
    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
