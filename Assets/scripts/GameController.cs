using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField]
    int levelNr = 0;
    float timer;
    [SerializeField]
    float gametime = 180;
    [SerializeField]
    Text clock;
    [SerializeField]
    Image playerHealth;
    [SerializeField]
    Image bossHealth;
    [SerializeField]
    Text pause;
    [SerializeField]
    Image endLevel;
    [SerializeField]
    GameObject endGame;
    bool end = false;
    int crits=0;
    int hit=0;
    float firstcrit=-1;
    // Start is called before the first frame update
    void Start() {
        // DontDestroyOnLoad(gameObject);
        timer = gametime;
        pause.enabled = false;      

    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        clock.text = timer.ToString("##");
        if (timer <= 0)
            EndLevel();
        if (end && Input.GetButtonDown("Submit")) {
            if (levelNr == 8) {
                Endscreen();
            }
            else {
                Time.timeScale = 1;
                ChangeLevel();
            }
        }
    }

    public void ChangeLevel() {

        ChangeLevel(levelNr + 1);
    }
    public void ChangeLevel(int nr) {
        SceneManager.LoadScene(nr);
    }

    public void Death() {
        EndLevel();
    }

    public void Pause(bool state) {
        if (state) {
            Time.timeScale = 1;
            pause.enabled = false;
        }
        else {
            Time.timeScale = 0;
            pause.enabled = true;
        }
    }

    public void SetPlayerHealth(float value) {
        playerHealth.fillAmount = value;
    }
    public void SetBossHealth(float value, bool crit) {
        bossHealth.fillAmount = value;
        if (crit) {
            crits++;
            hit++;
            if (firstcrit == -1) {
                firstcrit = gametime - timer;
            }
        }
        else {
            hit++;
        }
    }

    void EndLevel() {
        CollectValues();
        Time.timeScale = 0;
        endLevel.gameObject.SetActive(true);
        Text player = endLevel.transform.GetChild(0).GetComponent<Text>();
        Text boss = endLevel.transform.GetChild(1).GetComponent<Text>();
        Text time = endLevel.transform.GetChild(2).GetComponent<Text>();
        player.text = $"{playerHealth.fillAmount*100:F0} %" ;
        boss.text = $"{bossHealth.fillAmount*100:F0} %";
        time.text = $"{clock.text} s";
        end = true;

    }

    void CollectValues() {
        Values v = FindObjectOfType<Values>();

        Values.values list = new Values.values();
        list.time = clock.text;
        list.health = (int)(playerHealth.fillAmount * 100f);
        list.bosshealth = (int)(bossHealth.fillAmount * 100f);
        list.hits = hit;
        list.crit = crits;
        list.firstcrit = firstcrit;
        v.SetValues(list,levelNr);
    }

    void Endscreen() {
        endGame.gameObject.SetActive(true);
        Values v = FindObjectOfType<Values>();
        Values.values[] values = v.GetValues();
       
        for (int i =1; i <= values.Length; i++) {
           
            Transform x = endGame.transform.GetChild(i);           
            x.GetChild(0).GetComponent<Text>().text = values[i-1].health.ToString("#0");
            x.GetChild(1).GetComponent<Text>().text = values[i-1].bosshealth.ToString("#0");
            x.GetChild(2).GetComponent<Text>().text = values[i-1].time;
            x.GetChild(3).GetComponent<Text>().text = values[i-1].hits.ToString();
            x.GetChild(4).GetComponent<Text>().text = values[i-1].crit.ToString();
            x.GetChild(5).GetComponent<Text>().text = values[i-1].firstcrit.ToString("#0");
        }
    }
}


