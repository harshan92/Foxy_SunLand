using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //serialize variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 10f;

    //Finite state  machine
    private enum State { idle, running, jumping, falling }
    private State state=State.idle;

    private void Start() {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        coll= GetComponent<Collider2D>();
    }

    private void Update()
    {

        float HDirection = Input.GetAxis("Horizontal");
        inputs_manager(HDirection);
        velocityState();
        anim.SetInteger("state", (int)state);
    }

    private void inputs_manager(float HDirection)
    {
        //left move
        if (HDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        //right move
        if (HDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        //jump action
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jumping;
        }
    }

    private void velocityState(){
        if(state==State.jumping){

            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }

        }else if (state==State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }

        }else if(Mathf.Abs(rb.velocity.x) > 2f){
            state=State.running;
        }else{
            state=State.idle;
        }
    }
}
