using UnityEngine;

public class PanoramaScroll : MonoBehaviour
{
    public Material mat;
    public float scrollSpeed = 0.2f;
    public float verticalWobble = 0.1f;
    public float wobbleSpeed = 0.5f;

    void Update()
    {
        float rot = Mathf.Repeat(Time.time * scrollSpeed, Mathf.PI * 2);
        float vertical = Mathf.Sin(Time.time * wobbleSpeed) * verticalWobble;
        mat.SetFloat("_Rotation", rot);
        mat.SetFloat("_VerticalOffset", vertical);
    }
}
