using UnityEngine;

public class camerFollow : MonoBehaviour
{
    [SerializeField] private Transform knight;
    [SerializeField] private Transform princess;
    [SerializeField] private float camSmoothness = 0.125f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float x_Limit = 20f;
    [SerializeField] private float y_Limit = 14f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        Vector3 midpoint = (knight.position + princess.position) / 2;

        midpoint.x = Mathf.Clamp(midpoint.x, -x_Limit, x_Limit);
        midpoint.y = Mathf.Clamp(midpoint.y, -y_Limit, y_Limit);

        float distance = Vector3.Distance(knight.position, princess.position);
        float desiredZoom = Mathf.Clamp(5 + distance * 0.5f, 5, 10);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredZoom, camSmoothness);

        Vector3 desiredPosition = midpoint + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, camSmoothness);
    }
}
