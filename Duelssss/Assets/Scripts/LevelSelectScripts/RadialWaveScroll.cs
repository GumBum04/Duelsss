using UnityEngine;
using UnityEngine.UI;

public class RadialWaveScroll : MonoBehaviour
{
    public float speed = 0.5f; // How fast the wave moves
    private Material mat;

    void Start()
    {
        mat = GetComponent<Image>().material;
    }

    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset += Vector2.up * speed * Time.deltaTime; // scroll upward
        mat.mainTextureOffset = offset;
    }
}
