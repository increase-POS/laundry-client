using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace laundryApp.Classes
{
    class Animations
    {
        public static System.Windows.Media.Color ColorToColor(SolidColorBrush br)
        {
            return System.Windows.Media.Color.FromArgb(br.Color.A, br.Color.R, br.Color.G, br.Color.B);
        }
        public static TranslateTransform borderAnimation(int anim, Border control, Boolean Property)
        {
            Storyboard storyboard = new Storyboard();
            control.Opacity = 0;
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = anim;
            myDoubleAnimation.Duration = TimeSpan.FromMilliseconds(500);

            TranslateTransform translateTransform = new TranslateTransform();
            if (Property)
            {
                translateTransform.BeginAnimation(TranslateTransform.XProperty, myDoubleAnimation);
            }
            else
                translateTransform.BeginAnimation(TranslateTransform.YProperty, myDoubleAnimation);
            control.BeginAnimation(UIElement.OpacityProperty, myDoubleAnimation);
            return translateTransform;

        }

        //Create a Blink animation
       public static void CreateBlinkAnimation(TextBlock control)
    {
        var switchOffAnimation = new DoubleAnimation
        {
            To = 0,
            Duration = TimeSpan.Zero
        };

        var switchOnAnimation = new DoubleAnimation
        {
            To = 1,
            Duration = TimeSpan.Zero,
            BeginTime = TimeSpan.FromSeconds(0.5)
        };

        var blinkStoryboard = new Storyboard
        {
            Duration = TimeSpan.FromSeconds(1),
            RepeatBehavior = RepeatBehavior.Forever
        };

        Storyboard.SetTarget(switchOffAnimation, control);
        Storyboard.SetTargetProperty(switchOffAnimation, new PropertyPath(Canvas.OpacityProperty));
        blinkStoryboard.Children.Add(switchOffAnimation);

        Storyboard.SetTarget(switchOnAnimation, control);
        Storyboard.SetTargetProperty(switchOnAnimation, new PropertyPath(Canvas.OpacityProperty));
        blinkStoryboard.Children.Add(switchOnAnimation);

        control.BeginStoryboard(blinkStoryboard);

    }

        //        <TextBlock.Foreground>
        //              <SolidColorBrush />
        //          </TextBlock.Foreground>  
        public static void shakingColorControl(TextBlock control)
        {
           
            var changeColorAnimation1 = new ColorAnimation
            {
                From = ColorToColor(Application.Current.Resources["MainColor"] as SolidColorBrush),
                To = ColorToColor(Application.Current.Resources["Red"] as SolidColorBrush),
                Duration = TimeSpan.FromSeconds(0.1),
            };

            var changeColorAnimation2 = new ColorAnimation
            {
                From = ColorToColor(Application.Current.Resources["Red"] as SolidColorBrush),
                To = ColorToColor(Application.Current.Resources["Blue"] as SolidColorBrush),
                BeginTime = TimeSpan.FromSeconds(0.1),
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var changeColorAnimation3 = new ColorAnimation
            {
                From = ColorToColor(Application.Current.Resources["Blue"] as SolidColorBrush),
                To = ColorToColor(Application.Current.Resources["SecondColor"] as SolidColorBrush),
                BeginTime = TimeSpan.FromSeconds(0.2),
                Duration = TimeSpan.FromSeconds(0.1) 
            };
            var changeColorAnimation4 = new ColorAnimation
            {
                From = ColorToColor(Application.Current.Resources["SecondColor"] as SolidColorBrush),
                To = ColorToColor(Application.Current.Resources["MainColor"] as SolidColorBrush),
                BeginTime = TimeSpan.FromSeconds(0.3),
                Duration = TimeSpan.FromSeconds(0.2) 
            };

            Storyboard blinkStoryboard = new Storyboard();

            Storyboard.SetTarget(changeColorAnimation1, control);
            Storyboard.SetTarget(changeColorAnimation2, control);
            Storyboard.SetTarget(changeColorAnimation3, control);
            Storyboard.SetTarget(changeColorAnimation4, control);

            PropertyPath colorTargetPath =
            new PropertyPath("(0).(1)", TextBlock.ForegroundProperty, SolidColorBrush.ColorProperty);

            Storyboard.SetTargetProperty(changeColorAnimation1, colorTargetPath );
            Storyboard.SetTargetProperty(changeColorAnimation2, colorTargetPath );
            Storyboard.SetTargetProperty(changeColorAnimation3, colorTargetPath );
            Storyboard.SetTargetProperty(changeColorAnimation4, colorTargetPath );

            blinkStoryboard.Children.Add(changeColorAnimation1);
            blinkStoryboard.Children.Add(changeColorAnimation2);
            blinkStoryboard.Children.Add(changeColorAnimation3);
            blinkStoryboard.Children.Add(changeColorAnimation4);

            blinkStoryboard.Begin();
        }
           //         <TextBlock.RenderTransform>
           //             <TranslateTransform />
           //         </TextBlock.RenderTransform>
        public static void shakingControl(TextBlock control)
        {

            Storyboard blinkStoryboard = new Storyboard();


            var Animation1 = new DoubleAnimation
            {
                //From = 0,
                To = 7.5,
                Duration = TimeSpan.FromSeconds(0.1),
            };

            var Animation2 = new DoubleAnimation
            {
                //From = 7.5,
                To = -7.5,
                BeginTime = TimeSpan.FromSeconds(0.1),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation3 = new DoubleAnimation
            {
                //From = -7.5,
                To = 5,
                BeginTime = TimeSpan.FromSeconds(0.2),
                Duration = TimeSpan.FromSeconds(0.1),
            };

            var Animation4 = new DoubleAnimation
            {
                //From =5,
                To = -5,
                BeginTime = TimeSpan.FromSeconds(0.3),
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var Animation5 = new DoubleAnimation
            {
                //From = -2.5,
                To = 2.5,
                BeginTime = TimeSpan.FromSeconds(0.4),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation6 = new DoubleAnimation
            {
                //From = 2.5,
                To = -2.5,
                BeginTime = TimeSpan.FromSeconds(0.5),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation7 = new DoubleAnimation
            {
                //From = -2.5,
                To = 0,
                BeginTime = TimeSpan.FromSeconds(0.6),
                Duration = TimeSpan.FromSeconds(0.2)
            };

            Storyboard.SetTarget(Animation1, control);
            Storyboard.SetTarget(Animation2, control);
            Storyboard.SetTarget(Animation3, control);
            Storyboard.SetTarget(Animation4, control);
            Storyboard.SetTarget(Animation5, control);
            Storyboard.SetTarget(Animation6, control);
            Storyboard.SetTarget(Animation7, control);

            PropertyPath TargetPath =
            new PropertyPath("(0).(1)", TextBlock.RenderTransformProperty, TranslateTransform.XProperty);

            Storyboard.SetTargetProperty(Animation1, TargetPath);
            Storyboard.SetTargetProperty(Animation2, TargetPath);
            Storyboard.SetTargetProperty(Animation3, TargetPath);
            Storyboard.SetTargetProperty(Animation4, TargetPath);
            Storyboard.SetTargetProperty(Animation5, TargetPath);
            Storyboard.SetTargetProperty(Animation6, TargetPath);
            Storyboard.SetTargetProperty(Animation7, TargetPath);

            blinkStoryboard.Children.Add(Animation1);
            blinkStoryboard.Children.Add(Animation2);
            blinkStoryboard.Children.Add(Animation3);
            blinkStoryboard.Children.Add(Animation4);
            blinkStoryboard.Children.Add(Animation5);
            blinkStoryboard.Children.Add(Animation6);
            blinkStoryboard.Children.Add(Animation7);

            blinkStoryboard.Begin();
        }
        public static void shakingControl(ComboBox control)
        {

            Storyboard blinkStoryboard = new Storyboard();


            var Animation1 = new DoubleAnimation
            {
                //From = 0,
                To = 7.5,
                Duration = TimeSpan.FromSeconds(0.1),
            };

            var Animation2 = new DoubleAnimation
            {
                //From = 7.5,
                To = -7.5,
                BeginTime = TimeSpan.FromSeconds(0.1),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation3 = new DoubleAnimation
            {
                //From = -7.5,
                To = 5,
                BeginTime = TimeSpan.FromSeconds(0.2),
                Duration = TimeSpan.FromSeconds(0.1),
            };

            var Animation4 = new DoubleAnimation
            {
                //From =5,
                To = -5,
                BeginTime = TimeSpan.FromSeconds(0.3),
                Duration = TimeSpan.FromSeconds(0.1)
            };

            var Animation5 = new DoubleAnimation
            {
                //From = -2.5,
                To = 2.5,
                BeginTime = TimeSpan.FromSeconds(0.4),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation6 = new DoubleAnimation
            {
                //From = 2.5,
                To = -2.5,
                BeginTime = TimeSpan.FromSeconds(0.5),
                Duration = TimeSpan.FromSeconds(0.1)
            };
            var Animation7 = new DoubleAnimation
            {
                //From = -2.5,
                To = 0,
                BeginTime = TimeSpan.FromSeconds(0.6),
                Duration = TimeSpan.FromSeconds(0.2)
            };

            Storyboard.SetTarget(Animation1, control);
            Storyboard.SetTarget(Animation2, control);
            Storyboard.SetTarget(Animation3, control);
            Storyboard.SetTarget(Animation4, control);
            Storyboard.SetTarget(Animation5, control);
            Storyboard.SetTarget(Animation6, control);
            Storyboard.SetTarget(Animation7, control);

            PropertyPath TargetPath =
            new PropertyPath("(0).(1)", TextBlock.RenderTransformProperty, TranslateTransform.XProperty);

            Storyboard.SetTargetProperty(Animation1, TargetPath);
            Storyboard.SetTargetProperty(Animation2, TargetPath);
            Storyboard.SetTargetProperty(Animation3, TargetPath);
            Storyboard.SetTargetProperty(Animation4, TargetPath);
            Storyboard.SetTargetProperty(Animation5, TargetPath);
            Storyboard.SetTargetProperty(Animation6, TargetPath);
            Storyboard.SetTargetProperty(Animation7, TargetPath);

            blinkStoryboard.Children.Add(Animation1);
            blinkStoryboard.Children.Add(Animation2);
            blinkStoryboard.Children.Add(Animation3);
            blinkStoryboard.Children.Add(Animation4);
            blinkStoryboard.Children.Add(Animation5);
            blinkStoryboard.Children.Add(Animation6);
            blinkStoryboard.Children.Add(Animation7);

            blinkStoryboard.Begin();
        }
    }
}
