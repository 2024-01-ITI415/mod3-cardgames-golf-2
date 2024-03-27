using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//enum to track possible states of a floating score
public enum eFSState
{
    idle,
    pre,
    active,
    post
}
public class FloatingScore : MonoBehaviour
{
    [Header("Set Dynamically")]
    public eFSState state = eFSState.idle;

    [SerializeField]
    protected int _score = 0;
    public string scoreString;


    //sets both _score and scoreString
    public int score
    {
        get
        {
            return(_score);
        }
        set
        {
            _score = value;
            scoreString = _score.ToString("NO");//NO adds commas to the num
            //ToString formats at C# Standard Numeric Format Strings
            GetComponent<Text>().text = scoreString;
        }
    }

    public List<Vector2> bezierPts;
    public List<float> fontSizes;
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public string easingCurve = Easing.Out;

    //will receive SendMessage when done moving
    public GameObject reportFinishTo = null;

    public RectTransform rectTrans;
    private Text txt;

    //set up FloatingScore and movement
    //use of parameter defaults for eTimes & eTimeD
    public void Init(List<Vector2> ePts, float eTimeS = 0, float eTimeD = 1)
    {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.anchoredPosition = Vector2.zero;

        txt = GetComponent<Text>();

        bezierPts = new List<Vector2>(ePts);

        if(ePts.Count == 1)
        {
            transform.position = ePts[0];
            return;
        }

        if (eTimeS == 0) eTimeS = Time.time;
        timeStart = eTimeS;
        timeDuration = eTimeD;

        state = eFSState.pre;//ready to start moving
    }

    public void FSCallback(FloatingScore fs)
    {
        //when called by SendMessage, add score from calling FloatingScore
        score += fs.score;
    }
    void Update()
    {
        //return if not moving
        if (state == eFSState.idle) return;

        float u = (Time.time - timeStart) / timeDuration;
        //curve u using Easing class
        float uC = Easing.Ease(u, easingCurve);
        if (u < 0)
        {
            state = eFSState.pre;
                txt.enabled = false;
        }
        else
        {
            if (u >= 1)
            {
                uC = 1;
                state = eFSState.post;
                if(reportFinishTo != null)
                {
                    //if callback GameObject, use SendMEssage to call FSCallBack
                    reportFinishTo.SendMessage("FSCallback", this);
                    //after its sent, destroy gameobject
                    Destroy(gameObject);
                }
                else
                {
                    //if nothing to callback, let it sit
                    state = eFSState.active;
                }
            }
            else
            {
                //0<=u<1, means is active and moving
                state = eFSState.active;
                txt.enabled = true;
            }
            //use bezier to move to right point
            Vector2 pos = Utils.Bezier(uC, bezierPts);
            rectTrans.anchorMin = rectTrans.anchorMax = pos;
            if(fontSizes != null && fontSizes.Count > 0)
            {
                //if fontSize has values, adjust this GUIText
                int size = Mathf.RoundToInt(Utils.Bezier(uC, fontSizes));
                GetComponent<Text>().fontSize = size;
            }
        }
    }
}
