﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;

	[Header("Set in Inspector")]
	public TextAsset			deckXML;
	public TextAsset			layoutXML;
	public float				xOffset = 3;
	public float				yOffset = -2.5f; //why is this f but x isnt?
	public Vector3				layoutCenter;


	[Header("Set Dynamically")]
	public Deck					deck;
	public Layout				layout;
	public List<CardProspector> drawPile;
	public Transform			layoutAnchor;
	public CardProspector		target;
	public List<CardProspector> tableau;
	public List<CardProspector> discardPile;

	void Awake(){
		S = this;
	}

	void Start() {
		deck = GetComponent<Deck> ();
		deck.InitDeck (deckXML.text);
		Deck.Shuffle(ref deck.cards);

		//Card c;
		//for(int cNum = 0; cNum<deck.cards.Count; cNum++)
		//{
		//	c = deck.cards[cNum];
		//	c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
		//}
		layout = GetComponent<Layout> ();
		layout.ReadLayout(layoutXML.text);

		drawPile = ConvertListCardsToListCardProspectors(deck.cards);
		LayoutGame();
	}

	List<CardProspector> ConvertListCardsToListCardProspectors(List<Card> lCD)
	{
		List<CardProspector> lCP = new List<CardProspector> ();
		CardProspector tCP;
		foreach(Card tCD in lCD) {
			tCP = tCD as CardProspector;
			lCP.Add(tCP);
		}
		return(lCP);
	}

	//pull single card from drawPile and return
	CardProspector Draw()
	{
		CardProspector cd = drawPile[0];
		drawPile.RemoveAt(0);
		return(cd);
	}

    //LayoutGame() positions initial tableau
    void LayoutGame()
    {
        //create empty gameobject to serve as tableau anchor
		if (layoutAnchor == null)
		{
			GameObject tGO = new GameObject("_LayoutAnchor");
			layoutAnchor = tGO.transform;
			layoutAnchor.transform.position = layoutCenter;
		}

		CardProspector cp;
		foreach(slotDef tSD in layout.slotDefs)
		{
			//iterate through slotDefs as tSD
			cp = Draw();
			cp.faceUp = tSD.faceup;//set faceup value to the value in SlotDef
			cp.transform.parent = layoutAnchor; // make layoutanchor the parent
			cp.transform.localPosition = new Vector3 (layout.multiplier.x *tSD.x, 
				layout.multiplier.y * tSD.y, -tSD.layerID);
			cp.layoutID = tSD.id;
			cp.slotDef = tSD;
			//cards in tableau should have the cardstate tableau
			cp.state = eCardState.tableau;
			tableau.Add(cp);

		}
    }

}
