using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Win2dFun
{
	public sealed partial class MainPage : Page
	{
		private World world;
		private Win2dRenderer renderer;
		private bool initialized = false;

		public MainPage()
		{
			this.world = new World();
			this.renderer = new Win2dRenderer();

			this.InitializeComponent();

			this.canvas.TargetElapsedTime = new TimeSpan(166666); // 166666 for 60 FPS, 333333 for 30 FPS, 111111 for 90 FPS
		}

		private void Canvas_CreateResources(
			CanvasAnimatedControl sender,
			CanvasCreateResourcesEventArgs args)
		{
			this.initialized = true;
		}

		private void Canvas_Update(
			ICanvasAnimatedControl sender,
			CanvasAnimatedUpdateEventArgs args)
		{
			this.world.Update(args.Timing.ElapsedTime.TotalSeconds);
		}

		private void Canvas_Draw(
			ICanvasAnimatedControl sender,
			CanvasAnimatedDrawEventArgs args)
		{
			this.renderer.SetDrawingSession(args.DrawingSession);

			this.world.Draw(this.renderer);

			this.renderer.ClearDrawingSession();
		}

		private void Page_Loaded(object sender, RoutedEventArgs args)
		{
			// Register for keyboard events
			Window.Current.CoreWindow.KeyDown += this.KeyDown_UIThread;
			Window.Current.CoreWindow.KeyUp += this.KeyUp_UIThread;
		}

		private void Page_Unloaded(object sender, RoutedEventArgs args)
		{
			// Unregister keyboard events
			Window.Current.CoreWindow.KeyDown -= this.KeyDown_UIThread;
			Window.Current.CoreWindow.KeyUp -= this.KeyUp_UIThread;

			// Explicitly remove references to allow the Win2D controls to get garbage collected
			this.canvas.RemoveFromVisualTree();
			this.canvas = null;
		}

		private void KeyDown_UIThread(CoreWindow sender, KeyEventArgs args)
		{
			// This event runs on the UI thread.  If we want to process data structures that are
			// accessed on the game loop thread then we need to use some kind of thread
			// synchronization to ensure that the UI thread and game loop thread are not accessing
			// the same data at the same time.
			//
			// The two main options here would be to use locks / critical sections or to use
			// RunOnGameLoopThreadAsync to cause code to execute on the game loop thread.  This
			// example uses RunOnGameLoopThreadAsync.
			//
			// Since KeyEventArgs is not agile we cannot simply pass args to the game loop thread.
			// Although doing this may appear to work, it may have some surprising behavior.  For
			// example, calling a method on args may result in the method executing on the UI
			// thread (and so blocking the game loop thread).  There will also be surprising races
			// about which thread gets to set Handled first.
			//
			// Instead, we do as much processing as possible on the UI thread before passing
			// control to the game loop thread.

			if (!InputManager.IsRelevantKey(args.VirtualKey))
			{
				// It wasn't a relevant key that was pressed, so we don't handle this event
				return;
			}

			// Mark that we've handled this event.  It is important to do this synchronously inside
			// this event handler since the framework uses the Handled flag to determine whether or
			// not to send this event to other handlers.
			args.Handled = true;

			// Now schedule code to run on the game loop thread to handle the pressed key.  The
			// animated control will execute this before the next Update.
			VirtualKey key = args.VirtualKey;
			var action = this.canvas.RunOnGameLoopThreadAsync(
				() => this.world.InputManager.KeyDown(key));
		}

		private void KeyUp_UIThread(CoreWindow sender, KeyEventArgs args)
		{
			if (!InputManager.IsRelevantKey(args.VirtualKey))
			{
				return;
			}

			args.Handled = true;

			VirtualKey key = args.VirtualKey;
			var action = this.canvas.RunOnGameLoopThreadAsync(
				() => this.world.InputManager.KeyUp(key));
		}

		private async void Canvas_SizeChanged(object sender, SizeChangedEventArgs args)
		{
			Size newSize = args.NewSize;
			if (this.initialized)
			{
				await this.canvas.RunOnGameLoopThreadAsync(
					() => this.renderer.SetSize(newSize));
			}
			else
			{
				this.renderer.SetSize(newSize);
			}
		}

		private async void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs args)
		{
			double newValue = args.NewValue;
			if (this.initialized)
			{
				await this.canvas.RunOnGameLoopThreadAsync(
					() => this.world.InputManager.SliderValueChanged(newValue));
			}
			else
			{
				this.world.InputManager.SliderValueChanged(newValue);
			}
		}
	}
}
