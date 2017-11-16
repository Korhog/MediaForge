using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public delegate void SceneChangedHandler(SceneController sender, Scene scene);    

    public class SceneController
    {
        public event SceneChangedHandler SceneChanged;
        private Scene currentScene;
        public Scene CurrentScene {  get { return currentScene; } }

        private ObservableCollection<Scene> scenes;
        public ObservableCollection<Scene> Scenes { get { return scenes; } }

        public SceneController()
        {
            scenes = new ObservableCollection<Scene>();
        }

        /// <summary> Создаем новую сцену </summary>
        public Scene CreateScene(string name = null)
        {
            var scene = new Scene();
            scenes.Add(scene);
            SelectScene(scene);
            return scene;
        }

        /// <summary> Устанавливаем текущую сцену </summary>
        public void SelectScene(Scene scene)
        {
            currentScene = scene;
            SceneChanged?.Invoke(this, scene);
        }
    }
}
