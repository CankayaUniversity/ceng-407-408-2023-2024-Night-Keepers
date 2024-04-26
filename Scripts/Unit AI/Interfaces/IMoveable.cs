using UnityEngine;

public interface IMoveable
{
    void MoveUnit(Vector3 velocity);
    void StopUnit(bool stop);
}