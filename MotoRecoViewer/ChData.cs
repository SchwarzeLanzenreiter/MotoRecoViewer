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
    }

    class ChData
    {
        public string ChName { get; set; }
        public int ChMin { get; set; }
        public int ChMax { get; set; }
        public int ChColor { get; set; }
        public bool ChPreview { get; set; }
        public bool ChShow { get; set; }
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
        public ChData(string chName, int chMin, int chMax, int chColor, bool chPreview, bool chShow)
        {
            this.ChName = chName;
            this.ChMin = chMin;
            this.ChMax = chMax;
            this.ChColor = chColor;
            this.ChPreview = chPreview;
            this.ChShow = chShow;
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
                DataValue = value
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
        /// Sorting DataValue by LogData's DateTime as a key
        /// </summary>
        public void Sort()
        {
            this.LogData.Sort((a, b) => (int)(a.DataTime*1000 - b.DataTime*1000));   //Since we want to be able to compare them in int type, we need to convert them to ms and then subtract them.

        }
    }
}
