using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : GameTrigger
{
    public UnityEvent OnSpawn;

    [SerializeField] PlayerManager.PlayerState playerState;

    public void Spawn(PlayerManager player)
    {
        player.SetState(playerState);
        player.transform.position = transform.position;
        player.transform.rotation = Quaternion.identity;
        OnSpawn?.Invoke();
    }

    protected override void Enter()
    {
        base.Enter();

        if (!triggered) PlayerManager.Instance.SetCheckpoint(this);
    }
}
