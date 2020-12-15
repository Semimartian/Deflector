using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : Bomb
{
    [SerializeField] private Material mat;
    [SerializeField] private Color colourAtFullHealth;
    [SerializeField] private Color colourAtOneHealth;
    private byte healthAtStart;
    [SerializeField] private string colourName;

    private void Start()
    {
        healthAtStart = hp;
        UpdateGraphics();
    }

    public override void Hit(Vector3 hitPosition, Vector3 hitForce)
    {
        base.Hit(hitPosition, hitForce);
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {

        float t = ((float)(hp - 1) / (float)(healthAtStart - 1));

        Color myColour =
            Color.Lerp(colourAtOneHealth, colourAtFullHealth, t);
       // mat.color = myColour;

        mat.SetColor(colourName, myColour);
    }
}
