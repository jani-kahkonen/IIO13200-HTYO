using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TerrainEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Camera myCamera;

        private ModelVisual3D myModelVisual3D = new ModelVisual3D();

        private ModelVisual3D myTerrainVisual3D = new ModelVisual3D();

        private Point newPoint, oldPoint;

        private double xAngle, yAngle;

        private double sensitivity = 0.1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view
            // of the camera.
            // Specify where in the 3D scene the camera is.
            // Specify the direction that the camera is pointing.
            // Define camera's horizontal field of view in degrees.
            myCamera = new Camera(new Point3D(0, 512, 0), new Vector3D(0, 0, 1), new Vector3D(0, 1, 0), 60);

            // Assign the camera to the viewport.
            myViewport3D.Camera = myCamera.projection;

            myViewport3D.Children.Add(myTerrainVisual3D);

            myViewport3D.Children.Add(myModelVisual3D);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                myCamera.view.SetIdentity();
                newPoint = e.GetPosition(this);

                yAngle += oldPoint.Y - newPoint.Y;
                myCamera.TurnX(yAngle * sensitivity);

                xAngle += oldPoint.X - newPoint.X;
                myCamera.TurnY(xAngle * sensitivity);
            }

            oldPoint = e.GetPosition(this);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    myCamera.MoveZ(+1);
                    break;
                case Key.Down:
                    myCamera.MoveZ(-1);
                    break;
                case Key.Left:
                    myCamera.MoveX(-1);
                    break;
                case Key.Right:
                    myCamera.MoveX(+1);
                    break;
            }
        }

        private void BMP_Open(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            Microsoft.Win32.OpenFileDialog myOpenFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Filter files by extension.
            myOpenFileDialog.Filter = "Image files (.bmp)|*.bmp";

            // Show open file dialog box.
            if (myOpenFileDialog.ShowDialog() == true)
            {
                // Add the group of models to the ModelVisual3d.
                myTerrainVisual3D.Content = TerrainEditor.Scene.LoadTerrain(new Uri(myOpenFileDialog.FileName, UriKind.Relative));
            }
        }

        private void XML_Open(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            Microsoft.Win32.OpenFileDialog myOpenFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Filter files by extension.
            myOpenFileDialog.Filter = "XML files (*.xml)|*.xml";

            // Show open file dialog box.
            if (myOpenFileDialog.ShowDialog() == true)
            {
                // Add the group of models to the ModelVisual3d.
                myModelVisual3D.Content = TerrainEditor.Scene.LoadModel(new Uri(myOpenFileDialog.FileName, UriKind.Relative));
            }
        }

        private void XML_Save(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box.
            Microsoft.Win32.SaveFileDialog mySaveFileDialog = new Microsoft.Win32.SaveFileDialog();

            // Filter files by extension.
            mySaveFileDialog.Filter = "XML files (*.xml)|*.xml";

            // Show save file dialog box.
            if (mySaveFileDialog.ShowDialog() == true)
            {
                // Add the group of models to the ModelVisual3d.
                myModelVisual3D.Content = TerrainEditor.Scene.SaveModel(new Uri(mySaveFileDialog.FileName, UriKind.Relative));
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                // Returns the topmost Visual object of a hit test by specifying a Point.
                HitTestResult rayResult = VisualTreeHelper.HitTest(myViewport3D, e.GetPosition(myViewport3D));

                // Represents an intersection between a ray hit test and a MeshGeometry3D.
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;

                if (rayMeshResult != null)
                {
                    // Add the group of models to the ModelVisual3d.
                    myModelVisual3D.Content = TerrainEditor.Scene.CreateModel(rayMeshResult.PointHit);
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}