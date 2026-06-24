using UnityEngine;

public class WindAttackScript : MonoBehaviour
{
    public int AttackDmg = 10;

   

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Enemy"))
        {

            Enemy enemy = other.GetComponent<Enemy>();
            Debug.Log("enemy "+other.name);
            if (enemy != null)
            {
                enemy.DealDamage(AttackDmg);
            }

        }
    }
}
