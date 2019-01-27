using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatController : MonoBehaviour
{

    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            audio.Play(0);
            StartCoroutine(xunda(audio));
            //return;
        }

        IEnumerator xunda(AudioSource audio)
        {
            yield return new WaitForSecondsRealtime(audio.clip.length);
            SceneManager.LoadScene("fail");
        }
    }
}
