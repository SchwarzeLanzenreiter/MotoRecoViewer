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
    public struct TVData   //tvは、time と　value
    {
        public double DataTime;
        public double DataValue;
    }

    class ChData
    {
        public string ChName { get; set; }
        public int ChMin { get; set; }
        public int ChMax { get; set; }
        public int ChColor { get; set; }
        public bool ChPreview { get; set; }
        public bool ChShow { get; set; }
        public List<TVData> LogData { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="chName">データ表示名</param>
        /// <param name="chMin">チャート上最小値</param>
        /// <param name="chMax">チャート上最大値</param>
        /// <param name="chColor">チャートカラー</param>
        /// <param name="chPreview">プレビュー表示するかどうか</param>
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
        /// データを追加する
        /// </summary>
        /// <param name="time">タイムスタンプ</param>
        /// <param name="value">データ値</param>
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
        /// データクリア
        /// </summary>
        public void Clear()
        {
            this.LogData.Clear();
        }

        /// <summary>
        /// DecodeRuleの要素数を返す
        /// </summary>
        public int Count
        {
            get
            {
                return this.LogData.Count;
            }
        }

        /// <summary>
        /// 引数のtimeに対応した左側インデックスを返す
        /// 左側とは、時間が若い、という意味になる
        /// </summary>
        /// <param name="time">検索するtime</param>
        /// <returns>要素の位置（見つからなかった場合は配列長）</returns>
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
        /// 引数のtimeに対応した左側インデックスを返す
        /// 左側とは、時間が若い、という意味になる
        /// </summary>
        /// <param name="time">検索するtime</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="end">検索終了インデックス</param>
        /// <returns>要素の位置（見つからなかった場合は配列長）</returns>
        public int FindLeftIndex(double time, int start, int end)
        {
            int first = start;
            int last = end;

            if (last > this.LogData.Count - 1) { last = this.LogData.Count - 1; }

            int mid;
            do
            {
                mid = first + (last - first) / 2;
                if (time > this.LogData[mid].DataTime)
                    first = mid + 1;
                else
                    last = mid - 1;
                if (this.LogData[mid].DataTime == time)
                    return mid;
            } while (first <= last);
            return mid;
        }

        /// <summary>
        /// LogDataのDateTimeをKeyにDataValueをソートする
        /// </summary>
        public void Sort()
        {
            this.LogData.Sort((a, b) => (int)(a.DataTime*1000 - b.DataTime*1000));   // msに変換してから引き算する。MotoRecoではもともとタイムスタンプは1msが分解能
        }
    }
}
