﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToGame : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        SceneManager.LoadScene("main 1");
    }
}
