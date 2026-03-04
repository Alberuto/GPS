using UnityEngine;
public class MHMonsterHealth : MonoBehaviour {
    public int maxHealth = 3;
    private int currentHealth;

    void Start() { currentHealth = maxHealth; }
    public void TakeDamage(int damage) {

        currentHealth -= damage;
        Debug.Log($"MH Vida: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0) Destroy(gameObject);
    }
}