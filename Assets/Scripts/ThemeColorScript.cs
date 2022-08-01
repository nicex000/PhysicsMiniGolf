using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EThemeColor
{
    RED,
    GREEN,
    BLUE
}

public class ThemeColorScript : MonoBehaviour
{
    [SerializeField] private Material RedMat;
    [SerializeField] private Material GreenMat;
    [SerializeField] private Material BlueMat;
    [SerializeField] private MeshRenderer BallMesh;
    [SerializeField] private MeshRenderer ClubMesh;
    [SerializeField] private MeshRenderer FlagMesh;

    [SerializeField]
    private EThemeColor _themeColor;

    public EThemeColor ThemeColor
    {
        set
        {
            _themeColor = value;
            SetThemeColor();
        }
        get
        {
            return _themeColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetThemeColor();
    }

    private void SetThemeColor()
    {
        Material[] mats = ClubMesh.materials;
        switch (ThemeColor)
        {
            case EThemeColor.RED:
                BallMesh.material = RedMat;
                mats[1] = RedMat;
                ClubMesh.materials = mats;
                break;
            case EThemeColor.GREEN:
                BallMesh.material = GreenMat;
                mats[1] = GreenMat;
                ClubMesh.materials = mats; 
                break;
            case EThemeColor.BLUE:
                BallMesh.material = BlueMat;
                mats[1] = BlueMat;
                ClubMesh.materials = mats;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
