using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightTMPText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text menuText;           // Tham chiếu đến TextMeshPro cần highlight
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        if (menuText == null)
            menuText = GetComponent<TMP_Text>();
        menuText.color = normalColor;
    }

    // Khi con trỏ di chuyển vào đối tượng
    public void OnPointerEnter(PointerEventData eventData)
    {
        menuText.color = highlightColor;
    }

    // Khi con trỏ rời khỏi đối tượng
    public void OnPointerExit(PointerEventData eventData)
    {
        menuText.color = normalColor;
    }
}
