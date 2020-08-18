using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp.Core.SQLGis
{
    public class RanhGioiCapPhep
    {
        private string strConnect = string.Empty;
        public RanhGioiCapPhep()
        {
            //Lấy chuỗi connection
            strConnect = ConfigurationManager.ConnectionStrings["SqlGis"].ToString();
        }

        #region DCKS_VUNGMO
        public void VungMo_Insert(DCKS_VUNGMO objVungMo)
        {
            try
            {
                StringBuilder strCmd = new StringBuilder();
                strCmd.AppendFormat("INSERT INTO {0} ({1},{2},{3},{4},{5},{6})",
                    "sde.DCKS_DIEMGOCMO",
                    "IDMoQuang", "SHDiemMo", "KHDiemMo", "LoaiKS", "TenKhu", "SHAPE");
                strCmd.AppendFormat(" VALUES({0},'{1}','{2}','{3}',{4},{5})", objVungMo.IDMoQuang, objVungMo.SHDiemMo, objVungMo.KHDiemMo, objVungMo.LoaiKS, objVungMo.TenKhu, objVungMo.SHAPE);

                using (SqlConnection connection = new SqlConnection(strConnect))
                {
                    using (var cmd = new SqlCommand(strCmd.ToString(), connection))
                    {
                        try
                        {
                            connection.Open();
                            Int32 rowsAffected = cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void DiemMo_Insert(DCKS_DIEMMO objDiemMo)
        {
            try
            {
                StringBuilder strCmd = new StringBuilder();
                strCmd.AppendFormat("INSERT INTO {0} ({1},{2},{3},{4},{5},{6},{7},{8})",
                    "sde.DCKS_DIEMGOCMO",
                    "IDMoQuang", "SHDiemMo", "KHDiemMo", "LoaiKS", "TenKhu", "STTDiemGoc", "KHDiemGoc", "SHAPE");
                strCmd.AppendFormat(" VALUES({0},'{1}','{2}','{3}',{4},{5},'{6}',{7})", objDiemMo.IDMoQuang, objDiemMo.SHDiemMo, objDiemMo.KHDiemMo, objDiemMo.LoaiKS, objDiemMo.TenKhu, objDiemMo.STTDiemGoc, objDiemMo.KHDiemGoc, objDiemMo.SHAPE);

                using (SqlConnection connection = new SqlConnection(strConnect))
                {
                    using (var cmd = new SqlCommand(strCmd.ToString(), connection))
                    {
                        try
                        {
                            connection.Open();
                            Int32 rowsAffected = cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void DiemMo_Update(DCKS_DIEMMO objDiemMo)
        {
            try
            {
                StringBuilder strCmd = new StringBuilder();
                strCmd.AppendFormat("UPDATE {0} SET (IDMoQuang='{1}',SHDiemMo='{2}',KHDiemMo='{3}',LoaiKS={4},TenKhu={5},STTDiemGoc='{6}',KHDiemGoc={7},SHAPE={8})",
                    "sde.DCKS_DIEMGOCMO",
                    objDiemMo.IDMoQuang,
                    objDiemMo.SHDiemMo,
                    objDiemMo.KHDiemMo,
                    objDiemMo.LoaiKS,
                    objDiemMo.TenKhu,
                    objDiemMo.STTDiemGoc,
                    objDiemMo.KHDiemGoc,
                    objDiemMo.SHAPE);
                strCmd.AppendFormat(" WHERE IDMoQuang = {1}", objDiemMo.IDMoQuang);

                using (SqlConnection connection = new SqlConnection(strConnect))
                {
                    using (var cmd = new SqlCommand(strCmd.ToString(), connection))
                    {
                        try
                        {
                            connection.Open();
                            Int32 rowsAffected = cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void DiemMo_Delete(DCKS_DIEMMO objDiemMo)
        {
            try
            {
                StringBuilder strCmd = new StringBuilder();
                strCmd.AppendFormat("DELETE FROM {0} WHERE IDMoQuang={1}",
                    "sde.DCKS_DIEMGOCMO", objDiemMo.IDMoQuang);
                using (SqlConnection connection = new SqlConnection(strConnect))
                {
                    using (var cmd = new SqlCommand(strCmd.ToString(), connection))
                    {
                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void VungMo_Update()
        {

        }
        public void VungMo_Delete()
        {

        }
        #endregion

        #region DCKS_VUNGCP

        #endregion

        #region DCKS_KVCAM_TAMCAM

        #endregion
    }
    public class DCKS_VUNG_KVCAM_TAMCAM
    {
        public int OBJECTID { get; set; }
        public string MaDVHC { get; set; }
        public DateTime NgayQD { get; set; }
        public string SoQD { get; set; }
        public string SoBaoQuan { get; set; }
        public string LoaiHinh { get; set; }
        public int ThoiHan { get; set; }
        public string MaCam { get; set; }
        public int MaLoaiHinh { get; set; }
        public string TenKhu { get; set; }
        public int HinhThuc { get; set; }
        public string LoaiKS { get; set; }
        public int IDCamTamCam { get; set; }
        public string SHAPE { get; set; }
    }
    public class DCKS_VUNGCP
    {
        public int OBJECTID { get; set; }
        public string SHDiemMo { get; set; }
        public string KHDiemMo { get; set; }
        public string MaDVHC { get; set; }
        public string SoGiayPhep { get; set; }
        public string MaCSSXKD { get; set; }
        public int LoaiCapPhep { get; set; }
        public string TenKhu { get; set; }
        public int LoaiDieuChinh { get; set; }
        public int IdCapPhep { get; set; }
        public int HieuLuc { get; set; }
        public string LoaiKS { get; set; }
        public string TenCongTy { get; set; }
        public string TruongLienKet { get; set; }
        public string SHAPE { get; set; }
    }
    public class DCKS_VUNGMO
    {
        public int? OBJECTID { get; set; }
        public string SHDiemMo { get; set; } = "";
        public string KHDiemMo { get; set; } = "";
        public string MaDVHC { get; set; } = "";
        public string TenKhu { get; set; } = "";
        public int? LoaiHoatDong { get; set; }
        public int? LoaiDeNghi { get; set; }
        public int? LoaiCapPhep { get; set; }
        public int? LoaiDieuChinh { get; set; }
        public string LoaiKS { get; set; } = "";
        public string TruongLienKet { get; set; } = "";
        public int? IDMoQuang { get; set; }
        public string SHAPE { get; set; } = "";
    }
    public class DCKS_DIEMMO
    {
        public int? OBJECTID { get; set; }
        public string SHDiemMo { get; set; } = "";
        public string KHDiemMo { get; set; } = "";
        public string MaDVHC { get; set; } = "";
        public string TenKhu { get; set; } = "";
        public string KHDiemGoc { get; set; } = "";
        public int? LoaiHoatDong { get; set; }
        public int? STTDiemGoc { get; set; }
        public int? LoaiDeNghi { get; set; }
        public int? LoaiCapPhep { get; set; }
        public int? LoaiDieuChinh { get; set; }
        public string LoaiKS { get; set; } = "";
        public string TruongLienKet { get; set; } = "";
        public int? IDMoQuang { get; set; }
        public string SHAPE { get; set; } = "";
    }
    public class DCKS_DUONGMO
    {
        public int? OBJECTID { get; set; }
        public string SHDiemMo { get; set; } = "";
        public string KHDiemMo { get; set; } = "";
        public string MaDVHC { get; set; } = "";
        public string TenKhu { get; set; } = "";
        public int? LoaiHoatDong { get; set; }
        public int? LoaiDeNghi { get; set; }
        public int? LoaiCapPhep { get; set; }
        public int? LoaiDieuChinh { get; set; }
        public string LoaiKS { get; set; } = "";
        public string TruongLienKet { get; set; } = "";
        public int? IDMoQuang { get; set; }
        public string SHAPE { get; set; } = "";
    }
}