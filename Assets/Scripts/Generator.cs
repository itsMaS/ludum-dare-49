using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Generator : MonoBehaviour
{
    [SerializeField] int requiredRopesToAdvance;
    [SerializeField] float rotorSpeed = 10;
    [SerializeField] AnimationCurve pitchOverSeveredRopes;

    [Header("Dependancies")]
    [SerializeField] List<RopeConnection> Ropes;
    [SerializeField] CinemachineVirtualCamera generatorsCamera;


    [SerializeField] List<Transform> RightRotors;
    [SerializeField] List<Transform> LeftRotors;
    [SerializeField] AudioSource generatorAudio;

    float generatorPowerNormalized = 1;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        StartCoroutine(CheckForSeveredRopes());
    }

    private void Update()
    {
        RightRotors.ForEach(rotor => rotor.Rotate(Vector3.forward, rotorSpeed * Time.deltaTime * generatorPowerNormalized));
        LeftRotors.ForEach(rotor => rotor.Rotate(Vector3.forward, -rotorSpeed * Time.deltaTime * generatorPowerNormalized));
    }
    IEnumerator CheckForSeveredRopes()
    {
        int severedRopes = 0;
        while(severedRopes < Ropes.Count)
        {
            severedRopes = 0;
            foreach (var rope in Ropes)
            {
                if (rope.broken) { severedRopes++; }
                yield return null;

            }

            generatorPowerNormalized = (1 - (float)severedRopes / Ropes.Count);
            generatorAudio.pitch = pitchOverSeveredRopes.Evaluate(1 - generatorPowerNormalized);
            if (severedRopes > requiredRopesToAdvance && generatorsCamera.enabled) generatorsCamera.enabled = false;
            yield return null;
        }
    }

    public void SetupGenerator()
    {
        gameObject.SetActive(true);
    }
}
