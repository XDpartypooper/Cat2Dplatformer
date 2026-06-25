using UnityEngine;

public class FishItemScript : MonoBehaviour
{
    public PlayerController PC;
    private void Start()
    {
        PC = FindAnyObjectByType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            if (PC != null)
            {
                PC.EndofGame();
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
}
