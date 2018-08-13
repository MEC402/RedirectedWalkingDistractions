using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using System.Linq;

public class PixelTableWriter {
    public static void write(List<Texture2D> textures, string outputPath, int binCount)
    {
        StringBuilder sb = new StringBuilder();
        var headers = Enumerable.Range(0, textures.Count()).Select(num => "frame " + num).ToList();

        var values = textures.Select(texture => {
            Color[] pixels = texture.GetPixels();
            float avg = pixels.Average(color => color.grayscale);
            // return pixels.Select(color => color.grayscale).Select(level => level - avg);
            return pixels.Select(color => color.grayscale);
        }).ToList();

        // // bins: bin>frame>values>value
        // var bins = new IEnumerable<IEnumerable<float>>[binCount];

        float binInterval = 1.0f / binCount;

        // binCounts: frame>bin>count(values)
        List<int[]> binCounts = new List<int[]>();

        // foreach (var set in values)
        // {
        //     for (int i = 0; i < binCount; i++)
        //     {
        //         bins[i].Append(set.Where(value => value > binInterval * i && value <= binInterval * i + 1));
        //     }
        //     binCounts.Append(countBins);
        // }

        foreach (var frame in values)
        {
            int[] bins = Enumerable.Repeat(0, binCount).ToArray();
            for (int i = 0; i < binCount; i++)
            {
                bins[i] += frame.Where(value => value > binInterval * i && value <= binInterval * (i + 1)).Count();
            }
            binCounts.Add(bins);
        }

        var tableData = headers.Zip(binCounts, (header, data) => new { Header = header, Data = data });

        foreach (var entry in tableData)
        {
            sb.Append(entry.Header);
            sb.Append('\t');
            foreach(var value in entry.Data)
            {
                sb.Append(value);
                sb.Append('\t');
            }
            sb.AppendLine();
        }

        System.IO.File.WriteAllText(outputPath, sb.ToString());
    }
}
