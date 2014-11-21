using UnityEngine;
using System.Collections;

public class Rank : State {
	public Rank(string name):base(name){}

	float currTime = 0;

	public override void EnterState(FSM fsm)
	{
		DBG.Log("Enter Rank");

		if(GameCore.Instance.IsWin)
			DBG.Log("Game win!!!!!!!!!!!!!!!!!!!!!!");
		else
			DBG.Log("Game lose!!!!!!!!!!!!!!!!!!!!!!");

		GameCore.Instance.SaveBestScore();

		currTime = 0;
	}
	
	public override State RunState(FSM fsm)
	{
		currTime += Time.deltaTime;

		if(currTime < GameConfig.RankTime)
			return null;

		if(InputManager.IsPressAnyKey())
		{
			return m_nextStates["Gameplay"];
		}

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		GameCore.Instance.RestartGame();

		DBG.Log("Leave Rank");
	}
}
