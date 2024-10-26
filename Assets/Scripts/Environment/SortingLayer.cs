using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.Clamp(Mathf.FloorToInt(-transform.position.y), 0, 4);
    }
}
