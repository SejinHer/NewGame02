using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 posOffset;

    private void LateUpdate()
    {
        transform.position = target.transform.position + posOffset;
    }
}
