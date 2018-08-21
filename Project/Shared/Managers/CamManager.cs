using Microsoft.Xna.Framework;
using Platman.Component.Base;
using Platman.Managers;
using Platman.Managers.Resolution;
using System.Collections.Generic;

namespace Platman.Component.Managers
{
    public class CamManager
    {
        private static CamManager _instance;

        private ResolutionManager resolution;
        private Camera2D _screenCamera;
        private Camera2D _levelCamera;
        public List<Camera2D> OuthersCam { get; }
        public CamManager()
        {
            resolution = ResolutionManager.Instance;

            OuthersCam = new List<Camera2D>();
        }

        public void RecalculateTransformationMatrices()
        {
            ScreenCamera.RecalculateTransformationMatrices();
            LevelCamera.RecalculateTransformationMatrices();
            for (int i = 0; i < OuthersCam.Count; i++)
                OuthersCam[i].RecalculateTransformationMatrices();
        }
        
        public Camera2D CreateCameraIntance(DrawableBase comp)
        {
            Camera2D camera = new Camera2D();
            OuthersCam.Add(camera);
            comp.Unloaded += Comp_Unloaded;
            return camera;
        }

        private void Comp_Unloaded(object sender)
        {
            OuthersCam.Remove((sender as DrawableBase).Camera);
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 value) => resolution.ScreenToGameCoord(value);
        public Camera2D ScreenCamera { get => _screenCamera = _screenCamera ?? new Camera2D(); }
        public Camera2D LevelCamera { get => _levelCamera = _levelCamera ?? new Camera2D(); }
        public static CamManager Instance { get => _instance = _instance ?? new CamManager(); }
    }
}
