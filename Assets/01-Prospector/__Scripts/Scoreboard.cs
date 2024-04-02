using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard S;//scoreboard singleton

    [Header("Set in Inspector")]
    public GameObject prefabFloatingScore;

    [Header("Set Dynamically")]
    [SerializeField] private int _score = 0;
    [SerializeField] private string _scoreString;

    private Transform canvasTrans;

    public int score
    {
        get
        {
            return (_score);
        }
        set
        {
            _score = value;
            _scoreString = _score.ToString("NO");
        }
    }

    public string scoreString
    {
        get
        {
            return (_scoreString);    
        }
        set
        {
            _scoreString = value;
            GetComponent<Text>().text = _scoreString;
        }
    }

    private void Awake()
    {
        if(S == null)
        {
            S = this;//set private singleton
        }
        else
        {
            Debug.LogError("ERROR: Scoreboard.Awake(): S is already set!");
        }
        canvasTrans = transform.parent;
    }

    //adds fs.score to this.score when called by SendMessage
    public void FSCallback(FloatingScore fs)
    {
        score += fs.score;
    }

    //instantiate new floatingScore, initialize it, return pointer too it so function can do more with it
    public FloatingScore CreateFloatingScore(int amt, List<Vector2> pts)
    {
        GameObject go = Instantiate<GameObject>(prefabFloatingScore);
        go.transform.SetParent(canvasTrans);
        FloatingScore fs = go.GetComponent<FloatingScore>();
        fs.score = amt;
        fs.reportFinishTo = this.gameObject;//call back to this
        fs.Init(pts);
        return(fs);
    }

}

