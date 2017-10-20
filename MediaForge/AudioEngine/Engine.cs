using System;

using System.Threading.Tasks;

using Windows.Media.Audio;
using Windows.Media.Render;
using Windows.Storage;

namespace Audio.Core
{
    public class Engine
    {
        private AudioGraph m_audio_graph;

        private Engine(AudioGraph audioGraph)
        {
            m_audio_graph = audioGraph;
        }

        private static async Task<AudioGraph> CreateAudioGraph()
        {
            AudioGraphSettings settings = new AudioGraphSettings(AudioRenderCategory.Movie);
            var audioGraphResult = await AudioGraph.CreateAsync(settings);
            if (audioGraphResult.Status == AudioGraphCreationStatus.Success)
            {
                return audioGraphResult.Graph;
            }

            return null;  
        }

        public async static Task<Engine> Create()
        {
            var audioGraph = await CreateAudioGraph();
            var engine = new Engine(audioGraph);
            await engine.CreateDeviceOutputNode();

            return engine;
        }

        public async Task CreateNodeFromStorageFile(StorageFile source)
        {

        }

        public async Task CreateDeviceOutputNode()
        {
            CreateAudioDeviceOutputNodeResult result = await m_audio_graph.CreateDeviceOutputNodeAsync();
        }
    }
}
