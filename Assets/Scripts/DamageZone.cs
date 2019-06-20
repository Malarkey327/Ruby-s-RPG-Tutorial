using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public AudioClip zoneHit;

    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        Animator playerAnimator = other.gameObject.GetComponent<Animator>();

        if (controller != null)
        {
            if (controller.getInvincible())
                return;

            controller.ChangeHealth(-1);
            playerAnimator.SetTrigger("Hit");
            controller.PlaySound(zoneHit);
        }
    }
}
