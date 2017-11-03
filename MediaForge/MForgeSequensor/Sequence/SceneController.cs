using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public class SceneController
    {
        private ObservableCollection<Scene> scenes;
        public ObservableCollection<Scene> Scenes { get { return scenes; } }

        public SceneController()
        {
            scenes = new ObservableCollection<Scene>();
        }

        public Scene CreateScene()
        {
            var scene = new Scene();
            Scenes.Add(scene);

            return scene;
        }
    }
}
