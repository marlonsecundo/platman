using Microsoft.Xna.Framework;

namespace Platman.Managers.Resolution
{
	public interface IResolution
	{
		Rectangle TitleSafeArea { get; }
		Rectangle ScreenArea { get; }
		Matrix ScreenMatrix { get; }
		Point VirtualResolution { get; set; }
		Point ScreenResolution { get; set; }
		Matrix TransformationMatrix();
		Vector2 ScreenToGameCoord(Vector2 screenCoord);
		void ResetViewport();
	}
}
