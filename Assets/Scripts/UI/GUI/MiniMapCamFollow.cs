using UnityEngine;

public class MiniMapCamFollow : MonoBehaviour
{
    [SerializeField] private float x_Limit;
    [SerializeField] private float y_Limit;
    [SerializeField] private float camSmoothness;

    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        Vector3 target = player.transform.position - offset;

        target.x = Mathf.Clamp(target.x, -x_Limit, x_Limit);
        target.y = Mathf.Clamp(target.y, -y_Limit, y_Limit);

        this.transform.position = Vector3.Lerp(this.transform.position, target, camSmoothness * Time.deltaTime);
    }
}
