using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golf
{
    public enum eScoreEvent
    {
        draw,
        mine,
        mineGold,
        gameWin,
        gameLoss
    }
    public class ScoreManager : MonoBehaviour
    {
        static private ScoreManager S;
        static public int SCORE_FROM_PREV_ROUND = 0;
        static public int HIGH_SCORE = 0;

        [Header("Set Dynamically")]
        public int chain = 0;
        public int scoreRun = 0;
        public int score = 0;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
            }

            if (PlayerPrefs.HasKey("ProspectorHighScore"))
            {
                HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
            }

            score += SCORE_FROM_PREV_ROUND;

            SCORE_FROM_PREV_ROUND = 0;
        }

        static public void EVENT(eScoreEvent evt)
        {
            try
            {
                S.Event(evt);
            }
            catch (System.NullReferenceException nre)
            {
                Debug.LogError("ScoreManager:EVENT() called while S = null.\n" + nre);
            }
        }

        void Event(eScoreEvent evt)
        {
            switch (evt)
            {
                //same thing needs to happen if draw, win, or loss
                case eScoreEvent.draw:
                case eScoreEvent.gameWin:
                case eScoreEvent.gameLoss:
                    chain = 0;
                    score += scoreRun;
                    scoreRun = 0;
                    break;

                case eScoreEvent.mine:
                    chain++;
                    scoreRun += chain;
                    break;
            }

            //handles wins and losses
            switch (evt)
            {
                case eScoreEvent.gameWin:
                    //if win, add score to next round
                    SCORE_FROM_PREV_ROUND = score;
                    print("You win this round! Round score: " + score);
                    break;

                case eScoreEvent.gameLoss:
                    //if loss, check against highscore
                    if (HIGH_SCORE <= score)
                    {
                        print("You got the high score! High score: " + score);
                        HIGH_SCORE = score;
                        PlayerPrefs.SetInt("ProspectorHighScore", score);
                    }
                    else
                    {
                        print("Your final score for the game was: " + score);
                    }
                    break;

                default:
                    print("score:" + score + "  scoreRun: " + scoreRun + "   Chain: " + chain);
                    break;
            }
        }

        static public int CHAIN { get { return S.chain; } }
        static public int SCORE { get { return S.score; } }
        static public int SCORE_RUN { get { return S.scoreRun; } }
    }
}
