using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //Name
    [SerializeField]
    private string TypeName;

    //Beats
    [SerializeField]
    private string BeatsType;

    //Adds
    [SerializeField]
    private string AddsType;

    //Getters/Setters
    public string CardType
    {
        get { return TypeName; }
        set { TypeName = value; }
    }

    public string Beats
    {
        get { return BeatsType; }
        set { BeatsType = value; }
    }

    public string Adds
    {
        get { return AddsType; }
        set { AddsType = value; }
    }
}
