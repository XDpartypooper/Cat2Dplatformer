using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 10;
    public int CurrentHP;
    public float pushForce = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHP = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {

        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage(int dmg)
    {
        CurrentHP -= dmg;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage();


                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
