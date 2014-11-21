using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : Singleton<GameCore> {
	//control modules
	private FSM moduleFSM_ = null;

	public float cellSize_ = 128;

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






	void LateUpdate () {
		moduleFSM_.UpdateFSM();
	}
}
