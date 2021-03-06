﻿using UnityEngine;

public class Bullet : ICollideable, IProjectile, IPoolable
{
	/// <summary> ICollideable HasCollided Implementation. </summary>
	public bool HasCollided { get; set; }
	/// <summary> Reference to the Bullet GameObject. </summary>
	public GameObject BulletGO { get; private set; }
	public bool Active { get; set; }
	/// <summary> The speed at which the bullet travels. </summary>
	private float _bulletSpeed = 10f;
	/// <summary> Size of the Player. </summary>
	private float _size;
	/// <summary> Which Layers to check for collision. </summary>
	private LayerMask _collisionMask;
	/// <summary> Reference to the Rigibody2D component. </summary>
	private SpriteRenderer _spriteRenderer;
	/// <summary> The BoxCollider2D Component of the Bulletss. </summary>
	private BoxCollider2D _boxCollider2D;

	/// <summary>
	/// instantiates bullet and makes a new game object with all needed components
	/// </summary>
	public Bullet()
	{
		BulletGO = new GameObject
		{
			name = "Bullet"
		};

		BoxCollider2D boxCollider2D = BulletGO.AddComponent<BoxCollider2D>();
		_boxCollider2D = boxCollider2D;
		_boxCollider2D.isTrigger = true;

		Sprite sprite = Resources.Load<Sprite>("Sprites/Bullet");
		SpriteRenderer spriteRenderer = BulletGO.AddComponent<SpriteRenderer>();
		_spriteRenderer = spriteRenderer;
		_spriteRenderer.sprite = sprite;

		_collisionMask = ~LayerMask.GetMask("Projectile", "Player");
		BulletGO.layer = LayerMask.NameToLayer("Projectile");

		HasCollided = false;

		CollisionManager.COLLIDEABLES.Add(this);
	}

	/// <summary>
	/// Updates the bullet. Right now only moves it using transform.translate
	/// </summary>
	public void Update()
	{
		BulletGO.transform.Translate(BulletGO.transform.up * _bulletSpeed * Time.deltaTime, Space.World);
	}

	/// <summary>
	/// ICollideable IsColliding Implementation.
	/// </summary>
	public bool IsColliding()
	{
		Collider2D[] collisions = Physics2D.OverlapCircleAll(BulletGO.gameObject.transform.position, _size, _collisionMask);

		if(collisions.Length > 0)
		{
			foreach(Collider2D collider in collisions)
			{
				if(collider != this._boxCollider2D)
				{
					return true;
				}
			}
		}

		return false;
	}

	public void OnCollision()
	{
		Debug.Log(BulletGO.name + " Collided!");
		EventManager<Bullet>.InvokeEvent(EventType.ON_ASTEROID_DESTROYED, this);
	}

	/// <summary>
	/// Excecuted when bullet is reactivated. It will set all values to the desired valued
	/// </summary>
	public void OnActivate(float size, Vector3 startPos, float direction, float speed)
	{
		BulletGO.transform.position = startPos;
		BulletGO.transform.rotation = Quaternion.Euler(0, 0, direction);
		BulletGO.transform.localScale = new Vector3(size, size, size);
		_boxCollider2D.size = new Vector2(size, size);
		_size = size;

		HasCollided = false;
		BulletGO.SetActive(true);
	}

	/// <summary>
	/// Done when bullet is deactivated. Right now only disables the object
	/// </summary>
	public void OnDisable()
	{
		BulletGO.SetActive(false);
	}
}
