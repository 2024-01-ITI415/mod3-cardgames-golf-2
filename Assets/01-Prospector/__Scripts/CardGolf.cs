using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enum, defines variable type with prenamed values
public enum eCardState
{
    drawpile,
    tableau,
    target,
    discard
}


public class CardGolf : Card //extension of card class
{
    [Header("Set Dynamically: CardGolf")]
    public eCardState state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardGolf>();//stores which cards keep this one face down
    public int layoutID;//matches this card to to the XML tableau if it's a tableau card
    public slotDef slotDef;

    public override void OnMouseUpAsButton()
    {
        Golf.S.CardClicked(this);
        base.OnMouseUpAsButton();
    }
}
