using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;


namespace Shazzam.Shaders
{
    public class VrEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = 
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(VrEffect), 0);

        public static readonly DependencyProperty RotationXProperty = 
            DependencyProperty.Register("RotationX", typeof(double), typeof(VrEffect), 
                new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty RotationYProperty = 
            DependencyProperty.Register("RotationY", typeof(double), typeof(VrEffect), 
                new UIPropertyMetadata(-0.5, PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty ZoomProperty = 
            DependencyProperty.Register("Zoom", typeof(double), typeof(VrEffect), 
                new UIPropertyMetadata(0.5, PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty FovProperty = 
            DependencyProperty.Register("Fov", typeof(double), typeof(VrEffect), 
                new UIPropertyMetadata(120.0, PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty SizeProperty = 
            DependencyProperty.Register("Size", typeof(Size), typeof(VrEffect), 
                new UIPropertyMetadata(new Size(4096.0, 2048.0), PixelShaderConstantCallback(4)));
        public VrEffect()
        {
            PixelShader pixelShader = new()
            {
                UriSource = new Uri("Shaders/VrEffect.ps", UriKind.Relative)
            };
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(RotationXProperty);
            this.UpdateShaderValue(RotationYProperty);
            this.UpdateShaderValue(ZoomProperty);
            this.UpdateShaderValue(FovProperty);
            this.UpdateShaderValue(SizeProperty);
        }
        public Brush Input
        {
            get
            {
                return (Brush)this.GetValue(InputProperty);
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public double RotationX
        {
            get
            {
                return (double)this.GetValue(RotationXProperty);
            }
            set
            {
                this.SetValue(RotationXProperty, value);
            }
        }
        public double RotationY
        {
            get
            {
                return (double)this.GetValue(RotationYProperty);
            }
            set
            {
                this.SetValue(RotationYProperty, value);
            }
        }
        public double Zoom
        {
            get
            {
                return (double)this.GetValue(ZoomProperty);
            }
            set
            {
                this.SetValue(ZoomProperty, value);
            }
        }
        public double Fov
        {
            get
            {
                return (double)this.GetValue(FovProperty);
            }
            set
            {
                this.SetValue(FovProperty, value);
            }
        }
        public Size Size
        {
            get
            {
                return (Size)this.GetValue(SizeProperty);
            }
            set
            {
                this.SetValue(SizeProperty, value);
            }
        }
    }
}
