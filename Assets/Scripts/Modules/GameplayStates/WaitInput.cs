using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//wait inputs like swipe, open menu...etc 
public class WaitInput : State {
	public WaitInput(string name):base(name){}
	
	public override void EnterState(FSM fsm)
	{
		
	}
	
	public override State RunState(FSM fsm)
	{
		List<Cell> mergeTargetCells = new List<Cell>();

		bool isMoved = false;
		
		if(InputManager.IsSwipeLeft())
		{
			for(int i=0; i<CellMap.Instance.mapSize_X_; i++)
			{
				CellMap.Instance.GetMergeTargetCells(0, i, 4, 1, false, ref mergeTargetCells);
				isMoved |= CellMap.Instance.MergeCells(mergeTargetCells, true, false);
			}
		}
		else if(InputManager.IsSwipeRight())
		{
			for(int i=0; i<CellMap.Instance.mapSize_X_; i++)
			{
				CellMap.Instance.GetMergeTargetCells(0, i, 4, 1, true, ref mergeTargetCells);
				isMoved |= CellMap.Instance.MergeCells(mergeTargetCells, true, true);
			}
		}
		else if(InputManager.IsSwipeUp())
		{
			for(int i=0; i<CellMap.Instance.mapSize_X_; i++)
			{
				CellMap.Instance.GetMergeTargetCells(i, 0, 1, 4, false, ref mergeTargetCells);
				isMoved |= CellMap.Instance.MergeCells(mergeTargetCells, false, false);
			}
		}
		else if(InputManager.IsSwipeDown())
		{
			for(int i=0; i<CellMap.Instance.mapSize_X_; i++)
			{
				CellMap.Instance.GetMergeTargetCells(i, 0, 1, 4, true, ref mergeTargetCells);
				isMoved |= CellMap.Instance.MergeCells(mergeTargetCells, false, true);
			}
		}

		if(isMoved)
			return m_nextStates["MoveCell"];

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave WaitInput");
	}
}
