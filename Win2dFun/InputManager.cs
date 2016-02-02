using Windows.System;

namespace Win2dFun
{
	internal class InputManager
	{
		public bool LeftKeyPressed { get; private set; } = false;
		public bool RightKeyPressed { get; private set; } = false;
		public bool SpaceKeyPressed { get; private set; } = false;
		public bool EnterKeyPressed { get; private set; } = false;
		public bool RKeyPressed { get; private set; } = false;

		public void KeyDownOnGameLoopThread(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.Left:
					this.LeftKeyPressed = true;
					break;
				case VirtualKey.Right:
					this.RightKeyPressed = true;
					break;
				case VirtualKey.Space:
					this.SpaceKeyPressed = true;
					break;
				case VirtualKey.Enter:
					this.EnterKeyPressed = true;
					break;
				case VirtualKey.R:
					this.RKeyPressed = true;
					break;
			}
		}

		public void KeyUpOnGameLoopThread(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.Left:
					this.LeftKeyPressed = false;
					break;
				case VirtualKey.Right:
					this.RightKeyPressed = false;
					break;
				case VirtualKey.Space:
					this.SpaceKeyPressed = false;
					break;
				case VirtualKey.Enter:
					this.EnterKeyPressed = false;
					break;
				case VirtualKey.R:
					this.RKeyPressed = false;
					break;
			}
		}

		public static bool IsRelevantInput(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.Left:
				case VirtualKey.Right:
				case VirtualKey.Space:
				case VirtualKey.Enter:
				case VirtualKey.R:
					return true;
				default:
					return false;
			}
		}
	}
}
