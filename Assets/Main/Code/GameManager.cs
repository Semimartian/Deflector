using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public Shooter[] shootersToKill;
    }
    public static Vector3 playerPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController player;

    private static GameManager instance;
    public static bool allowAutomaticShooting =true;
    [SerializeField] private Wave[] waves;
    private static int waveIndex;
    private static bool waitingForNextWave = false;
   // Start is called before the first frame update
   void Start()
    {
        instance = this;
        /* for (int j = 0; j < instance.waves.Length; j++)
         {
             Shooter[] shooters = instance.waves[j].shootersToKill;
             for (int i = 0; i < shooters.Length; i++)
             {
                 shooters[i].Awaken();
             }
         }*/
        // AwakeCurrentWave();
        waveIndex = -1;
        CheckWaveState();
        Routine();
    }

    private void FixedUpdate()
    {
       // Routine();
    }

    private void Routine()
    {
        playerPosition = instance.playerTransform.position;

        Invoke("Routine", 0.05f);
    }

    public static void CheckWaveState()
    {
        if(waveIndex >-1 && waveIndex < instance.waves.Length)
        {
            Shooter[] shooters = instance.waves[waveIndex].shootersToKill;
            for (int i = 0; i < shooters.Length; i++)
            {
                if (shooters[i].IsAlive)
                {
                    return;
                }
            }

        }

        waitingForNextWave = true;
        instance.player.StartRunning();
    }

    public static void StartNextWave()
    {
        Debug.Log("Next Wave!");
        waveIndex++;
        instance.player.StopRunning();
        AwakeCurrentWave();

    }

    private static void AwakeCurrentWave()
    {
        Shooter[] shooters = instance.waves[waveIndex].shootersToKill;
        for (int i = 0; i < shooters.Length; i++)
        {
            shooters[i].Awaken();
        }
    }
}
