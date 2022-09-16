using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.tag.Equals("Projectile")) 
            Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction) {
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
}
