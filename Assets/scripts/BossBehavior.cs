using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {
    enum State {
        idle,
        choose_attack,
        attack1,
        attack2,
        attack3,
    }
    //State state;
    [SerializeField]
    GameObject bossProjectile;
    float timer;
    Transform player;
    int wave = 0;
    [SerializeField]
    State state;
    Animator animator;
    [SerializeField]
    GameObject[] shots;
    // Start is called before the first frame update
    void Start() {
        timer = Time.time + 2f;
        state = State.idle;
        player = FindObjectOfType<player>().transform;
        animator = GetComponent<Animator>();
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
                    if (wave < 2) {
                        timer = Time.time + 0.5f;
                        wave++;
                        Attack2();
                    }
                    else {
                        timer = Time.time + 3f;
                        state = State.idle;
                        wave = 0;
                    }


                }
                break;
            case State.attack3:
                if (timer < Time.time) {
                    if (wave == 0) {
                        timer = Time.time + 1.5f;
                        wave++;
                        Attack3();
                    }
                    else {
                        wave = 0;
                        state = State.idle;
                        timer = Time.time + 3f;
                    }

                }
                break;

        }

    }

    void ChooseAttack() {
        int rng = Random.Range(1, 10);

        if (rng < 6) {
            timer = Time.time + 0.7f;
            state = State.attack1;

            Attack();
        }
        else if (rng < 8) {

            timer = Time.time + 0.5f;
            state = State.attack2;
            Attack2();
        }
        else {
            timer = Time.time + 2f;
            state = State.attack3;
            animator.SetBool("castBoss", true);
            SpawnAttack3();
        }
    }

    void Attack() {
        GameObject shot;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        shot = Instantiate<GameObject>(bossProjectile, transform.position + (direction*0.7f), Quaternion.identity);
        shot.GetComponent<Projectile>().SetDirection(direction);

    }

    void Attack2() {
        GameObject shot;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction = Rotate(direction, -45);
        int n = 7;

        for (int i = 0; i < n; i++) {
            shot = Instantiate<GameObject>(bossProjectile, transform.position + (direction * 0.7f), Quaternion.identity);
            shot.GetComponent<Projectile>().SetDirection(direction);
            direction = Rotate(direction, 90 / (n - 1));
        }


    }

    void SpawnAttack3() {
        
        Vector2 positiom = new Vector2(-1, 0);        
        float angle;
        int n=20;
        
       
        if (player.position.y > 0) {
            
            if (player.position.x < 0) {
                positiom = Rotate(positiom, -45).normalized;
                angle = (90 / (n - 2));

            }
            else {
                
                positiom = Rotate(positiom, 135).normalized;
                angle = (90 / (n - 2));
            }
        }
        else {
           
            positiom = positiom = Rotate(positiom, 45).normalized;
            angle = (90 / (n - 2));
        }
        shots = new GameObject[n];
        positiom *= 0.7f;

        for (int i = 0; i < n; i++) {
            Vector3 position = new Vector3(transform.position.x + positiom.x, transform.position.y + positiom.y, 0);            
            shots[i] = Instantiate<GameObject>(bossProjectile, position, Quaternion.identity);            
            positiom = Rotate(positiom, angle);
        }
    }

    void Attack3() {
        animator.SetBool("castBoss", false);
        for( int i = 0;i < shots.Length;i++) {
            if (shots[i] == null)
                continue;
            Vector2 direction = (shots[i].transform.position - transform.position).normalized;
            shots[i].GetComponent<Projectile>().SetDirection(direction);
        }       
        
    }


    Vector2 Rotate(Vector2 aPoint, float aDegree) {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }


}
