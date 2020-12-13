using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform myTransform;
    private CameraAnchor anchor;

    [SerializeField] private Transform target;
    [SerializeField] private Transform actionAnchorTransform;
    [SerializeField] private Transform runningAnchorTransform;
    private CameraAnchor actionAnchor;
    private CameraAnchor runningAnchor;
    [SerializeField] private AnimationCurve transitionCurve;
    private bool isTransitioning = false;

    private void Awake()
    {
        actionAnchor = new CameraAnchor(actionAnchorTransform);
        runningAnchor = new CameraAnchor(runningAnchorTransform);

        anchor = actionAnchor;

        myTransform = transform;
    }

    private void FixedUpdate()
    {
        if (!isTransitioning)
        {
            myTransform.position = 
              (target.position + anchor.positionOffset);
        }
    }

    public void TransitionToRunningState()
    {
        anchor = runningAnchor;
        StartCoroutine(TransitionCoroutine());
    }

    public void TransitionToActionState()
    {
        anchor = actionAnchor;
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        isTransitioning = true;
        Vector3 originalPosition = myTransform.position;
        Quaternion originalRotation = myTransform.rotation;

        float time = 0;
        float endTime = transitionCurve.keys[transitionCurve.length - 1].time;

        while (time < endTime)
        {
            //float deltaTime = Time.deltaTime;
            time += Time.fixedDeltaTime;

            float t = transitionCurve.Evaluate(time);
            Vector3 anchorPosition = (target.position + anchor.positionOffset);
            myTransform.position = Vector3.Lerp(originalPosition, anchorPosition, t);
            // Vector3.MoveTowards(transform.position, target.position, speed * deltaTime);
            myTransform.rotation = Quaternion.Lerp(originalRotation, anchor.rotation, t);
            //Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed * deltaTime);

            yield return new WaitForFixedUpdate();

        }

        isTransitioning = false;

    }
}
