using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDSControl
{
    public class MatchResult
    {
        public Rectangle _clsRectangle;
        public double _dScore = 0.0f;
    }

    public class PatternMatch
    {
        public static MatchResult Match(Mat clsSource,Mat clsPattern, TemplateMatchModes eTemplateMatchModes = TemplateMatchModes.CCoeffNormed)
        {
            double dMinScore, dMaxScore;
            OpenCvSharp.Point sMinPoint, sMaxPoint;
            Mat clsResult = new Mat();

            if (clsSource == null) return null;
            if (clsPattern == null) return null;
            if (clsSource.Type() != clsPattern.Type()) return null;

            Cv2.MatchTemplate(clsSource, clsPattern, clsResult, eTemplateMatchModes);
            Cv2.MinMaxLoc(clsResult, out dMinScore, out dMaxScore, out sMinPoint, out sMaxPoint);

            MatchResult clsMatchResult = new MatchResult();
            clsMatchResult._clsRectangle = new Rectangle(sMaxPoint.X, sMaxPoint.Y, clsPattern.Width, clsPattern.Height);
            clsMatchResult._dScore = dMaxScore;

            return clsMatchResult;
        }
    }
}
