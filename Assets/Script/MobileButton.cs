using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public bool isPressed = false;

    [Header("Button Animation")]
    public float pressedScale = 0.85f;
    public float animationSpeed = 15f;

    private RectTransform rectTransform;
    private Vector3 normalScale;
    private Vector3 targetScale;

    private Image buttonImage;
    private Color normalColor;
    private Color pressedColor;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>();

        normalScale = rectTransform.localScale;
        targetScale = normalScale;

        if (buttonImage != null)
        {
            normalColor = buttonImage.color;
            pressedColor = new Color(0.65f, 0.65f, 0.65f, normalColor.a);
        }
    }

    private void Update()
    {
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            animationSpeed * Time.unscaledDeltaTime
        );

        if (buttonImage != null)
        {
            Color targetColor = isPressed ? pressedColor : normalColor;

            buttonImage.color = Color.Lerp(
                buttonImage.color,
                targetColor,
                animationSpeed * Time.unscaledDeltaTime
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        targetScale = normalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        targetScale = normalScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        targetScale = normalScale;
    }
}