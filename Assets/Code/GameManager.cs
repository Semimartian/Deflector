using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Vector3 playerPosition;
    [SerializeField] private Transform playerTransform;
    private static GameManager instance;
    public static bool allowAutomaticShooting =true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        playerPosition = instance.playerTransform.position;
    }
}
