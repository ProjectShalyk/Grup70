using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float attackRate = 1f; // attacks per second
    protected float lastAttackTime;

    protected Animator animator;

    // Virtual methods

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {
        Debug.Log("Weapon attack");
    }

    public virtual void Defend()
    {
        Debug.Log("Weapon defend");
    }

    protected bool CanAttack()
    {
        return Time.time >= lastAttackTime + 1f / attackRate;
    }
}
