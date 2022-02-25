using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.ML.OnnxRuntime;

namespace AHSPersonDetection.Detection
{
    public class Prediction
    {
        public string? Label { get; set; }
        public float Confidence { get; set; }

        public static List<Prediction> GetPredictions(string imageName)
        {
            string assetsPath = GetAbsolutePath(@"../../../Detection/Assets");
            string modelFilePath = Path.Combine(assetsPath, "Model", "FasterRCNN-10.onnx");
            string imageFilePath = Path.Combine(assetsPath, "Input", imageName);


            using Image<Rgb24> image = Image.Load<Rgb24>(imageFilePath);


            float ratio = 800f / Math.Min(image.Width, image.Height);
            image.Mutate(x => x.Resize((int)(ratio * image.Width), (int)(ratio * image.Height)));

            var paddedHeight = (int)(Math.Ceiling(image.Height / 32f) * 32f);
            var paddedWidth = (int)(Math.Ceiling(image.Width / 32f) * 32f);
            Tensor<float> input = new DenseTensor<float>(new[] { 3, paddedHeight, paddedWidth });
            var mean = new[] { 102.9801f, 115.9465f, 122.7717f };

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgb24> pixelRow = accessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        input[0, y, x] = pixelRow[x].B - mean[0];
                        input[1, y, x] = pixelRow[x].G - mean[1];
                        input[2, y, x] = pixelRow[x].R - mean[2];
                    }
                }
            }
            );

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("image", input)
            };

            using var session = new InferenceSession(modelFilePath);
            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);

            var resultsArray = results.ToArray();
            float[] boxes = resultsArray[0].AsEnumerable<float>().ToArray();
            long[] labels = resultsArray[1].AsEnumerable<long>().ToArray();
            float[] confidences = resultsArray[2].AsEnumerable<float>().ToArray();
            var predictions = new List<Prediction>();
            var minConfidence = 0.6f;
            for (int i = 0; i < boxes.Length - 4; i += 4)
            {
                var index = i / 4;
                if (confidences[index] >= minConfidence)
                {
                    predictions.Add(new Prediction
                    {
                        Label = LabelMap.Labels[labels[index]],
                        Confidence = confidences[index]
                    });
                }
            }

            return predictions;
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

    }


}