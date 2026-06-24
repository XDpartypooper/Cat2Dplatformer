using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Start()
    {
 
        gameObject.GetComponent<Renderer>().enabled = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}

