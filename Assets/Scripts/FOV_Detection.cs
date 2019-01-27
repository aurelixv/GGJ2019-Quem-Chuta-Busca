using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FOV_Detection : MonoBehaviour {

    public Transform player;
    public float maxAngle = 45;
    public float maxRadius = 15;
    public Slider slider;
    public float multiplicador = 0.005f;
    public Color color = Color.yellow;
    public Image spotBar;
    public Gradient gradient;
    public GradientColorKey[] gradientColorKey;
    public GradientAlphaKey[] gradientAlphaKey;
    public AudioSource audio;

    private bool isInFOV = false;

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward* maxRadius;

        // FOV angle
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        // ray to the player
        Gizmos.color = isInFOV ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        // forward facing ray
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);

    }

    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius) {
        Collider[] overlaps = new Collider[250];
        // checks every object in radius and puts it in array, count is how many obj we have
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
        
        foreach (Collider overlap in overlaps) {
            if (overlap != null) {
                if(overlap.transform == target) {
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0; // dont count height in distance calculation

                    // get angle between this object (forward facing) and the other object
                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    // if the angle is <= maxAngle it is in the FOV
                    if (angle <= maxAngle) {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;
                        //Raycast the ray and check if it is inside the radius
                        if (Physics.Raycast(ray, out hit, maxRadius)) {
                            // check if it is the desired object
                            if (hit.transform == target) {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start() {
        slider.value = 0.0f;
        spotBar.color = Color.red;
        gradientColorKey = new GradientColorKey[3];
        gradientColorKey[0].color = Color.yellow;
        gradientColorKey[0].time = 0.25f;
        gradientColorKey[1].color = Color.magenta;
        gradientColorKey[1].time = 0.5f;
        gradientColorKey[2].color = Color.red;
        gradientColorKey[2].time = 0.75f;

        gradientAlphaKey = new GradientAlphaKey[3];
        gradientAlphaKey[0].alpha = 0.5f;
        gradientAlphaKey[0].time = 0.0f;
        gradientAlphaKey[1].alpha = 0.5f;
        gradientAlphaKey[1].time = 0.33f;
        gradientAlphaKey[2].alpha = 0.5f;
        gradientAlphaKey[2].time = 0.66f;

        gradient.SetKeys(gradientColorKey, gradientAlphaKey);
    }

    // Update is called once per frame
    private void Update() {
        isInFOV = inFOV(transform, player, maxAngle, maxRadius);

        if (isInFOV) {
            if (!audio.isPlaying) {
                audio.Play();
                StartCoroutine(WaitForAudio(audio));
            }
            //StartCoroutine(WaitAudio());
            //yield WaitForAudio(audio);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("fail");
        }
    }

    private void FixedUpdate() {
        if (isInFOV) {
            slider.value += multiplicador;
            spotBar.color = gradient.Evaluate(slider.value);
        }
        else if(slider.value > 0) {
            slider.value -= multiplicador/2;
            spotBar.color = gradient.Evaluate(slider.value);
        }
    }

    private IEnumerator WaitForAudio(AudioSource au) {
        do {
            yield return null;
        } while ( au.isPlaying );
    }


    IEnumerator WaitAudio()
    {
        Debug.Log("1");
        yield return new WaitForSeconds(5);
    }
}
