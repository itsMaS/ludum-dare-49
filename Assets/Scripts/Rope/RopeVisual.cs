using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), ExecuteAlways]
public class RopeVisual : MonoBehaviour
{
    [SerializeField] private int points = 5;

    public Transform point1;
    public Transform point2;

    public float length = 10;

    float dropAmount = 1;
    public float maxDrop = 1;

    [SerializeField] AnimationCurve dropAnimation;

    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) lr = GetComponent<LineRenderer>();
#endif
        float distance = Vector2.Distance(point1.position, point2.position);
        dropAmount = Mathf.Lerp(maxDrop, 0, Mathf.InverseLerp(0, length, distance));
        lr.positionCount = points + 1;
        for (int i = 0; i <= points; i++)
        {
            float lerp = (float)i / points;
            float toMiddle = Mathf.InverseLerp(0.5f, 0, Mathf.Abs(lerp - 0.5f));
            Vector2 horizontal = Vector2.Lerp(point1.position, point2.position, lerp);

            horizontal.y -= dropAnimation.Evaluate(toMiddle) * dropAmount;
            lr.SetPosition(i, horizontal);
        }
    }
}