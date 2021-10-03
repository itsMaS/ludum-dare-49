using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public UnityEvent onDeath;
    public UnityEvent onRespawn;


    [System.Serializable]
    public enum PlayerState { Cinematic, Swim, Dead }
    public enum Death { Impaled, Robot, System }

    public PlayerState currentState = PlayerState.Cinematic;


    [SerializeField] CheckPoint spawnPoint;
    
    [SerializeField] float strokeForce;
    [SerializeField] float chargedForce;
    [SerializeField] float turningSensitivity = 10;
    [SerializeField] float swimThreshold = 1;

    [SerializeField] float chargedRotationMultiplier = 3;
    [SerializeField] float chargeChanceAccumulationIncrease = 0.1f;
    [SerializeField, Range(0,1)] float chargedChance = 0.2f;

    [SerializeField] Camera cam;
    [SerializeField] AudioSource strokeSound;
    [SerializeField] List<Rigidbody2D> IKPoints;
    [SerializeField] CinemachineVirtualCamera playersCamera;

    Coroutine strokeCouroutine;
    Rigidbody2D rb;
    Animator an;

    float startingGravity;
    float chargeChanceAccumulation = 0;


    public bool chargeEnabled = false;

    List<SpriteRenderer> Renderers = new List<SpriteRenderer>();
    public bool charged { get; private set; }

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        GetComponentsInChildren<SpriteRenderer>(Renderers);
    }
    private void Start()
    {
        spawnPoint.Spawn(this);
        startingGravity = rb.gravityScale;
        DOVirtual.DelayedCall(0.5f, () => playersCamera.enabled = true);
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Cinematic:
                CinematicMovement();
                break;
            case PlayerState.Swim:
                NormalMovement();
                break;
            default:
                break;
        }
    }

    void CinematicMovement()
    {
        an.SetBool("Swimming", false);
        rb.gravityScale = 0;
    }
    void NormalMovement()
    {
        rb.gravityScale = startingGravity;

        Vector2 movementVector = new Vector2();
        Vector2 axisMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetMouseButton(0))
        {
            Vector2 pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);
            movementVector = (pointerPos - (Vector2)transform.position).normalized;
        }
        else if(axisMovement.magnitude > 0.01f)
        {
            movementVector = axisMovement.normalized;
        }

        if(movementVector.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turningSensitivity* chargedRotationMultiplier));
        }
        an.SetBool("Swimming", movementVector.magnitude > 0.1f);
    }

    private void StartPrep()
    {
        charged = chargeEnabled && Random.value < chargedChance+chargeChanceAccumulation;
        if (charged)
        {
            chargeChanceAccumulation = 0;
            TransitionStatesVisual(true);
        }
        else
        {
            chargeChanceAccumulation += chargeChanceAccumulationIncrease;
        }
    }
    private void StartStroke()
    {
        strokeCouroutine = StartCoroutine(Stroke(charged ? chargedForce : strokeForce));
        strokeSound.pitch = 1 + Random.Range(-0.2f, 0.2f) + (charged ? 0.5f : 0);
        strokeSound.Play();
    }
    private void EndStroke()
    {

        if(strokeCouroutine != null)
        {
            StopCoroutine(strokeCouroutine);
            strokeCouroutine = null;
            TransitionStatesVisual(false);
        }

    }

    private void TransitionStatesVisual(bool charged)
    {
        Renderers.ForEach(rend => rend.DOColor(charged ? Color.white : Color.black, 0.3f));
    }

    IEnumerator Stroke(float force)
    {
        while(true)
        {
            rb.AddForce(force * transform.up);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IBreakable breakable = null;
        if(charged && (breakable = collision.gameObject.GetComponentInParent<IBreakable>()) != null)
        {
            breakable.Break(rb.velocity,collision.contacts[0].point);
        }
    }

    #region API Methods
    public void SetState(PlayerState state)
    {
        currentState = state;
    }
    public void RespawnPlayer()
    {
        IKPoints.ForEach(ik => ik.simulated = true);
        rb.simulated = true;
        spawnPoint.Spawn(this);
        onRespawn.Invoke();
        an.SetBool("Dead", false);
        rb.velocity = Vector2.zero;
    }

    [ContextMenu("KIll player")]
    public void KillPlayer(Death death)
    {
        if (currentState == PlayerState.Dead) return;
        TransitionStatesVisual(false);
        Debug.Log($"Player died!");
        SetState(PlayerState.Dead);
        rb.gravityScale = startingGravity * 2;
        switch(death)
        {
            case Death.Impaled:
                rb.simulated = false;
                break;
            case Death.Robot:
                IKPoints.ForEach(ik => ik.simulated = false);
                break;
        }

        onDeath?.Invoke();
        DOVirtual.DelayedCall(3, () => RespawnPlayer());
        an.SetBool("Dead", true);
    }
    public void SetCheckpoint(CheckPoint checkpoint)
    {
        spawnPoint = checkpoint;
    }

    #endregion
}
