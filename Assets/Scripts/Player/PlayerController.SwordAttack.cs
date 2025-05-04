using UnityEngine;

public partial class PlayerController
{
    void SwordStart()
    {
        swordAnimator = transform.GetChild(2).GetComponent<Animator>();
        swordCollider = transform.GetChild(3).GetComponent<Collider2D>();
        //AnimatorClipInfo[] clips = swordAnimator.GetCurrentAnimatorClipInfo(0);

        //SwordAttack();
    }

    void SwordAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            swordAnimator.Play("SwordSwing");
            swordCollider.enabled = true;
            Invoke(nameof(SwordEnd), swordAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }
    }

    void SwordEnd()
    {
        isAttacking = false;
        swordCollider.enabled = false;
    }
}