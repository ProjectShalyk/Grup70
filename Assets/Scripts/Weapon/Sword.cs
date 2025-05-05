using UnityEngine;

public class Sword : Weapon
{
    public float attackRange = 1.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            Attack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Defend();
        }
    }

    public override void Attack()
    {
        lastAttackTime = Time.time;
        animator.Play("SwordSwing");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GetDamage(damage);
        }
    }

    public override void Defend()
    {
        Debug.Log("Sword block activated");
        // defend
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
