using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor 
{
    public Vector3 positionOffset;
    public Quaternion rotation;
    public CameraAnchor(Transform anchorTransform)
    {
        positionOffset = anchorTransform.localPosition;
        rotation = anchorTransform.rotation;
    }
}
