using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] List<Rigidbody2D> Points;

    [Header("Rope physics")]
    [SerializeField] float rigidity = 1;
    [SerializeField] float drag = 1;
    [SerializeField] int pointsPerSegment = 10;
    public float length = 5;

    LineRenderer lr;
    float calculatedDistance = 0;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        InitialSetup();
    }
    private void Update()
    {
        Simulate();
    }

    public void InitialSetup()
    {
        for (int i = 1; i < Points.Count-1; i++)
        {
            Points[i].transform.position = Vector2.Lerp(Points[0].position, Points[Points.Count-1].position, (float)i / Points.Count);
        }

        lr.positionCount = Points.Count - 1;
        for (int i = 1; i < Points.Count - 1; i++)
        {
            Points[i].position = Vector3.Lerp(Points[0].position, Points[Points.Count-1].position, (float)i/Points.Count);
        }
    }
    private void Simulate()
    {
        calculatedDistance = 0;

        lr.positionCount = (Points.Count - 1) * pointsPerSegment;

        Points.ForEach(rb => rb.drag = drag);

        FillPoints(Points[0].position, Points[1].position, 0);

        calculatedDistance += Vector2.Distance(Points[0].position, Points[1].position);

        for (int i = 1; i < Points.Count - 1; i++)
        {
            Rigidbody2D previous = Points[i - 1];
            Rigidbody2D next = Points[i + 1];
            Rigidbody2D current = Points[i];
            calculatedDistance += Vector2.Distance(current.position, next.position);

            Vector2 targetPoint = Vector2.Lerp(previous.position, next.position, 0.5f);
            current.AddForce((targetPoint - current.position)* rigidity);

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

    public float GetTension()
    {
        return calculatedDistance / length;
    }
    public Rigidbody2D GetFirstPoint()
    {
        return Points[0];
    }
    public Rigidbody2D GetLastPoint()
    {
        return Points[Points.Count-1];
    }
    public void AddBreakTurulence(float amount)
    {
        Vector2 ropeDirection = Points[0].position - Points[Points.Count - 1].position;

        for (int i = 0; i < Points.Count-1; i++)
        {
            Points[i].AddForce(Vector2.Perpendicular(ropeDirection)*amount* ((i % 2 == 0) ? -1 : 1));
        }
        DestroyRope();
    }
    public void DestroyRope()
    {
        GetComponent<RopeGulp>().enabled = false;
        //GetComponent<LineRenderer>().SetColors()
    }
}
