using UnityEngine;

public class TreeFallScript : MonoBehaviour
{
    public bool PlayerInside;
    [SerializeField] private PlayerController PC;
    [SerializeField] private Animator treeAni;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PC = FindAnyObjectByType<PlayerController>();
        treeAni = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerInside) return;//not inside
    }

    private void StartAction()
    {
        if (PlayerInside == true)
        {
            Debug.Log("Action start of -" + this.name);
             
            treeAni.Play("TreeDrop");
            this.GetComponent<BoxCollider2D>().enabled = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            if (PC != null)
            {
                PlayerInside = true;
                collision.SendMessage("SetCATNPC", this.gameObject, SendMessageOptions.DontRequireReceiver);
                //have to change this
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            if (PC != null)
            {
                PlayerInside = false;
                collision.SendMessage("RemoveCATNPC", SendMessageOptions.DontRequireReceiver);
             
            }
        }
    }
}
