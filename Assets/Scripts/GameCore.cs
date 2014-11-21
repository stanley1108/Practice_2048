using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : Singleton<GameCore> {
	//control modules
	private FSM moduleFSM_ = null;

	private bool isFinishedGame_ = false;
	public bool IsFinishedGame
	{
		get{return isFinishedGame_;}
	}
	private bool isWin_ = false;
	public bool IsWin
	{
		get{return isWin_;}
	}

	[SerializeField]
	List<Sprite> numberTextures;

	public Sprite GetCellSprite(int index)
	{
		if((index < 0)||(index >= numberTextures.Count))
		{
			Debug.LogError("GetCellTexture() use wrong index: " + index);
			return null;
		}
		return numberTextures[index];
	}

	// Use this for initialization
	void Start () {

		moduleFSM_ = new FSM();

		Gameplay gamePlayModule = new Gameplay("Gameplay");
		Rank 	 rankModule = new Rank("Rank");


		moduleFSM_.AddState(gamePlayModule);
		moduleFSM_.AddState(rankModule);

		gamePlayModule.AddNextState(rankModule.Name, rankModule);
		rankModule.AddNextState(gamePlayModule.Name, gamePlayModule);

		moduleFSM_.ChangeState(gamePlayModule);

		LoadBestScore();
	}

	string stateMsg_ = "Playing Game";
	Rect stateMsgUIRect_ = new Rect(10, 10, 300, 100);

	string scoreMsg_ = "Current Score:00000";
	Rect scoreMsgUIRect_ = new Rect(10, 25, 300, 100);

	string bestScoreMsg_ = "Best Score:00000";
	Rect bestSMsgUIRect_ = new Rect(10, 40, 300, 100);

	int currScore_ = 0;
	public int CurrScore
	{
		get{return currScore_;}
	}
	int bestScore_ = 0;
	int comboCount = 0;

	void OnGUI()
	{
		SetCurrScoreMsg(currScore_);
		SetBestScoreMsg(bestScore_);
		GUI.Label(stateMsgUIRect_, stateMsg_);
		GUI.Label(scoreMsgUIRect_, scoreMsg_);
		GUI.Label(bestSMsgUIRect_, bestScoreMsg_);
	}

	public void SetBestScoreMsg(int score)
	{
		bestScoreMsg_ = string.Format("Best Score:{0:D5}", score);
	}

	public void SetCurrScoreMsg(int score)
	{
		scoreMsg_ = string.Format("Current Score:{0:D5}", score);
	}

	public void WinGame()
	{
		isFinishedGame_ = true;
		isWin_ = true;

		stateMsg_ = "Game Win!!!!!!!!!!!!!!!!!!!!!!";
	}

	public void LoseGame()
	{
		isFinishedGame_ = true;
		isWin_ = false;

		stateMsg_ = "Game Lose.........";
	}

	public void RestartGame()
	{
		isFinishedGame_ = false;
		isWin_ = false;

		stateMsg_ = "Playing Game";

		currScore_ = 0;
		ResetComboCount();
	}

	public void AddScore(int plusScore)
	{
		if(plusScore <= 0)
			return;

		currScore_ += plusScore;

		if(currScore_ > bestScore_)
		{
			bestScore_ = currScore_;
		}
	}

	public void LoadBestScore()
	{
		if(PlayerPrefs.HasKey("bestScore"))
			bestScore_ = PlayerPrefs.GetInt("bestScore");

	}

	public void SaveBestScore()
	{
		PlayerPrefs.SetInt("bestScore", bestScore_);
		PlayerPrefs.Save();
	}

	public void AddComboCount()
	{
		comboCount++;


		if(comboCount >= GameConfig.ComboCountForDouble)
		{
			// ask cellMap to set one cell be a bomb
			if(CellMap.Instance.IsExistBomb == false)
				CellMap.Instance.RandomGenerateBomb();
		}
		
		stateMsg_ = "Combo : " + comboCount;
	}

	public void ResetComboCount()
	{
		comboCount = 0;

		stateMsg_  = "Playing Game";
	}

	void LateUpdate () {
		moduleFSM_.UpdateFSM();
	}
}
