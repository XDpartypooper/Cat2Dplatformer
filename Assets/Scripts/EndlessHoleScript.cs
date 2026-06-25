using UnityEngine;

public class EndlessHoleScript : MonoBehaviour
{
    public Transform spawnpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnpoint.position;
            }
        }
    }
}
