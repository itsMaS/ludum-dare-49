using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), ExecuteAlways]
public class RagdollPhysicsPoint : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float correctionForce;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        transform.position = target.position;
    }
    private void Update()
    {
        if(!Application.isPlaying) transform.position = target.position;
    }
    private void FixedUpdate()
    {
        Vector3 correctionDirection = target.position - transform.position;
        rb.AddForce(correctionDirection*correctionForce);
    }
}
