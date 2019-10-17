
# region C# Libraries
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
# endregion


namespace OptimizationMathCode
{
    public partial class OptimizationForm : Form
    {

        #region Variables
        double initialYValue = 390;
        double BaseVal = 0.1;
        double height, slantheight, volume;
        int SqrP1x, SqrP1y, SqrP2x, SqrP2y, SqrP3x, SqrP3y, SqrP4x, SqrP4y; // 4-positions of square 
        int SqrPMidx, SqrPMidy, TriP2Midx, TriP2Midy; //midpoint of square and tip of pyramid
        double SqrPMidDistance; // used in calculating SqrPMidx and SqrPMidy
        double surfacearea =1000; //initial surface area 
        double theta = Math.PI / 6; // used to make drawing 3D

        double maxValueBaseBar; //adjusts the values of the base trackbar according to the surface area 
        double minValueBaseBar;

        int MinYValue; //adjusts the values of the graph according to the surface area affecting Max/Min Volume
        int MaxYValue;
        double OptimalBase; //Point at where maximum volume occurs

        double BaseTickValue; //used to determine what each tick should reprepsent in terms of a change in base value
        double tempHeight; //temperoary values used when populating graph/table 
        double tempVolume;


        Pen blackPen = new Pen(Color.Black, 3);
        Pen redPen = new Pen(Color.Red, 2);
        Pen greenPen = new Pen(Color.Green, 4);

   

        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        #endregion

        public OptimizationForm() //Constructor
        {
            SetValuesOfBaseTrackBar((Math.Sqrt(1000 / 2))); //sets initial values for base trackbar for SA=1000 (initial SA)
            InitializeComponent();
            GraphSplineInitialize(); //graphs the initial function for a SA =1000(initial)

        }

     
        #region Calculate_Points 
        // Calculate_Points uses the surface area and the basevalue to calculate all the points 
        // for the primary pyramid drawing
        private void Calculate_Points(double BaseVal)
        {
            SqrP1x = 0;
            SqrP1y = (int)initialYValue;

            SqrP2x = (int)BaseVal;
            SqrP2y = (int)initialYValue;

            SqrP3x = (int)(SqrP2x + Math.Ceiling(Math.Cos(theta) * BaseVal));
            SqrP3y = (int)(initialYValue - Math.Ceiling(Math.Sin(theta) * BaseVal));

            SqrP4x = 0 + (int)Math.Ceiling(Math.Cos(Math.PI / 6) * BaseVal);
            SqrP4y = (int)(initialYValue - Math.Ceiling(Math.Sin(theta) * BaseVal));

            SqrPMidDistance = (int)Math.Floor(((Math.Sqrt((Math.Pow((SqrP3x - SqrP1x), 2) + Math.Pow((SqrP3y - SqrP1y), 2)))) / 2));
            SqrPMidx = (int)(Math.Cos(theta / 2) * SqrPMidDistance);
            SqrPMidy = (int)Math.Floor((initialYValue - (Math.Sin(theta / 2) * SqrPMidDistance)));
            TriP2Midx = SqrPMidx;
            TriP2Midy = (int)(SqrPMidy - height);
        }
        #endregion

        #region TrackBar Code
        private void SATrackBarValueChanged(object sender, EventArgs e)
        {
            pyramidPictureBox.Invalidate(); //clear the picture box
            chart1.Series["Volume"].Points.Clear(); //clears old dots on the function
            surfacearea = SATrackBar.Value;
            SALabelOutput.Text = surfacearea.ToString();
            SurfaceAreaOutputMain.Text = surfacearea.ToString() + " un^2";
            maxValueBaseBar = Calculations.VolumeLastZero(surfacearea);
            SetValuesOfBaseTrackBar(maxValueBaseBar);
            PopulateGraphAndTable();
            HideOptimizedValues();

        }

        private void BaseTrackBar_ValueChanged(object sender, EventArgs e)
        {
            pyramidPictureBox.Invalidate(); //clear the picture box
            chart1.Series["Volume"].Points.Clear(); //clears old dots on the function
  
            BaseVal = (BaseTrackBar.Value) * BaseTickValue;

          
            height = Calculations.CalculateHeight(BaseVal, surfacearea);
            volume = Calculations.Volume(BaseVal, height);
            slantheight = Calculations.SlantHeight(BaseVal, height);

            BaseLabelOutput.Text = (BaseVal).ToString();
            VolumeOutput.Text = (volume.ToString());
            BaseOutput.Text = (BaseVal.ToString());
            HeightOutput.Text = (height.ToString());
            slantHeightOutput.Text = (slantheight.ToString());


            Calculate_Points(BaseVal);
            DrawingPyramid();
            AddingDotGraph(BaseVal, height);
            DrawSquareShape(BaseVal);
            DrawTriangleShape(BaseVal, slantheight);

        }
        
