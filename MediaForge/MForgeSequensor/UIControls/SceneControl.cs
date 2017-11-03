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
    using Sequence;

    // Элемент управления сценами
    public sealed class SceneControl : ItemsControl
    {
        private SceneController controller;

        public SceneControl()
        {
            DefaultStyleKey = typeof(SceneControl);

            // Внутренний контроллер
            controller = new SceneController();

            controller.CreateScene();
            controller.CreateScene();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var list = GetTemplateChild("PART_Scenes") as ListView;
            list.ItemsSource = controller.Scenes;
            list.ItemClick += (sender, e) => { };
        }
    }
}
