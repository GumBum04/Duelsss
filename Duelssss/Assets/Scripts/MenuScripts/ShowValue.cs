using UnityEngine;
using TMPro;

public class ShowValue : MonoBehaviour
{
    TMP_Text percentageText;

    void Start()
    {
        percentageText = GetComponent<TMP_Text>();
    }

    public void textUpdate(float value)
    {
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
