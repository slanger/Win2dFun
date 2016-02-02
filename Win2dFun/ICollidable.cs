using Windows.Foundation;

namespace Win2dFun
{
	internal interface ICollidable
	{
		Rect GetCollider();
		bool IsColliding(ICollidable other);
	}
}
