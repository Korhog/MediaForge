using MForge.Sequensor.Sequence.Interfaces;
using System.Collections.ObjectModel;

namespace MForge.Sequensor.Sequence
{
    public delegate void SceneChangedHandler(SceneController sender, IScene scene);    

    public class SceneController
    {
        public event SceneChangedHandler SceneChanged;
        private IScene currentScene;
        public IScene CurrentScene {  get { return currentScene; } }

        private ObservableCollection<IScene> scenes;
        public ObservableCollection<IScene> Scenes { get { return scenes; } }

        public SceneController()
        {
            scenes = new ObservableCollection<IScene>();
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
        public void SelectScene(IScene scene)
        {
            currentScene = scene;
            SceneChanged?.Invoke(this, scene);
        }
    }
}
