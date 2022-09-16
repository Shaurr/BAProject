using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }
    public void SetText(int damage) {
        Text text = GetComponentInChildren<Text>();           
        text.text= damage.ToString();
        if (damage > 10) {
            text.color = Color.magenta;
        }
        else {
            text.color = Color.red;
        }
    }

    
}
