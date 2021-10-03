using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Wall : MonoBehaviour, IBreakable
{
    List<Rigidbody2D> Parts = new List<Rigidbody2D>();
    [SerializeField] float ejectionForce = 100;
    [SerializeField] float breakThreshold;

    bool broken = false;
    private void Awake()
    {
        GetComponentsInChildren<Rigidbody2D>(Parts);
        Parts.Remove(GetComponent<Rigidbody2D>());
    }

    private void Start()
    {
        Parts.ForEach(part => part.isKinematic = true);
    }
    public void Break(Vector2 velocity, Vector2 collisionPoint)
    {
        if (broken || velocity.magnitude < breakThreshold) return;
        broken = true;
        Parts.ForEach(part =>
        {
            part.isKinematic = false;
            part.AddForce((part.position - collisionPoint)*velocity*ejectionForce, ForceMode2D.Force);
            part.transform.DOScale(0, 10).OnComplete(() => part.gameObject.SetActive(false));
        });
    }
}
