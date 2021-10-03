using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : GameTrigger
{
    [SerializeField] float penetrationTime;
    [SerializeField] AudioSource sound;
    protected override void Enter()
    {
        Debug.Log($"Spike {gameObject} destroyed player");

        base.Enter();
        DOVirtual.DelayedCall(penetrationTime, () => PlayerManager.Instance.KillPlayer(PlayerManager.Death.Impaled));
        sound.Play();
    }
}
