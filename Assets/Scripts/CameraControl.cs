using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject player;
    public Transform target;
    public float smoothingFactor;
    private Vector3 positionToKeep;
    public Vector3 maxVals, minVals;

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {


        positionToKeep = new Vector3(player.transform.position.x, player.transform.position.y + 5, transform.position.z);
        Vector3 boundPosition = new Vector3(Mathf.Clamp(positionToKeep.x, minVals.x, maxVals.x),
                                            Mathf.Clamp(positionToKeep.y, minVals.y, maxVals.y),
                                            positionToKeep.z);
        transform.position = Vector3.Lerp(transform.position, boundPosition, smoothingFactor * Time.deltaTime);
    }

}
