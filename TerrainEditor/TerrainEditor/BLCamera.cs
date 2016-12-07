using System;
using System.Windows.Media.Media3D;

namespace TerrainEditor
{
    class Camera
    {
        public PerspectiveCamera projection { get; set; }

        public Matrix3D view = new Matrix3D();

        public void MoveX(double speed)
        {
            projection.Position += Vector3D.CrossProduct(projection.LookDirection, projection.UpDirection) * speed;
        }

        public void MoveY(double speed)
        {
            // Parameters: value.
        }

        public void MoveZ(double speed)
        {
            projection.Position += projection.LookDirection * speed;
        }

        public void TurnX(double angle)
        {
            double sy = Math.Sin(angle * (Math.PI / 180));
            double cy = Math.Cos(angle * (Math.PI / 180));

            Matrix3D matrix = view;

            // Up vector:
            view.M22 = matrix.M22 * cy + matrix.M32 * -sy;
            view.M32 = matrix.M22 * sy + matrix.M32 * cy;

            // Look vector:
            view.M23 = matrix.M23 * cy + matrix.M33 * -sy;
            view.M33 = matrix.M23 * sy + matrix.M33 * cy;

            projection.LookDirection = new Vector3D(view.M13, view.M23, view.M33);

            projection.UpDirection = new Vector3D(view.M12, view.M22, view.M32);
        }

        public void TurnY(double angle)
        {
            double sx = Math.Sin(angle * (Math.PI / 180));
            double cx = Math.Cos(angle * (Math.PI / 180));

            Matrix3D matrix = view;

            // Up vector:
            view.M12 = matrix.M12 * cx + matrix.M32 * -sx;
            view.M32 = matrix.M12 * sx + matrix.M32 * cx;

            // Look vector:
            view.M13 = matrix.M13 * cx + matrix.M33 * -sx;
            view.M33 = matrix.M13 * sx + matrix.M33 * cx;

            projection.LookDirection = new Vector3D(view.M13, view.M23, view.M33);

            projection.UpDirection = new Vector3D(view.M12, view.M22, view.M32);
        }

        public void TurnZ()
        {
            // Parameters: angle.
        }

        public Camera(Point3D position, Vector3D look, Vector3D up, double fov)
        {
            projection = new PerspectiveCamera(position, look, up, fov);
        }
    }
}