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
        Debug.Log($"TRIGGER ENTER {gameObject} {triggeredOnce} triggered: {triggered} Col: {collision.tag}");
        if (!collision.CompareTag("Player") || (triggeredOnce && triggered)) return;
        Debug.Log($"TRIGGERING");
        OnEnter.Invoke();
        Enter();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || triggeredOnce && triggered) return;
        OnExit.Invoke();
        Exit();
    }

    protected virtual void Enter()
    {
    }
    protected virtual void Exit()
    {
    }
}
