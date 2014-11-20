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
			return;


	}
	
	public override State RunState(FSM fsm)
	{

//		if(InputManager.IsSwipeRight())
//		{
//			Debug.Log("Change to next state");
//			return m_nextStates["Rank"];
//		}

		List<Cell> mergeTargetCells = new List<Cell>();

		if(InputManager.IsSwipeLeft())
		{
			for(int i=0; i<GameCore.Instance.mapSize_X_; i++)
			{
				GameCore.Instance.GetMergeTargetCells(0, i, 4, 1, false, ref mergeTargetCells);
				GameCore.Instance.MergeCells(mergeTargetCells, true, false);
			}
		}
		else if(InputManager.IsSwipeRight())
		{
			for(int i=0; i<GameCore.Instance.mapSize_X_; i++)
			{
				GameCore.Instance.GetMergeTargetCells(0, i, 4, 1, true, ref mergeTargetCells);
				GameCore.Instance.MergeCells(mergeTargetCells, true, true);
			}
		}
		else if(InputManager.IsSwipeUp())
		{
			for(int i=0; i<GameCore.Instance.mapSize_X_; i++)
			{
				GameCore.Instance.GetMergeTargetCells(i, 0, 1, 4, false, ref mergeTargetCells);
				GameCore.Instance.MergeCells(mergeTargetCells, false, false);
			}
		}
		else if(InputManager.IsSwipeDown())
		{
			for(int i=0; i<GameCore.Instance.mapSize_X_; i++)
			{
				GameCore.Instance.GetMergeTargetCells(i, 0, 1, 4, true, ref mergeTargetCells);
				GameCore.Instance.MergeCells(mergeTargetCells, false, true);
			}
		}

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave Gameplay");
	}
}
