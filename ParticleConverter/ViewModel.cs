using HelixToolkit.SharpDX.Core;
using SharpDX;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;

namespace ParticleConverter
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public EffectsManager EffectsManager { get; }

        public Camera Camera { get; }

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera { };

            // 座標面の作成
            GridGeometry3D = LineBuilder.GenerateGrid(10);

            LineBuilder lineBuilderX = new LineBuilder();
            lineBuilderX.AddLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0));
            AxisX = lineBuilderX.ToLineGeometry3D();

            LineBuilder lineBuilderY = new LineBuilder();
            lineBuilderY.AddLine(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            AxisY = lineBuilderY.ToLineGeometry3D();

            LineBuilder lineBuilderZ = new LineBuilder();
            lineBuilderZ.AddLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
            AxisZ = lineBuilderZ.ToLineGeometry3D();

            // 座標面のトランスフォーム作成
            GridGeometry3DTransform = new System.Windows.Media.Media3D.TranslateTransform3D(-5, 0, -5);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private LineGeometry3D _LineGeometry3D;
        private LineGeometry3D _GridGeometry3D;
        private LineGeometry3D _AxisX;
        private LineGeometry3D _AxisY;
        private LineGeometry3D _AxisZ;
        private System.Windows.Media.Media3D.Transform3D _GridGeometry3DTransform;

        /// <summary>
        ///     ''' 座標面グリッド
        ///     ''' </summary>
        ///     ''' <value>
        ///     ''' The grid geometry3 d.
        ///     ''' </value>
        public LineGeometry3D GridGeometry3D
        {
            get
            {
                return _GridGeometry3D;
            }
            set
            {
                _GridGeometry3D = value;
                OnPropertyChanged(nameof(GridGeometry3D));
            }
        }

        public LineGeometry3D LineGeometry3D
        {
            get
            {
                return _LineGeometry3D;
            }
            set
            {
                _LineGeometry3D = value;
                OnPropertyChanged(nameof(LineGeometry3D));
            }
        }

        public LineGeometry3D AxisX
        {
            get
            {
                return _AxisX;
            }
            set
            {
                _AxisX = value;
                OnPropertyChanged(nameof(AxisX));
            }
        }

        public LineGeometry3D AxisY
        {
            get
            {
                return _AxisY;
            }
            set
            {
                _AxisY = value;
                OnPropertyChanged(nameof(AxisY));
            }
        }

        public LineGeometry3D AxisZ
        {
            get
            {
                return _AxisZ;
            }
            set
            {
                _AxisZ = value;
                OnPropertyChanged(nameof(AxisZ));
            }
        }

        /// <summary>
        ///     ''' Gets or sets the grid geometry3 d transform.
        ///     ''' </summary>
        ///     ''' <value>
        ///     ''' The grid geometry3 d transform.
        ///     ''' </value>
        public System.Windows.Media.Media3D.Transform3D GridGeometry3DTransform
        {
            get
            {
                return _GridGeometry3DTransform;
            }
            set
            {
                _GridGeometry3DTransform = value;
                OnPropertyChanged(nameof(GridGeometry3DTransform));
            }
        }

        #endregion
    }
}
