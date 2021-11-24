using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BotW_Installer.Libraries
{
    internal class Anim
    {
        public static void Double(FrameworkElement parentControl, string control, DependencyProperty property, double value, int timeSpan = 1000)
        {
            DoubleAnimation anim = new();
            anim.To = value;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(timeSpan));

            Storyboard.SetTargetName(anim, control);
            Storyboard.SetTargetProperty(anim, new PropertyPath(property));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anim);

            storyboard.Begin(parentControl);
        }
    }
}
