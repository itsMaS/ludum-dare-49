using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour, IBreakable
{
    [SerializeField] float shootThreshold;
    [SerializeField] float cannonRotationSpeed = 10;
    [SerializeField] float shootCooldown = 10;
    [SerializeField] SpriteRenderer gradient;
    [SerializeField] Rope rope;
    [SerializeField] float ropeSpeed = 20;
    [SerializeField] ParticleSystem DeathParticle;
    [SerializeField] float playerPullForce;
    [SerializeField] float playerEjectForce;
    [SerializeField] float destroyPlayerRange = 3;

    [SerializeField] Transform cannonBase;
    float rotationModifier = 0;

    float shootingAccumulation = 0;
    float coolDownAmount;

    Rigidbody2D player = null;

    private void Start()
    {
        rope.transform.parent = null;
        rope.GetFirstPoint().transform.position = transform.position;
        rope.GetLastPoint().transform.position = transform.position;
        rope.InitialSetup();

        cannonBase.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        PlayerManager.Instance.onDeath.AddListener(() => player = null);
    }
    private void Update()
    {
        cannonBase.Rotate(Vector3.forward, Time.deltaTime * cannonRotationSpeed * rotationModifier);
        if(player)
        {
            cannonBase.right = cannonBase.position - player.transform.position;
            gradient.enabled = false;
        }

        coolDownAmount -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Rigidbody2D lastPoint = rope.GetLastPoint();
        rope.GetFirstPoint().MovePosition(transform.position);
        if(player)
        {
            lastPoint.MovePosition(Vector2.MoveTowards(lastPoint.position, player.position, Time.fixedDeltaTime * ropeSpeed));
            player.AddForce(((Vector2)transform.position - player.position).normalized*playerPullForce);
            if(Vector2.Distance(player.position, transform.position) <= destroyPlayerRange)
            {
                DestroyPlayer();
            }
        }
        else
        {
            lastPoint.MovePosition(transform.position);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(coolDownAmount <= 0 && collision.CompareTag("Player") && PlayerManager.Instance.currentState != PlayerManager.PlayerState.Dead)
        {
            gradient.enabled = Time.frameCount % 2 == 0;
            rotationModifier = 0;
            shootingAccumulation += Time.fixedDeltaTime;

            if(shootingAccumulation >= shootThreshold)
            {
                shootingAccumulation = 0;
                player = collision.attachedRigidbody;
                Shoot(collision.attachedRigidbody);
            }
        }
        else
        {
            rotationModifier = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            rotationModifier = 1;
            gradient.enabled = true;
        }
    }

    private void Shoot(Rigidbody2D player)
    {

    }

    public void Break(Vector2 velocity, Vector2 collisionPoint)
    {
        return;
        DeathParticle.Play();
        DeathParticle.gameObject.transform.parent = null;
        Destroy(rope.gameObject);
        Destroy(gameObject);
    }

    private void DestroyPlayer()
    {
        coolDownAmount = shootCooldown;
        player.AddForce((player.position - (Vector2)transform.position).normalized*playerEjectForce, ForceMode2D.Impulse);
        //PlayerManager.Instance.KillPlayer(PlayerManager.Death.System);
        player = null;
    }
}
