using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Common
{
    public class ConvertCoordinate2019
    {
        private double a = 6378137.0; //Bán trục lớn
        private double b = 6356752.3142451793;//Bán trục nhỏ
        private double DX = 191.9044143;
        private double DY = 39.30318279;
        private double DZ = 11.450;
        private double E0 = 500000.0; //Độ lệch đông
        private double F = 0.003352811; //Độ dẹt
        //private double muichieu = 0.99990000; //Múi chiếu
        //private double KTtruc = 107.75; //Kinh tuyến trục
        private double N0 = 0.00;
        private double VTtruc = 0.00000; //Vĩ tuyến trục
        private double S = 0.252906278; //Scale
        private double X_rot = -0.00928836; //X_Rotation
        private double Y_rot = 0.0198; //Y_Rotation
        private double Z_rot = -0.00427372; //Z_Rotation

        private double m_a, m_b, m_e2;
        private double m_N, m_pow2N, m_pow3N;
        private double phi, v, rho, n2, phi1, M, P, IV, VI, I, II, III, IIIA;
        private double sinphi, cosphi, tanphi;
        private double pow2sinphi, pow2cosphi, pow3cosphi, pow5cosphi, pow2tanphi, pow4tanphi;
        private double pow2P, pow3P, pow4P, pow5P, pow6P;
        private double m_dSemiMajorAxis = 6378137;
        private double m_dSemiMinorAxis = 6356752.3142451793;
        private double m_dLatitudeOrigin = 0.0;
        private double m_dFalseNorthing = 0.0;
        private double m_dFalseEasting = 500000;
        public double LongitudeOrigin { get; set; } = 105.0;
        public double ScaleFactorAtOrigin { get; set; } = 0.9999;
        public double KTtruc { get; set; } = 105.0;
        public double muichieu { get; set; } = 0.99990000;
        private double SqlSquare(double n)
        {
            return Math.Pow(n, 2);
        }
        private double Pow2(double n)
        {
            return Math.Pow(n, 2);
        }
        private double Pow3(double n)
        {
            return Math.Pow(n, 3);
        }
        private double Pow4(double n)
        {
            return Math.Pow(n, 4);
        }
        private double Pow5(double n)
        {
            return Math.Pow(n, 5);
        }
        private double Pow6(double n)
        {
            return Math.Pow(n, 6);
        }
        private double Pow7(double n)
        {
            return Math.Pow(n, 7);
        }

        public void LatLonToTransMercatorInt(double latitude, double longitude, ref double easting, ref double northing)
        {
            try
            {
                phi = latitude * Math.PI / 180.0;

                m_a = m_dSemiMajorAxis * ScaleFactorAtOrigin;
                m_b = m_dSemiMinorAxis * ScaleFactorAtOrigin;
                m_e2 = ((m_a * m_a) - (m_b * m_b)) / (m_a * m_a);
                m_N = (m_a - m_b) / (m_a + m_b);
                m_pow2N = m_N * m_N;
                m_pow3N = m_N * m_pow2N;

                sinphi = Math.Sin(phi);
                cosphi = Math.Cos(phi);
                tanphi = Math.Tan(phi);

                pow2sinphi = Math.Pow(sinphi, 2);
                pow2cosphi = Math.Pow(cosphi, 2);
                pow3cosphi = pow2cosphi * cosphi;
                pow5cosphi = pow3cosphi * pow2cosphi;
                pow2tanphi = Math.Pow(tanphi, 2);
                pow4tanphi = Math.Pow(pow2tanphi, 2);

                v = m_a / Math.Sqrt(1.0 - m_e2 * (pow2sinphi));
                rho = (v * (1.0 - m_e2)) / (1.0 - m_e2 * pow2sinphi);
                n2 = (v / rho) - 1.0;
                phi1 = m_dLatitudeOrigin * Math.PI / 180.0;

                M = m_b * (((1.0 + m_N + ((m_pow2N) * 5.0 / 4.0) + ((m_pow3N) * 5.0 / 4.0)) * (phi - phi1))
                    - ((3.0 * m_N + 3.0 * (m_pow2N) + (m_pow3N) * 21.0 / 8.0) * Math.Sin(phi - phi1) * Math.Cos(phi + phi1))
                    + ((((m_pow2N) * 15.0 / 8.0) + ((m_pow3N) * 15.0 / 8.0)) * (Math.Sin(2.0 * (phi - phi1))) * (Math.Cos(2.0 * (phi + phi1))))
                    - (((m_pow3N) * 35.0 / 24.0) * (Math.Sin(3.0 * (phi - phi1))) * (Math.Cos(3.0 * (phi + phi1)))));

                P = (longitude - LongitudeOrigin) * Math.PI / 180;

                pow2P = P * P;
                pow3P = pow2P * P;
                pow4P = pow2P * pow2P;
                pow5P = pow4P * P;
                pow6P = pow2P * pow4P;

                I = M + m_dFalseNorthing;
                II = (v / 2.0) * sinphi * cosphi;
                III = (v / 24.0) * sinphi * (pow3cosphi) * (5.0 - (pow2tanphi) + 9.0 * n2);
                IIIA = (v / 720.0) * sinphi * (pow5cosphi) * (61.0 - 58.0 * (pow2tanphi) + (pow4tanphi));

                northing = I + ((Pow2(P)) * II) + ((pow4P) * III) + ((pow6P) * IIIA);

                IV = v * cosphi;
                v = (v / 6.0) * (pow3cosphi) * ((v / rho) - (pow2tanphi));
                VI = (v / 120.0) * (pow5cosphi) * (5.0 - 18.0 * (pow2tanphi) + (pow4tanphi) + 14.0 * n2 - 58.0 * (pow2tanphi) * n2);
                VI = (v / 120.0) * (pow5cosphi) * (5.0 - 18.0 * (pow2tanphi) + (pow4tanphi) + 14 * n2 - 58.0 * (pow2tanphi) * n2);

                easting = m_dFalseEasting + (P * IV) + ((pow3P) * v) + ((pow5P) * VI);
            }
            catch (Exception ex)
            {
                easting = northing = 0.0;
            }
        }

        public void TransMercatorToLatLonInt(double easting, double northing, out double latitude, out double longitude)
        {
            try
            {
                latitude = longitude = 0.0;
                m_a = m_dSemiMajorAxis * ScaleFactorAtOrigin;
                m_b = m_dSemiMinorAxis * ScaleFactorAtOrigin;
                m_e2 = (Math.Pow(m_a, 2) - Math.Pow(m_b, 2)) / Math.Pow(m_a, 2);
                m_N = (m_a - m_b) / (m_a + m_b);
                m_pow2N = m_N * m_N;
                m_pow3N = m_N * m_pow2N;

                double phiDash = ((northing - m_dFalseNorthing) / m_a) + (m_dLatitudeOrigin * Math.PI / 180);
                phi = phiDash;
                phi1 = (m_dLatitudeOrigin * Math.PI / 180.0);

                M = m_b * (((1.0 + m_N + ((m_pow2N) * 5.0 / 4) + ((m_pow3N) * 5.0 / 4.0)) * (phi - phi1))
                - ((3.0 * m_N + 3.0 * (m_pow2N) + (m_pow3N) * 21.0 / 8.0) * Math.Sin(phi - phi1) * Math.Cos(phi + phi1))
                + ((((m_pow2N) * 15.0 / 8.0) + ((m_pow3N) * 15.0 / 8.0)) * (Math.Sin(2 * (phi - phi1))) * (Math.Cos(2.0 * (phi + phi1))))
                - (((m_pow3N) * 35.0 / 24.0) * (Math.Sin(3.0 * (phi - phi1))) * (Math.Cos(3.0 * (phi + phi1)))));

                while (Math.Abs(northing - m_dFalseNorthing - M) > 0.00001)
                {
                    phi = phi + ((northing - m_dFalseNorthing - M) / m_a);
                    M = m_b * (((1 + m_N + ((m_pow2N) * 5.0 / 4.0) + ((m_pow3N) * 5.0 / 4.0)) * (phi - phi1))
                    - ((3.0 * m_N + 3.0 * (m_pow2N) + (m_pow3N) * 21.0 / 8.0) * Math.Sin(phi - phi1) * Math.Cos(phi + phi1))
                    + ((((m_pow2N) * 15.0 / 8.0) + ((m_pow3N) * 15.0 / 8.0)) * (Math.Sin(2.0 * (phi - phi1))) * (Math.Cos(2 * (phi + phi1))))
                    - (((m_pow3N) * 35.0 / 24.0) * (Math.Sin(3.0 * (phi - phi1))) * (Math.Cos(3.0 * (phi + phi1)))));
                }

                v = m_a / Math.Sqrt(1.0 - m_e2 * (Pow2((Math.Sin(phi)))));
                rho = (v * (1.0 - m_e2)) / (1 - m_e2 * (Pow2(Math.Sin(phi))));
                n2 = (v / rho) - 1.0;
                double VII = (Math.Tan(phi)) / (2.0 * rho * v);
                double VIII = ((Math.Tan(phi)) / (24.0 * rho * (Pow3(v)))) * (5.0 + (3.0 * (Pow2(Math.Tan(phi)))) + n2 - (9.0 * n2 * (Pow2(Math.Tan(phi)))));
                double IX = (Math.Tan(phi) / (720.0 * rho * (Pow5(v)))) * (61.0 + (90.0 * (Pow2(Math.Tan(phi)))) + (45.0 * (Pow4(Math.Tan(phi)))));
                double Et = easting - m_dFalseEasting;
                latitude = (phi - ((Pow2(Et)) * VII) + ((Pow4(Et)) * VIII) - ((Pow6(Et)) * IX)) * 180.0 / Math.PI;
                double X = ((1 / Math.Cos(phi)) / v);
                double XI = ((1 / Math.Cos(phi)) / (6.0 * (Pow3(v)))) * ((v / rho) + 2.0 * (Pow2(Math.Tan(phi))));
                double XII = ((1 / Math.Cos(phi)) / (120.0 * (Pow5(v)))) * (5.0 + (28.0 * (Pow2(Math.Tan(phi)))) + (24.0 * (Pow4(Math.Tan(phi)))));
                double XIIA = ((1.0 / Math.Cos(phi)) / (5040.0 * (Pow7(v)))) * (61.0 + (662.0 * (Pow2(Math.Tan(phi)))) + (1320.0 * (Pow4(Math.Tan(phi)))) + (720 * (Pow6(Math.Tan(phi)))));
                longitude = LongitudeOrigin + ((Et * X) - ((Pow3(Et)) * XI) + ((Pow5(Et)) * XII) - ((Pow7(Et)) * XIIA)) * 180.0 / Math.PI;
            }
            catch (Exception ex)
            {
                latitude = longitude = 0.0;
            }
        }

        public void WGS84_to_VN2000_XYZ(double latitude, double longitude, double height, ref double easting, ref double northing, ref double H)
        {
            if (b == 0) b = a * (1 - F);
            //' Lat long H to XYZ WGS84
            double X = Lat_Long_H_to_X(latitude, longitude, height, a, b);
            double Y = Lat_Long_H_to_Y(latitude, longitude, height, a, b);
            double Z = Lat_H_to_Z(latitude, height, a, b);
            //'XYZ to X1Y1Z1 VN2000
            double X1 = Helmert_X(X, Y, Z, DX, Y_rot, Z_rot, S);
            double Y1 = Helmert_Y(X, Y, Z, DY, X_rot, Z_rot, S);
            double Z1 = Helmert_Z(X, Y, Z, DZ, X_rot, Y_rot, S);
            //'X1Y1Z1 to Lat Long H VN2000
            double B1 = XYZ_to_Lat(X1, Y1, Z1, a, b);
            double L1 = XYZ_to_Long(X1, Y1);
            double H1 = XYZ_to_H(X1, Y1, Z1, a, b);
            //'Lat Long H to E N
            easting = Lat_Long_to_East(B1, L1, a, b, E0, muichieu, VTtruc, KTtruc);
            northing = Lat_Long_to_North(B1, L1, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            H = H1;
        }
        public void VN2000_to_WGS84_BLH(double east, double north, double height, ref double latitude, ref double longitude, ref double H)
        {
            //' E N to Lat Long VN2000
            double B1 = E_N_to_Lat(east, north, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double L1 = E_N_to_Long(east, north, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double H1 = height;
            //' Lat long H to XYZ VN2000
            double X1 = Lat_Long_H_to_X(B1, L1, height, a, b);
            double Y1 = Lat_Long_H_to_Y(B1, L1, height, a, b);
            double Z1 = Lat_H_to_Z(B1, L1, a, b);
            //'X1Y1Z1 to XYZ WGS84
            double X = Helmert_X(X1, Y1, Z1, DX, Y_rot, Z_rot, S);
            double Y = Helmert_Y(X1, Y1, Z1, DY, X_rot, Z_rot, S);
            double Z = Helmert_Z(X1, Y1, Z1, DX, X_rot, Y_rot, S);
            //'XYZ to Lat Long H WGS84
            double WGS84Lat = XYZ_to_Lat(X, Y, Z, a, b);
            double WGS84Long = XYZ_to_Long(X, Y);
            double WGS84H = XYZ_to_H(X, Y, Z, a, b);
            //Return
            latitude = WGS84Lat - 2.90417781181418E-03 - 0.00006;
            longitude = WGS84Long + 3.76088254250817E-03 - 0.00013;
            H = WGS84H - 91.4520812714472 - 4.2632564145606E-14 + 47;
        }
        public void NBT_to_WGS84_Lat(double North, double East, double Height)
        {
            //' E N to Lat Long VN2000
            double B1 = E_N_to_Lat(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double L1 = E_N_to_Long(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double H1 = Height;
            //' Lat long H to XYZ VN2000
            double X1 = Lat_Long_H_to_X(B1, L1, Height, a, b);
            double Y1 = Lat_Long_H_to_Y(B1, L1, Height, a, b);
            double Z1 = Lat_H_to_Z(B1, L1, a, b);
            //'X1Y1Z1 to XYZ WGS84
            double X = Helmert_X(X1, Y1, Z1, DX, Y_rot, Z_rot, S);
            double Y = Helmert_Y(X1, Y1, Z1, DY, X_rot, Z_rot, S);
            double Z = Helmert_Z(X1, Y1, Z1, DX, X_rot, Y_rot, S);
            //'XYZ to Lat Long H WGS84
            double WGS84Lat = XYZ_to_Lat(X, Y, Z, a, b);
            double WGS84Long = XYZ_to_Long(X, Y);
            double WGS84H = XYZ_to_H(X, Y, Z, a, b);
            //Return
            //double NBT_to_WGS84_Lat = WGS84Lat - 2.90417781181418E-03 - 0.00006;
        }
        public void NBT_to_WGS84_Long(double North, double East, double Height)
        {
            //' E N to Lat Long VN2000
            double B1 = E_N_to_Lat(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double L1 = E_N_to_Long(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double H1 = Height;
            //' Lat long H to XYZ VN2000
            double X1 = Lat_Long_H_to_X(B1, L1, Height, a, b);
            double Y1 = Lat_Long_H_to_Y(B1, L1, Height, a, b);
            double Z1 = Lat_H_to_Z(B1, L1, a, b);
            //'X1Y1Z1 to XYZ WGS84
            double X = Helmert_X(X1, Y1, Z1, DX, Y_rot, Z_rot, S);
            double Y = Helmert_Y(X1, Y1, Z1, DY, X_rot, Z_rot, S);
            double Z = Helmert_Z(X1, Y1, Z1, DX, X_rot, Y_rot, S);
            //'XYZ to Lat Long H WGS84
            double WGS84Lat = XYZ_to_Lat(X, Y, Z, a, b);
            double WGS84Long = XYZ_to_Long(X, Y);
            double WGS84H = XYZ_to_H(X, Y, Z, a, b);
            //Return
            //double NBT_to_WGS84_Long = WGS84Long + 3.76088254250817E-03 - 0.00013
        }
        public void NBT_to_WGS84_H(double North, double East, double Height)
        {
            //' E N to Lat Long VN2000
            double B1 = E_N_to_Lat(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double L1 = E_N_to_Long(East, North, a, b, E0, N0, muichieu, VTtruc, KTtruc);
            double H1 = Height;
            //' Lat long H to XYZ VN2000
            double X1 = Lat_Long_H_to_X(B1, L1, Height, a, b);
            double Y1 = Lat_Long_H_to_Y(B1, L1, Height, a, b);
            double Z1 = Lat_H_to_Z(B1, L1, a, b);
            //'X1Y1Z1 to XYZ WGS84
            double X = Helmert_X(X1, Y1, Z1, DX, Y_rot, Z_rot, S);
            double Y = Helmert_Y(X1, Y1, Z1, DY, X_rot, Z_rot, S);
            double Z = Helmert_Z(X1, Y1, Z1, DX, X_rot, Y_rot, S);
            //'XYZ to Lat Long H WGS84
            double WGS84Lat = XYZ_to_Lat(X, Y, Z, a, b);
            double WGS84Long = XYZ_to_Long(X, Y);
            double WGS84H = XYZ_to_H(X, Y, Z, a, b);
            //Return
            //double NBT_to_WGS84_H = WGS84H - 91.4520812714472 - 4.2632564145606E-14 + 47
        }

        /// <summary>
        /// 'Computed Helmert transformed X coordinate.
        /// </summary>
        /// <param name="X"> cartesian XYZ coords (X,Y,Z), X translation (DX) all in meters ; _</param>
        /// <param name="Y"> cartesian XYZ coords (X,Y,Z), X translation (DX) all in meters ; _</param>
        /// <param name="Z"> cartesian XYZ coords (X,Y,Z), X translation (DX) all in meters ; _</param>
        /// <param name="DX">cartesian XYZ coords (X,Y,Z), X translation (DX) all in meters ; _</param>
        /// <param name="Y_rot"> Y and Z rotations in seconds of arc (Y_Rot, Z_Rot) and scale in ppm (s).</param>
        /// <param name="Z_rot"> Y and Z rotations in seconds of arc (Y_Rot, Z_Rot) and scale in ppm (s).</param>
        /// <param name="S">< Y and Z rotations in seconds of arc (Y_Rot, Z_Rot) and scale in ppm (s)./param>
        /// <returns></returns>
        private double Helmert_X(double X, double Y, double Z, double DX, double Y_rot, double Z_rot, double S)
        {
            double sfactor = S * 0.000001;
            double RadY_Rot = (Y_rot / 3600) * (Math.PI / 180);
            double RadZ_Rot = (Z_rot / 3600) * (Math.PI / 180);
            return X + (X * sfactor) - (Y * RadZ_Rot) + (Z * RadY_Rot) + DX;
        }
        private double Helmert_Y(double X, double Y, double Z, double DY, double X_rot, double Z_rot, double S)
        {
            double sfactor = S * 0.000001;
            double RadX_Rot = (X_rot / 3600) * (Math.PI / 180);
            double RadZ_Rot = (Z_rot / 3600) * (Math.PI / 180);
            return (X * RadZ_Rot) + Y + (Y * sfactor) - (Z * RadX_Rot) + DY;
        }
        private double Helmert_Z(double X, double Y, double Z, double DZ, double X_rot, double Y_rot, double S)
        {
            double sfactor = S * 0.000001;
            double RadX_Rot = (X_rot / 3600) * (Math.PI / 180);
            double RadY_Rot = (Y_rot / 3600) * (Math.PI / 180);
            return (-1 * X * RadY_Rot) + (Y * RadX_Rot) + Z + (Z * sfactor) + DZ;
        }

        /// <summary>
        /// 'Convert XYZ to Latitude (PHI) in Dec Degrees.
        /// 'THIS FUNCTION REQUIRES THE "Iterate_XYZ_to_Lat" FUNCTION
        /// 'THIS FUNCTION IS CALLED BY THE "XYZ_to_H" FUNCTION
        /// </summary>
        /// <param name="X"> XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.</param>
        /// <param name="Y"> XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.</param>
        /// <param name="Z"> XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.</param>
        /// <param name="a"> XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.</param>
        /// <param name="b"> XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.</param>
        /// <returns></returns>
        private double XYZ_to_Lat(double X, double Y, double Z, double a, double b)
        {
            double RootXYSqr = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            double e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double PHI1 = Math.Atan(Z / (RootXYSqr * (1 - e2)));
            double PHI = Iterate_XYZ_to_Lat(a, e2, PHI1, Z, RootXYSqr);
            return PHI * (180 / Math.PI);
        }
        /// <summary>
        /// Iteratively computes Latitude (PHI).
        /// 'THIS FUNCTION IS CALLED BY THE "XYZ_to_PHI" FUNCTION
        /// 'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE
        ///  "Projection and Transformation Calculations.xls" SPREADSHEET
        /// </summary>
        /// <param name="a">ellipsoid semi major axis (a) in meters</param>
        /// <param name="e2">eta squared (e2)</param>
        /// <param name="pHI1"> estimated value for latitude (PHI1) in radians;</param>
        /// <param name="z">Cartesian Z coordinate (Z) in meters;</param>
        /// <param name="rootXYSqr">RootXYSqr computed from X & Y in meters.</param>
        /// <returns></returns>
        private double Iterate_XYZ_to_Lat(double a, double e2, double PHI1, double Z, double RootXYSqr)
        {
            double V = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(PHI1), 2)));
            double PHI2 = Math.Atan((Z + (e2 * V * Math.Sin(PHI1))) / RootXYSqr);
            while (Math.Abs(PHI1 - PHI2) > 0.000000001)
            {
                PHI1 = PHI2;
                V = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(PHI1), 2)));
                //        PHI2 = Atn((Z + (e2 * V * (Sin(PHI1)))) / RootXYSqr)
                PHI2 = Math.Atan((Z + (e2 * V * (Math.Sin(PHI1)))) / RootXYSqr);
            }
            return PHI2;
        }

        /// <summary>
        /// 'Convert XYZ to Longitude (LAM) in Dec Degrees.
        /// </summary>
        /// <param name="X"> X and Y cartesian coords in meters.</param>
        /// <param name="Y"> X and Y cartesian coords in meters.</param>
        /// <returns></returns>
        private double XYZ_to_Long(double X, double Y)
        {
            if (X > 0) //longitude is in the W90 thru 0 to E90 hemisphere
            {
                return (Math.Atan(Y / X)) * (180.0 / Math.PI);
            }
            else if (X < 0 && Y >= 0)//longitude is in the E90 to E180 quadrant
            {
                return ((Math.Atan(Y / X)) * (180.0 / Math.PI)) + 180.0;
            }
            else if (X < 0 && Y < 0)//longitude is in the E180 to W90 quadrant
            {
                return ((Math.Atan(Y / X)) * (180 / Math.PI)) - 180.0;
            }
            return 0.0;
        }

        /// <summary>
        /// 'Convert XYZ to Ellipsoidal Height.
        ///  XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.
        ///  'REQUIRES THE "XYZ_to_Lat" FUNCTION
        /// </summary>
        /// <param name="X">X cartesian coords meters</param>
        /// <param name="Y">Y cartesian coords meters</param>
        /// <param name="Z">Z cartesian coords meters</param>
        /// <param name="a">a ellipsoid axis dimensions meters</param>
        /// <param name="b">b ellipsoid axis dimensions meters</param>
        /// <returns></returns>
        private double XYZ_to_H(double X, double Y, double Z, double a, double b)
        {
            double PHI = XYZ_to_Lat(X, Y, Z, a, b);
            double RadPHI = PHI * (Math.PI / 180);
            double RootXYSqr = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            double e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double V = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            double H = (RootXYSqr / Math.Cos(RadPHI)) - V;
            return H;
        }

        /// <summary>
        /// 'Convert geodetic coords lat (PHI), long (LAM) and height (H) to cartesian X coordinate.
        /// </summary>
        /// <param name="PHI">Latitude decimal degrees</param>
        /// <param name="LAM">Longitude decimal degrees</param>
        /// <param name="H">Ellipsoidal height meters</param>
        /// <param name="a">ellipsoid axis dimensions (a & b) meters</param>
        /// <param name="b">ellipsoid axis dimensions (a & b) meters</param>
        /// <returns></returns>
        private double Lat_Long_H_to_X(double PHI, double LAM, double H, double a, double b)
        {
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            //    V = a / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
            double V = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            return (V + H) * (Math.Cos(RadPHI)) * (Math.Cos(RadLAM));
        }

        /// <summary>
        /// 'Convert geodetic coords lat (PHI), long (LAM) and height (H) to cartesian Y coordinate.e.
        /// </summary>
        /// <param name="PHI">Latitude decimal degrees</param>
        /// <param name="LAM">Longitude decimal degrees</param>
        /// <param name="H">Ellipsoidal height meters</param>
        /// <param name="a">ellipsoid axis dimensions (a & b) meters</param>
        /// <param name="b">ellipsoid axis dimensions (a & b) meters</param>
        /// <returns></returns>
        private double Lat_Long_H_to_Y(double PHI, double LAM, double H, double a, double b)
        {
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double V = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            //    Lat_Long_H_to_Y = (V + H) * (Cos(RadPHI)) * (Sin(RadLAM))
            return (V + H) * (Math.Cos(RadPHI)) * (Math.Sin(RadLAM));
        }
        /// <summary>
        /// 'Convert geodetic coord components latitude (PHI) and height (H) to cartesian Z coordinate.
        /// </summary>
        /// <param name="PHI"> Latitude (PHI) decimal degrees; _</param>
        /// <param name="H"> Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.</param>
        /// <param name="a"> Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.</param>
        /// <param name="b"> Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.</param>
        /// <returns></returns>
        private double Lat_H_to_Z(double PHI, double H, double a, double b)
        {
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            //'Compute eccentricity squared and nu
            double e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double V = a / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(RadPHI)), 2))));
            //'Compute X
            return ((V * (1 - e2)) + H) * (Math.Sin(RadPHI));
        }

        /// <summary>
        /// 'Project Latitude and longitude to Transverse Mercator eastings.
        ///  Latitude (PHI) and Longitude (LAM) in decimal degrees; _
        ///   ellipsoid axis dimensions (a & b) in meters; _
        ///    eastings of false origin (e0) in meters; _
        ///     central meridian scale factor (f0); _
        ///      latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.
        /// </summary>
        /// <param name="PHI">Latitude decimal degrees</param>
        /// <param name="LAM">Longitude decimal degrees</param>
        /// <param name="a">ellipsoid axis dimensions (a & b) in meters</param>
        /// <param name="b">ellipsoid axis dimensions (a & b) in meters</param>
        /// <param name="E0"></param>
        /// <param name="f0">central meridian scale factor (f0)</param>
        /// <param name="PHI0">latitude (PHI0) of false origin in decimal degree</param>
        /// <param name="LAM0">longitude (LAM0) of false origin in decimal degree</param>
        /// <returns></returns>
        private double Lat_Long_to_East(double PHI, double LAM, double a, double b, double E0, double f0, double PHI0, double LAM0)
        {
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double RadPHI0 = PHI0 * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double nu = af0 / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            double rho = (nu * (1 - e2)) / (1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            double eta2 = (nu / rho) - 1;
            double p = RadLAM - RadLAM0;
            double IV = nu * (Math.Cos(RadPHI));
            //V = (nu / 6) * ((Cos(RadPHI)) ^ 3) * ((nu / rho) - ((Tan(RadPHI) ^ 2)))
            double V = (nu / 6) * (Math.Pow(Math.Cos(RadPHI), 3) * ((nu / rho) - Math.Pow(Math.Tan(RadPHI), 2)));
            //VI = (nu / 120) * ((Cos(RadPHI)) ^ 5) * (5 - (18 * ((Tan(RadPHI)) ^ 2)) + ((Tan(RadPHI)) ^ 4) + (14 * eta2) - (58 * ((Tan(RadPHI)) ^ 2) * eta2))
            double VI = (nu / 120) * (Math.Pow(Math.Cos(RadPHI), 5) * (5 - (18 * Math.Pow(Math.Tan(RadPHI), 2) + (Math.Pow(Math.Tan(RadPHI), 4) + (14 * eta2) - (58 * (Math.Pow(Math.Tan(RadPHI), 22) * eta2))))));
            //Lat_Long_to_East = E0 + (p * IV) + ((p ^ 3) * V) + ((p ^ 5) * VI)
            return E0 + (p * IV) + (Math.Pow(p, 3) * V) + (Math.Pow(p, 5) * VI);
        }

        /// <summary>
        /// 'Project Latitude and longitude to Transverse Mercator northings
        /// 'REQUIRES THE "Marc" FUNCTION
        /// </summary>
        /// <param name="PHI"> Latitude (PHI) in decimal degrees; _</param>
        /// <param name="LAM"> Longitude (LAM) in decimal degrees; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="E0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="N0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0); _</param>
        /// <param name="PHI0"> latitude (PHI0) of false origin in decimal degrees.</param>
        /// <param name="LAM0"> and longitude (LAM0) of false origin in decimal degrees.</param>
        /// <returns></returns>
        private double Lat_Long_to_North(double PHI, double LAM, double a, double b, double E0, double N0, double f0, double PHI0, double LAM0)
        {
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double RadPHI0 = PHI0 * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double nu = af0 / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            double rho = (nu * (1 - e2)) / (1 - (e2 * Math.Pow(Math.Sin(RadPHI), 2)));
            double eta2 = (nu / rho) - 1;
            double p = RadLAM - RadLAM0;

            double m = Marc(bf0, N, RadPHI0, RadPHI);
            double i = m + N0;
            double II = (nu / 2) * (Math.Sin(RadPHI)) * (Math.Cos(RadPHI));
            //III = ((nu / 24) * (Sin(RadPHI)) * ((Cos(RadPHI)) ^ 3)) * (5 - ((Tan(RadPHI)) ^ 2) + (9 * eta2))
            double III = (nu / 24) * (Math.Sin(RadPHI) * Math.Pow(Math.Cos(RadPHI), 3) * (5 - (Math.Pow(Math.Tan(RadPHI), 2) + (9 * eta2))));
            //    IIIA = ((nu / 720) * (Sin(RadPHI)) * ((Cos(RadPHI)) ^ 5)) * (61 - (58 * ((Tan(RadPHI)) ^ 2)) + ((Tan(RadPHI)) ^ 4))
            double IIIA = ((nu / 720) * Math.Sin(RadPHI) * Math.Pow(Math.Cos(RadPHI), 5)) * (61 - (58 * Math.Pow(Math.Tan(RadPHI), 2)) + Math.Pow(Math.Tan(RadPHI), 4));
            //    Lat_Long_to_North = i + ((p ^ 2) * II) + ((p ^ 4) * III) + ((p ^ 6) * IIIA)
            return i + (Math.Pow(p, 2) * II) + (Math.Pow(p, 4) * III) + (Math.Pow(p, 6) * IIIA);
        }

        /// <summary>
        /// 'Un-project Transverse Mercator eastings and northings back to latitude.
        /// 'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS
        /// </summary>
        /// <param name="East"> eastings (East) in meters; _</param>
        /// <param name="North"> northings (North) in meters; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="E0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="N0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0) and _</param>
        /// <param name="PHI0"> latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.</param>
        /// <param name="LAM0"> latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.</param>
        /// <returns></returns>
        private double E_N_to_Lat(double East, double North, double a, double b, double E0, double N0, double f0, double PHI0, double LAM0)
        {
            double Pi = Math.PI;
            double RadPHI0 = PHI0 * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double Et = East - E0;
            double PHId = InitialLat(North, N0, af0, RadPHI0, N, bf0);
            //    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
            double nu = af0 / Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(PHId)), 2)));
            //    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(PHId)), 2));
            //    eta2 = (nu / rho) - 1
            double eta2 = (nu / rho) - 1;
            //'Compute Latitude
            double VII = (Math.Tan(PHId)) / (2 * rho * nu);
            //    VIII = ((Tan(PHId)) / (24 * rho * (nu ^ 3))) * (5 + (3 * ((Tan(PHId)) ^ 2)) + eta2 - (9 * eta2 * ((Tan(PHId)) ^ 2)))
            double VIII = ((Math.Tan(PHId)) / (24 * rho * Math.Pow(nu, 3))) * (5 + (3 * Math.Pow((Math.Tan(PHId)), 2)) + eta2 - (9 * eta2 * Math.Pow((Math.Tan(PHId)), 2)));
            //    IX = ((Tan(PHId)) / (720 * rho * (nu ^ 5))) * (61 + (90 * ((Tan(PHId)) ^ 2)) + (45 * ((Tan(PHId)) ^ 4)))
            double IX = ((Math.Tan(PHId)) / (720 * rho * Math.Pow(nu, 5))) * (61 + (90 * Math.Pow((Math.Tan(PHId)), 2)) + (45 * Math.Pow((Math.Tan(PHId)), 4)));
            //    E_N_to_Lat = (180 / Pi) * (PHId - ((Et ^ 2) * VII) + ((Et ^ 4) * VIII) - ((Et ^ 6) * IX))
            return (180 / Pi) * (PHId - (Math.Pow(Et, 2) * VII) + (Math.Pow(Et, 4) * VIII) - (Math.Pow(Et, 6) * IX));
        }

        /// <summary>
        /// 'Un-project Transverse Mercator eastings and northings back to longitude.
        /// 'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS
        /// </summary>
        /// <param name="East"> eastings (East) in meters; _</param>
        /// <param name="North"> northings (North) in meters; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="E0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="N0"> eastings (e0) and northings (n0) of false origin in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0) and _</param>
        /// <param name="PHI0"> latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.</param>
        /// <param name="LAM0"> latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.</param>
        /// <returns></returns>
        private double E_N_to_Long(double East, double North, double a, double b, double E0, double N0, double f0, double PHI0, double LAM0)
        {
            double Pi = Math.PI;
            double RadPHI0 = PHI0 * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double Et = East - E0;
            //'Compute initial value for latitude (PHI) in radians
            double PHId = InitialLat(North, N0, af0, RadPHI0, N, bf0);
            //'Compute nu, rho and eta2 using value for PHId
            //nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
            double nu = af0 / Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(PHId)), 2)));
            //rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(PHId)), 2));
            //eta2 = (nu / rho) - 1
            double eta2 = (nu / rho) - 1;
            //'Compute Longitude
            //    X = ((Cos(PHId)) ^ -1) / nu
            double X = 1 / Math.Cos(PHId) / nu;
            //    XI = (((Cos(PHId)) ^ -1) / (6 * (nu ^ 3))) * ((nu / rho) + (2 * ((Tan(PHId)) ^ 2)))
            double XI = (1 / Math.Cos(PHId) / (6 * Math.Pow(nu, 3))) * ((nu / rho) + (2 * Math.Pow((Math.Tan(PHId)), 2)));
            //    XII = (((Cos(PHId)) ^ -1) / (120 * (nu ^ 5))) * (5 + (28 * ((Tan(PHId)) ^ 2)) + (24 * ((Tan(PHId)) ^ 4)))
            double XII = (1 / Math.Cos(PHId) / (120 * Math.Pow(nu, 5))) * (5 + (28 * Math.Pow((Math.Tan(PHId)), 2)) + (24 * Math.Pow((Math.Tan(PHId)), 4)));
            //    XIIA = (((Cos(PHId)) ^ -1) / (5040 * (nu ^ 7))) * (61 + (662 * ((Tan(PHId)) ^ 2)) + (1320 * ((Tan(PHId)) ^ 4)) + (720 * ((Tan(PHId)) ^ 6)))
            double XIIA = (1 / Math.Cos(PHId) / (5040 * Math.Pow(nu, 7))) * (61 + (662 * Math.Pow((Math.Tan(PHId)), 2)) + (1320 * Math.Pow((Math.Tan(PHId)), 4)) + (720 * Math.Pow((Math.Tan(PHId)), 6)));
            //    E_N_to_Long = (180 / Pi) * (RadLAM0 + (Et * X) - ((Et ^ 3) * XI) + ((Et ^ 5) * XII) - ((Et ^ 7) * XIIA))
            double E_N_to_Long = (180 / Pi) * (RadLAM0 + (Et * X) - (Math.Pow(Et, 3) * XI) + (Math.Pow(Et, 5) * XII) - (Math.Pow(Et, 7) * XIIA));
            return E_N_to_Long;
        }

        /// <summary>
        /// 'Compute initial value for Latitude (PHI) IN RADIANS.
        /// 'REQUIRES THE "Marc" FUNCTION
        /// 'THIS FUNCTION IS CALLED BY THE "E_N_to_Lat", "E_N_to_Long" and "E_N_to_C" FUNCTIONS
        /// 'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE  "Projection and Transformation Calculations.xls" SPREADSHEET
        /// </summary>
        /// <param name="North"> northing of point (North) and northing of false origin (n0) in meters; _</param>
        /// <param name="N0"> northing of point (North) and northing of false origin (n0) in meters; _</param>
        /// <param name="afo"> semi major axis multiplied by central meridian scale factor (af0) in meters; _</param>
        /// <param name="PHI0"> latitude of false origin (PHI0) IN RADIANS; _</param>
        /// <param name="N"> n (computed from a, b and f0) and _</param>
        /// <param name="bfo"> ellipsoid semi major axis multiplied by central meridian scale factor (bf0) in meters.</param>
        /// <returns></returns>
        private double InitialLat(double North, double N0, double afo, double PHI0, double N, double bfo)
        {
            //'First PHI value (PHI1)
            double PHI1 = ((North - N0) / afo) + PHI0;
            //'Calculate M
            double m = Marc(bfo, N, PHI0, PHI1);
            //'Calculate new PHI value (PHI2)
            double PHI2 = ((North - N0 - m) / afo) + PHI1;
            //'Iterate to get final value for InitialLat
            while (Math.Abs(North - N0 - m) > 0.00001)
            {
                PHI2 = ((North - N0 - m) / afo) + PHI1;
                m = Marc(bfo, N, PHI0, PHI2);
                PHI1 = PHI2;
            }
            //nitialLat = PHI2;
            return PHI2;
        }
        /// <summary>
        /// 'Compute meridional arc.
        /// 'THIS FUNCTION IS CALLED BY THE  "Lat_Long_to_North" and "InitialLat" FUNCTIONS
        /// 'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE "Projection and Transformation Calculations.xls" SPREADSHEET
        /// </summary>
        /// <param name="bf0"> ellipsoid semi major axis multiplied by central meridian scale factor (bf0) in meters; _</param>
        /// <param name="N"> n (computed from a, b and f0); _</param>
        /// <param name="PHI0"> lat of false origin (PHI0) and initial or final latitude of point (PHI) IN RADIANS.</param>
        /// <param name="PHI"> lat of false origin (PHI0) and initial or final latitude of point (PHI) IN RADIANS.</param>
        /// <returns></returns>
        private double Marc(double bf0, double N, double PHI0, double PHI)
        {
            //Marc = bf0 * (((1 + N + ((5 / 4) * (N ^ 2)) + ((5 / 4) * (N ^ 3))) * (PHI - PHI0)) _
            //    - (((3 * N) + (3 * (N ^ 2)) + ((21 / 8) * (N ^ 3))) * (Sin(PHI - PHI0)) * (Cos(PHI + PHI0))) _
            //    + ((((15 / 8) * (N ^ 2)) + ((15 / 8) * (N ^ 3))) * (Sin(2 * (PHI - PHI0))) * (Cos(2 * (PHI + PHI0)))) _
            //    - (((35 / 24) * (N ^ 3)) * (Sin(3 * (PHI - PHI0))) * (Cos(3 * (PHI + PHI0)))))

            double Marc1 = bf0 * (((1 + N + ((5 / 4) * Math.Pow(N, 2)) + ((5 / 4) * Math.Pow(N, 3))) * (PHI - PHI0))
                            - (((3 * N) + (3 * Math.Pow(N, 2)) + ((21 / 8) * Math.Pow(N, 3))) * (Math.Sin(PHI - PHI0)) * (Math.Cos(PHI + PHI0)))
                            + ((((15 / 8) * Math.Pow(N, 2)) + ((15 / 8) * Math.Pow(N, 3))) * (Math.Sin(2 * (PHI - PHI0))) * (Math.Cos(2 * (PHI + PHI0))))
                            - (((35 / 24) * Math.Pow(N, 3)) * (Math.Sin(3 * (PHI - PHI0))) * (Math.Cos(3 * (PHI + PHI0)))));

            return Marc1;
        }

        /// <summary>
        /// 'Compute convergence (in decimal degrees) from latitude and longitude
        /// </summary>
        /// <param name="PHI"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="LAM"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="LAM0"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0).</param>
        /// <returns></returns>
        private double Lat_Long_to_C(double PHI, double LAM, double LAM0, double a, double b, double f0)
        {
            //'Convert angle measures to radians
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            //'Compute af0, bf0 and e squared (e2)
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            //'Compute nu, rho, eta2 and p
            //    nu = af0 / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
            double nu = af0 / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(RadPHI)), 2))));
            //    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(RadPHI)) ^ 2))
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(RadPHI)), 2));
            double eta2 = (nu / rho) - 1;
            double p = RadLAM - RadLAM0;
            //'Compute Convergence
            double XIII = Math.Sin(RadPHI);
            //    XIV = ((Sin(RadPHI) * ((Cos(RadPHI)) ^ 2)) / 3) * (1 + (3 * eta2) + (2 * (eta2 ^ 2)))
            double XIV = ((Math.Sin(RadPHI) * Math.Pow((Math.Cos(RadPHI)), 2)) / 3) * (1 + (3 * eta2) + (2 * Math.Pow(eta2, 2)));
            //    XV = ((Sin(RadPHI) * ((Cos(RadPHI)) ^ 4)) / 15) * (2 - ((Tan(RadPHI)) ^ 2))
            double XV = ((Math.Sin(RadPHI) * Math.Pow((Math.Cos(RadPHI)), 4)) / 15) * (2 - Math.Pow((Math.Tan(RadPHI)), 2));
            //    Lat_Long_to_C = (180 / Pi) * ((p * XIII) + ((p ^ 3) * XIV) + ((p ^ 5) * XV))
            return (180 / Pi) * ((p * XIII) + (Math.Pow(p, 3) * XIV) + (Math.Pow(p, 5) * XV));
        }

        /// <summary>
        /// 'Compute convergence (in decimal degrees) from easting and northing
        /// 'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS
        /// </summary>
        /// <param name="East"> Eastings (East) and Northings (North) in meters; _</param>
        /// <param name="North"> Eastings (East) and Northings (North) in meters; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="E0"> easting (e0) and northing (n0) of true origin in meters; _</param>
        /// <param name="N0"> easting (e0) and northing (n0) of true origin in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0); _</param>
        /// <param name="PHI0"> latitude of central meridian (PHI0) in decimal degrees.</param>
        /// <returns></returns>
        private double E_N_to_C(double East, double North, double a, double b, double E0, double N0, double f0, double PHI0)
        {
            //'Convert angle measures to radians
            double Pi = Math.PI;
            double RadPHI0 = PHI0 * (Pi / 180);
            //'Compute af0, bf0, e squared (e2), n and Et
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double Et = East - E0;
            //'Compute initial value for latitude (PHI) in radians
            double PHId = InitialLat(North, N0, af0, RadPHI0, N, bf0);
            //'Compute nu, rho and eta2 using value for PHId
            double nu = af0 / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(PHId)), 2))));
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(PHId)), 2));
            double eta2 = (nu / rho) - 1;
            //'Compute Convergence
            double XVI = (Math.Tan(PHId)) / nu;
            //    XVII = ((Tan(PHId)) / (3 * (nu ^ 3))) * (1 + ((Tan(PHId)) ^ 2) - eta2 - (2 * (eta2 ^ 2)))
            double XVII = ((Math.Tan(PHId)) / (3 * Math.Pow(nu, 3))) * (1 + Math.Pow((Math.Tan(PHId)), 2) - eta2 - (2 * Math.Pow(eta2, 2)));
            //    XVIII = ((Tan(PHId)) / (15 * (nu ^ 5))) * (2 + (5 * ((Tan(PHId)) ^ 2)) + (3 * ((Tan(PHId)) ^ 4)))
            double XVIII = ((Math.Tan(PHId)) / (15 * Math.Pow(nu, 5))) * (2 + (5 * Math.Pow((Math.Tan(PHId)), 2)) + (3 * Math.Pow((Math.Tan(PHId)), 4)));
            //    E_N_to_C = (180 / Pi) * ((Et * XVI) - ((Et ^ 3) * XVII) + ((Et ^ 5) * XVIII))
            return (180 / Pi) * ((Et * XVI) - (Math.Pow(Et, 3) * XVII) + (Math.Pow(Et, 5) * XVIII));

        }

        /// <summary>
        /// 'Compute local scale factor from latitude and longitude
        /// </summary>
        /// <param name="PHI"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="LAM"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="LAM0"> latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0).</param>
        /// <returns></returns>
        private double Lat_Long_to_LSF(double PHI, double LAM, double LAM0, double a, double b, double f0)
        {
            // 'Convert angle measures to radians
            double Pi = Math.PI;
            double RadPHI = PHI * (Pi / 180);
            double RadLAM = LAM * (Pi / 180);
            double RadLAM0 = LAM0 * (Pi / 180);
            //'Compute af0, bf0 and e squared (e2)
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            //'Compute nu, rho, eta2 and p
            double nu = af0 / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(RadPHI)), 2))));
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(RadPHI)), 2));
            double eta2 = (nu / rho) - 1;
            double p = RadLAM - RadLAM0;
            //'Compute local scale factor
            double XIX = (Math.Pow(Math.Cos(RadPHI), 2) / 2) * (1 + eta2);
            //    XX = ((Cos(RadPHI) ^ 4) / 24) * (5 - (4 * ((Tan(RadPHI)) ^ 2)) + (14 * eta2) - (28 * ((Tan(RadPHI * eta2)) ^ 2)))
            double XX = (Math.Pow(Math.Cos(RadPHI), 4) / 24) * (5 - (4 * Math.Pow((Math.Tan(RadPHI)), 2)) + (14 * eta2) - (28 * Math.Pow((Math.Tan(RadPHI * eta2)), 2)));
            return f0 * (1 + (Math.Pow(p, 2) * XIX) + (Math.Pow(p, 4) * XX));
        }
        /// <summary>
        /// 'Compute local scale factor from from easting and northing
        /// </summary>
        /// <param name="East"> Eastings (East) and Northings (North) in meters; _</param>
        /// <param name="North"> Eastings (East) and Northings (North) in meters; _</param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) in meters; _</param>
        /// <param name="b"></param>
        /// <param name="E0"> easting (e0) and northing (n0) of true origin in meters; _</param>
        /// <param name="N0"></param>
        /// <param name="f0"> central meridian scale factor (f0); _</param>
        /// <param name="PHI0"> latitude of central meridian (PHI0) in decimal degrees.</param>
        /// <returns></returns>
        private double E_N_to_LSF(double East, double North, double a, double b, double E0, double N0, double f0, double PHI0)
        {
            double Pi = Math.PI;
            double RadPHI0 = PHI0 * (Pi / 180);
            //'Compute af0, bf0, e squared (e2), n and Et
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double Et = East - E0;
            //'Compute initial value for latitude (PHI) in radians
            double PHId = InitialLat(North, N0, af0, RadPHI0, N, bf0);
            //'Compute nu, rho and eta2 using value for PHId
            double nu = af0 / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(PHId)), 2))));
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(PHId)), 2));
            double eta2 = (nu / rho) - 1;
            //'Compute local scale factor
            double XXI = 1 / (2 * rho * nu);
            double XXII = (1 + (4 * eta2)) / (24 * Math.Pow(rho, 2) * Math.Pow(nu, 2));
            //    E_N_to_LSF = f0 * (1 + ((Et ^ 2) * XXI) + ((Et ^ 4) * XXII))
            return f0 * (1 + (Math.Pow(Et, 2) * XXI) + (Math.Pow(Et, 4) * XXII));
        }
        /// <summary>
        /// 'Compute (t-T) correction in decimal degrees at point (AtEast, AtNorth) to point (ToEast,ToNorth)
        /// 'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS
        /// </summary>
        /// <param name="AtEast"> Eastings (AtEast) and Northings (AtNorth) in meters, of point where (t-T) is being computed; _</param>
        /// <param name="AtNorth"> Eastings (AtEast) and Northings (AtNorth) in meters, of point where (t-T) is being computed; _</param>
        /// <param name="ToEast"> Eastings (ToEast) and Northings (ToNorth) in meters, of point at other end of line to which (t-T) is being computed; _</param>
        /// <param name="ToNorth">< Eastings (ToEast) and Northings (ToNorth) in meters, of point at other end of line to which (t-T) is being computed; _/param>
        /// <param name="a"> ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _</param>
        /// <param name="b"> ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _</param>
        /// <param name="E0"> ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _</param>
        /// <param name="N0"> ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _</param>
        /// <param name="f0"> central meridian scale factor (f0); _</param>
        /// <param name="PHI0"> latitude of central meridian (PHI0) in decimal degrees.</param>
        /// <returns></returns>
        private double E_N_to_t_minus_T(double AtEast, double AtNorth, double ToEast, double ToNorth, double a, double b, double E0, double N0, double f0, double PHI0)
        {
            double Pi = Math.PI;
            double RadPHI0 = PHI0 * (Pi / 180);
            //'Compute af0, bf0, e squared (e2), n and Nm (Northing of mid point)
            double af0 = a * f0;
            double bf0 = b * f0;
            double e2 = (Math.Pow(af0, 2) - Math.Pow(bf0, 2)) / Math.Pow(af0, 2);
            double N = (af0 - bf0) / (af0 + bf0);
            double Nm = (AtNorth + ToNorth) / 2;
            //'Compute initial value for latitude (PHI) in radians
            double PHId = InitialLat(Nm, N0, af0, RadPHI0, N, bf0);
            //'Compute nu, rho and eta2 using value for PHId
            double nu = af0 / (Math.Sqrt(1 - (e2 * Math.Pow((Math.Sin(PHId)), 2))));
            double rho = (nu * (1 - e2)) / (1 - Math.Pow(e2 * (Math.Sin(PHId)), 2));
            //'Compute (t-T)
            double XXIII = 1 / (6 * nu * rho);
            //    E_N_to_t_minus_T = (180 / Pi) * ((2 * (AtEast - E0)) + (ToEast - E0)) * (AtNorth - ToNorth) * XXIII
            return (180 / Pi) * ((2 * (AtEast - E0)) + (ToEast - E0)) * (AtNorth - ToNorth) * XXIII;
        }

    }
}
