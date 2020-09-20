﻿using UnityEngine;

public class RotateRightCommand : IGameObjectCommand
{
	private float _rotationPower = 150;

	public void Execute(GameObject origin)
	{
		origin.transform.Rotate(Vector3.forward, -_rotationPower * Time.deltaTime);
	}
}
