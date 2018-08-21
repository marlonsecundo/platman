using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Component.Managers;
using Platman.Managers.Resolution;

namespace Platman.Component.Managers
{
    public class ResolutionManager
    {
        private GameRoot game;
        private static ResolutionManager _instance;
        public static ResolutionManager Instance => _instance = _instance ?? new ResolutionManager();

        private ResolutionComponent resolution;

     
        public Rectangle TitleSafeArea => resolution.TitleSafeArea;

        public Point ScreenSize => resolution.ScreenResolution;

        public Rectangle ScreenArea => resolution.ScreenArea; 
       
        public Matrix ScreenMatrix => resolution.ScreenMatrix;

        public Matrix Matrix => resolution.TransformationMatrix();

        public ResolutionManager()
        {
            game = GameRoot.Instance;
            resolution = new ResolutionComponent(game, GameRoot.Instance.Graphics, new Point(1920, 1080), GetDisplay(), false, true);
        }

        public void ResetResolution(Point size)
        {
            game.Components.Remove(resolution);
            game.Services.RemoveService(typeof(IResolution));
            resolution = null;
            resolution = new ResolutionComponent(game, game.Graphics, new Point(1920, 1080), size, false, true);
            CamManager.Instance.RecalculateTransformationMatrices();
        }

        public  Vector2 ScreenToGameCoord(Vector2 screenCoord)
        {
            return resolution.ScreenToGameCoord(screenCoord);
        }

        public  void ResetViewport()
        {
            resolution.ResetViewport();
        }

        private Point GetDisplay()
        {

            return new Point(1200, 800);

            var model = DataBase.Settings.Instance.Model.Resolution;

            if (model != Point.Zero)
                return model;

            var s1 = game.Window.ClientBounds;
            var s2 = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.TitleSafeArea;
            
            return s1.Width * s1.Height > s2.Height * s2.Width ? s1.Size : s2.Size; 
        }





    }
}
