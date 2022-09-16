using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : MonoBehaviour
{
    public struct values {
        public string time;
        public int health;
        public int bosshealth;
        public int hits;
        public int crit;
        public float firstcrit;
    }

    values[] valueslist = new values[8];
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    
    public void SetValues(values v, int i) {
        valueslist[i - 1] = v;
    }

    public values[] GetValues() {
        return valueslist;
    }
}

