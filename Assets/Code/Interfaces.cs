using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    void Hit();
}

public interface IExplodable
{
    void Explode(Vector3 explosionPosition, float explosionForce);
}