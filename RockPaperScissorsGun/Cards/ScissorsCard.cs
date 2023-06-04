using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsCard : Card
{
    public ScissorsCard()
    {
        CardType = "Scissors";

        Beats = "Paper";

        Adds = "Fire";
    }
}
