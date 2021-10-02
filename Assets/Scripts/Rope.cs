using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class Rope : MonoBehaviour
{
    [SerializeField] List<Rigidbody2D> Points;

    [Header("Rope physics")]
    [SerializeField] float rigidity = 1;
    [SerializeField] float drag = 1;
    [SerializeField] int pointsPerSegment = 10;
    [SerializeField] float length = 3;

    LineRenderer lr;
    
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        Simulate(true);
    }
    private void Update()
    {
        if (!Application.isPlaying) lr = GetComponent<LineRenderer>();
        Simulate(false);
    }

    private void Simulate(bool start)
    {
        lr.positionCount = (Points.Count - 1) * pointsPerSegment;

        Points.ForEach(rb => rb.drag = drag);

        FillPoints(Points[0].position, Points[1].position, 0);

        for (int i = 1; i < Points.Count - 1; i++)
        {
            Rigidbody2D previous = Points[i - 1];
            Rigidbody2D next = Points[i + 1];
            Rigidbody2D current = Points[i];

            Vector2 targetPoint = Vector2.Lerp(previous.position, next.position, 0.5f);
            if(start)
            {
                current.position = targetPoint;
            }
            else
            {
                current.AddForce((targetPoint - current.position) * rigidity);
            }

            FillPoints(current.position, next.position, i);
        }
    }
    private void FillPoints(Vector2 p1, Vector2 p2, int segment)
    {
        for (int i = 0; i < pointsPerSegment; i++)
        {
            lr.SetPosition(segment * pointsPerSegment + i, Vector2.Lerp(p1,p2, (float)i/pointsPerSegment));
        }
    }
}
