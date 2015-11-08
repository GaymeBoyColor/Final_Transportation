using UnityEngine;
using System.Collections;

public class Airplane {

	public bool isActive;
	public int x, z;
	public int cargo, cargoCapacity;
	private int xMoveDirection;
	private int zMoveDirection;
	public int points;

	public void SetDirection (int xMoveDirection, int zMoveDirection)	{
		this.xMoveDirection = xMoveDirection;
		this.zMoveDirection = zMoveDirection;
	}

	public void Movement ()	{
		x += xMoveDirection;
		z += zMoveDirection;
		zMoveDirection = 0;
		xMoveDirection = 0;
	}

	public void PointTotals ()	{
		points += cargo;
	}
}