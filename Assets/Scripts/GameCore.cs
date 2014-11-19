using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : Singleton<GameCore> {
	//control modules
	private FSM moduleFSM_ = null;

	public int mapSize_X = 4;
	public int mapSize_Y = 4;
	private Dictionary<int, Dictionary<int, Cell>> cellMaps = new Dictionary<int, Dictionary<int, Cell>>();

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

		//setup cells
		for(int i=0; i < mapSize_X; i++)
		{
			cellMaps.Add(i, new Dictionary<int, Cell>());
			for(int j=0; j< mapSize_Y; j++)
			{
				cellMaps[i].Add(j, null);
			}
		}
	}
	

	void LateUpdate () {
		moduleFSM_.UpdateFSM();
	}
}
