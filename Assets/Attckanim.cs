using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attckanim : MonoBehaviour
{
    float timer;
    bool anim = false;
    float step;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim) {
            step += Time.deltaTime;
            if (step >= timer) {
                anim = false;
                image.fillAmount = 1;
            }
            else {
                image.fillAmount = step/timer;
            }
        }
    }

    public void StartAnim(float cooldown) {
        timer = cooldown;
        step = 0;
        anim = true;
    }
}
