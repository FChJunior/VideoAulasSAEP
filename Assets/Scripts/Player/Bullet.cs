using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    Animator anim;
    AudioSource explosion;

    void Start()
    {
        anim = GetComponent<Animator>();
        explosion = GetComponent<AudioSource>();
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        speed = 0;
        explosion.Play();
        anim.SetTrigger("hit");
        Destroy(gameObject, 0.25f);
    }
}
