using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LightFlicker : MonoBehaviour
{
    [MinMaxSlider(0, 10), SerializeField] Vector2 timeBetweenBursts;

    private void Start()
    {
        //StartCoroutine(Flicker());
    }
    //IEnumerator Flicker()
    //{
    //    yield
    //}
}
