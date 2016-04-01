using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI;

namespace Win2dFun
{
	internal class Win2dRenderer
	{
		private double canvasToWorldUnitsRatioX = 0;
		private double canvasToWorldUnitsRatioY = 0;

		private CanvasDrawingSession drawingSession = null;

		public void SetSize(Size newSize)
		{
			this.canvasToWorldUnitsRatioX = newSize.Width / World.Width;
			this.canvasToWorldUnitsRatioY = newSize.Height / World.Height;
		}

		public void SetDrawingSession(CanvasDrawingSession drawingSession)
		{
			this.drawingSession = drawingSession;
		}

		public void ClearDrawingSession()
		{
			this.SetDrawingSession(null);
		}

		public void DrawRectangle(Rect rect, Color color)
		{
			this.drawingSession.DrawRectangle(CanvasUnitsFromWorldUnits(rect), color);
		}

		public void FillRectangle(Rect rect, Color color)
		{
			this.drawingSession.FillRectangle(CanvasUnitsFromWorldUnits(rect), color);
		}

		public void DrawText(string text, float x, float y, Color color)
		{
			this.drawingSession.DrawText(text, x, y, color);
		}

		public Rect CanvasUnitsFromWorldUnits(Rect worldRect)
		{
			return new Rect(
				worldRect.X * this.canvasToWorldUnitsRatioX,
				worldRect.Y * this.canvasToWorldUnitsRatioY,
				worldRect.Width * this.canvasToWorldUnitsRatioX,
				worldRect.Height * this.canvasToWorldUnitsRatioY);
		}
	}
}
