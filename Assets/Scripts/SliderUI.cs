using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    [SerializeField] Transform groupLayout; 
    [SerializeField] float speed;

    Slider slider;
    RectTransform rtGroupLayout;

    float screenWidth;

    void Start()
    {
        slider = GetComponent<Slider>();
        rtGroupLayout = groupLayout.GetComponent<RectTransform>();

        slider.enabled = false;

        screenWidth = Screen.width;
        slider.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        float countChild = groupLayout.childCount;
        float groupLayoutWidth = 110 * (countChild);

        rtGroupLayout.sizeDelta = new(groupLayoutWidth, 0);

        if (countChild > screenWidth/110)
        {
            slider.enabled = true;

            float distancePerSide = (groupLayoutWidth - screenWidth) / 2f;

            float distanceMovePerPercent = distancePerSide / 0.5f;

            float sliderMoveDistance = (slider.value - 0.5f) * distanceMovePerPercent;

            float smoothMove = Mathf.Lerp(rtGroupLayout.localPosition.x, sliderMoveDistance, speed * Time.deltaTime);
            rtGroupLayout.localPosition = new(smoothMove, rtGroupLayout.localPosition.y);
            //                                          ||
            /*Vector2 newLocalPosition = new(sliderMoveDistance, rtGroupLayout.localPosition.y);
            rtGroupLayout.localPosition = Vector3.Lerp(rtGroupLayout.localPosition, newLocalPosition, speed * Time.deltaTime);*/

        }


        
    }
}