        private void SetValuesOfBaseTrackBar(double lastzero)
        {
            if (lastzero > 140)
            {
                maxValueBaseBar = Math.Floor(lastzero);
            }
            else
            {
                maxValueBaseBar = lastzero - 0.001 * lastzero; //to avoid the value actually be reached, causing negative distances which leads 
            }                                               // to the drawing crashing when trying to render(not in range)
            BaseTickValue = maxValueBaseBar / 140;
            minValueBaseBar = 1;
        }
        //sets the values of the Base trackbar according to the surface area given 


        #endregion

        #region Drawing Code 
        // The code below is to draw the shapes of the pyramid, square net, and triangle net 
        //  using previously calculated values.
        public void DrawingPyramid()
        {
            Bitmap pyramidDrawing = new Bitmap(this.pyramidPictureBox.Width, this.pyramidPictureBox.Height);
            using (Graphics g = Graphics.FromImage(pyramidDrawing))
            {
                g.DrawLine(blackPen, SqrP1x, SqrP1y, SqrP2x, SqrP2y);
                g.DrawLine(redPen, SqrP1x, SqrP1y, SqrP4x, SqrP4y);
                g.DrawLine(blackPen, SqrP2x, SqrP2y, SqrP3x, SqrP3y);
                g.DrawLine(redPen, SqrP4x, SqrP4y, SqrP3x, SqrP3y);

                // slant lines
                g.DrawLine(greenPen, SqrP1x, SqrP1y, TriP2Midx, TriP2Midy);
                g.DrawLine(greenPen, SqrP2x, SqrP2y, TriP2Midx, TriP2Midy);
                g.DrawLine(greenPen, SqrP3x, SqrP3y, TriP2Midx, TriP2Midy);
                g.DrawLine(redPen, SqrP4x, SqrP4y, TriP2Midx, TriP2Midy);
            }
            pyramidPictureBox.Image = pyramidDrawing;
        }
        public void DrawSquareShape(double BaseValue)
        {
            Rectangle SquareNet = new Rectangle(0, 0, (int)BaseValue, (int)BaseValue);

            Bitmap netshapesquare = new Bitmap(this.pictureBox2.Width, this.pictureBox2.Height);
            using (Graphics g = Graphics.FromImage(netshapesquare))
            {
                Pen bluePen = new Pen(Color.Blue, 3);
                g.DrawRectangle(greenPen, SquareNet);
                g.FillRectangle(blueBrush, SquareNet);

            }
            pictureBox2.Image = netshapesquare;
        }
        public void DrawTriangleShape(double BaseValue, double HeightValue)
        {
            Point pTri1 = new Point((int)BaseValue / 2, 0);
            Point pTri2 = new Point(0, (int)HeightValue);
            Point pTri3 = new Point((int)(BaseValue), (int)HeightValue);

            Bitmap netshapetriangle = new Bitmap(this.pictureBox3.Width, this.pictureBox3.Height);
            using (Graphics h = Graphics.FromImage(netshapetriangle))
            {
                Pen bluePen = new Pen(Color.Blue, 3);

                h.DrawLine(bluePen, pTri1, pTri2);
                h.DrawLine(bluePen, pTri1, pTri3);
                h.DrawLine(bluePen, pTri3, pTri2);
            }
            this.pictureBox3.Image = netshapetriangle;
        }
        #endregion

