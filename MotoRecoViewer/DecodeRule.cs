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
    class DecodeRule
    {
        private List<string> ChName;
        private List<ushort> Id;
        private List<string> Formula;
        private List<int> ChMin;
        private List<int> ChMax;
        private List<int> ChColor;
        private List<bool> ChPreview;
        private List<bool> ChShow;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecodeRule()
        {
            ChName = new List<string>();
            Id = new List<ushort>();
            Formula = new List<string>();
            ChMin = new List<int>();
            ChMax = new List<int>();
            ChColor = new List<int>();
            ChPreview = new List<bool>();
            ChShow = new List<bool>();
        }

        /// <summary>
        /// 引数の文字列式を、CANDataの数値に置き換えた文字列を返す
        /// </summary>
        /// <param name="Formula">計算式</param>
        /// <param name="Data">CANデータ</param>
        public string DecodeFormula(string Formula, FormMain.CanData data)
        {
            //計算式
            System.Text.StringBuilder exp = new System.Text.StringBuilder(Formula);

            //string exp = string.Copy(Formula);
            string str;

            byte nibble = (byte)(0x0F);

            //式中のLoData1～LoData8の文字列を、数値に置き換える
            for (int i = 0; i < 8; i++)
            {
                str = (nibble & data.data[i]).ToString();
                exp = exp.Replace("LoData" + (i + 1).ToString(), str);
            }

            //式中のHiData1～HiData8の文字列を、数値に置き換える
            for (int i = 0; i < 8; i++)
            {
                str = (nibble & data.data[i] >> 4).ToString();
                exp = exp.Replace("HiData" + (i + 1).ToString(), str);
            }

            //式中のData1～Data8の文字列を、数値に置き換える
            //Data1～Data8の置き換えを、LoDataやHiDataの前に実行すると、LoData1の中のData1のみ置換されてうまく動かない事に注意
            for (int i = 0; i < 8; i++)
            {
                str = data.data[i].ToString();
                exp = exp.Replace("Data" + (i + 1).ToString(), str);
            }

            return exp.ToString();
        }

        /// <summary>
        /// 引数の文字列式を、固定計算式のインデックスに変換する
        /// ユーザー定義のデータとCanDataはインデックスが1異なる事に注意
        /// </summary>
        /// <param name="Formula">計算式</param>
        /// <param name="Data">CANデータ</param>
        public double FixedFormula(string Formula, FormMain.CanData data)
        {
            // nibble計算用
            byte nibble = (byte)(0x0F);

            // 車速
            double frSpeed;
            double rrSpeed;

            // Formulaに対応した計算を以下列挙する
            switch (Formula)
            {
                // CANID 10C
                // K51 Engine RPM
                // (LoData4*255+Data3)*4
                case "#K51_RPM":
                    return ((nibble & data.data[3]) * 255d + data.data[2]) * 5d;
     
                // CANID 10C
                // K51 Throttel Valve Position
                // Data6/255*100
                case "#K51_ThrottelValvePosition":
                    return data.data[5] / 255d * 100d;

                // CANID 10C
                // K51 Throttel Position
                // Data8/255*100
                case "#K51_ThrottelPosition":
                    return data.data[7] / 255d * 100d;

                // CANID 110
                // K51 Throttel Grip Position
                // Data6/255*100
                case "#K51_ThrottelGripPosition":
                    return data.data[5] / 255d * 100d;

                // CANID 120
                // K51 FrBrake1
                // Data3
                case "#K51_FrBrake1":     
                    return data.data[2];

                // CANID 120
                // K51 FrBrake2
                // Data5
                case "#K51_FrBrake2":
                    return data.data[4];

                // CANID 120
                // K51 RrBrake1
                // Data4
                case "#K51_RrBrake1":
                    return data.data[3];

                // CANID 120
                // K51 RrBrake2
                // Data6
                case "#K51_RrBrake2":
                    return data.data[5];

                // CANID 29C
                // K51 FrStroke
                // Data6+LoData7*255
                case "#K51_FrStroke":
                    return data.data[5] ;

                // CANID 29C
                // K51 RrStroke
                // Data8*4
                case "#K51_RrStroke":
                    return (data.data[7] + data.data[6]/256d) / 2d * 10d;

                // CANID 10C
                // K51 LeanAngle
                // (Data5*90/128-90) * -1
                // -1を掛けるのは、画面上右側を進行方向と考えて、右コーナーなら画面下側に、左コーナーなら画面上側にリーンアングルを表記したいので
                case "#K51_LeanAngle":
                    return (((double)data.data[4] * 90d / 128d) - 90d) * (-1d);

                // CANID 293
                // K51 FrSpeed1
                // (Data5-208)*32+(Data4/8)
                case "#K51_FrSpeed1":
                    return (data.data[4] - 208d)*32d + (data.data[3]/8d);

                // CANID 293
                // K51 FrSpeed2
                // (Data3*2)+(Data2/128)
                case "#K51_FrSpeed2":
                    return (data.data[2] * 2d) + (data.data[1] / 128d);

                // CANID 293
                // K51 RrSpeed
                // (Data5-208)*32+(Data1/8)
                case "#K51_RrSpeed":
                    // RrSpeedは、Data5は本来Fr用の為、車速差がある時に繰り上がりタイミングが異なってしまうため、
                    // 便宜的にFrSpeedと車速差が16km/h以下になるよう調整してしまう
                    frSpeed = (data.data[4] - 208d) * 32d + (data.data[3] / 8d);
                    rrSpeed = (data.data[4] - 208d) * 32d + (data.data[0] / 8d);

                    if (rrSpeed > frSpeed + 16d)
                    {
                        rrSpeed = rrSpeed - 32d;
                    }

                    if (rrSpeed < frSpeed - 16d)
                    {
                        rrSpeed = rrSpeed + 32d;
                    }

                    return rrSpeed;

                // CANID 293
                // K51 SlipRate
                // rrSpeed/frSpeed*100-100
                case "#K51_SlipRate":
                    // RrSpeedは、Data5は本来Fr用の為、車速差がある時に繰り上がりタイミングが異なってしまうため、
                    // 便宜的にFrSpeedと車速差が16km/h以下になるよう調整してしまう
                    frSpeed = (data.data[4] - 208d) * 32d + (data.data[3] / 8d);
                    rrSpeed = (data.data[4] - 208d) * 32d + (data.data[0] / 8d);

                    if (rrSpeed > frSpeed + 16d)
                    {
                        rrSpeed = rrSpeed - 32d;
                    }

                    if (rrSpeed < frSpeed - 16d)
                    {
                        rrSpeed = rrSpeed + 32d;
                    }

                    // 0割防止
                    if (frSpeed == 0)
                    {
                        return 0;
                    }

                    return rrSpeed/frSpeed*100d-100d;




                // CANID 174
                // K51_YawRate
                // Data2*125/128+Data1/256-125
                case "#K51_YawRate":
                    return data.data[1] * 125d / 128d + data.data[0] / 256d - 125d;

                // CANID 174
                // K51_YAxisG
                // (Data6+Data5/256-128)/32 
                case "#K51_YAxisG":
                    return (data.data[5] + (data.data[4] / 256d) - 128d) / 32d;

                // CANID 178
                // K51_RollRate
                // Data2*125/128+Data1/256-125
                case "#K51_RollRate":
                    return data.data[1] * 125d / 128d + data.data[0] / 256d - 125d;

                // CANID 178
                // K51_XAxisG
                // (Data6+Data5/256-128)/32
                case "#K51_XAxisG":
                    return (data.data[5] + (data.data[4] / 256d) - 128d) / 32d;

                // CANID 17C
                // K51_ZAxisG
                // (Data6+Data5/256-128)/32
                case "#K51_ZAxisG":
                    return (data.data[5] + (data.data[4] / 256d) - 128d) / 32d;






                // CANID 7FF
                // GPS Latitude
                // (Data8*16777216+Data7*65536+Data6*256+Data5-90000000)/1000000
                case "#GPS_Latitude":
                    return (data.data[7] * 16777216d + data.data[6] * 65536d + data.data[5] * 256d + data.data[4] - 90000000d) / 1000000d;

                // CANID 7FF
                // GPS Longitude
                // (Data4*16777216+Data3*65536+Data2*256+Data1-180000000)/1000000
                case "#GPS_Longitude":
                    return (data.data[3] * 16777216d + data.data[2] * 65536d + data.data[1] * 256d + data.data[0] - 180000000d) / 1000000d;

                // CANID 7FE
                // GPS Speed
                // (Data8*16777216+Data7*65536+Data6*256+Data5)/1000000*3600/1000
                // unit of GPS speed is m/sec so *3600/1000 to convert to km/h
                case "#GPS_Speed":
                    return (data.data[7] * 16777216d + data.data[6] * 65536d + data.data[5] * 256d + data.data[4]) / 1000000d * 3600 / 1000;

                // CANID 7FE
                // GPS Altitude
                // (Data4*16777216+Data3*65536+Data2*256+Data1-1000000000)/1000000
                case "#GPS_Altitude":
                    return (data.data[3] * 16777216d + data.data[2] * 65536d + data.data[1] * 256d + data.data[0] - 1000000000d) / 1000000d;

                // CANID 7FE
                // GPS Distance
                // 積分計算が後処理で必要になるので、ダミーで0を返すだけとする。
                case "#GPS_Distance":
                    return 0;


                // 計算定義が間違っていてここまで来た場合は0を返す
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Decodeルールを追加する
        /// </summary>
        /// <param name="chName">デコードルールの識別名。</param>
        /// <param name="id">デコードルールのCAN ID（16進数表記）。</param>
        /// <param name="formula">デコード計算式。</param>
        public void AddData(string chName, ushort id, string formula, int chColor, int chMin, int chMax,  bool chPreview, bool chShow)
        {
            this.ChName.Add(chName);
            this.Id.Add(id);
            this.Formula.Add(formula);
            this.ChMin.Add(chMin);
            this.ChMax.Add(chMax);
            this.ChColor.Add(chColor);
            this.ChPreview.Add(chPreview);
            this.ChShow.Add(chShow);
        }

        /// <summary>
        /// Decodeルールをクリアする
        /// </summary>
        public void Clear()
        {
            this.ChName.Clear();
            this.Id.Clear();
            this.Formula.Clear();
            this.ChMin.Clear();
            this.ChMax.Clear();
            this.ChColor.Clear();
            this.ChPreview.Clear();
            this.ChShow.Clear();
        }

        /// <summary>
        /// 引数のチャンネル名のインデックスを返す
        /// </summary>
        /// <param name="str">検索するチャンネル名</param>
        public int IndexOf(ushort usid)
        {
            return this.Id.IndexOf(usid);
        }

        /// <summary>
        /// 引数のチャンネル名のインデックスを返す2
        /// </summary>
        /// <param name="str">検索するチャンネル名</param>
        /// <param name = "index" > 検索開始するIndex </ param >
        public int IndexOf(ushort usid, int index)
        {
            return this.Id.IndexOf(usid, index);
        }

        /// <summary>
        /// 引数のFormulaのインデックスを返す
        /// </summary>
        /// <param name="str">検索するFormula</param>
        public int FormulaIndexOf(string formula)
        {
            return this.Formula.IndexOf(formula);
        }

        /// <summary>
        /// DecodeRuleの要素数を返す
        /// </summary>
        public int Count
        {
            get
            {
                return this.Id.Count;
            }
        }

        /// <summary>
        /// 引数のインデックスのチャンネル名を返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public string GetChName(int index)
        {
            return this.ChName[index];
        }

        /// <summary>
        /// 引数のインデックスのデコードルールを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public string GetDecodeRule(int index)
        {
            return this.Formula[index];
        }

        /// <summary>
        /// 引数のインデックスのChartMinを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public int GetChartMin(int index)
        {
            return this.ChMin[index];
        }

        /// <summary>
        /// 引数のインデックスのChartMaxを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public int GetChartMax(int index)
        {
            return this.ChMax[index];
        }

        /// <summary>
        /// 引数のインデックスのChartColorを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public int GetChartColor(int index)
        {
            return this.ChColor[index];
        }

        /// <summary>
        /// 引数のインデックスのChartPreviewを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public bool GetChartPreview(int index)
        {
            return this.ChPreview[index];
        }

        /// <summary>
        /// 引数のインデックスのChartShowを返す
        /// </summary>
        /// <param name="index">検索するチャンネル名</param>
        public bool GetChartShow(int index)
        {
            return this.ChShow[index];
        }
    }
}
