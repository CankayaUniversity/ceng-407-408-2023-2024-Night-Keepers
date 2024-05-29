using System;

public class PlayerUnit : Unit
{
    public static event Action<Unit> OnPlacedUnitDied;

    public override void Die()
    {
        OnPlacedUnitDied?.Invoke(this);
        base.Die();
    }
}