using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Slider UISlider;
    private ClubSwinger Swinger;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Swinger)
        {
            float Percent = Swinger.GetPowerPercent();
            UISlider.value = Percent;
            UISlider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, Percent);
        }
        else
        {
            Swinger = FindObjectOfType<ClubSwinger>();
        }
    }
}
