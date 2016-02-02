using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Collections.Generic;

namespace Win2dFun
{
	internal class World
	{
		private const int PlayerStartX = 96;
		private const int PlayerStartY = 96;

		private Player player;

		private List<IUpdatable> updatables;
		private List<IDrawable> drawables;
		private List<ICollidable> collidables;

		//private bool enterKeyProcessed = false;
		private bool rKeyProcessed = false;

		public InputManager InputManager { get; private set; }

		public List<ICollidable> Collidables { get { return this.collidables; } }

		public World()
		{
			this.InputManager = new InputManager();
			this.player = new Player(PlayerStartX, PlayerStartY, this.InputManager);
			var obstacles = new RectCollider[]
			{
				new RectCollider(0, 768, 768, 256),
				new RectCollider(16, 512, 16, 256),
				new RectCollider(384, 704, 96, 16),
				new RectCollider(128, 672, 96, 16),
				new RectCollider(256, 624, 96, 16),
				new RectCollider(384, 576, 96, 16),
				new RectCollider(512, 576, 96, 16),
				new RectCollider(512, 496, 96, 16),
				new RectCollider(256, 368, 96, 16),
				new RectCollider(160, 368, 96, 16)
			};

			this.updatables = new List<IUpdatable>();
			this.updatables.Add(this.player);

			this.drawables = new List<IDrawable>();
			this.drawables.Add(this.player);
			this.drawables.Add(obstacles[0]);
			this.drawables.Add(obstacles[1]);
			this.drawables.Add(obstacles[2]);
			this.drawables.Add(obstacles[3]);
			this.drawables.Add(obstacles[4]);
			this.drawables.Add(obstacles[5]);
			this.drawables.Add(obstacles[6]);
			this.drawables.Add(obstacles[7]);
			this.drawables.Add(obstacles[8]);
			this.drawables.Add(obstacles[9]);

			this.collidables = new List<ICollidable>();
			this.collidables.Add(this.player);
			this.collidables.Add(obstacles[0]);
			this.collidables.Add(obstacles[1]);
			this.collidables.Add(obstacles[2]);
			this.collidables.Add(obstacles[3]);
			this.collidables.Add(obstacles[4]);
			this.collidables.Add(obstacles[5]);
			this.collidables.Add(obstacles[6]);
			this.collidables.Add(obstacles[7]);
			this.collidables.Add(obstacles[8]);
			this.collidables.Add(obstacles[9]);

			this.player.AddCollidables(this.collidables);
		}

		public void Update(
			ICanvasAnimatedControl sender,
			CanvasAnimatedUpdateEventArgs args)
		{
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
				updatable.Update();
			}
		}

		public void Draw(
			ICanvasAnimatedControl sender,
			CanvasAnimatedDrawEventArgs args)
		{
			var cds = args.DrawingSession;
			foreach (var drawable in this.drawables)
			{
				drawable.Draw(cds);
			}
		}
	}
}
