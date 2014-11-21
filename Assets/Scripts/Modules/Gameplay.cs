using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : State {
	public Gameplay(string name):base(name){}


	private FSM gameplayFSM_ = null;


	
	public override void EnterState(FSM fsm)
	{
		Debug.Log("Enter Gameplay");

		if(gameplayFSM_ != null)
		{
			CellMap.Instance.ResetMap();
			gameplayFSM_.ChangeState("GenerateCell");
			return;
		}

		gameplayFSM_ = new FSM();
		
		WaitInput wait = new WaitInput("WaitInput");
		MoveCell moveCell = new MoveCell("MoveCell");
		GenerateCell generateCell = new GenerateCell("GenerateCell");
		CalResult calResult = new CalResult("CalResult");
		
		gameplayFSM_.AddState(wait);
		gameplayFSM_.AddState(moveCell);
		gameplayFSM_.AddState(generateCell);
		gameplayFSM_.AddState(calResult);
		
		wait.AddNextState(moveCell.Name, moveCell);
		moveCell.AddNextState(generateCell.Name, generateCell);
		generateCell.AddNextState(calResult.Name, calResult);
		calResult.AddNextState(wait.Name, wait);
		
		gameplayFSM_.ChangeState(wait);

	}
	
	public override State RunState(FSM fsm)
	{
		gameplayFSM_.UpdateFSM();

		if(GameCore.Instance.IsFinishedGame)
			return m_nextStates["Rank"];

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave Gameplay");
	}
}
