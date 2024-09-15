using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Sprite[] states;
    public Collider2D exit;
    private SpriteRenderer state;
    public LayerMask player;
    public Vector2 size, offSet;
    public Animator door;
    private bool open, collision;

    void Start()
    {
        state = GetComponent<SpriteRenderer>();
        state.sprite = states[0];
    }

    void Update()
    {
        SwitchStates();
    }

    void SwitchStates()
    {
        collision = Physics2D.OverlapBox(transform.position, size + offSet, 0, player);

        if (collision && Input.GetKeyDown(KeyCode.L)) open = !open;

        exit.enabled = open;
        state.sprite = open ? states[1] : states[0];
        door.SetBool("Open", open);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size + offSet);
    }
}
