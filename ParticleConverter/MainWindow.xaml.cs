using HelixToolkit.SharpDX.Core;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ParticleConverter.dialogs;
using ParticleConverter.util;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Particle = ParticleConverter.util.Particle;

namespace ParticleConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly Dictionary<string, string> oldValues = new Dictionary<string, string>();
        private readonly util.ImageConverter ImageConverter = new util.ImageConverter();
        public MainWindow()
        {
            InitializeComponent();
            Load_Langugae();
            ColorCodeBox_TextChanged(ColorCodeBox, null);
            FolderPathBox.Text = Settings.Default.FolderPath;
        }
        private void Load_Langugae()
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                string applicationDirPath = System.IO.Path.GetDirectoryName(assembly.Location);
                DirectoryInfo di = new DirectoryInfo(applicationDirPath + "\\lang");
                FileInfo[] files =
                    di.GetFiles("*.xaml");
                foreach (FileInfo path in files)
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = System.IO.Path.GetFileNameWithoutExtension(path.Name)
                    };
                    LanguageBox.Items.Add(cbi);
                }
                string culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                LanguageBox.SelectedIndex = 0;

                //システムに対応する言語があったらそっちに合わせる
                int index = 0;
                foreach (ComboBoxItem cbi in LanguageBox.Items)
                {
                    if (cbi.Content.Equals(culture))
                    {
                        LanguageBox.SelectedIndex = index;
                    }
                    index++;
                }
            }
            catch
            {
                MessageBox.Show("言語ファイルの読み込みに失敗しました\nFailed to load language files.",
                    "エラー/Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Minimize_Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            //OptionsView.MaxHeight = Options.ActualHeight - OptionsTitle.ActualHeight;
            //OptionsView.Height = Options.ActualHeight - OptionsTitle.ActualHeight;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            OptionsView.ScrollToEnd();
        }


        /// <summary>
        /// フィルターつきのテキストボックスの更新
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="str"></param>
        private void Update_FilterTextBox(TextBox tb, string str)
        {
            tb.Text = str;
            if (!oldValues[tb.Name].Equals(str))
            {
                oldValues[tb.Name] = str;
                Update_Preview();
            }
        }

        private void ColorCodeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            try
            {
                Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(ColorCodeBox.Text);
                oldValues[sb.Name] = sb.Text;
                Update_Preview();
            }
            catch
            {
                sb.Text = oldValues[sb.Name];
            }
        }

        private void NumlicBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            if (decimal.TryParse(sb.Text, out decimal d) && d > 0)
            {
                oldValues[sb.Name] = sb.Text;
            }
            else
            {
                SystemSounds.Beep.Play();
                sb.Text = oldValues[sb.Name];
            }
        }

        private void FilterTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            sb.KeyDown += EnterKey_Down;
            oldValues.Add(sb.Name, sb.Text);
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cb.Checked += CheckBox_Check_Changed;
            cb.Unchecked += CheckBox_Check_Changed;
        }

        private void CheckBox_Check_Changed(object sender, RoutedEventArgs e)
        {
            Update_Preview();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.SelectionChanged += Combobox_Selection_Changed;
        }

        private void Combobox_Selection_Changed(object sender, RoutedEventArgs e)
        {
            Update_Preview();
        }

        private void Update_Preview()
        {
            if (ImageConverter.IsLoaded && UsePreviewBox.IsChecked.Value)
            {
                int coord = int.Parse(((ComboBoxItem)CoordinateAxis.SelectedItem).Tag.ToString());
                int verAlig = int.Parse(((ComboBoxItem)VerticalAlignmentBox.SelectedItem).Tag.ToString());
                int horAlig = int.Parse(((ComboBoxItem)HorizontalAlignmentBox.SelectedItem).Tag.ToString());
                //Mat smat = ImageConverter.GetModifiedImage();
                //System.Windows.Size size = ImageConverter.GetBlocks();
                //Bitmap bitmap = smat.ToBitmap();
                //var mb = new MeshBuilder(false, true);

                //IList<Point3D> pnts = new List<Point3D>
                //{
                //    new Point3D(0, 0, 0),
                //    new Point3D(size.Width, 0, 0),
                //    new Point3D(size.Width, size.Height, 0),
                //    new Point3D(0, size.Height, 0)
                //};

                //mb.AddPolygon(pnts);

                //var mesh = mb.ToMesh(false);

                //PointCollection pntCol = new PointCollection
                //{
                //    new System.Windows.Point(0, 0),
                //    new System.Windows.Point(bitmap.Size.Width, 0),
                //    new System.Windows.Point(bitmap.Size.Width, bitmap.Size.Height),
                //    new System.Windows.Point(0, bitmap.Size.Height)
                //};
                //mesh.TextureCoordinates = pntCol;

                //ImageBrush brush = new ImageBrush();

                //using (Stream stream = new MemoryStream())
                //{
                //    bitmap.Save(stream, ImageFormat.Png);
                //    stream.Seek(0, SeekOrigin.Begin);
                //    BitmapImage img = new BitmapImage();
                //    img.BeginInit();
                //    img.CacheOption = BitmapCacheOption.OnLoad;
                //    img.StreamSource = stream;
                //    img.EndInit();
                //    brush.ImageSource = img;
                //}

                //brush.TileMode = TileMode.Tile;
                //brush.ViewportUnits = BrushMappingMode.Absolute;
                //brush.ViewboxUnits = BrushMappingMode.Absolute;
                //brush.Stretch = Stretch.None;
                //brush.AlignmentX = AlignmentX.Left;
                //brush.AlignmentY = AlignmentY.Top;
                //brush.Viewport = new System.Windows.Rect(0, 0, brush.ImageSource.Width, brush.ImageSource.Height);
                //DiffuseMaterial mat = new DiffuseMaterial(brush);

                //GeometryModel3D gModel3D = new GeometryModel3D { Geometry = mesh, Material = mat, BackMaterial = mat };

                //PreviewModel.Content = gModel3D;
                Particle[] particles = ImageConverter.GetParticles(coord, verAlig, horAlig);
                //ParticleModel.Children.Clear();
                var points = new PointGeometry3D();
                var vectors = new Vector3Collection();
                var colors = new Color4Collection();
                var ptIdx = new IntCollection();
                int i = 0;
                foreach (Particle particle in particles)
                {
                    vectors.Add(new Vector3((float)particle.x, (float)particle.y, (float)particle.z));
                    if (UseStaticDustColor.IsChecked.Value)
                    {
                        Color c = (Color)ColorConverter.ConvertFromString(ColorCodeBox.Text);
                        colors.Add(new Color4(c.R / 255f, c.G / 255f, c.B / 255f, 1.0f));
                    }
                    else
                    {
                        colors.Add(new Color4(particle.r / 255f, particle.g / 255f, particle.b / 255f, 1.0f));
                    }
                    ptIdx.Add(i);
                    i++;
                }
                points.Positions = vectors;
                points.Colors = colors;
                points.Indices = ptIdx;
                ParticleModel.Geometry = points;
                double size = double.Parse(ParticleSizeBox.Text);
                ParticleModel.Size = new System.Windows.Size(3 * Math.Sqrt(size), 3 * Math.Sqrt(size));
                ParticleCounter.Text = $"Particles: {particles.Length}";
            }
        }

        private void EnterKey_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DependencyObject ancestor = ((Control)sender).Parent;
                //フォーカスできる親を探してフォーカス
                while (ancestor != null)
                {
                    if (ancestor is UIElement element && element.Focusable)
                    {
                        element.Focus();
                        break;
                    }

                    ancestor = VisualTreeHelper.GetParent(ancestor);
                }
            }
        }

        /// <summary>
        /// カラープレビューの更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            try
            {
                Color color = (Color)ColorConverter.ConvertFromString(sb.Text);
                Ellipse auter = new Ellipse
                {
                    Fill = System.Windows.Media.Brushes.Snow,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 25,
                    Height = 25
                };
                Ellipse inner = new Ellipse
                {
                    Fill = new SolidColorBrush(color),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1, 1, 1, 1),
                    Width = 23,
                    Height = 23
                };
                if (ColorCanvas != null)
                {
                    ColorCanvas.Children.Clear();
                    ColorCanvas.Children.Add(auter);
                    ColorCanvas.Children.Add(inner);
                }
            }
            catch
            {
                //意図的に何もしない
            }
        }

        private void BrowsImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog { Filter = "Image File|*.jpg;*.png;*.jpeg" };
            if (of.ShowDialog() == true)
            {
                FilePathBox.Text = of.FileName;
                ImageFileLoad();
            }
        }

        private void Sync_SizeBoxes()
        {
            System.Windows.Size size = ImageConverter.GetBlocks();
            Update_FilterTextBox(SizeWBox, size.Width.ToString());
            Update_FilterTextBox(SizeHBox, size.Height.ToString());
        }

        private void Sync_ResolutionBoxes()
        {
            Update_FilterTextBox(ResolutionWidthBox, ImageConverter.ResizedWidth.ToString());
            Update_FilterTextBox(ResolutionHeightBox, ImageConverter.ResizedHeight.ToString());
        }

        private void ImageFileLoad()
        {
            try
            {
                ImageConverter.Load(FilePathBox.Text);
                if (AutoSizeBox.IsChecked.Value)
                {
                    Sync_SizeBoxes();
                }
                if (AutoResolutionBox.IsChecked.Value)
                {
                    Sync_ResolutionBoxes();
                }
                ImageConverter.ResizedHeight = int.Parse(ResolutionHeightBox.Text);
                ImageConverter.ResizedWidth = int.Parse(ResolutionWidthBox.Text);
                ImageConverter.IsFlip = ImageFlipBox.IsChecked.Value;
                ImageConverter.Density = double.Parse(ParticleDensityBox.Text);
                if (ImageConverter.ResizedHeight * ImageConverter.ResizedWidth >= 3000)
                {

                }
                else
                {
                    UsePreviewBox.IsChecked = true;
                }
            }
            catch
            {
                MessageBox.Show("画像ファイルの読み込みに失敗しました\nFailed to load an image file",
                "エラー/Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                this.Close();
            }
        }

        private void AutoSizeBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ImageConverter.IsLoaded)
            {
                Sync_SizeBoxes();
            }
        }

        private void AutoResolutionBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ImageConverter.IsLoaded)
            {
                ResolutionWidthBox.Text = ImageConverter.SourseWidth.ToString();
                ResolutionHeightBox.Text = ImageConverter.SourseHeight.ToString();
                ImageConverter.ResizedWidth = int.Parse(ResolutionWidthBox.Text);
                ImageConverter.ResizedHeight = int.Parse(ResolutionHeightBox.Text);
                if (AutoSizeBox.IsChecked.Value)
                {
                    Sync_SizeBoxes();
                }
                else
                {
                    ImageConverter.Density = ImageConverter.ResizedWidth / double.Parse(SizeWBox.Text);
                }
            }
        }

        private void ParticleDensityBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ImageConverter.IsLoaded && double.TryParse(ParticleDensityBox.Text, out double density))
            {
                if (decimal.Parse(ParticleDensityBox.Text) >= 0)
                {
                    ImageConverter.Density = density;
                    Sync_SizeBoxes();
                }
            }
        }

        private void MenuButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

            ContextMenu mi = this.Resources["Menu"] as ContextMenu;
            ((MenuItem)mi.Items[0]).Header = this.Resources["DeveloperTwitter"];
            ((MenuItem)mi.Items[1]).Header = this.Resources["BugReport"];
            ((MenuItem)mi.Items[2]).Header = this.Resources["About"];
            mi.IsOpen = true;
        }

        // 解像度ボックスのフォーカス外れた
        private void ResolutionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            NumlicBox_LostFocus(sender, e);
            if (ImageConverter.IsLoaded)
            {
                if (sender.Equals(ResolutionWidthBox))
                {
                    int reheight = (int)(ImageConverter.SourseHeight * (double.Parse(sb.Text) / ImageConverter.SourseWidth));
                    Update_FilterTextBox(ResolutionHeightBox, reheight.ToString());
                }
                if (sender.Equals(ResolutionHeightBox))
                {
                    int rewidth = (int)(ImageConverter.SourseWidth * (double.Parse(sb.Text) / ImageConverter.SourseHeight));
                    Update_FilterTextBox(ResolutionWidthBox, rewidth.ToString());
                }
                ImageConverter.ResizedWidth = int.Parse(ResolutionWidthBox.Text);
                ImageConverter.ResizedHeight = int.Parse(ResolutionHeightBox.Text);
                if (AutoSizeBox.IsChecked.Value)
                {
                    Sync_SizeBoxes();
                }
                else
                {
                    ImageConverter.Density = ImageConverter.ResizedWidth / double.Parse(SizeWBox.Text);
                }
                Update_Preview();
            }
        }

        private void LanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri("lang/" + ((ComboBoxItem)LanguageBox.SelectedItem).Content + ".xaml", UriKind.Relative)
            };

            // リソースディクショナリを変更
            Resources.MergedDictionaries[0] = dictionary;
        }

        private void SizeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            NumlicBox_LostFocus(sender, e);
            if (ImageConverter.IsLoaded)
            {
                if (sender.Equals(SizeWBox))
                {
                    double reheight = ImageConverter.ResizedHeight / (ImageConverter.ResizedWidth / double.Parse(sb.Text));
                    Update_FilterTextBox(SizeHBox, reheight.ToString());
                    Update_FilterTextBox(ParticleDensityBox, (ImageConverter.ResizedWidth / double.Parse(sb.Text)).ToString());
                    ImageConverter.Density = ImageConverter.ResizedWidth / double.Parse(sb.Text);
                }
                if (sender.Equals(SizeHBox))
                {
                    double rewidth = ImageConverter.ResizedWidth / (ImageConverter.ResizedHeight / double.Parse(sb.Text));
                    Update_FilterTextBox(SizeWBox, rewidth.ToString());
                    Update_FilterTextBox(ParticleDensityBox, (ImageConverter.ResizedHeight / double.Parse(sb.Text)).ToString());
                    ImageConverter.Density = ImageConverter.ResizedWidth / double.Parse(sb.Text);
                }
            }
        }


        private void ParticleSizeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox)sender;
            if (decimal.TryParse(sb.Text, out decimal d) && d <= decimal.Parse("1.00") && d > 0)
            {
                oldValues[sb.Name] = sb.Text;
                Update_Preview();
            }
            else
            {
                SystemSounds.Beep.Play();
                sb.Text = oldValues[sb.Name];
            }
        }

        private void ImageFlipBox_Checked(object sender, RoutedEventArgs e)
        {
            ImageConverter.IsFlip = ImageFlipBox.IsChecked.Value;
        }

        private void ImageRotationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (int.TryParse((string)((ComboBoxItem)ImageRotationBox.SelectedItem).Tag, out int angle))
            {
                ImageConverter.Angle = angle;
            }
        }

        private void UsePreviewBox_Checked(object sender, RoutedEventArgs e)
        {
            Update_Preview();
        }

        private void BrowsFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPathBox.Text = dialog.FileName;
                Settings.Default.FolderPath = dialog.FileName;
                Settings.Default.Save();
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImageConverter.IsLoaded)
            {
                ExportButton.IsEnabled = false;
                Options.IsEnabled = false;
                BrowsImageButton.IsEnabled = false;
                BrowsFolderButton.IsEnabled = false;
                ButtonProgressAssist.SetValue(ExportButton, 0);
                int coord = int.Parse(((ComboBoxItem)CoordinateAxis.SelectedItem).Tag.ToString());
                int verAlig = int.Parse(((ComboBoxItem)VerticalAlignmentBox.SelectedItem).Tag.ToString());
                int horAlig = int.Parse(((ComboBoxItem)HorizontalAlignmentBox.SelectedItem).Tag.ToString());
                Particle[] particles = ImageConverter.GetParticles(coord, verAlig, horAlig);
                ButtonProgressAssist.SetMaximum(ExportButton, particles.Length + 20);
                ButtonProgressAssist.SetValue(ExportButton, 20);
                ExportButton.UpdateLayout();
                string fileName = System.IO.Path.GetFileNameWithoutExtension(FilePathBox.Text);
                string filePath = FolderPathBox.Text + "\\" + fileName + ".mcfunction";
                Encoding enc = new System.Text.UTF8Encoding(); ;
                StreamWriter writer = null;
                try
                {
                    string cs = "~";
                    switch (((ComboBoxItem)CoordinateModeBox.SelectedItem).Tag)
                    {
                        case "Relative":
                            cs = "~";
                            break;
                        case "Local":
                            cs = "^";
                            break;
                    }
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                    }
                    writer = new StreamWriter(filePath, false, enc);
                    for (int i = 0; i < particles.Length; i++)
                    {
                        var p = particles[i];
                        string axis = $"{cs}{p.x} {cs}{p.y} {cs}{p.z}";
                        string particle = "minecraft:" + ParticleTypeBox.Text;
                        if (ParticleTypeBox.SelectedValue.Equals("dust"))
                        {
                            if (UseStaticDustColor.IsChecked.Value)
                            {
                                Color color = (Color)ColorConverter.ConvertFromString(ColorCodeBox.Text);
                                particle += $" {Math.Round(color.R / 255.0d, 2)} {Math.Round(color.G / 255.0d, 2)} {Math.Round(color.B / 255.0d, 2)} {ParticleSizeBox.Text}";
                            }
                            else
                            {
                                particle += $" {Math.Round(p.r / 255.0d, 2)} {Math.Round(p.g / 255.0d, 2)} {Math.Round(p.b / 255.0d, 2)} {ParticleSizeBox.Text}";
                            }
                        }
                        string particleString = $"particle {particle} {axis} 0 0 0 0 1 {((ComboBoxItem)DisplayModeBox.SelectedItem).Tag} {ParticleViewerBox.Text}";
                        await Task.Run(() =>
                        {
                            writer.WriteLine(particleString);
                        });
                        ButtonProgressAssist.SetValue(ExportButton, 20 + 1 + i);
                    }
                }
                catch
                {
                    MessageBox.Show("ファイルの書き込みに失敗しました\nFailed to export a file.",
                        "エラー/Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                finally
                {
                    if (writer != null) writer.Close();
                }
                ButtonProgressAssist.SetValue(ExportButton, 0);
                Options.IsEnabled = true;
                ExportButton.IsEnabled = true;
                BrowsImageButton.IsEnabled = true;
                BrowsFolderButton.IsEnabled = true;
                SystemSounds.Beep.Play();
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void Show_DevsTwitter(object sender, RoutedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://twitter.com/newkemo431")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }

        private void Show_BugReport(object sender, RoutedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://github.com/kemo14331/Particle-Converter/issues")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
        private async void Show_About(object sender, RoutedEventArgs e)
        {
            var dialog = new About();
            var result = await DialogHost.ShowDialog(dialog);
        }

    }
}
