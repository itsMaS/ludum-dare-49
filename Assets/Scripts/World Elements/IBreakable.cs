using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable
{
    void Break(Vector2 velocity, Vector2 collisionPoint);
}
