using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;

namespace Win2dFun
{
	internal class Player : IUpdatable, IDrawable, ICollidable
	{
		private const int Width = 16;
		private const int Height = 48;
		private const double SpeedX = 5;
		private const double MaxPositiveVelocityY = 50;
		private const double MaxNegativeVelocityY = -20;
		private const double GravitationalAcceleration = 1;

		private InputManager inputManager;

		// TODO: Remove
		private List<ICollidable> collidables;

		private Rect collider = new Rect(0, 0, Width, Height);

		private bool grounded;
		private ICollidable groundCache;

		// Because of the way the coordinate system works, a negative y velocity means the entity
		// is rising, whereas a positive y velocity means the entity is falling
		private double velocityY;

		private bool spaceKeyProcessed = false;

		public Player(double x, double y, InputManager inputManager)
		{
			this.inputManager = inputManager;
			this.Reset(x, y);
		}

		public Rect GetCollider()
		{
			return this.collider;
		}

		public bool IsColliding(ICollidable other)
		{
			return RectCollider.RectsIntersect(this.collider, other.GetCollider());
		}

		public void Reset(double x, double y)
		{
			this.collider.X = x;
			this.collider.Y = y;
			this.grounded = false;
			this.groundCache = null;
			this.velocityY = 0;
		}

		public void Update()
		{
			double oldX = this.collider.X;
			double oldY = this.collider.Y;
			double velocityX = 0;
			bool movingLeft = false;
			bool movingRight = false;

			if (this.inputManager.LeftKeyPressed && this.inputManager.RightKeyPressed)
			{
				// Left and Right keys cancel each other out
			}
			else if (this.inputManager.LeftKeyPressed)
			{
				velocityX = -SpeedX;
				movingLeft = true;
			}
			else if (this.inputManager.RightKeyPressed)
			{
				velocityX = SpeedX;
				movingRight = true;
			}

			if (!this.grounded)
			{
				this.velocityY += GravitationalAcceleration;
				this.velocityY = Math.Min(this.velocityY, MaxPositiveVelocityY);
			}
			else
			{
				if (this.inputManager.SpaceKeyPressed)
				{
					if (!this.spaceKeyProcessed)
					{
						this.velocityY = MaxNegativeVelocityY;
						this.grounded = false;
						this.groundCache = null;

						this.spaceKeyProcessed = true;
					}
				}
				else
				{
					this.spaceKeyProcessed = false;
				}
			}

			if (movingLeft || movingRight)
			{
				double newX = this.collider.X + velocityX;
				var pathCollider = new RectCollider(
					movingLeft ? newX : this.collider.X + this.collider.Width,
					this.collider.Y,
					Math.Abs(this.collider.X - newX),
					this.collider.Height);

				foreach (var collidable in this.collidables)
				{
					if (collidable != this && pathCollider.IsColliding(collidable))
					{
						Debug.Assert(!this.IsColliding(collidable));

						Rect collided = collidable.GetCollider();
						newX = movingLeft ? collided.X + collided.Width : collided.X - this.collider.Width;
						pathCollider = new RectCollider(
							movingLeft ? newX : this.collider.X + this.collider.Width,
							this.collider.Y,
							Math.Abs(this.collider.X - newX),
							this.collider.Height);
					}
				}

				this.collider.X = newX;
			}

			if (!this.grounded)
			{
				// TODO Epsilon
				bool movingUp = this.velocityY < 0;
				double newY = this.collider.Y + this.velocityY;
				var pathCollider = new RectCollider(
					this.collider.X,
					movingUp ? newY : this.collider.Y + this.collider.Height,
					this.collider.Width,
					Math.Abs(this.collider.Y - newY));

				foreach (var collidable in this.collidables)
				{
					if (collidable != this && pathCollider.IsColliding(collidable))
					{
						Debug.Assert(!this.IsColliding(collidable));

						if (movingUp)
						{
							this.velocityY = 0;
						}
						else
						{
							this.velocityY = 0;
							this.grounded = true;
							this.groundCache = collidable;
						}

						Rect collided = collidable.GetCollider();
						newY = movingUp ? collided.Y + collided.Height : collided.Y - this.collider.Height;
						pathCollider = new RectCollider(
							this.collider.X,
							movingUp ? newY : this.collider.Y + this.collider.Height,
							this.collider.Width,
							Math.Abs(this.collider.Y - newY));
					}
				}

				this.collider.Y = newY;
			}
			else
			{
				var groundCheck = new RectCollider(
					this.collider.X,
					this.collider.Y - 1,
					this.collider.Width,
					this.collider.Height);
				if (!groundCheck.IsColliding(this.groundCache))
				{
					this.grounded = false;
					this.groundCache = null;
				}
			}
		}

		public void Draw(CanvasDrawingSession cds)
		{
			cds.DrawRectangle(this.collider, Colors.Red);
		}

		public void AddCollidables(List<ICollidable> collidables)
		{
			this.collidables = collidables;
		}
	}
}
