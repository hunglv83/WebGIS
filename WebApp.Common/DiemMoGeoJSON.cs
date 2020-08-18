using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApp.Common
{
    public class SharpMapControl
    {
        public void DiemMo()
        {

        }
    }
    public class DiemMoGeoJSON
    {
        public string LayTatCa()
        {
            List<Feature> lstDM = new List<Feature>();
            ConvertCoordinate cv = new ConvertCoordinate();
            List<IPosition> coordinates = new List<IPosition>();
            double dbLng = 0.0, dbLat = 0.0;
            //cv.TransMercatorToLatLonInt(419555.992, 2402145.992, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419547.000, 2402132.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419464.000, 2402157.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419320.000, 2402223.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419375.000, 2402033.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419022.000, 2402312.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419046.000, 2402329.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419055.000, 2402348.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419077.000, 2402373.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419198.000, 2402368.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419246.000, 2402390.000, ref dbLat, ref dbLng);
            //cv.TransMercatorToLatLonInt(419555.992, 2402145.992, ref dbLat, ref dbLng);


            cv.TransMercatorToLatLonInt(419555.992, 2402145.992, ref dbLat, ref dbLng);
            Position pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419547.000, 2402132.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419464.000, 2402157.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419320.000, 2402223.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419375.000, 2402033.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419022.000, 2402312.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419046.000, 2402329.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419055.000, 2402348.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419077.000, 2402373.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419198.000, 2402368.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419246.000, 2402390.000, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);
            cv.TransMercatorToLatLonInt(419555.992, 2402145.992, ref dbLat, ref dbLng);
            pnt = new Position(dbLat, dbLng);
            coordinates.Add(pnt);

            Polygon polygon = new Polygon(new List<LineString> { new LineString(coordinates) });
            Dictionary<string, object> properties = new Dictionary<string, object> {
                                                                { "id","dm1"},
                                                                { "Name", "Sông Lô - Yên Sơn" },
                                                                { "DonViKT", "Cty TNHH Hiep Phu" }
                                                            };
            Feature feature = new Feature(polygon, properties);
            lstDM.Add(feature);
            return JsonConvert.SerializeObject(lstDM);
        }
    }
    public partial class DiemMo
    {
        public int MoId { get; set; }
        public string TenMo { get; set; }
        public string TenHuyen { get; set; }
        public string TenXa { get; set; }
        public string LoaiKhoangSan { get; set; }
        public string ToaDoKhepGoc { get; set; }
        public string DiaChi { get; set; }
        public string ChuDauTu { get; set; }
        public decimal DienTich { get; set; }
        public string NamBDKhaiThac { get; set; }
        public string SoVBPDChuTruong { get; set; }
        public string SoGPThamDo { get; set; }
        public string SoQDPDDongCua { get; set; }
        public string SoQDPDThanhTra { get; set; }
        public string TruLuongPheDuyet { get; set; }
        public string CongSuatKhaiThac { get; set; }
        public string MoTa { get; set; }
    }
}
