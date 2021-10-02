using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rope))]
public class RopeConnection : MonoBehaviour
{
    Rope rope;
    private void Awake()
    {
        rope = GetComponent<Rope>();
    }
    public void Connect(PlayerController player, Vector2 point)
    {

    }
}
