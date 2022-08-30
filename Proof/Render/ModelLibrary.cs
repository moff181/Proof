using Proof.Core.Logging;

namespace Proof.Render
{
    public class ModelLibrary
    {
        private enum ModelLoadState
        {
            Nothing = 0,
            Vertices = 1,
            Indices = 2,
        }

        private readonly ALogger _logger;

        private readonly Dictionary<string, Model?> _library;

        public ModelLibrary(ALogger logger)
        {
            _logger = logger;
            _library = new Dictionary<string, Model?>();
        }

        public Model? Get(string label)
        {
            if(_library.TryGetValue(label, out Model? model))
            {
                return model;
            }

            model = LoadModel(label);
            if(model == null)
            {
                _logger.LogWarn($"Could not load model from {label}");
            }

            _library.Add(label, model);
            return model;
        }

        private Model? LoadModel(string filePath)
        {
            _logger.LogInfo($"Loading model from {filePath}...");

            if(string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                _logger.LogWarn($"File for model does not exist: {filePath}");
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);
            ModelLoadState state = ModelLoadState.Nothing;

            var vertices = new List<float>();
            var indices = new List<int>();

            foreach(string rawLine in lines)
            {
                if(string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                string line = rawLine.Replace(" ", "").Replace("\t", "");

                if (line.Trim() == "Vertices:")
                {
                    state = ModelLoadState.Vertices;
                    continue;
                }
                if (line.Trim() == "Indices:")
                {
                    state = ModelLoadState.Indices;
                    continue;
                }

                if(state == ModelLoadState.Nothing)
                {
                    _logger.LogWarn("Unexpected value while loading model.");
                    continue;
                }

                string[] split = line.Split(',');
                foreach(string s in split)
                {
                    if (state == ModelLoadState.Vertices)
                    {
                        if(!float.TryParse(s, out float val))
                        {
                            _logger.LogWarn($"Could not parse vertex value as a float: {s}");
                            continue;
                        }
                        vertices.Add(val);
                    }
                    else if (state == ModelLoadState.Indices)
                    {
                        if (!int.TryParse(s, out int val))
                        {
                            _logger.LogWarn($"Could not parse index value as an int: {s}");
                            continue;
                        }
                        indices.Add(val);
                    }
                }
            }

            return new Model(_logger, vertices.ToArray(), indices.ToArray());
        }
    }
}
