using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Slider UISlider;
    [SerializeField] private TMP_Text HoleText;
    [SerializeField] private TMP_Text StrokesText;
    [SerializeField] private TMP_Text ParText;
    [SerializeField] private TMP_Text ClearText;
    private ClubSwinger Swinger;
    void Start()
    {
        NewHole();
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

    public void SetHoleAndPar(int Hole, int Par)
    {
        HoleText.text = Hole.ToString();
        ParText.text = Par.ToString();
    }

    public void SetStrokes(int Strokes)
    {
        StrokesText.text = Strokes.ToString();
    }


    public void NewHole()
    {
        ClearText.gameObject.SetActive(false);
    }

    public void ClearHole(WinType WinType)
    {
        switch (WinType)
        {
            case WinType.Albatross:
                ClearText.text = "Albatross !!!";
                break;
            case WinType.Eagle:
                ClearText.text = "Eagle !!";
                break;
            case WinType.Birdie:
                ClearText.text = "Birdie !";
                break;
            case WinType.Par:
                ClearText.text = WinType.ToString();
                break;
            case WinType.Bogey:
                ClearText.text = WinType.ToString();
                break;
            case WinType.Double_Bogey:
                ClearText.text = "Double Bogey";
                break;
            case WinType.Hole_Cleared:
                ClearText.text = WinType.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(WinType), WinType, null);
        }

        ClearText.gameObject.SetActive(true);
    }

}
