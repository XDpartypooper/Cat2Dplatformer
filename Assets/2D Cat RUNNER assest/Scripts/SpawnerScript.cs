using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [Header("Spawn obj")]
    [SerializeField] private GameObject[] Obstacles;

    [Header("Spawn info")]
    public float ObstacleSpeed = 1f;
    public float obstacleSpawnTime = 2f;
    [SerializeField] private float TimeUntilObstacleSpawn;
    


    private void Update()
    {
        if(!GameController.instance.isPlaying) return;//if not playing return

        SpawnLoop();
    }

    private void SpawnLoop()
    {
        TimeUntilObstacleSpawn += Time.deltaTime;

        if (TimeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            Spawn();
            TimeUntilObstacleSpawn = 0f;
        }
    }

    private void Spawn()
    {
        if (Obstacles == null) { return; }

        GameObject obstacleToSpawn = Obstacles[Random.Range(0, Obstacles.Length)];

        GameObject SpawnedObstacles = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);//

        Rigidbody2D ObstacleRB = SpawnedObstacles.GetComponent<Rigidbody2D>();

        ObstacleRB.linearVelocity = Vector2.left * ObstacleSpeed;
    }
}
