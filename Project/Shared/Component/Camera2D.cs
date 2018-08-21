using Microsoft.Xna.Framework;
using Platman.Component.Managers;

namespace Platman.Component
{
    public delegate void MovedEventHandler(Vector2 amount);

    public class Camera2D
    {
     //   Resolution resolution;

        private float _zoom;
        private float _rotation;
        private Vector2 _position;
        private Matrix _transform = Matrix.Identity;
        private bool _isViewTransformationDirty = true;
        private Matrix _camTranslationMatrix = Matrix.Identity;
        private Matrix _camRotationMatrix = Matrix.Identity;
        private Matrix _camScaleMatrix = Matrix.Identity;
        private Matrix _resTranslationMatrix = Matrix.Identity;
        private Vector3 _camTranslationVector = Vector3.Zero;
        private Vector3 _camScaleVector = Vector3.Zero;
        private Vector3 _resTranslationVector = Vector3.Zero;

        public event MovedEventHandler CameraMovedEvent;
        public Rectangle ViewArea { get; set; }

        public Rectangle Viewport
        {
            get { return new Rectangle(0, 0, 1920, 1080); }
        }

        ResolutionManager resolution;

        public Camera2D()
        {
            CameraMovedEvent += Camera2D_CameraMovedEvent;

            resolution = ResolutionManager.Instance;

            _zoom = 1;
            _rotation = 0.0f;
            Position = new Vector2(Viewport.Width / 2, Viewport.Height / 2);

            RecalculateTransformationMatrices();
        }



        private bool CheckInsideInArea(Vector2 position)
        {
            if (ViewArea != Rectangle.Empty)
            {
                 if (position.X + Viewport.Width / 2 <= ViewArea.Right &&
                      position.Y - Viewport.Height / 2 + Viewport.Height <= ViewArea.Bottom &&
                      position.X >= ViewArea.Left && position.Y >= ViewArea.Top)
                     return true;
                 else
                     return false;
            }
            else
                return true;
        }

        private void Camera2D_CameraMovedEvent(Vector2 amount)
        {

        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (/*CheckInsideInArea(value)*/true)
                {
                    if (value != _position)
                        CameraMovedEvent(value - _position);

                    _position = value;
                    _isViewTransformationDirty = true;
                }
            }
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
                _isViewTransformationDirty = true;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _isViewTransformationDirty = true;
            }
        }

        public Matrix Matrix
        {
            get
            {
                if (_isViewTransformationDirty)
                {
                    _camTranslationVector.X = -_position.X;
                    _camTranslationVector.Y = -_position.Y;

                    Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);
                    Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

                    _camScaleVector.X = _zoom;
                    _camScaleVector.Y = _zoom;
                    _camScaleVector.Z = 1;

                    Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

                    _resTranslationVector.X = Viewport.Width * 0.5f;
                    _resTranslationVector.Y = Viewport.Height * 0.5f;
                    _resTranslationVector.Z = 0;

                    Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

                    _transform = _camTranslationMatrix *
                                 _camRotationMatrix *
                                 _camScaleMatrix *
                                 _resTranslationMatrix *
                                 ResolutionManager.Instance.Matrix;

                    _isViewTransformationDirty = false;
                }

                return _transform;
            }
        }

        public void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }


    }
}
