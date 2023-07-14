using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using GKtoolkit;
namespace GrafikaProjekt4
{
    public class AppViewModel : INotifyPropertyChanged
    {
        private CustomBitmap _bitmap;
        private ObservableCollection<IObject3D> objects3d;
        private List<LightSource> lights;
        private DispatcherTimer timer;
        private GKtoolkit.Camera mainCamera;
        private int interval;
        private double angle;
        private double angleChange;
        private BitmapArray bitmapArray;

        public event EventHandler<CameraChangedEventArgs> CameraChanged;

        private bool _noShading;
        private bool _constant;
        private bool _gouraud;
        private bool _phong;
        private bool _regularCam;
        private bool _followCam;
        private bool _attachCam;
        private bool _showLine;
        private bool _fill;
        private bool _vert;
        private bool _animation;

        public bool noShading
        {
            get { return _noShading; }
            set { _noShading = value; SetShading(); OnPropertyChanged(nameof(noShading)); }
        }
        public bool constant
        {
            get { return _constant; }
            set { _constant = value; SetShading(); OnPropertyChanged(nameof(constant)); }
        }
        public bool gouraud
        {
            get { return _gouraud; }
            set { _gouraud = value; SetShading(); OnPropertyChanged(nameof(gouraud)); }
        }
        public bool phong
        {
            get { return _phong; }
            set { _phong = value; SetShading(); OnPropertyChanged(nameof(phong)); }
        }
        public bool regularCam
        {
            get { return _regularCam; }
            set { _regularCam = value; SetCamera(); OnPropertyChanged(nameof(regularCam)); }
        }
        public bool followCam
        {
            get { return _followCam; }
            set { _followCam = value; SetCamera(); OnPropertyChanged(nameof(followCam)); }
        }
        public bool attachCam
        {
            get { return _attachCam; }
            set { _attachCam = value; SetCamera(); OnPropertyChanged(nameof(attachCam)); }
        }
        public bool showLine
        {
            get { return _showLine; }
            set { _showLine = value; OnPropertyChanged(nameof(showLine)); }
        }
        public bool fill
        {
            get { return _fill; }
            set { _fill = value; OnPropertyChanged(nameof(fill)); }
        }

        public bool vert
        {
            get { return _vert; }
            set { _vert = value; OnPropertyChanged(nameof(vert)); }
        }

        public bool animation
        {
            get { return _animation; }
            set { _animation = value; OnPropertyChanged(nameof(animation)); 
                if(!_animation)
                {
                    timer.Tick -= OnAnimation_Tick;
                }
                else
                {
                    timer.Tick += new EventHandler(OnAnimation_Tick);
                }
            }
        }

        public double ka
        {
            get { return mainCamera != null ?  mainCamera.ka :  0; }
            set { if (mainCamera != null) { mainCamera.ka = value; }
                OnPropertyChanged(nameof(ka)); }
        }

        public double kd
        {
            get { return mainCamera != null ? mainCamera.kd : 0; }
            set { if (mainCamera != null) { mainCamera.kd = value; }
                OnPropertyChanged(nameof(kd)); }
        }

        public double ks
        {
            get { return mainCamera != null ? mainCamera.ks : 0; }
            set { if (mainCamera != null) { mainCamera.ks = value; }
                OnPropertyChanged(nameof(ks)); }
        }


