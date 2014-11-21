using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellMap : Singleton<CellMap> {
	public int mapSize_X_ = 4;
	public int mapSize_Y_ = 4;
	private List<List<Cell>> cellMaps_ = new List<List<Cell>>();
	
	private List<Cell> unUsedCells_ = new List<Cell>();
	private List<Cell> usedCells_ = new List<Cell>();

	public GameObject baseCell;

	private bool isReadyUpgradeCell_ = false;
	public bool IsReadyUpgradeCell
	{
		get{return isReadyUpgradeCell_;}
		set{isReadyUpgradeCell_ = value;}
	}

	private Cell.Level upgradLevel_ = Cell.Level.Num_2;
	public Cell.Level UpgradeLevel
	{
		set{upgradLevel_ = value;}
	}

	private bool isExistBomb_ = false;
	public bool IsExistBomb
	{
		get{return isExistBomb_;}
		set{isExistBomb_ = value;}
	}

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
//			2,2,2,2,
//			0,4,4,2,
//			2,2,8,8,
//			2,2,8,8,

//			0,0,0,0,
//			0,0,0,0,
//			0,0,0,0,
//			0,1024,1024,2,
			 2, 4, 0,32,
			 4, 8,16, 8,
			 8,16,32, 16,
			16, 8, 4, 8,
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
				else if(currLevel == 16)
					newUnUsedCell.CurrLevel = Cell.Level.Num_16;
				else if(currLevel == 32)
					newUnUsedCell.CurrLevel = Cell.Level.Num_32;
				else if(currLevel == 64)
					newUnUsedCell.CurrLevel = Cell.Level.Num_64;
				else if(currLevel == 128)
					newUnUsedCell.CurrLevel = Cell.Level.Num_128;
				else if(currLevel == 256)
					newUnUsedCell.CurrLevel = Cell.Level.Num_256;
				else if(currLevel == 512)
					newUnUsedCell.CurrLevel = Cell.Level.Num_512;
				else if(currLevel == 1024)
					newUnUsedCell.CurrLevel = Cell.Level.Num_1024;
				else if(currLevel == 2048)
					newUnUsedCell.CurrLevel = Cell.Level.Num_2048;

			}
		}
		
		return;
		
		
//		List<Cell> mergeTargetCells = new List<Cell>();
//		GetMergeTargetCells(0, 0, 4, 1, false, ref mergeTargetCells);
//		MergeCells(mergeTargetCells, true, false);
//		
//		GetMergeTargetCells(0, 1, 4, 1, false, ref mergeTargetCells);
//		MergeCells(mergeTargetCells, true, false);
//		
//		GetMergeTargetCells(0, 2, 4, 1, false, ref mergeTargetCells);
//		MergeCells(mergeTargetCells, true, false);
//		
//		GetMergeTargetCells(0, 3, 4, 1, false, ref mergeTargetCells);
//		MergeCells(mergeTargetCells, true, false);
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

	//return: is any cell moved 
	public bool MergeCells(List<Cell> originCell, bool moveXDir, bool isReverse)
	{
//		Debug.Log("Before merge:");
//		string msg = "";
//		foreach(Cell cellItem in originCell)
//		{
//			if(cellItem == null)
//				continue;
//			msg += "Cell_" + cellItem.currPos_X + "_" + cellItem.currPos_Y + "is " + cellItem.CurrLevel + "\n";
//		}
//		
//		Debug.Log(msg);
		bool isMoved = false;
		
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

				if((resultCell[mergeIdx].currPos_X != resultCell[mergeIdx].targetPos_X) ||
				   (resultCell[mergeIdx].currPos_Y != resultCell[mergeIdx].targetPos_Y))
					isMoved = true;

				continue;
			}
			
			if(resultCell[mergeIdx].CurrLevel == originCell[i].CurrLevel)
			{
				resultCell[mergeIdx].state_ = Cell.State.MovingToDie;
				originCell[i].CurrLevel = (Cell.Level)(((int)originCell[i].CurrLevel)+1);
				resultCell[mergeIdx].CurrLevel = originCell[i].CurrLevel;
				resultCell[mergeIdx] = originCell[i];
				
				CalTargetPos(resultCell[mergeIdx], mergeIdx, maxIdx, moveXDir, isReverse);
				resultCell[mergeIdx].state_ = Cell.State.MovingToMerge;
				
				mergeIdx++;
				isMoved = true;
			}
			else
			{
				mergeIdx++;
				resultCell[mergeIdx] = originCell[i];
				
				CalTargetPos(resultCell[mergeIdx], mergeIdx, maxIdx, moveXDir, isReverse);
				resultCell[mergeIdx].state_ = Cell.State.Moving;

				if((resultCell[mergeIdx].currPos_X != resultCell[mergeIdx].targetPos_X) ||
				   (resultCell[mergeIdx].currPos_Y != resultCell[mergeIdx].targetPos_Y))
					isMoved = true;
			}
		}

		return isMoved;
		
