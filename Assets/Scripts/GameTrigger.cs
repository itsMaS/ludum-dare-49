using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class GameTrigger : MonoBehaviour
{
    [SerializeField] bool triggeredOnce = true;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    protected bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggeredOnce && triggered) return;
        OnExit.Invoke();
        Enter();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggeredOnce && triggered) return;
        OnEnter.Invoke();
        Exit();
    }

    protected virtual void Enter()
    {
    }
    protected virtual void Exit()
    {
    }
}
