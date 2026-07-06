using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [Header("Spawn obj - static object")]
    [SerializeField] private GameObject[] Obstacles1;
    [Header("Spawn obj - flying object")]
    [SerializeField] private GameObject[] Obstacles2;

    [Header("Spawn info- static")]

    public float ObstacleSpeed = 6f;
    public float OstacleSpawnTime = 2f;
    private float CurrObstacleSpeed;
    private float CurrbstacleSpawnTime;
    [SerializeField] private float TimeUntilObstacleSpawn;


    [Header("Spawn info- Projectile object")]
    public float FLYINGObstacleSpeed = 7f;
    public float FLYINGObstacleSpawnTime = 1.2f;
    private float currFLYINGObstacleSpeed;
    private float CurrFLYINGObstacleSpawnTime;
    [SerializeField] private float TimeUntilFLYINGObstacleSpawn;

    [Header("Spawn factor & details")]
    [Range(0, 1)] public float SpawnTimefactor = 0.1f;
    [Range(0, 1)] public float Speedfactor = 0.2f;

    [SerializeField] private Transform ObstacleParent;

    private float TimeAlive;

    private void Start()
    {
        TimeAlive = 1f;
        CurrObstacleSpeed = ObstacleSpeed;
        CurrbstacleSpawnTime = OstacleSpawnTime;
        currFLYINGObstacleSpeed = FLYINGObstacleSpeed;
        CurrFLYINGObstacleSpawnTime = FLYINGObstacleSpawnTime;
    }


    private void Update()
    {
        if (!GameController.instance.isPlaying) { return; }//if not playing return
        TimeAlive += Time.deltaTime;
        IncreaseSpeed();
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        TimeUntilObstacleSpawn += Time.deltaTime;
        TimeUntilFLYINGObstacleSpawn += Time.deltaTime;

        if (TimeUntilObstacleSpawn >= CurrbstacleSpawnTime)
        {
            Spawn();
            TimeUntilObstacleSpawn = 0f;
        }

        if (TimeUntilFLYINGObstacleSpawn >= CurrFLYINGObstacleSpawnTime)
        {
            SpawnFLYING();
            TimeUntilFLYINGObstacleSpawn = 0f;
        }
    }

    private void IncreaseSpeed()
    {
        CurrObstacleSpeed = ObstacleSpeed * Mathf.Pow(TimeAlive, Speedfactor);
        CurrbstacleSpawnTime = OstacleSpawnTime / Mathf.Pow(TimeAlive, SpawnTimefactor);

        currFLYINGObstacleSpeed = FLYINGObstacleSpeed * Mathf.Pow(TimeAlive, Speedfactor);
        CurrFLYINGObstacleSpawnTime = FLYINGObstacleSpawnTime / Mathf.Pow(TimeAlive, SpawnTimefactor);
    }

    public void ClearSpawnedObstacles()
    {
        foreach (Transform child in ObstacleParent)
        {
            Destroy(child.gameObject);
        }
        TimeAlive = 1f;
        CurrObstacleSpeed = ObstacleSpeed;
        CurrbstacleSpawnTime = OstacleSpawnTime;
    }

    private void Spawn()
    {
        if (Obstacles1 == null) { return; }

        GameObject obstacleToSpawn = Obstacles1[Random.Range(0, Obstacles1.Length)];

        GameObject SpawnedObstacles = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        SpawnedObstacles.transform.parent = ObstacleParent;

        Rigidbody2D ObstacleRB = SpawnedObstacles.GetComponent<Rigidbody2D>();

        ObstacleRB.linearVelocity = Vector2.left * CurrObstacleSpeed;
    }

    private void SpawnFLYING()
    {
        if (Obstacles2 == null) { return; }

        GameObject obstacleToSpawn2 = Obstacles2[Random.Range(0, Obstacles2.Length)];

        Vector3 FlyingspawnPosition = new Vector3(transform.position.x, transform.position.y + Random.Range(1.7f, 3f), transform.position.z);

        GameObject SpawnedObstacles2 = Instantiate(obstacleToSpawn2, FlyingspawnPosition, Quaternion.identity);
        
        SpawnedObstacles2.transform.parent = ObstacleParent;

        Rigidbody2D ObstacleRB2 = SpawnedObstacles2.GetComponent<Rigidbody2D>();

        ObstacleRB2.linearVelocity = Vector2.left * currFLYINGObstacleSpeed;
    }
}
