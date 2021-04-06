// MIT License
// 
// Copyright (c) 2019 Schwarze Lanzenreiter
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoRecoViewer
{
    public struct TVData   //tv means Time and Value
    {
        public double DataTime { get; set; }
        public double DataValue { get; set; }
        public double DataValueFiltered { get; set; }
    }

    class ChData
    {
        public string ChName { get; set; }
        public int ChMin { get; set; }
        public int ChMax { get; set; }
        public int ChColor { get; set; }
        public bool ChPreview { get; set; }
        public bool ChShow { get; set; }
        public bool ChFilter { get; set; }
        public List<TVData> LogData { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="chName">channel name</param>
        /// <param name="chMin">Minimum value on the chart</param>
        /// <param name="chMax">Maximum value on the chart</param>
        /// <param name="chColor">Chart color</param>
        /// <param name="chPreview">Subchart display flag</param>
        /// <param name="chShow">Mainchart display flag</param>
        public ChData(string chName, int chMin, int chMax, int chColor, bool chPreview, bool chShow, bool chUseFilter)
        {
            this.ChName = chName;
            this.ChMin = chMin;
            this.ChMax = chMax;
            this.ChColor = chColor;
            this.ChPreview = chPreview;
            this.ChShow = chShow;
            this.ChFilter = chUseFilter;
            this.LogData = new List<TVData>();
        }

        /// <summary>
        /// Add new data
        /// </summary>
        /// <param name="time">Timestamp</param>
        /// <param name="value">Data value</param>
        public void AddData(double time, double value)
        {
            TVData data = new TVData
            {
                DataTime = time,
                DataValue = value,
                DataValueFiltered = 0.0
            };

            this.LogData.Add(data);
        }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Clear()
        {
            this.LogData.Clear();
        }

        /// <summary>
        /// Returns the number of elements in a Channel data.
        /// </summary>
        public int Count
        {
            get
            {
                return this.LogData.Count;
            }
        }

        /// <summary>
        /// Returns the left-hand index corresponding to the argument Time. The left side means "young time".
        /// </summary>
        /// <param name="time">Time to search</param>
        /// <returns>The time index of the argument.</returns>
        public int FindLeftIndex(double time)
        {
            int first = 0;
            int last;
            int mid;

            if (this.LogData.Count < 3) {
                return 0;
            } else
            {
                last = this.LogData.Count - 1;
            }

            do {
                mid = first + (last - first) / 2;

                if (mid == this.LogData.Count - 1)
                {
                    return mid;
                }

                if ((this.LogData[mid].DataTime <= time) && ( time < this.LogData[mid + 1].DataTime ))
                {
                    return mid;
                }


                if (time > this.LogData[mid].DataTime)
                {
                    first = mid + 1;
                }
                else
                {
                    last = mid - 1;
                }
                
            } while (first <= last);

            return mid;
        }

        /// <summary>
        /// Returns the left-hand index corresponding to the argument Time. The left side means "young time".
        /// </summary>
        /// <param name="time">Time to search</param>
        /// <param name="start">Binary search start index</param>
        /// <param name="end">Binary search end index</param>
        /// <returns>The time index of the argument.</returns>
        public int FindLeftIndex(double time, int start, int end)
        {
            if (this.LogData.Count < 3)
            {
                return 0;
            }

           int first,last,mid;

            first = start;
            last = end;

            do
            {
                mid = first + (last - first) / 2;

                if (mid == this.LogData.Count - 1)
                {
                    return mid;
                }

                if ((this.LogData[mid].DataTime <= time) && (time < this.LogData[mid + 1].DataTime))
                {
                    return mid;
                }


                if (time > this.LogData[mid].DataTime)
                {
                    first = mid + 1;
                }
                else
                {
                    last = mid - 1;
                }

            } while (first <= last);

            return mid;
        }

        /// <summary>
        /// Sorting DataValue by LogData's DateTime as a key
        /// </summary>
        public void Sort()
        {
            this.LogData.Sort((a, b) => (int)(a.DataTime*1000 - b.DataTime*1000));   //Since we want to be able to compare them in int type, we need to convert them to ms and then subtract them.

        }

        /// <summary>
        /// Filter raw data
        /// </summary>
        public void FilterData()
        {
            Double[] SrcData = new Double[LogData.Count];

            //Copy LogData.DataValue to ArrayOfData
            int i;
            for (i=0; i < LogData.Count; i++)
            {
                SrcData[i] = LogData[i].DataValue;
            }

            double[] filtered = Butterworth(SrcData, 0.01, 1);

            //Copy filtered value to LogData
            for (i = 0; i < LogData.Count; i++)
            {
                TVData data;
                data = LogData[i];
                data.DataValueFiltered = filtered[i];
                LogData[i] = data;
            }
        }

        /// <summary>
        /// LowPassFilter
        /// https://apps.dtic.mil/sti/pdfs/AD1060538.pdf
        /// </summary>
        public static double[] Butterworth(double[] indata, double deltaTimeinsec,double CutOff)
        {
            if (indata == null) return null;
            if (CutOff == 0) return indata;
            double Samplingrate = 1 / deltaTimeinsec;
            long dF2 = indata.Length - 1; // The data range is set with dF2
            double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
            double[] data = indata; // Ptr., changes passed data
            for (long r = 0; r < dF2; r++)
            {
                Dat2[2 + r] = indata[r];
            }
            Dat2[1] = Dat2[0] = indata[0];
            Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];
            const double pi = 3.14159265358979;
            double wc = Math.Tan(CutOff * pi / Samplingrate);
            double k1 = 1.414213562 * wc; // Sqrt(2) * wc
            double k2 = wc * wc;
            double a = k2 / (1 + k1 + k2);
            double b = 2 * a;
            double c = a;
            double k3 = b / k2;
            double d = -2 * a + k3;
            double e = 1 - (2 * a) - k3;
            // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
            double[] DatYt = new double[dF2 + 4];
            DatYt[1] = DatYt[0] = indata[0];
            for (long s = 2; s < dF2 + 2; s++)
            {
                DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                + d * DatYt[s - 1] + e * DatYt[s - 2];
            }
            DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];
            // FORWARD filter
            double[] DatZt = new double[dF2 + 2];
            DatZt[dF2] = DatYt[dF2 + 2];
            DatZt[dF2 + 1] = DatYt[dF2 + 3];
            for (long t = -dF2 + 1; t <= 0; t++)
            {
                DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                + d * DatZt[-t + 1] + e * DatZt[-t + 2];
            }
            // Calculated points are written
            for (long p = 0; p < dF2; p++)
            {
                data[p] = DatZt[p];
            }
            return data;
        }
    }
}
