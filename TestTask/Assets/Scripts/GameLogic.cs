using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
    public enum GameMode { Easy, Hard }
    public GameMode gameMode;
    public int chanceWin;

    public enum ChoiceFigure { Stone, Scissors, Paper }
    public ChoiceFigure playerChoice;
    public ChoiceFigure aiChoice;

    int kWinPlayer;
    int kWinAi;

    System.Random random = new System.Random();
    [SerializeField] private Text scoreText;
    [SerializeField] private Text logBattle;
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject menuUI;

    public void RestartBattle()
    {
        logBattle.text = "";
        battleUI.SetActive(true);
        resultUI.SetActive(false);
    }
    public void Slider_Changed(float value)
    {
        chanceWin = (int)value;
    }
    public void StartEasyMode()
    {
        gameMode = GameMode.Easy;
        StartBattle();
    }
    public void StartHardMode()
    {
        gameMode = GameMode.Hard;
        StartBattle();
    }
    void StartBattle()
    {
        menuUI.SetActive(false);
        battleUI.SetActive(true);
    }

    //first phase
    public void Pick(int pChoise)//0 stone 1 scissors 2 paper
    {
        playerChoice = (ChoiceFigure)pChoise;
        StartCoroutine(StartGame());
    }

    //second phase
    IEnumerator StartGame()
    {
        PickAI();
        ShowChoiceAI();
        yield return new WaitForSeconds(2.0f);
        ResultRound();
    }

    void PickAI()
    {
        switch(gameMode)
        {
            case GameMode.Easy:
                Array values = Enum.GetValues(typeof(ChoiceFigure));
                aiChoice = (ChoiceFigure)values.GetValue(random.Next(values.Length));
                break;
            case GameMode.Hard:
                aiChoice = (ChoiceFigure)GetWinCombWithChance((int)playerChoice);
                break;
        }
    }
    void ShowChoiceAI()
    {
        battleUI.SetActive(false);
        logBattle.text += "AI выбрал " + GetNameFigure(aiChoice);
    }

    int GetWinCombWithChance(int choice)
    {
        if(random.Next(100) <= chanceWin)
        {
            return GetFigurePosBack((int)playerChoice,1);
        }
        else
        {
            if (random.Next(100) < 50)
                return GetFigurePosBack((int)playerChoice, 0);
            else
                return GetFigurePosBack((int)playerChoice, 2);
        }
    }

    int GetFigurePosBack(int indexPlayer,int step) //example:stone+2=paper
    {
        return (indexPlayer - step < 0) ? 2 - (step - (indexPlayer + 1)) : indexPlayer - step;
    }

    void ResultRound()
    {
        int winner = GetWinnerFigure((int)playerChoice, (int)aiChoice);
        if (winner == (int)playerChoice)
        {
            kWinPlayer++;
            ResultUI("победил Игрок");
        }
        else if (winner == (int)aiChoice)
        {
            kWinAi++;
            ResultUI("победил AI");
        }
        else
        {
            ResultUI("ничья");
        }
    }

    void ResultUI(string text)
    {
        logBattle.text += " = " +text;
        scoreText.text = kWinPlayer + ":" + kWinAi;
                resultUI.SetActive(true);
    }

    string GetNameFigure(ChoiceFigure cf)
    {
        switch (cf)
        {
            case ChoiceFigure.Paper:
                return "Бумагу";
            case ChoiceFigure.Scissors:
                return "Ножницы";
            case ChoiceFigure.Stone:
                return "Камень";
            default:
                return "Огонь,Вода и бутылка лимонада цуефа!";
        }
    }

    int GetWinnerFigure(int a, int b)
    {
        if (a == b) return 0;
        return Math.Abs(a - b) == 1 ? (a > b ? b : a) : (a > b ? a : b);
    }

}
