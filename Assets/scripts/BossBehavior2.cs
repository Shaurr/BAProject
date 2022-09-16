using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior2 : MonoBehaviour {
    enum State {
        idle,
        choose_attack,
        attack1,
        attack2,
        attack3,
    }
    State state;
    [SerializeField]
    GameObject bossProjectile;
    float timer;
    Transform player;
    int wave = 0;    
    Animator animator;
    Vector2 moveVector;
    [SerializeField]
    bool moving = true;
    [SerializeField]
    float movementSpeed;
    // Start is called before the first frame update
    void Start() {
        timer = Time.time + 2f;
        state = State.idle;
        player = FindObjectOfType<player>().transform;
        animator = GetComponent<Animator>();
        ChooseTarget();
    }

    // Update is called once per frame
    void Update() {


        switch (state) {
            case State.idle:
                if (timer < Time.time) {
                    state = State.choose_attack;

                    ChooseAttack();
                }
                break;
            case State.attack1:
                if (timer < Time.time) {
                    state = State.idle;

                }
                break;
            case State.attack2:
                if (timer < Time.time) {

                    if (wave < 10) {
                        timer = Time.time + 0.3f;
                        wave++;
                        Attack2();
                    }
                    else {
                        timer = Time.time + 4.2f;
                        state = State.idle;
                        wave = 0;
                        moving = true;
                    }


                }
                break;
            case State.attack3:
                if (timer < Time.time) {
                    if (wave == 0) {
                        timer = Time.time + 1f;
                        wave++;
                        Attack3();
                    }
                    else { 
                        wave = 0;
                        moving = true;
                        state = State.idle;
                        timer = Time.time + 1.7f;
                    }
                    
                }
                break;
        }

        if (moving) {

            transform.Translate(moveVector * Time.deltaTime * movementSpeed);
        }
    }

    void ChooseAttack() {
        int rng = Random.Range(0, 10);
        
        if (rng < 5) {
            timer = Time.time + 0.9f;
            state = State.attack1;

            Attack();
        }
        else if (rng == 9) {
            timer = Time.time + 1f;
            state = State.attack2;
            animator.SetBool("castBoss", true);
            moving = false;
            ChooseTarget();
        }
        else {
            timer = Time.time + 0.5f;
            state = State.attack3;
            moving = false;
            ChooseTarget();            
        }
    }

    void ChooseTarget() {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        moveVector = new Vector2(x, y);
        moveVector = moveVector.normalized;
    }

    void Attack() {
        GameObject shot;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        shot = Instantiate<GameObject>(bossProjectile, (transform.position + (direction * 0.7f)), Quaternion.identity);
        shot.GetComponent<Projectile>().SetDirection(direction.normalized);

    }

    void Attack2() {
        animator.SetBool("castBoss", false);
        GameObject shot;
        Vector2 positiom = new Vector2(-0.7f, 0);
        positiom = Rotate(positiom, wave * 5);
        int n = 12;

        for (int i = 0; i < n; i++) {
            Vector3 position = new Vector3(transform.position.x + positiom.x, transform.position.y + positiom.y, 0);
            shot = Instantiate<GameObject>(bossProjectile, position, Quaternion.identity);
            Vector2 direction = (position - transform.position).normalized;
            shot.GetComponent<Projectile>().SetDirection(direction);
            positiom = Rotate(positiom, 360 / (n));
        }


    }

    void Attack3() {
        Debug.Log("Attack3");
        GameObject shot;
        int n = 7;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction = Rotate(direction, -45);


        for (int i = 0; i < n; i++) {

            shot = Instantiate<GameObject>(bossProjectile, transform.position + (direction * 0.7f), Quaternion.identity);
            shot.GetComponent<Projectile>().SetDirection(direction);
            direction = Rotate(direction, 90 / (n - 1));
        }

    }


    Vector2 Rotate(Vector2 aPoint, float aDegree) {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
   
        if (collision.collider.tag == "Wall") {
            ChooseTarget();
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.tag == "Wall") {
            ChooseTarget();
         
        }
    }
}
