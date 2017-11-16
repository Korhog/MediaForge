using MForge.Sequensor.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MForge.Sequensor.UIControls
{
    public sealed class SceneControl : Control
    {
        SceneController controller;
        public SceneController Controller { get { return controller; } }

        public SceneControl()
        {
            this.DefaultStyleKey = typeof(SceneControl);
            controller = new SceneController();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var items = GetTemplateChild("Scenes") as ListView;
            items.ItemsSource = controller.Scenes;

            items.SelectionChanged += (sender, e) =>
            {
                var list = sender as ListView;
                var scene = list.SelectedItem as Scene;
                controller.SelectScene(scene);
            };

            Button btn = GetTemplateChild("AddButton") as Button;
            btn.Click += (sender, e) =>
            {
                if (controller == null) return;
                controller.CreateScene();
            };
        }
    }
}
