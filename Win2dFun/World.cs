using System.Collections.Generic;
using Windows.UI;

namespace Win2dFun
{
	internal class World : IUpdatable, IDrawable
	{
		public const double Width = 40;
		public const double Height = 30;

		private const double PlayerStartX = 3.75;
		private const double PlayerStartY = 3.75;

		public InputManager InputManager { get; private set; }

		private Player player;

		private List<IUpdatable> updatables;
		private List<IDrawable> drawables;
		private List<ICollidable> collidables;

		//private bool enterKeyProcessed = false;
		private bool rKeyProcessed = false;

		private double framesPerSecond = 0;

		public World()
		{
			this.InputManager = new InputManager();
			this.player = new Player(PlayerStartX, PlayerStartY, this.InputManager);
			const double ratioX = 0.0390625;
			const double ratioY = 0.0390625;
			var obstacles = new RectCollider[]
			{
				new RectCollider(0 * ratioX, 512 * ratioY, 768 * ratioX, 256 * ratioY),
				new RectCollider(16 * ratioX, 256 * ratioY, 16 * ratioX, 256 * ratioY),
				new RectCollider(384 * ratioX, 448 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(128 * ratioX, 416 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(256 * ratioX, 368 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(384 * ratioX, 320 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(512 * ratioX, 320 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(512 * ratioX, 240 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(256 * ratioX, 112 * ratioY, 96 * ratioX, 16 * ratioY),
				new RectCollider(160 * ratioX, 112 * ratioY, 96 * ratioX, 16 * ratioY)
			};

			this.updatables = new List<IUpdatable>();
			this.updatables.Add(this.player);

			this.drawables = new List<IDrawable>();
			this.drawables.Add(this.player);
			foreach (var obstacle in obstacles)
			{
				this.drawables.Add(obstacle);
			}

			this.collidables = new List<ICollidable>();
			this.collidables.Add(this.player);
			foreach (var obstacle in obstacles)
			{
				this.collidables.Add(obstacle);
			}

			this.player.AddCollidables(this.collidables);
		}

		public void Update(double elapsedSeconds)
		{
			this.framesPerSecond = 1 / elapsedSeconds;

			/*
			if (!this.inputManager.EnterKeyPressed)
			{
				this.enterKeyProcessed = false;
				return;
			}

			if (this.enterKeyProcessed)
			{
				return;
			}

			this.enterKeyProcessed = true;
			System.Diagnostics.Debug.WriteLine("Update called");
			*/

			if (!this.InputManager.RKeyPressed)
			{
				this.rKeyProcessed = false;
			}
			else if (!this.rKeyProcessed)
			{
				this.player.Reset(PlayerStartX, PlayerStartY);

				this.rKeyProcessed = true;
			}

			foreach (var updatable in this.updatables)
			{
				updatable.Update(elapsedSeconds);
			}
		}

		public void Draw(Win2dRenderer renderer)
		{
			foreach (var drawable in this.drawables)
			{
				drawable.Draw(renderer);
			}

			renderer.DrawText(
				"FPS: " + this.framesPerSecond,
				10,
				10,
				Colors.White);
		}
	}
}
