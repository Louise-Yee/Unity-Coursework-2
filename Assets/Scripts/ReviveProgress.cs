using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveProgress : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;
    public static Transform targetObject; // Reference to your 3D Transform
    void Start(){
        text.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null){
            UpdateUIPosition();
        }
    }
    private void UpdateUIPosition()
    {
        if (targetObject != null)
        {
            // Get world position from Transform
            Vector3 worldPosition = targetObject.position;

            // Convert world position to screen point
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

            // Convert screen point to local point for RectTransform
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                image.transform.parent as RectTransform, // Parent RectTransform
                screenPoint, // Screen point to convert
                null, // Use current camera
                out anchoredPosition); // Output variable

            // Set anchored position for UI element
            image.rectTransform.anchoredPosition = anchoredPosition; 
            text.rectTransform.anchoredPosition = anchoredPosition; 
        }
    }
}
