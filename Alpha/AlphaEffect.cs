
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace Shazzam.Shaders
{

    public class AlphaEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = 
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(AlphaEffect), 0);

        public static readonly DependencyProperty PositionProperty = 
            DependencyProperty.Register("Position", typeof(double), typeof(AlphaEffect), 
                new UIPropertyMetadata(2.0, PixelShaderConstantCallback(0)));
        public AlphaEffect()
        {
            PixelShader pixelShader = new()
            {
                UriSource = new Uri("Shaders/AlphaEffect.ps", UriKind.Relative)
            };
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(PositionProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public Dock Position
        {
            get
            {
                var p = (double)this.GetValue(PositionProperty);
                return p switch
                {
                    0.0 => Dock.Left,
                    1.0 => Dock.Top,
                    2.0 => Dock.Right,
                    3.0 => Dock.Bottom,
                    _ => Dock.Right,
                };
            }
            set
            {
                switch (value)
                {
                    case Dock.Left: this.SetValue(PositionProperty, 0.0); break;
                    case Dock.Top: this.SetValue(PositionProperty, 1.0); break;
                    case Dock.Right: this.SetValue(PositionProperty, 2.0); break;
                    case Dock.Bottom: this.SetValue(PositionProperty, 3.0); break;
                }
            }
        }
    }
}
