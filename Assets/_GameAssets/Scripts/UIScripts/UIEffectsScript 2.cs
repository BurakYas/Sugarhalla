using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIEffectsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text targetText;
    public Color hoverColor = Color.yellow;
    public float hoverScale = 1.2f;
    public float duration = 0.2f;

    private Color originalColor;
    private Vector3 originalScale;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<TMP_Text>();
        originalColor = targetText.color;
        originalScale = targetText.rectTransform.localScale;
    }

    // OnPointerEnter is called when the mouse pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetText.DOColor(hoverColor, duration);
        targetText.rectTransform.DOScale(hoverScale, duration);
    }

    // OnPointerExit is called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        targetText.DOColor(originalColor, duration);
        targetText.rectTransform.DOScale(originalScale, duration);
    }
}