//		Debug.Log("After merge");
//		msg = "";
//		foreach(Cell cellItem in resultCell)
//		{
//			if(cellItem == null)
//				continue;
//			msg += "Cell_" + cellItem.targetPos_X + "_" + cellItem.targetPos_Y + "is " + cellItem.CurrLevel + "\n";
//		}
//		Debug.Log(msg);
	}
	
	public Cell GetUnUsedCell()
	{
		if(unUsedCells_.Count == 0)
			return null;
		
		Cell popedCell = unUsedCells_[0];
		
		usedCells_.Add(popedCell);
		unUsedCells_.Remove(popedCell);
		
		popedCell.Reset();
		
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

	public bool IsMoveable()
	{
		Cell currCell = null;
		Cell nextXCell = null;
		Cell nextYCell = null;

		for(int i=0; i<mapSize_X_; i++)
		{
			for(int j=0; j<mapSize_Y_; j++)
			{
				currCell = cellMaps_[i][j];
				nextXCell = nextYCell = null;

				if(i != mapSize_X_-1)
					nextXCell = cellMaps_[i+1][j];
				if(j != mapSize_Y_-1)
					nextYCell = cellMaps_[i][j+1];

				if(currCell == null)
					return true;

				if((nextXCell) && (currCell.CurrLevel == nextXCell.CurrLevel))
					return true;

				if((nextYCell) && (currCell.CurrLevel == nextYCell.CurrLevel))
					return true;
			}
		}
		return false;
	}

	public bool Is2048()
	{
		foreach(Cell cellItem in usedCells_)
		{
			if(cellItem.CurrLevel == Cell.Level.Num_2048)
				return true;
		}

		return false;
	}

	public void ResetMap()
	{
		foreach(Cell cellItem in usedCells_)
		{
			cellItem.gameObject.SetActive(false);
			unUsedCells_.Add(cellItem);
		}

		usedCells_.Clear();

		for(int i=0; i < mapSize_X_; i++)
		{
			for(int j=0; j< mapSize_Y_; j++)
			{
				cellMaps_[i][j] = null;
			}
		}

		isExistBomb_ = false;
		isReadyUpgradeCell_ = false;
	}

	public void RandomGenerateCell()
	{
		int posX = Random.Range(0, mapSize_X_);
		int posY = Random.Range(0, mapSize_Y_);

		while(true)
		{
			if(cellMaps_[posX][posY] == null)
			{
				Cell newCell = GetUnUsedCell();
				newCell.SetCurrPos(posX, posY);

				cellMaps_[posX][posY] = newCell;
				return;
			}

			posX = Random.Range(0, mapSize_X_);
			posY = Random.Range(0, mapSize_Y_);
		}
	}

	public void RandomGenerateBomb()
	{
		int bombIdx = Random.Range(0, usedCells_.Count);

		while(true)
		{
			if((usedCells_[bombIdx].CurrLevel != Cell.Level.Num_1024) &&
			   (usedCells_[bombIdx].CurrLevel != Cell.Level.Num_2048))
			{
				usedCells_[bombIdx].IsBomb = true;
				break;
			}
			bombIdx = Random.Range(0, usedCells_.Count);
		}

		isExistBomb_ = true;
	}

	public void UpgradCells()
	{
		foreach(Cell cellItem in usedCells_)
		{
			if(cellItem.CurrLevel == upgradLevel_)
			{
				cellItem.CurrLevel = (Cell.Level)(((int)cellItem.CurrLevel)+1);
				cellItem.state_ = Cell.State.Begin;
			}
		}
	}
}
