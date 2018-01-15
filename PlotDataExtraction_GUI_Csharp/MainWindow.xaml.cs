using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;




namespace PlotDataExtraction_GUI_Csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private class CoordList
        {
            public double x
            {
                get;
                set;
            }

            public double y
            {
                get;
                set;
            }

            public Ellipse myellipse // myellipse might enable us to select an ellipse later to place it
            {
                get;
                set;
            }

        }

        List<CoordList> xylist_raw = new List<CoordList>();
        List<CoordList> xylist = new List<CoordList>();

        //Global Image variable -- snip image will be saved to this variable
        public static class GlobalImageVar
        {
            private static System.Drawing.Image _globalbmp;
            
            public static System.Drawing.Image GlobalBMP
            {
                get
                {
                    return _globalbmp;
                }
                set
                {
                    _globalbmp = value;
                }
            }
        }

        private double x1_xr;
        private double x1_yr;
        private double x2_xr;
        private double x2_yr;
        private double y1_xr;
        private double y1_yr;
        private double y2_xr;
        private double y2_yr;

        private double x1;
        private double x2;
        private double y1;
        private double y2;

        void ClickityClick(object sender, MouseEventArgs e)
        {
            this.ResizeMode = ResizeMode.NoResize; //prevent users from resizing window after placing ellipses (since they don't scale with window resizing)
            Point pnt = e.GetPosition(Canvas1);
            double dia = 4;

            double xvalue = 0; //graph x-value
            double yvalue = 0; //graph y-value

            Ellipse myEllipse_def = new Ellipse { Width = dia, Height = dia, Fill = Brushes.Red }; // definition point

            RadioButton rbx1 = radioButtonx1;
            RadioButton rbx2 = radioButtonx2;
            RadioButton rby1 = radioButtony1;
            RadioButton rby2 = radioButtony2;
            RadioButton rb_p = rb_pickpoints;

            if (rbx1.IsChecked == true)
            {
                x1_xr = pnt.X;
                x1_yr = pnt.Y;
                Canvas1.Children.Add(myEllipse_def);
                Canvas.SetLeft(myEllipse_def, pnt.X - dia / 2);
                Canvas.SetTop(myEllipse_def, pnt.Y - dia / 2);
                rbx2.IsChecked = true;
            }

            else if (rbx2.IsChecked == true)
            {
                x2_xr = pnt.X;
                x2_yr = pnt.Y;
                Canvas1.Children.Add(myEllipse_def);
                Canvas.SetLeft(myEllipse_def, pnt.X - dia / 2);
                Canvas.SetTop(myEllipse_def, pnt.Y - dia / 2);
                rby1.IsChecked = true;
            }
            else if (rby1.IsChecked == true)
            {
                y1_xr = pnt.X;
                y1_yr = pnt.Y;
                Canvas1.Children.Add(myEllipse_def);
                Canvas.SetLeft(myEllipse_def, pnt.X - dia / 2);
                Canvas.SetTop(myEllipse_def, pnt.Y - dia / 2);
                rby2.IsChecked = true;
            }
            else if (rby2.IsChecked == true)
            {
                y2_xr = pnt.X;
                y2_yr = pnt.Y;
                Canvas1.Children.Add(myEllipse_def);
                Canvas.SetLeft(myEllipse_def, pnt.X - dia / 2);
                Canvas.SetTop(myEllipse_def, pnt.Y - dia / 2);
                rb_p.IsChecked = true;
            }

            else if (rb_p.IsChecked == true)
            {
                Ellipse myEllipse = new Ellipse { Width = dia, Height = dia, Fill = Brushes.Lime };
                Canvas1.Children.Add(myEllipse);
                Canvas.SetLeft(myEllipse, pnt.X - dia / 2);
                Canvas.SetTop(myEllipse, pnt.Y - dia / 2);
                

                double[] arr = new double[2];
                arr[0] = pnt.X;
                arr[1] = pnt.Y;

                xylist_raw.Add(new CoordList { x = pnt.X, y = pnt.Y, myellipse = myEllipse }); // hold on to raw points...might be useful later


                double[] xint_xy = new double[2]; //coordinates that intercept with the x axis (canvas coordinate system)
                double[] yint_xy = new double[2]; //coordinates that intercept with the y axis (canvas coordinate system)
                xint_xy = FunctionClass.axis_intercept(x1_xr,x1_yr,x2_xr,x2_yr,arr[0],arr[1]);
                yint_xy = FunctionClass.axis_intercept(y1_xr, y1_yr, y2_xr, y2_yr, arr[0], arr[1]);


                if (xlog == 1)
                {
                    double[] xy1_canvas_x = { x1_xr, x1_yr };
                    double[] xy2_canvas_x = { x2_xr, x2_yr };
                    double[] xyp_canvas_x = xint_xy;
                    xvalue = FunctionClass.logcoord_real_x(x1, x2, xy1_canvas_x, xy2_canvas_x, xyp_canvas_x);
                }
                else if (xlog == 0)
                {
                    xvalue = FunctionClass.lincoord_real(x1, x2, x1_xr, x2_xr, xint_xy[0]); // returns graph x coordinate value
                }


                if (ylog == 1)
                {
                    double[] xy1_canvas_y = { y1_xr, y1_yr };
                    double[] xy2_canvas_y = { y2_xr, y2_yr };
                    double[] xyp_canvas_y = yint_xy;
                    yvalue = FunctionClass.logcoord_real_y(y1, y2, xy1_canvas_y, xy2_canvas_y, xyp_canvas_y);
                }
                else if (ylog == 0)
                {
                    yvalue = FunctionClass.lincoord_real(y1, y2, y1_yr, y2_yr, yint_xy[1]); // returns graph y coordinate value
                }

                xylist.Add(new CoordList { x = xvalue, y = yvalue, myellipse = myEllipse }); // myellipse might enable us to select an ellipse later
                CollectionViewSource itemCollectionViewSource = (CollectionViewSource)FindResource("ItemCollectionViewSource");
                itemCollectionViewSource.Source = xylist;
                dataGrid1.Items.Refresh();
            }

        }


        private double _factor = 0.3;

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // When mouse over canvas, show image in magnifier window
            Point center = e.GetPosition(Canvas1);
            double wdth = MagnifierRectangle.ActualWidth * _factor;
            double hght = MagnifierRectangle.ActualHeight * _factor;

            double offset = 49; //used to center the viewbox correctly...no clue why 49 works.

            Rect viewboxRect = new Rect(center.X - wdth / 2, center.Y - hght / 2 + offset, wdth, hght);
            MagnifierBrush.Viewbox = viewboxRect;
        }

        
        private void LaunchSnipTool(object sender, System.EventArgs e)
        {
            this.ResizeMode = ResizeMode.CanResize;
            Canvas1.Children.Remove<Ellipse>();
            this.Hide();
            System.Threading.Thread.Sleep(0200); // give the form a second to hide before launching snipping tool
            
            //SnippingTool Output
            System.Drawing.Image bmp = SnippingTool.Snip();

            //Convert Image for use in WPF canvas
            Image image = new Image();
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            GlobalImageVar.GlobalBMP = BitmapImage2Bitmap(bi);
            ResizeImage(sender, e);
            Canvas1.Background = new SolidColorBrush(Colors.White);
        }

        private System.Drawing.Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }

        private void ResizeImage(object sender, EventArgs e)
        {
            
            if (GlobalImageVar.GlobalBMP != null)
            {
                double wdth = Canvas1.ActualWidth;
                double hght = Canvas1.ActualHeight;
                double picwidth = GlobalImageVar.GlobalBMP.Width;
                double picheight = GlobalImageVar.GlobalBMP.Height;

                double borderwidth = 0; //leaves some canvas visible on all sides
                double ratio = Math.Min((wdth - borderwidth) / picwidth, (hght - borderwidth) / picheight);

                double new_width = picwidth * ratio;
                double new_height = picheight * ratio;

                var newimage = new System.Drawing.Bitmap((int)wdth, (int)hght);

                double shift_hor = (wdth - new_width) / 2;
                double shift_vert = (hght - new_height) / 2;

                using (var graphics = System.Drawing.Graphics.FromImage(newimage))
                    graphics.DrawImage(GlobalImageVar.GlobalBMP, (int)shift_hor, (int)shift_vert, (int)new_width, (int)new_height);

                Image image = new Image();
                MemoryStream ms = new MemoryStream();
                newimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                ImgCanvas.Source = bi;


                this.Show();
            }
            else
            {
                return;
            }
        }

        private void textBoxx1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_x1.Text))
            {
                //System.Windows.MessageBox.Show("Empty");
                return;
            }
            else
            {
                x1 = Convert.ToDouble(textBox_x1.Text);
            }
        }

        private void textBoxx2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_x2.Text))
            {
                //System.Windows.MessageBox.Show("Empty");
                return;
            }
            else
            {
                x2 = Convert.ToDouble(textBox_x2.Text);
            }
        }

        private void textBoxy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_y1.Text))
            {
                //System.Windows.MessageBox.Show("Empty");
                return;
            }
            else
            {
                y1 = Convert.ToDouble(textBox_y1.Text);
            }
        }

        private void textBoxy2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_y2.Text))
            {
                //System.Windows.MessageBox.Show("Empty");
                return;
            }
            else
            {
                y2 = Convert.ToDouble(textBox_y2.Text);
            }
        }


        private int xlog=0;
        private int ylog=0;

       private void x_log_check(object sender, RoutedEventArgs e)
        {
            xlog = 1;
        }

        private void x_log_uncheck(object sender, RoutedEventArgs e)
        {
            xlog = 0;
        }


        private void y_log_check(object sender, RoutedEventArgs e)
        {
            ylog = 1;
        }

        private void y_log_uncheck(object sender, RoutedEventArgs e)
        {
            ylog = 0;
        }


        private void clearPoints(object sender, RoutedEventArgs e)
        {
            Canvas1.Children.Remove<Ellipse>();
            optionsExpander.IsExpanded = false;

            xylist.Clear();
            CollectionViewSource itemCollectionViewSource = (CollectionViewSource)FindResource("ItemCollectionViewSource");
            itemCollectionViewSource.Source = xylist;
            dataGrid1.Items.Refresh();

            this.ResizeMode = ResizeMode.CanResize;
        }

        
    }

}
