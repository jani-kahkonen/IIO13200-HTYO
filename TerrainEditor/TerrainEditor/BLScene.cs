using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TerrainEditor
{
    class Scene
    {
        private static Model3DGroup myModel3DGroup = new Model3DGroup();

        private static Point3DCollection myPoint3DCollection = new Point3DCollection();

        public static GeometryModel3D CreateCube(Point3D point)
        {
            // Define mesh geometry.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a vertex positions.
            myMeshGeometry3D.Positions.Add(new Point3D(point.X - 0.5, point.Y + 0.5, point.Z - 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X + 0.5, point.Y + 0.5, point.Z - 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X - 0.5, point.Y - 0.5, point.Z - 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X + 0.5, point.Y - 0.5, point.Z - 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X - 0.5, point.Y + 0.5, point.Z + 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X + 0.5, point.Y + 0.5, point.Z + 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X - 0.5, point.Y - 0.5, point.Z + 0.5));
            myMeshGeometry3D.Positions.Add(new Point3D(point.X + 0.5, point.Y - 0.5, point.Z + 0.5));

            // Create a triangle indices.
            myMeshGeometry3D.TriangleIndices = new Int32Collection(new int[]
            {
                0, 1, 2, 2, 1, 3,
                4, 0, 6, 6, 0, 2,
                7, 5, 6, 6, 5, 4,
                3, 1, 7, 7, 1, 5,
                4, 5, 0, 0, 5, 1,
                3, 7, 2, 2, 7, 6
            });

            // Define diffuse material.
            DiffuseMaterial myDiffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            // Apply the material and the mesh to the geometry model.
            return new GeometryModel3D(myMeshGeometry3D, myDiffuseMaterial);
        }

        public static Model3DGroup LoadModel(Uri file)
        {
            myModel3DGroup.Children.Clear();

            XmlLoader.Load(file.ToString(), out myPoint3DCollection);

            foreach (Point3D point in myPoint3DCollection)
            {
                myModel3DGroup.Children.Add(CreateCube(point));
            }

            return myModel3DGroup;
        }

        public static Model3DGroup SaveModel(Uri file)
        {
            myModel3DGroup.Children.Clear();

            XmlLoader.Save(file.ToString(), myPoint3DCollection);

            foreach (Point3D point in myPoint3DCollection)
            {
                myModel3DGroup.Children.Add(CreateCube(point));
            }

            return myModel3DGroup;
        }

        public static Model3DGroup CreateModel(Point3D point)
        {
            myPoint3DCollection.Add(point);

            myModel3DGroup.Children.Add(CreateCube(point));

            return myModel3DGroup;
        }

        public static Model3DGroup LoadTerrain(Uri file)
        {
            int w, h, s;
            byte[] pixelArray;

            BmpLoader.Load(file, out w, out h, out s, out pixelArray);

            // Define mesh geometry.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            for (int z = 0; z != h; z++)
            {
                for (int x = 0; x != w; x++)
                {
                    myMeshGeometry3D.Positions.Add(new Point3D(x - (w / 2), pixelArray[s * z + x], z - (h / 2)));
                }
            }

            for (int z = 0; z != (h - 1); z++)
            {
                for (int x = 0; x != (w - 1); x++)
                {
                    int index1 = w * z + x;
                    int index2 = w * z + x + 1;
                    int index3 = w * (z + 1) + x;
                    int index4 = w * (z + 1) + x + 1;

                    myMeshGeometry3D.TriangleIndices.Add(index3);
                    myMeshGeometry3D.TriangleIndices.Add(index2);
                    myMeshGeometry3D.TriangleIndices.Add(index1);

                    myMeshGeometry3D.TriangleIndices.Add(index4);
                    myMeshGeometry3D.TriangleIndices.Add(index2);
                    myMeshGeometry3D.TriangleIndices.Add(index3);
                }
            }

            Model3DGroup myTerrain3DGroup = new Model3DGroup();

            // Add the directional light to the model group.
            myTerrain3DGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1)));

            // Add the geometry model to the model group.
            myTerrain3DGroup.Children.Add(new GeometryModel3D(myMeshGeometry3D, new DiffuseMaterial(new SolidColorBrush(Colors.Green))));

            return myTerrain3DGroup;
        }
    }
}