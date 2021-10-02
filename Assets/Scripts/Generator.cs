using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Generator : MonoBehaviour
{
    [SerializeField] int requiredRopesToAdvance;

    [Header("Dependancies")]
    [SerializeField] List<RopeConnection> Ropes;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;



    private void Start()
    {
        StartCoroutine(CheckForSeveredRopes());
    }

    IEnumerator CheckForSeveredRopes()
    {
        int severedRopes = 0;
        while(severedRopes < requiredRopesToAdvance)
        {
            severedRopes = 0;
            foreach (var rope in Ropes)
            {
                if (rope.broken) { severedRopes++; }
                yield return null;

            }
            yield return null;
        }
        playerFollowCamera.enabled = true;
    }
}
