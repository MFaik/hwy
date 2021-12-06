using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField] Image PP;
    [SerializeField] TextMeshProUGUI TextMesh;

    Image s_PP;
    TextMeshProUGUI s_textMesh;

    void Start() {
        s_PP = PP;
        s_textMesh = TextMesh;
    }

    public static void SetText() {

    }
}
