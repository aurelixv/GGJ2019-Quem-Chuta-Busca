using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Acrescentador : MonoBehaviour
{

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Time.time * 0.05f;
    }
}
