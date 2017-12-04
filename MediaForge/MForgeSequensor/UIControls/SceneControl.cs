using MForge.Sequensor.Sequence;
using MForge.Sequensor.Sequence.Interfaces;
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

namespace MForge.Sequensor.UIControls
{
    public sealed class SceneControl : Control
    {
        ListView items;
        SceneController controller;
        public SceneController Controller { get { return controller; } }

        public SceneControl()
        {
            this.DefaultStyleKey = typeof(SceneControl);
            controller = new SceneController();
        }

        public void DeleteScene(IScene scene)
        {
            if (items.SelectedItem as Scene == scene)
            {
                var newScene = controller.Scenes.Where(s => s != scene).FirstOrDefault();
                items.SelectedItem = newScene;
            }            
            controller.Scenes.Remove(scene);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            items = GetTemplateChild("Scenes") as ListView;
            items.ItemsSource = controller.Scenes;

            items.SelectionChanged += (sender, e) =>
            {
                var list = sender as ListView;
                var scene = list.SelectedItem as Scene;
                controller.SelectScene(scene);
            };

            items.DragItemsStarting += (sender, e) =>
            {
                var item = e.Items.FirstOrDefault() as IScene;
                if (item != null)
                    e.Data.Properties.Add("Context", item);
            };

            Button btn = GetTemplateChild("AddButton") as Button;
            btn.Click += (sender, e) =>
            {
                if (controller == null)
                    return;

                var scene = controller.CreateScene();
                items.SelectedItem = items.Items.LastOrDefault();
            };
        }
    }
}
