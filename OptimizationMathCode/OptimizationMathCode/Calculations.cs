using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMathCode
{
    class Calculations
    {


        #region Calculate Volume
        //CalculateHeight: double double -> double 
        // Calculates the volume of a square based pyramid based on the height of the pyramid
        //  and the side length of the sqare base.
        public static double Volume (double BaseVal, double height)
        {
            return ((BaseVal * BaseVal * height)/3); //volume of a sphere 
        }
        #endregion 

        #region Calculate Height(unrounded)
        //CalculateHeight: double double -> Double 
        // Calculate height takes in information about the the side length of the square and 
        //   surface area of the square based pyramid to calculate the height of the pyramid
        public static double CalculateHeight (double BaseVal, double surfacearea)
        {
           return 
               (Math.Sqrt((Math.Pow(surfacearea, 2) - 2 * surfacearea * Math.Pow(BaseVal, 2))
               / (4 * Math.Pow(BaseVal, 2))));
        }
        #endregion

        #region Calculate SlantHeight
        //CalculateSlantHeight: double double -> double 
        // Calculates the slanted height a triangle used in the square based pyramid (not the pyramids height)
        //   using the height of the pyramid and base side value
        public static double SlantHeight(double BaseVal, double height)
        {
            return Math.Sqrt((Math.Pow(height,2) + (Math.Pow(BaseVal,2))));

        }
        #endregion

        #region Calculate Optimal Base 
        //CalculateSlantHeight: double -> double 
        // Calculates the optimal base length of a square based pyramid given its surface area, 
        //  to yield the maximum volume: used in determining basebar range in questions 
        public static double OptimalBase(double SurfaceArea)
        {
            return (Math.Sqrt(SurfaceArea)/2);
        }
        #endregion

        #region Calculate f(Volume) Last Zero 
        //LastZero: double -> double 
        // Calculates the last zero of the volume function, any value beyond this one will result in a negative 
        //  volume which does not exist
        public static double VolumeLastZero(double SurfaceArea)
        {
            return (Math.Sqrt(SurfaceArea /2));
        }
        #endregion

        #region Round2decimalPlaces
        //LastZero: double -> double 
        // Rounds the number inputted to 2 decimal places, returns it
        public static double Round2DcimalPlaces(double unrounded)
        {
            return 
                (Math.Floor(unrounded*Math.Pow(10,2)))/ Math.Pow(10, 2);
        }
        #endregion



    }

}

// use optimization to calculate the optimal base and show the graph of the funtion 
// from -80 to +40 from that value, 