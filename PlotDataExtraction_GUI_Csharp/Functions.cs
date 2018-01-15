using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDataExtraction_GUI_Csharp
{
    class FunctionClass
    {
        static public double[] axis_intercept(double p0_x, double p0_y, double p1_x, double p1_y, double xp, double yp)
        {
            
            if (p1_y == p0_y)
            {
                p1_y = p1_y + 0.000000001;
            }
            double A = p1_x - p0_x;
            double B = p1_y - p0_y;
            double C = p0_x - A / B * p0_y;

            double ax_int_y = (A * B * xp + B * B *yp - A * B * C) / (A * A + B * B);
            double ax_int_x = A * ax_int_y / B + C;

            double[] ax_int = new double[] { ax_int_x, ax_int_y };

            return ax_int;

        }

        static public double lincoord_real(double real_0, double real_1, double raw_0, double raw_1, double point_raw)
        {
            // linear transform point on axis from raw canvas coordinates to graph coordinates (real) ... works for both x and y axis
            if (raw_1 == raw_0)
            {
                raw_1 = raw_1 + 0.000000001;
            }

            double R = (point_raw - raw_0) / (raw_1 - raw_0);
            double ans = R * (real_1 - real_0) + real_0;

            return ans;
        }

        static public double logcoord_real_x(double real_0, double real_1, double[] xy0_canvas, double[] xy1_canvas, double[] xyp_canvas)
        {
            double del1 = Math.Sqrt(Math.Pow(xy0_canvas[0] - xy1_canvas[0], 2) + Math.Pow(xy0_canvas[1] - xy1_canvas[1], 2));
            double delp = Math.Sqrt(Math.Pow(xy0_canvas[0] - xyp_canvas[0], 2) + Math.Pow(xy0_canvas[1] - xyp_canvas[1], 2));

            if(xyp_canvas[0] - xy0_canvas[0] < 0)
            {
                delp = delp * (xyp_canvas[0] - xy0_canvas[0]) / Math.Abs(xyp_canvas[0] - xy0_canvas[0]);
            }

            double A = del1 / (Math.Log(real_1) - Math.Log(real_0));
            double B = -A * Math.Log(real_0);

            double yp = Math.Exp((delp - B) / A);

            return yp;
        }

        static public double logcoord_real_y(double real_0, double real_1, double[] xy0_canvas, double[] xy1_canvas, double[] xyp_canvas)
        {
            double del1 = Math.Sqrt(Math.Pow(xy0_canvas[0] - xy1_canvas[0], 2) + Math.Pow(xy0_canvas[1] - xy1_canvas[1], 2));
            double delp = Math.Sqrt(Math.Pow(xy0_canvas[0] - xyp_canvas[0], 2) + Math.Pow(xy0_canvas[1] - xyp_canvas[1], 2));
            
            if(xy0_canvas[1] - xyp_canvas[1] < 0)
            {
                delp = delp * (xy0_canvas[1] - xyp_canvas[1]) / Math.Abs(xy0_canvas[1] - xyp_canvas[1]); //reversed from logcoord_real_x because positive y is down in canvas coordinates
            }
            

            double A = del1 / (Math.Log(real_1) - Math.Log(real_0));
            double B = -A * Math.Log(real_0);

            double yp = Math.Exp((delp - B) / A);

            return yp;
        }

    }
}