        #region Graphing Code 
        // the functions below are to graph the point on the graph along with the curve, 
        // these are two seperact graphs functions that are graphed onto the same chart
        private void AddingDotGraph(double baseXValue, double heightValue)
        {
            var chart = chart1.ChartAreas[0];
            chart.AxisX.IntervalType = DateTimeIntervalType.Number;
            chart.AxisX.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = false;

            chart.AxisX.Minimum = 0; //1
            chart.AxisX.Maximum = maxValueBaseBar; //140

            OptimalBase = Calculations.OptimalBase(surfacearea);
            MinYValue = (int)Math.Floor(Calculations.Volume(minValueBaseBar, Calculations.CalculateHeight(minValueBaseBar, surfacearea)));
            MaxYValue = (int)Math.Floor(Calculations.Volume(OptimalBase, Calculations.CalculateHeight(OptimalBase, surfacearea)) * 1.1);
            chart.AxisY.Minimum = MinYValue;
            chart.AxisY.Maximum = MaxYValue;

            chart.AxisX.Interval = maxValueBaseBar / 28; //28 intervals
            chart.AxisY.Interval = (MaxYValue-MinYValue) / 10; //10 intervals

            int VolumePoint = (int)Math.Ceiling(Calculations.Volume(BaseVal, height));
            chart1.Series["Volume"].Points.AddXY(BaseVal, VolumePoint);

        }
        public void GraphSplineInitialize()
        {
            chart1.Series.Add("Volume - un^3");
            chart1.Series["Volume - un^3"].ChartType = SeriesChartType.Spline;
            chart1.Series["Volume - un^3"].Color = Color.Red;
            chart1.Series[0].IsVisibleInLegend = true;
        }
        private void PopulateGraphAndTable()
        {
            chart1.Series["Volume - un^3"].Points.Clear(); //clears old dot
            var chart = chart1.ChartAreas[0];
            chart.AxisX.IntervalType = DateTimeIntervalType.Number;
            chart.AxisX.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = true;

            OptimalBase = Calculations.OptimalBase(surfacearea);
            chart.AxisX.Minimum = minValueBaseBar; //1
            chart.AxisX.Maximum = maxValueBaseBar; //140
            MinYValue = (int)Math.Floor(Calculations.Volume(minValueBaseBar, Calculations.CalculateHeight(minValueBaseBar, surfacearea)));
            MaxYValue = (int)Math.Floor(Calculations.Volume(OptimalBase, Calculations.CalculateHeight(OptimalBase, surfacearea)) * 1.1);

            chart.AxisY.Minimum = MinYValue;
            chart.AxisY.Maximum = MaxYValue;

            chart.AxisX.Interval = maxValueBaseBar / 28; //28 intervals
            chart.AxisY.Interval = (MaxYValue - MinYValue) / 10; //10 intervals

            int counter = 0;
            dataGridView1.Rows.Clear();

            for (double i = minValueBaseBar; i < maxValueBaseBar; i += BaseTickValue)
            {
                tempHeight = Calculations.CalculateHeight(i, surfacearea);
                tempVolume = Calculations.Volume(i, tempHeight);
                chart1.Series["Volume - un^3"].Points.AddXY(i, tempVolume);

                dataGridView1.Rows.Add();
                dataGridView1.Rows[counter].Cells["Col1"].Value = i;
                dataGridView1.Rows[counter].Cells["Col2"].Value = tempHeight;
                dataGridView1.Rows[counter].Cells["Col3"].Value = tempVolume;
                counter++;
            }
        }

        #endregion

        #region FullScreen+Escape 
        // onclick events are to bring the form in and out of fullscreen
        // very useful as the app needs a full screen
        private void FullSceenClick(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
        }

        private void KeyDownForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                TopMost = false;
            }
        }

        #endregion

        #region Show/Hide Optimized Values
        // depending on a change in SA trackbar value or a click, hides or hows the optimized values for volume
        private void ShowOptimizedValues(object sender, EventArgs e)
        {
            OptimizedBaseOutput.Visible = true;
            OptimizedHeightOutput.Visible = true;
            OptimizedVolumeOutput.Visible = true;

            OptimizedBaseOutput.Text = (Calculations.OptimalBase(surfacearea)).ToString();
            OptimizedHeightOutput.Text = Calculations.CalculateHeight(Calculations.OptimalBase(surfacearea), surfacearea).ToString();
            OptimizedVolumeOutput.Text = Calculations.Volume(Calculations.OptimalBase(surfacearea),
                                       Calculations.CalculateHeight(Calculations.OptimalBase(surfacearea), surfacearea)).ToString();
        }

        private void HideOptimizedValues()
        {
            if (OptimizedBaseOutput.Visible = true)
            {
                OptimizedBaseOutput.Visible = false;
                OptimizedHeightOutput.Visible = false;
                OptimizedVolumeOutput.Visible = false;
            }
        }
        #endregion 

    }
}
