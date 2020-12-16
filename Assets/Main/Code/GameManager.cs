using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public StickManEnemy[] enemiesToKill;
    }
    public static Vector3 playerPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController player;
    [SerializeField] private MainCamera cameraController;

    private static GameManager instance;
    public static bool allowAutomaticShooting =true;
    [SerializeField] private Wave[] waves;
    private static int waveIndex;
    public static StickManEnemy[] GetCurrentWaveEnemies()
    {
        return instance.waves[waveIndex].enemiesToKill;
    }
    private static bool waitingForNextWave = false;
    [Header("BossFight")]
    [SerializeField] private Boss boss;
    private static bool inBossFight = false;
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

    private void Routine()
    {
        playerPosition = instance.playerTransform.position;

        Invoke("Routine", 0.05f);
    }

    public static void CheckWaveState()
    {
        if (inBossFight)
        {

        }
        else
        {
            if ( waveIndex > -1 && waveIndex < instance.waves.Length)
            {
                /* Shooter[] shooters = instance.waves[waveIndex].shootersToKill;
                 for (int i = 0; i < shooters.Length; i++)
                 {
                     if (shooters[i].IsAlive)
                     {
                         return;
                     }
                 }*/

                StickManEnemy[] enemies = instance.waves[waveIndex].enemiesToKill;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].IsAlive)
                    {
                        return;
                    }
                }
            }

            waitingForNextWave = true;
            instance.player.StartRunning();
            instance.cameraController.TransitionTo(CameraStates.Running);

        }

    }

    public static void StartNextWave(bool bossTrigger)
    {

        Debug.Log("Next Wave!");
        waveIndex++;

        if (!bossTrigger)
        {
            //return;
        }

        instance.player.StopRunning();
        AwakeCurrentWave();
        if (bossTrigger)
        {
            inBossFight = true;
            instance.StartCoroutine(instance.PlayBossScene());
            instance.cameraController.TransitionTo(CameraStates.Boss);
        }
        else
        {
            instance.cameraController.TransitionTo(CameraStates.Action);
        }
    }

    private static void AwakeCurrentWave()
    {
        /*Shooter[] shooters = instance.waves[waveIndex].shootersToKill;
        for (int i = 0; i < shooters.Length; i++)
        {
            shooters[i].Awaken();
        }*/

        StickManEnemy[] enemies = instance.waves[waveIndex].enemiesToKill;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Awaken();
        }
    }

    private IEnumerator PlayBossScene()
    {
        yield return new WaitForSeconds(0.3f);
        boss.WakeUp();
        yield return new WaitForSeconds(1.85f);
        instance.player.FRENZY();
    }

    public static void OnBossDeath()
    {
        instance.player.EndFrenzy();

    }
}
