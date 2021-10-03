using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rope)), ExecuteAlways]
public class RopeConnection : MonoBehaviour
{
    [SerializeField] Rigidbody2D connectedRigidbody;
    [SerializeField] Transform visualConnection;
    [SerializeField] Rigidbody2D IKInfluence;
    [SerializeField] AudioSource BreakSound;

    [SerializeField] float pullForce;
    [SerializeField] float IKPullForce;
    [SerializeField] float ropeBreakForce;

    [SerializeField] float tensionNeededToBreak = 2;
    [SerializeField] float elasticity;
    [SerializeField] float breakOnLenght;

    public bool broken = false;

    Rope rope;
    private void Awake()
    {
        rope = GetComponent<Rope>();
    }
    public void Connect(PlayerManager player, Vector2 point)
    {

    }
    private void Start()
    {
        rope.GetLastPoint().gravityScale = 0;
        rope.GetLastPoint().position = visualConnection.position;
        broken = false;
    }
    private void Update()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            if (!rope) rope.GetComponent<Rope>();
            rope.GetLastPoint().transform.position = visualConnection.position;
        }
#endif
    }
    private void FixedUpdate()
    {
        if(!broken)
        {
            rope.GetLastPoint().MovePosition(visualConnection.position);
            
            float tension = rope.GetTension();
            Vector3 rockPullVector = ((Vector3)rope.GetFirstPoint().position - visualConnection.position);
            if(tension > 1)
            {
                rope.length += elasticity * Time.fixedDeltaTime;
                connectedRigidbody.AddForce((tension - 1) * pullForce * rockPullVector);

                if (rope.length > breakOnLenght)
                {
                    Break();
                    return;
                }
            }
            if(tension > tensionNeededToBreak)
            {
                Break();
                return;
            }
            if(IKInfluence)
                IKInfluence.AddForce(tension * IKPullForce * rockPullVector);
        }
    }

    public void Break()
    {
        BreakSound.Play();
        broken = true;
        Rigidbody2D lastPoint = rope.GetLastPoint();
        lastPoint.gravityScale = 1;
        rope.AddBreakTurulence(ropeBreakForce);
    }
}
