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

		public double SliderValue { get; private set; } = 0;

		public void KeyDown(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.A:
					this.LeftKeyPressed = true;
					break;
				case VirtualKey.D:
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

		public void KeyUp(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.A:
					this.LeftKeyPressed = false;
					break;
				case VirtualKey.D:
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

		public void SliderValueChanged(double newValue)
		{
			this.SliderValue = newValue;
		}

		public static bool IsRelevantKey(VirtualKey key)
		{
			switch (key)
			{
				case VirtualKey.A:
				case VirtualKey.D:
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
