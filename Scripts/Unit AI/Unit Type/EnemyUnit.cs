
public class EnemyUnit : Unit
{
    public override void Die()
    {
        base.Die();
        EnemySpawnManager.Instance.DecreaseAliveEnemyCount();
    }
}