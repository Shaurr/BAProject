using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
    [SerializeField]
    float movementspeed = 3f;
    [SerializeField]
    GameObject projectile;
    int direction;
    [SerializeField]
    int maxLp = 150;
    [SerializeField]
    int lp;
    Animator animator;
    GameController gameController;
    bool pause = false;
    AudioSource clip;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    float cooldown = 1;
    float cooldownTimer = 0;
    bool damageLock = false;
    [SerializeField]
    GameObject attackanim;

    // Start is called before the first frame update
    void Start() {
        lp = maxLp;
        direction = 0;
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        clip = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Cancel")){
            if (pause) {
                gameController.Pause(true);
                pause = false;
            }
            else {
                gameController.Pause(false);
                pause = true;
            }

        }

        if (pause)
            return;

        bool moving = true;
        Vector3 movement = Vector3.zero;
        if (Input.GetButton("Horizontal")) {

            float x = Input.GetAxis("Horizontal");
            if (x > 0) {
                direction = 0;
                animator.SetInteger("direction", direction);
                movement = new Vector3(x * movementspeed, 0, 0);
            }
            else {
                direction = 2;
                animator.SetInteger("direction", direction);
                movement = new Vector3(x * movementspeed, 0, 0);
            }
        }
        else if (Input.GetButton("Vertical")) {
            float y = Input.GetAxis("Vertical");
            if (y > 0) {
                direction = 3;
                animator.SetInteger("direction", direction);
                movement = new Vector3(0, y * movementspeed, 0);
            }
            else {
                direction = 1;
                animator.SetInteger("direction", direction);
                movement = new Vector3(0, y * movementspeed, 0);
            }
        }
        else {
            animator.SetBool("isMoving", false);
            moving = false;
        }
        if (moving) {
            animator.SetBool("isMoving", true);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }

        

        if (Input.GetButtonDown("Fire1") && cooldownTimer < Time.time) {
            cooldownTimer = Time.time + cooldown;
            Vector3 spawnposition;
            Vector2 shotdirection;
            switch (direction) {
                case 0:
                    spawnposition = new Vector3(transform.position.x + 0.2f, transform.position.y, 0);
                    shotdirection = new Vector2(1, 0);
                    break;
                case 1:
                    spawnposition = new Vector3(transform.position.x, transform.position.y - 0.2f, 0);
                    shotdirection = new Vector2(0, -1);
                    break;
                case 2:
                    spawnposition = new Vector3(transform.position.x - 0.2f, transform.position.y, 0);
                    shotdirection = new Vector2(-1, 0);
                    break;
                default:
                    spawnposition = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
                    shotdirection = new Vector2(0, 1);
                    break;
            }

            animator.SetTrigger("casting");
            attackanim.GetComponent<Attckanim>().StartAnim(cooldown);

            GameObject shot = Instantiate(projectile, spawnposition, Quaternion.Euler(0, 0, -90 * direction));
            shot.GetComponent<Projectile>().SetDirection(shotdirection * 2);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Boss")) {
            TakeDamage();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Projectile")) {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        if (damageLock)
            return;
        
        damageLock = true;
        StartCoroutine("damageAnimation");
        lp -= 10;
        gameController.SetPlayerHealth((float)lp / (float)maxLp);
        if (lp <= 0) {            
            clip.PlayOneShot(death);
            gameController.Death();
        }
        else {
            clip.Play();
        }
    }

    IEnumerator damageAnimation() {
        SpriteRenderer sR = GetComponent<SpriteRenderer>();
        Color before = sR.color;
        sR.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sR.color = before;
        damageLock = false;
    }

}
