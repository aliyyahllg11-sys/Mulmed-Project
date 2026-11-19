using UnityEngine;

// Kamera mengikuti pemain secara horizontal dan tidak keluar dari batas level.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float levelWidth = 80f;
    public float fixedY = 5.5f;
    Camera cam;

    void Awake() { cam = GetComponent<Camera>(); }

    void LateUpdate()
    {
        if (target == null) return;
        float half = cam.orthographicSize * cam.aspect;
        float tx = Mathf.Clamp(target.position.x, half, Mathf.Max(half, levelWidth - half));
        float x = Mathf.Lerp(transform.position.x, tx, 0.2f);
        transform.position = new Vector3(x, fixedY, -10f);
    }
}
