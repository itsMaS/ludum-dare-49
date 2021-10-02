using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(LineRenderer))]
public class RopeGulp : MonoBehaviour
{
    LineRenderer lr;

    [SerializeField] AnimationCurve front;

    [SerializeField] float waveSpeed = 1;
    [SerializeField] bool forward = true;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (!Application.isPlaying) lr = GetComponent<LineRenderer>();

        AnimationCurve modifiedCurve = new AnimationCurve();
        foreach (var key in front.keys)
        {
            Keyframe keyframe;
            if (forward)
            {
                keyframe = new Keyframe(((Time.time*waveSpeed) % 1) + key.time, key.value);
            }
            else
            {
                keyframe = new Keyframe((1- ((Time.time * waveSpeed) % 1)) + key.time, key.value);
            }
            modifiedCurve.AddKey(keyframe);
        }
        lr.widthCurve = modifiedCurve;
    }
}
