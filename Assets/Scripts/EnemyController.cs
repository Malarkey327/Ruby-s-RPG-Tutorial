using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    AudioSource audioSource_1;
    public ParticleSystem smokeEffect;
    public ParticleSystem hitEffect;
    public AudioClip playerHit;
    public AudioClip[] hit;
    AudioClip botHit;

    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    bool broken = true;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            audioSource.mute = true;
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    // Checks if collision occured with player and damages player if true
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        Animator playerAnimator = other.gameObject.GetComponent<Animator>();               

        if (player != null)
        {
            if (player.getInvincible())
                return;

            int index = Random.Range(0, hit.Length);
            player.ChangeHealth(-1);
            hitEffect.Play();
            playerAnimator.SetTrigger("Hit");
            botHit = hit[index];
            player.PlaySound(botHit);
            player.PlaySound(playerHit);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        smokeEffect.Stop();

        animator.SetTrigger("Fixed");
    }
}
