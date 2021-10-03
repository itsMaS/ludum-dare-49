using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] float delay;

    [MinMaxSlider(0, 1), SerializeField] Vector2 maxIntensity;
    [MinMaxSlider(0, 1), SerializeField] Vector2 minIntensity;

    [MinMaxSlider(0, 10), SerializeField] Vector2 timeBetweenBursts;
    [MinMaxSlider(0, 20), SerializeField] Vector2Int burstCount;
    [MinMaxSlider(0, 1), SerializeField] Vector2 timeIntervalForBursts;


    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine(Flicker());
    }
    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(delay);

        while(true)
        {
            for (int i = 0; i < Random.Range(burstCount.x,burstCount.y); i++)
            {
                float wait = Random.Range(timeIntervalForBursts.x, timeIntervalForBursts.y);
                sr.DOFade(Random.Range(maxIntensity.x, maxIntensity.y), wait);
                yield return new WaitForSeconds(wait);
                sr.DOFade(Random.Range(minIntensity.x, minIntensity.y), wait);
                yield return new WaitForSeconds(wait);
            }
            yield return new WaitForSeconds(Random.Range(timeBetweenBursts.x, timeBetweenBursts.y));
        }

    }
}
