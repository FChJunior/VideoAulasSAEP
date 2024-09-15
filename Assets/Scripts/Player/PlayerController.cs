using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed, maxJumpForce;
    private float speed, jumpForce;
    public Transform foot, gun;
    public LayerMask ground;
    public float ray;
    public HUD life;
    private Rigidbody2D player;
    private Animator anim;
    private SpriteRenderer skin;
    private AudioSource audioSource;
    private Vector2 move;

    private bool inGround, inFall, inRun, toRight = true, isDead, invensible, hit, inShoot, inMeelee;

    public AudioClip[] sfx;
    public GameObject bullet, sword;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        skin = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        noStop();
    }

    void Update()
    {
        if (isDead || !life.inPlay) return;
        Detections();
        VerifyDead();
        VerifyHit();
        Jumping();
        Attack();
    }
    void FixedUpdate()
    {
        if (isDead || !life.inPlay) return;
        Moviment();
    }

    void VerifyDead()
    {
        isDead = life.hp <= 0;
        if (isDead)
        {
            anim.SetTrigger("Dead");
            Stop();
            AudioManeger(2);
        }
    }
    void VerifyHit()
    {
        if (hit && !invensible)
        {
            life.hp -= 2;
            invensible = true;
            anim.SetTrigger("Hit");
            StartCoroutine(Invensible());
        }
    }
    void Moviment()
    {
        move.x = Input.GetAxisRaw("Horizontal") * speed;
        move.y = player.velocity.y;
        player.velocity = move;

        transform.eulerAngles = toRight ? Vector3.zero : Vector3.up * 180;
        anim.SetBool("Run", inRun);
    }
    void Jumping()
    {
        if (inGround && Input.GetKeyDown(KeyCode.Space) && jumpForce >= maxJumpForce)
        {
            player.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }

        if (inFall) anim.SetTrigger("Fall");
    }
    void Attack()
    {
        if (inGround)
        {
            if (Input.GetKeyDown(KeyCode.K) && !inMeelee && !inShoot) // Meelee
            {
                anim.SetTrigger("Meelee");
                AudioManeger(0);
            }
            if (Input.GetKeyDown(KeyCode.J) && !inMeelee && !inShoot) // Shoot
            {
                AudioManeger(1);
                anim.SetTrigger("Shoot");
            }
        }
    }
    void MeeleeOn()
    {
        sword.SetActive(true);
        inMeelee = true;
        Stop();
    }
    void MeeleeOff()
    {
        sword.SetActive(false);
        inMeelee = false;
        noStop();
    }
    void Shoot()
    {
        inShoot = true;
        Instantiate(bullet, gun.position, transform.rotation);
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        Stop();
        yield return new WaitForSeconds(0.25f);
        noStop();
        inShoot = false;
        yield return new WaitForSeconds(0.1f);
    }

    void AudioManeger(int clip)
    {
        audioSource.clip = sfx[clip];
        audioSource.Play();
    }
    void Detections()
    {
        inGround = Physics2D.OverlapCircle(foot.position, ray, ground);
        inFall = player.velocity.y < -0.1f;
        inRun = move.x != 0;
        toRight = move.x > 0 ? true : move.x < 0 ? false : toRight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foot.position, ray);
    }

    void Stop()
    {
        speed = 0;
        jumpForce = 0;
    }
    void noStop()
    {
        speed = maxSpeed;
        jumpForce = maxJumpForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inMeelee)
        {
            if (other.gameObject.tag == "Spike")
            {
                hit = true;
            }
            if (other.gameObject.tag == "Acid")
            {
                life.hp = 0;
            }
        }
        if (other.gameObject.tag == "Exit")
        {
            PlayerPrefs.SetInt("life", life.hp);
            SceneManager.LoadScene("Cena 2");
        }
        if (other.gameObject.tag == "Life")
        {
            life.hp += 5;
            Destroy(other.gameObject);
        }
    }
    IEnumerator Invensible()
    {
        Stop();
        yield return new WaitForSeconds(0.5f);
        noStop();
        for (int i = 0; i < 12; i++)
        {
            skin.material.color = Color.gray;
            yield return new WaitForSeconds(0.25f);
            skin.material.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        invensible = false;
        yield return new WaitForSeconds(0.1f);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            hit = false;
        }
    }

}
