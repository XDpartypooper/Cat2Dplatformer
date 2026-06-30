using UnityEngine;

public class CatRunnerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            //Destroy(gameObject);//gameover
            gameObject.SetActive(false);
            GameController.instance.GameOver();
        }
    }
}

    