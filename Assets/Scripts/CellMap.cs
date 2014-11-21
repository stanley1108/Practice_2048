using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellMapManager : Singleton<CellMapManager> {
	public int mapSize_X_ = 4;
	public int mapSize_Y_ = 4;
	private List<List<Cell>> cellMaps_ = new List<List<Cell>>();
	
	private List<Cell> unUsedCells_ = new List<Cell>();
	private List<Cell> usedCells_ = new List<Cell>();

	public GameObject baseCell;

	// Use this for initialization
	void Start () {
		//setup cells
		for(int i=0; i < mapSize_X_; i++)
		{
			cellMaps_.Add(new List<Cell>());
			for(int j=0; j< mapSize_Y_; j++)
			{
				cellMaps_[i].Add(null);
				
				GameObject cellObj = Instantiate(baseCell) as GameObject;
				cellObj.name = "Cell_"+ i + "_" + j;
				Cell newCell = cellObj.GetComponent<Cell>();
				unUsedCells_.Add(newCell);
				cellObj.SetActive(false);
				
				cellObj.transform.parent = transform;
			}
		}
		
		TestMerge();
	}

	private void TestMerge()
	{
		int[] testIntMap = {
			2,2,2,2,
			0,4,4,2,
			2,2,8,8,
			0,2,4,2,
		};
		Cell newUnUsedCell = null;
		for(int i=0; i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				int currLevel = testIntMap[i + j*4];
				if(currLevel == 0)
				{
					cellMaps_[i][j] = null;
					continue;
				}
				
				newUnUsedCell = GetUnUsedCell();
				newUnUsedCell.SetCurrPos(i, j);
				cellMaps_[i][j] = newUnUsedCell;
				
				if(currLevel == 2)
					newUnUsedCell.CurrLevel = Cell.Level.Num_2;
				else if(currLevel == 4)
					newUnUsedCell.CurrLevel = Cell.Level.Num_4;
				else if(currLevel == 8)
					newUnUsedCell.CurrLevel = Cell.Level.Num_8;
			}
		}
		
		return;
		
		
		List<Cell> mergeTargetCells = new List<Cell>();
		GetMergeTargetCells(0, 0, 4, 1, false, ref mergeTargetCells);
		MergeCells(mergeTargetCells, true, false);
		
		GetMergeTargetCells(0, 1, 4, 1, false, ref mergeTargetCells);
		MergeCells(mergeTargetCells, true, false);
		
		GetMergeTargetCells(0, 2, 4, 1, false, ref mergeTargetCells);
		MergeCells(mergeTargetCells, true, false);
		
		GetMergeTargetCells(0, 3, 4, 1, false, ref mergeTargetCells);
		MergeCells(mergeTargetCells, true, false);
	}

	public void GetMergeTargetCells(int headX, int headY, int rangeX, int rangeY, bool isRevert, ref List<Cell> output)
	{
		output.Clear();
		for(int i = headX; i < headX+rangeX; i++)
		{
			for(int j=headY; j < headY+rangeY; j++)
			{
				output.Add(cellMaps_[i][j]);
			}
		} 
		
		if(isRevert)
			output.Reverse();
	}
	
	private void CalTargetPos(Cell cellItem, int targetIdx, int maxIdx, bool moveXDir, bool isReverse)
	{
		int newIdx = targetIdx;
		
		if(isReverse)
			newIdx = maxIdx - targetIdx;
		
		if(moveXDir)
		{
			cellItem.SetTargetPos(newIdx, cellItem.currPos_Y);
			
			cellMaps_[cellItem.currPos_X][cellItem.currPos_Y] = null;
			cellMaps_[cellItem.targetPos_X][cellItem.targetPos_Y] = cellItem;
		}
		else
		{
			cellItem.SetTargetPos(cellItem.currPos_X, newIdx);
			
			cellMaps_[cellItem.currPos_X][cellItem.currPos_Y] = null;
			cellMaps_[cellItem.targetPos_X][cellItem.targetPos_Y] = cellItem;
		}
	}
	
	public void MergeCells(List<Cell> originCell, bool moveXDir, bool isReverse)
	{
		Debug.Log("Before merge:");
		string msg = "";
		foreach(Cell cellItem in originCell)
		{
			if(cellItem == null)
				continue;
			msg += "Cell_" + cellItem.currPos_X + "_" + cellItem.currPos_Y + "is " + cellItem.CurrLevel + "\n";
		}
		
		Debug.Log(msg);
		
		List<Cell> resultCell = new List<Cell>();
		for(int i=0; i<originCell.Count; i++)
			resultCell.Add(null);
		
		int mergeIdx = 0;
		int listLength = originCell.Count;
		int maxIdx = listLength - 1;
		
		for(int i=0; i<listLength; i++)
		{
			if(originCell[i] == null)
				continue;
			
			if(resultCell[mergeIdx] == null)
			{
				resultCell[mergeIdx] = originCell[i];
				
				CalTargetPos(resultCell[mergeIdx], mergeIdx, maxIdx, moveXDir, isReverse);
				resultCell[mergeIdx].state_ = Cell.State.Moving;
				continue;
			}
			
			if(resultCell[mergeIdx].CurrLevel == originCell[i].CurrLevel)
			{
				resultCell[mergeIdx].state_ = Cell.State.MovingToDie;
				originCell[i].CurrLevel = (Cell.Level)(((int)originCell[i].CurrLevel)+1);
				resultCell[mergeIdx].CurrLevel = originCell[i].CurrLevel;
				resultCell[mergeIdx] = originCell[i];
				
				CalTargetPos(resultCell[mergeIdx], mergeIdx, maxIdx, moveXDir, isReverse);
				resultCell[mergeIdx].state_ = Cell.State.Moving;
				
				mergeIdx++;
			}
			else
			{
				mergeIdx++;
				resultCell[mergeIdx] = originCell[i];
				
				CalTargetPos(resultCell[mergeIdx], mergeIdx, maxIdx, moveXDir, isReverse);
				resultCell[mergeIdx].state_ = Cell.State.Moving;
			}
		}
		
		Debug.Log("After merge");
		msg = "";
		foreach(Cell cellItem in resultCell)
		{
			if(cellItem == null)
				continue;
			msg += "Cell_" + cellItem.targetPos_X + "_" + cellItem.targetPos_Y + "is " + cellItem.CurrLevel + "\n";
		}
		Debug.Log(msg);
	}
	
	public Cell GetUnUsedCell()
	{
		if(unUsedCells_.Count == 0)
			return null;
		
		Cell popedCell = unUsedCells_[0];
		
		usedCells_.Add(popedCell);
		unUsedCells_.Remove(popedCell);
		
		popedCell.gameObject.SetActive(true);
		
		return popedCell;
	}
	
	public void RecycleCell(Cell cellItem)
	{
		if(usedCells_.Contains(cellItem))
			usedCells_.Remove(cellItem);
		else
			Debug.LogError("RecycleCell(): unUsed cell : "+cellItem.name);
		
		cellItem.CurrLevel = Cell.Level.Num_2;
		cellItem.gameObject.SetActive(false);
		
		unUsedCells_.Add(cellItem);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
