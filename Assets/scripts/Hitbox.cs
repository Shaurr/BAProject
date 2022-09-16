using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    int damage = 10;
    AudioSource clip;
    // Start is called before the first frame update
    void Start()
    {
        clip = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Projectile")){
            gameObject.GetComponentInParent<Boss>().TakeDamage(damage);
            clip.Play();
        }
    }
}
