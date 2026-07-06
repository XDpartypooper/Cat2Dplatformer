using UnityEngine;

public class ObstacleClearScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.transform.parent.gameObject);
        }
        Destroy(collision.transform.parent.gameObject);
    }
}
