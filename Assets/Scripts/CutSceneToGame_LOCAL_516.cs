using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutSceneToGame : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

        while (AnimationState.time >= AnimationState.length)
            yield return null;
        
        SceneManager.LoadScene("main 1");
    }
}
