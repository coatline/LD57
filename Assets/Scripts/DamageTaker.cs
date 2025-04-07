using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public event System.Action<float> TookDamage;
    public event System.Action Died;

    [SerializeField] SoundType damageSound;
    [SerializeField] SoundType deathSound;
    [SerializeField] float maxHealth;

    float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            SoundPlayer.I.PlaySound(deathSound, transform.position);
            Died?.Invoke();
        }
        else
            SoundPlayer.I.PlaySound(damageSound, transform.position);
    }
}
