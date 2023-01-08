using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCard : Card
{
    public RockCard()
    {
        CardType = "Rock";

        Beats = "Scissors";

        Adds = "Water";
    }
}
