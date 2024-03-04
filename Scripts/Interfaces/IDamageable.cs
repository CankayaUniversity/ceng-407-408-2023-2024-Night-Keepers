
public interface IDamageable
{
    int CurrentHealth { get; set; }

    void TakeDamage(int damageAmount);

    void Die();
}