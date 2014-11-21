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


	}

	string msg_ = "Playing Game";
	Rect msgShowRect = new Rect(10, 10, 300, 200);
	void OnGUI()
	{
		GUI.Label(msgShowRect, msg_);
	}


	public void WinGame()
	{
		isFinishedGame_ = true;
		isWin_ = true;

		msg_ = "Game Win!!!!!!!!!!!!!!!!!!!!!!";
	}

	public void LoseGame()
	{
		isFinishedGame_ = true;
		isWin_ = false;

		msg_ = "Game Lose.........";
	}

	public void RestartGame()
	{
		isFinishedGame_ = false;
		isWin_ = false;

		msg_ = "Playing Game";
	}

	void LateUpdate () {
		moduleFSM_.UpdateFSM();
	}
}
