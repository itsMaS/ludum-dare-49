using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float strokeForce;
    [SerializeField] float turningSensitivity = 10;
    [SerializeField] float swimThreshold = 1;

    [SerializeField] Camera cam;

    Coroutine strokeCouroutine;
    Rigidbody2D rb;
    Animator an;

    Vector2 target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 movementVector = target - (Vector2)transform.position;

        if(Input.GetMouseButton(0))
        {
            target = pointerPos;
            transform.up = Vector3.Lerp(transform.up, movementVector, turningSensitivity * Time.deltaTime);
        }
        else
        {
            transform.up = Vector3.Lerp(transform.up, (Vector3)pointerPos - transform.position, turningSensitivity*0.5f * Time.deltaTime);
            target = transform.position;
        }
        an.SetBool("Swimming", movementVector.magnitude > swimThreshold);

    }

    private void StartStroke()
    {
        strokeCouroutine = StartCoroutine(Stroke());
    }
    private void EndStroke()
    {
        if(strokeCouroutine != null)
        {
            StopCoroutine(strokeCouroutine);
            strokeCouroutine = null;
        } 
    }

    IEnumerator Stroke()
    {
        while(true)
        {
            rb.AddForce(strokeForce* transform.up);
            yield return new WaitForFixedUpdate();
        }
    }
}
