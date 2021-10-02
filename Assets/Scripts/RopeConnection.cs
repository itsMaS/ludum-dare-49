using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rope)), ExecuteAlways]
public class RopeConnection : MonoBehaviour
{
    [SerializeField] Rigidbody2D connectedRigidbody;
    [SerializeField] Transform visualConnection;
    [SerializeField] Rigidbody2D IKInfluence;

    Rope rope;
    private void Awake()
    {
        rope = GetComponent<Rope>();
    }
    public void Connect(PlayerController player, Vector2 point)
    {

    }

    private void Update()
    {
#if UNITY_EDITOR
        rope = GetComponent<Rope>();
        rope.GetLastPoint().position = visualConnection.position;
#endif
    }
    private void FixedUpdate()
    {
        rope.GetLastPoint().MovePosition(visualConnection.position);
    }
}
