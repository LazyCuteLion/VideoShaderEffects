
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;


namespace Shazzam.Shaders
{

    public class TileEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(TileEffect), 0);

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(double), typeof(TileEffect),
            new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(double), typeof(TileEffect),
            new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty SpacingSizeProperty =
            DependencyProperty.Register("SpacingSize", typeof(Size), typeof(TileEffect),
                new UIPropertyMetadata(new Size(0.0, 0.0), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty SpacingColorProperty =
            DependencyProperty.Register("SpacingColor", typeof(Color), typeof(TileEffect),
                new UIPropertyMetadata(Colors.Transparent, PixelShaderConstantCallback(4)));

        public static readonly DependencyProperty TargetSizeProperty = 
            DependencyProperty.Register("TargetSize", typeof(Size), typeof(TileEffect),
                new UIPropertyMetadata(new Size(0.0, 0.0)));

        public TileEffect()
        {
            PixelShader pixelShader = new()
            {
                UriSource = new Uri("Shaders/TileEffect.ps", UriKind.Relative)
            };
            this.PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RowsProperty);
            UpdateShaderValue(ColumnsProperty);
            UpdateShaderValue(SpacingSizeProperty);
            UpdateShaderValue(SpacingColorProperty);
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public double Rows
        {
            get { return (double)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public double Columns
        {
            get { return (double)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public Size TargetSize
        {
            get { return (Size)GetValue(TargetSizeProperty); }
            set { SetValue(TargetSizeProperty, value); }
        }

        public double Spacing
        {
            get
            {
                var spacing = (Size)this.GetValue(SpacingSizeProperty);
                return spacing.Width * TargetSize.Width;
            }
            set
            {
                if (TargetSize.Width > 0 && TargetSize.Height > 0)
                    this.SetValue(SpacingSizeProperty, 
                        new Size(value / TargetSize.Width, value / TargetSize.Height));
                else
                    this.SetValue(SpacingSizeProperty, new Size(0.0, 0.0));
            }
        }

        public Color SpacingColor
        {
            get
            {
                return ((Color)(this.GetValue(SpacingColorProperty)));
            }
            set
            {
                this.SetValue(SpacingColorProperty, value);
            }
        }
    }

}

