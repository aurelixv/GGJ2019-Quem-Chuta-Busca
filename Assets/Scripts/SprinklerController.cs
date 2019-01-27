using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SprinklerController : MonoBehaviour {

    public Transform player;
    public float maxAngle = 20;
    public float maxRadius = 5;
    public int speed = 2;
    public bool direction = false;
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
        Collider[] overlaps = new Collider[200];
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
    }

    // Update is called once per frame
    private void Update() {
        isInFOV = inFOV(transform, player, maxAngle, maxRadius);
        if (direction) {
            transform.Rotate(0, speed, 0);
        } else {
            transform.Rotate(0, - speed, 0);
        }
    }

    private void FixedUpdate() {
        if (isInFOV) {
            StartCoroutine(xunda(audio));
        }
    }

    IEnumerator xunda(AudioSource audio) {
        audio.Play();
        yield return new WaitForSecondsRealtime(audio.clip.length);
        SceneManager.LoadScene("fail");
    }
}
