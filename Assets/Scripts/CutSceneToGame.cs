using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneToGame : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        int sec = 2;
        StartCoroutine(Wait(sec));
    }

    IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene("main 1");
    }
}
