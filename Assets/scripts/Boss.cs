using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    bool damageNr;
    [SerializeField]
    bool damageNrUp = false;
    [SerializeField]
    DamageNumber dNr;
    [SerializeField]
    int hp;
    [SerializeField]
    int maxHp = 300;
    GameController gameController;
    bool damageLock = false;
    Canvas canvas;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        gameController = FindObjectOfType<GameController>();
        canvas =  FindObjectOfType<Canvas>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        if (damageLock)
            return;     
        hp -= damage;

        gameController.SetBossHealth((float)hp / maxHp,damage>10);
        if (hp <= 0) {
            GetComponent<AudioSource>().Play();
            gameController.Death();            
        }
        damageLock = true;
        if (damageNr && damage > 0) {
           
            DamageNumber damageText = Instantiate(dNr, canvas.transform);
            damageText.SetText(damage);
            if (damageNrUp) {
                damageText.transform.localPosition = new Vector3(95, 425, 0);
            }  
            
        }
        if (damage > 0) {
            anim.SetTrigger("damage");
        }


        damageLock = false;

    }
}
