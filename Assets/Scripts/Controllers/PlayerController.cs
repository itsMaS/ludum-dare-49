using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager instance => PlayerManager.Instance;
    public void SetState(int state)
    {
        instance.SetState((PlayerManager.PlayerState)state);
    }
    public void Kill()
    {
    }
    public void KillBySpikes()
    {
        instance.KillPlayer(PlayerManager.Death.Impaled);
    }
}
