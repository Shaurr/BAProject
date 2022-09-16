using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController: MonoBehaviour
{


    public void StartGame(int lvl) {
        SceneManager.LoadScene(lvl,LoadSceneMode.Single);
    }

    public void Quit() {
        Application.Quit();
    }

    public void ChangePanel() {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
       

    }
}
