using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Acrescenta : MonoBehaviour
{
    public Image SpotBar;
    // Start is called before the first frame update
    void Start()
    {
        SpotBar.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        SpotBar.fillAmount = Time.time / 0.05f;
    }
}