        public CustomBitmap bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                _bitmap = value;
                OnPropertyChanged(nameof(bitmap));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string cMode = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cMode));
        }
        public AppViewModel()
        {

            angle = Math.PI / 150;
            angleChange = Math.PI / 150;
            timer = new DispatcherTimer(DispatcherPriority.Background);
            lights = new List<LightSource>();

            interval = 10;

            bitmap = new CustomBitmap(400, 400, 96, 96);
            objects3d = new ObservableCollection<IObject3D>();

            // Add Lights
            LightSource ls1 = new LightSource(10, 5, 6);
            LightSource ls2 = new LightSource(-10, 6, 6);
            LightSource ls3 = new LightSource(-6, 6, -6);
            LightSource ls4 = new LightSource(0, 10, 0);

            lights.Add(ls1);
            lights.Add(ls2);
            lights.Add(ls3);
            lights.Add(ls4);

            // Shading
            noShading = true;
            constant = false;
            gouraud = false;
            phong = false;

            // Cameras
            regularCam = true;
            followCam = false;
            attachCam = false;

            // Triangle lines
            showLine = false;

            fill = true;
            vert = false;
            animation = true;

            // Init canvas buffer
            BitmapArray bitmapArray = new BitmapArray(bitmap.writeableBitmap.BackBufferStride *
                bitmap.writeableBitmap.PixelHeight,
                bitmap.writeableBitmap.BackBufferStride,
                bitmap.writeableBitmap.BackBufferStride /
                bitmap.writeableBitmap.PixelWidth);
            bitmap.writeableBitmap.CopyPixels(bitmapArray.array, bitmap.writeableBitmap.BackBufferStride, 0);

            this.bitmapArray = bitmapArray;

            Scene();

            SetCamera();

            Animation();

            OnPropertyChanged(nameof(bitmap));
        }

        private void RedrawShapes()
        {
            bitmapArray.Clean(Colors.DarkGray);

            Color color = Colors.Black;

            Translation followCubeT = new Translation(zAxis: -Math.Sin(5 * (-Math.PI + angle % (Math.PI * 2))));
            MotionTransformation followCubeTransformation = new MotionTransformation(
                translation: followCubeT
                );

            Angle angleRotation = new Angle(yAngle: Math.PI / 32);
            MotionTransformation transformation = new MotionTransformation(
                angleRotation
                );


            Translation sphereTranslation = new Translation(zAxis: Math.Sin(20 * (-Math.PI + angle % (Math.PI * 2))));
            Angle sphereRotation = new Angle(yAngle: Math.PI / 16);
            MotionTransformation sphereTransformation = new MotionTransformation(
                sphereRotation,
                translation: sphereTranslation
                );

            for (int i = 0; i < objects3d.Count; i++)
            {
                IModel3D model = objects3d[i].model;

                if (i == 0)
                {
                    model.SetTransformation(transformation);
                }
                else if (i == 2)
                {
                    model.SetTransformation(sphereTransformation);
                }
                else if (i == 4)
                {
                    model.SetTransformation(followCubeTransformation);
                }
                else
                {
                    model.SetTransformation(new MotionTransformation());
                }

                model.Transform();
                model.Project();
                model.CheckWhatTrianglesToDraw(mainCamera);

                if (fill)
                {
                    model.Fill(bitmapArray, lights);
                }
                if (showLine)
                {
                    model.DrawOn((x, y) => bitmapArray.SetPixel(x, y, Colors.Black));
                }

                if (vert)
                {
                    model.HighlightVertices(bitmapArray);
                }

            }
            bitmap.writeableBitmap.WritePixels(new Int32Rect(0, 0,
                bitmap.writeableBitmap.PixelWidth, bitmap.writeableBitmap.PixelHeight),
                bitmapArray.array, bitmap.writeableBitmap.BackBufferStride, 0);
            OnPropertyChanged(nameof(bitmap));
        }
        private void Animation()
        {
            timer.Interval = new TimeSpan(interval);
            timer.Start();
        }

        private void OnAnimation_Tick(object sender, EventArgs e)
        {
            angle += angleChange;
            RedrawShapes();
        }

        private AModel3D CreateModel<T>(BitmapArray bitmapArray,
            ICamera camera, BaseDimensions dimensions, Action<IObject3D> action) where T : AModel3D, new()
        {
            AModel3D model = new T();
            model.SetDimensions(dimensions);
            model.SetBitmapArrayTo(bitmapArray);
            model.SetCameraReference(camera);
            model.InitModel();
            model.SubscribeToCameraChange(this);
            action(model);
            return model;
        }

        private void SetCamera()
        {
            if (attachCam)
            {
                // Following camera
                mainCamera = new GKtoolkit.Camera(1, cameraPosition: new Position(3, 10, 30),
                    cameraFocusPoint: new Position(0, 0, 0)
                    );
                mainCamera.SetChild(objects3d[objects3d.Count-1],
                    new CustomVector(new double[4] { 0, 20, 5, 0 }, 4));
            }
            else if (followCam)
            {
                mainCamera = new GKtoolkit.Camera(2, cameraPosition: new Position(30, 10, 3),
                    cameraFocusPoint: objects3d[0].centerPoint
                    );
                mainCamera.SetFocus(objects3d[0]);
            }
            else
            {
                // Regular camera
                mainCamera = new GKtoolkit.Camera(0, cameraPosition: new Position(3, 10, 30));
            }

            CameraChanged?.Invoke(this, new CameraChangedEventArgs { newCamera = mainCamera });

        }

        private void Scene()
        {
            Translation translation2 = new Translation(xAxis: 6.0, yAxis: 3.0);
            Translation translation3 = new Translation(yAxis: 6.0);

            MotionTransformation bigCubeTransformation = new MotionTransformation(
                translation: translation2
                );

            MotionTransformation sphereTranslation = new MotionTransformation(
                translation: translation3
                );

            objects3d.Add(CreateModel<SimpleCube>(bitmapArray, mainCamera,
                new BaseDimensions(5, 5, 5), (model) => { }));

            objects3d[0].SetTransformation(bigCubeTransformation);
            objects3d[0].Transform();

            objects3d.Add(CreateModel<SimpleCube>(bitmapArray, mainCamera,
                new BaseDimensions(2, 2, 2), (model) => { }));

            objects3d[1].SetTransformation(bigCubeTransformation);
            objects3d[1].Transform();

            objects3d.Add(CreateModel<Sphere>(bitmapArray, mainCamera,
                new BaseDimensions(4, Math.PI / 12.0, Math.PI / 12.0), (model) => { }));

            objects3d[2].SetTransformation(sphereTranslation);
            objects3d[2].Transform();

            objects3d.Add(CreateModel<Pyramid>(bitmapArray, mainCamera,
                new BaseDimensions(4, 0, 0), (model) => { }));

            objects3d.Add(CreateModel<SimpleCube>(bitmapArray, mainCamera,
                new BaseDimensions(4, 4, 4), (model) => { }));

        }

        private void SetShading()
        {
            if (mainCamera != null)
            {
                int shad = -1;
                if (constant)
                {
                    shad = 0;
                }
                else if (gouraud)
                {
                    shad = 1;
                }
                else if (phong)
                {
                    shad = 2;
                    foreach (IObject3D obj in objects3d)
                    {
                        IModel3D model = obj.model;
                        model.SetNormalVectorsInVertices();
                    }
                }
                mainCamera.shading = shad;
            }
        }
    }
}
