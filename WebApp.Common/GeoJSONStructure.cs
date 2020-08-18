using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WebApp.Common
{
    public class GeoJSONStructure
    {
        public string type { get; set; } = "FeatureCollection";
        public Feature111[] features { get; set; }
    }
    public class Feature111
    {
        public Geometry geometry { get; set; }
        public string type { get; set; } = "Feature";
        public string id { get; set; }
        public Dictionary<string, object> properties { get; set; }
    }
    public class Geometry
    {
        public string type { get; set; } = "Point";
        public List<List<List<double>>> coordinates { get; set; }
    }
    public class Point2D1
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public abstract class Point3D : Point2D1
    {
        public double Z { get; set; }
    }
    public class GeoJSONDiemMo
    {
        public string CreatJSON()
        {
            var points = new List<Point>
            {
                new Point(new Position(52.370725881211314, 4.889259338378906)),
                new Point(new Position(52.3711451105601, 4.895267486572266)),
                new Point(new Position(52.36931095278263, 4.892091751098633)),
                new Point(new Position(52.370725881211314, 4.889259338378906))
            };

            var multiPoint = new MultiPoint(points);
            var actualJson = JsonConvert.SerializeObject(multiPoint);

            var model = new FeatureCollection();
            var expectedIds = new List<string>();
            var expectedIndexes = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                var id = "id" + i;
                expectedIds.Add(id);
                expectedIndexes.Add(i);

                var geom = new Point(
                    new Position(51.010, -0.034)
                );
                var props = new Dictionary<string, object>
                {
                    { "test1", "1" },
                    { "test2", 2 }
                };
                var feature = new Feature(geom, props, id);
                model.Features.Add(feature);
            }
            return JsonConvert.SerializeObject(model);
        }
    }
}
