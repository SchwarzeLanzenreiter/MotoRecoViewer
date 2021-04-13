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
    public struct DecodeData
    {
        public string ChName { get; set; }
        public ushort Id { get; set; }
        public string Formula { get; set; }
        public int ChMin { get; set; }
        public int ChMax { get; set; }
        public int ChColor { get; set; }
        public bool ChPreview { get; set; }
        public bool ChShow { get; set; }
        public bool ChFilter { get; set; }
        public double ChCutOff { get; set; }
    }

    class DecodeRule
    {
        private List<DecodeData> Data;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecodeRule()
        {
            Data = new List<DecodeData>();
        }

        /// <summary>
        /// Decode情報をdddファイルとしてセーブ
        /// </summary>
        public void SaveToCSV(string FilePath)
        {
            //Itemが0なら即抜ける
            if (Data.Count < 1)
            {
                return;
            }

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
            new System.IO.StreamWriter(FilePath, false, enc);
            
            //レコードを書き込む
            for (int i=0; i<Data.Count; i++)
            {
                sr.Write(Data[i].ChName + ",");
                sr.Write(Data[i].Id.ToString("X3") + ",");
                sr.Write(Data[i].Formula + ",");
                sr.Write(Data[i].ChColor.ToString() + ",");
                sr.Write(Data[i].ChMin.ToString() + ",");
                sr.Write(Data[i].ChMax.ToString() + ",");
                sr.Write(Data[i].ChPreview.ToString() + ",");
                sr.Write(Data[i].ChShow.ToString() + ",");
                sr.Write(Data[i].ChFilter.ToString());

                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
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

            string str;
            byte nibble = (byte)(0x0F);

            //式中のLoData1～LoData8の文字列を、数値に置き換える
            for (int i = 0; i < 8; i++)
            {
                //Lower 4bitは、Higher 4bitを0でマスクするだけ
                str = (nibble & data.data[i]).ToString();
                exp = exp.Replace("LoData" + (i + 1).ToString(), str);
            }

            //式中のHiData1～HiData8の文字列を、数値に置き換える
            for (int i = 0; i < 8; i++)
            {
                //Higher 4bitは、4bit右シフトするだけ
                str = (data.data[i] >> 4).ToString();
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
                // BMW Engine RPM
                // (LoData4*255+Data3)*5
                case "#BMW_RPM":
                    return ((nibble & data.data[3]) * 256d + data.data[2]) * 5d;
     
                // CANID 10C
                // BMW Throttel Valve Position
                // Data6/255*100
                case "#BMW_ThrottelValvePosition":
                    return data.data[5] / 255d * 100d;

                // CANID 10C
                // BMW Throttel Position
                // Data8/255*100
                case "#BMW_ThrottelPosition":
                    return data.data[7] / 255d * 100d;

                // CANID 110
                // BMW Ignition Timing
                // LoData4*256+Data3
                // ToDo need to confirm later
                //case "#BMW_IgnitionTiming":
                //    return ((nibble & data.data[3])*256d + data.data[2]) / 5d;

                // CANID 110
                // BMW Throttel Grip Position
                // Data6/255*100
                case "#BMW_ThrottelGripPosition":
                    return data.data[5] / 255d * 100d;

                // CANID 120
                // BMW FrBrake1
                // Data3
                case "#BMW_FrBrake1":     
                    return data.data[2] / 4d;

                // CANID 120
                // BMW FrBrake2
                // Data5
                case "#BMW_FrBrake2":
                    return data.data[4] / 4d;

                // CANID 120
                // BMW RrBrake1
                // Data4
                case "#BMW_RrBrake1":
                    return data.data[3] / 2d;

                // CANID 120
                // BMW RrBrake2
                // Data6
                case "#BMW_RrBrake2":
                    return data.data[5] / 2d;

                // CANID 29C
                // BMW FrStroke
                // (LoData7*256+Data6)/5
                case "#BMW_FrStroke":
                    return ((nibble & data.data[6])*256d + data.data[5])/5d;

                // CANID 29C
                // BMW RrStroke
                // (Data8*16+HiData7)/3
                case "#BMW_RrStroke":
                    return (data.data[7] * 16d + (data.data[6] >> 4)) / 3d;

                // CANID 10C
                // BMW LeanAngle
                // (Data5*90/128-90)
                case "#BMW_LeanAngle":
                    return (((double)data.data[4] * 90d / 127d) - 90d);

                // CANID 293
                // BMW RrSpeed
                // (LoData2*256+Data1)/8
                case "#BMW_RrSpeed":
                    return ((nibble & data.data[1])*256d + data.data[0]) / 8d;

                // CANID 293
                // BMW FrSpeed2
                // (Data3*16+HiData2)/8
                case "#BMW_FrSpeed2":
                    return (data.data[2] * 16d + (data.data[1] >> 4)) / 8d;

                // CANID 293
                // BMW FrSpeed1
                // (LoData5*256+Data4)/8
                case "#BMW_FrSpeed1":
                    return ((nibble & data.data[4]) * 256 + data.data[3]) / 8d;

                // CANID 293
                // BMW_DistFrSpeed1
                // 積分計算が後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_DistFrSpeed1":
                    return 0;

                // CANID 293
                // BMW SlipRate
                // rrSpeed/frSpeed*100-100
                case "#BMW_SlipRate":
                    rrSpeed = (((nibble & data.data[1]) * 256d) + data.data[0]) / 8d;
                    frSpeed = (((nibble & data.data[4]) * 256d) + data.data[3]) / 8d;

                    // 0割防止
                    if (frSpeed == 0)
                    {
                        return 0;
                    }

                    return rrSpeed/frSpeed*100d-100d;

                // CANID 2BC
                // BMW BMW_Gear
                // HiData6
                // 1 4 7 8 11 13 が 1-6速に該当
                case "#BMW_Gear":
                    switch(data.data[5] >> 4)
                    {
                        case 1:return 1;
                        case 4:return 2;
                        case 7:return 3;
                        case 8:return 4;
                        case 11:return 5;
                        case 13:return 6;
                        default: return 0;
                    }

                // CANID 174
                // BMW_YawRate
                // Data2*125/128+Data1/256-125
                case "#BMW_YawRate":
                    return data.data[1] * 125d / 127d + data.data[0] / 255d - 125d;

                // CANID 174
                // BMW_YAxisG
                // (Data6+Data5/256-128)/32 
                case "#BMW_YAxisG":
                    return (data.data[5] + (data.data[4] / 255d) - 128d) / 32d;

                // CANID 178
                // BMW_RollRate
                // Data2*125/128+Data1/256-125
                case "#BMW_RollRate":
                    return data.data[1] * 125d / 127d + data.data[0] / 255d - 125d;

                // CANID 178
                // BMW_XAxisG
                // (Data6+Data5/256-128)/32
                case "#BMW_XAxisG":
                    return (data.data[5] + (data.data[4] / 255d) - 128d) / 32d;

                // CANID 17C
                // BMW_ZAxisG
                // (Data6+Data5/256-128)/32
                case "#BMW_ZAxisG":
                    return (data.data[5] + (data.data[4] / 255d) - 128d) / 32d;


                // CANID 3FA
                // BMW_AirTemp
                // Data1*0.75-50
                case "#BMW_AirTemp":
                    return data.data[0] * 0.75 - 48d;

                // CANID 3FA
                // BMW_OilTemp
                // Data2-25
                case "#BMW_OilTemp":
                    return data.data[1] - 25d;

                // CANID 2BC
                // BMW_WaterTemp
                // Data3*0.76-25
                case "#BMW_WaterTemp":
                    return data.data[2] * 0.76 - 25d;


                // CANID 2B0
                // BMW_DistanceCounterFr
                // LoData5*253+Data4
                case "#BMW_DistCountFr":
                    return (nibble & data.data[4]) * 253d + data.data[3];

                // CANID 2B0
                // BMW_AccumulatedDistCountFr
                // 積算計算が後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_AccumulatedDistCountFr":
                    return 0;

                // CANID 2B0
                // BMW_DistanceCounterRr
                // Data3
                case "#BMW_DistCountRr":
                    return data.data[2];

                // CANID 2B0
                // BMW_DistanceCounterRr
                // 積算計算が後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_AccumulatedDistCountRr":
                    return 0;

                // CANID 2BC
                // BMW_FuelCounter
                // Data4+Data5*256
                case "#BMW_FuelCount":
                    return data.data[4] * 256 + data.data[3];

                // CANID 2BC
                // BMW_FuelCounter
                // 積算計算が後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_AccumulatedFuelCount":
                    return 0;

                // CANID 2D0
                // BMW_FuelLevel
                // Data2*256+Data1
                case "#BMW_FuelLevel":
                    return (data.data[1] * 256d + data.data[0] ) / 562d * 100d;

                // CANID 3F8
                // BMW_OdMeter
                // Data4*65536+Data3*256+Data2
                case "#BMW_OdMeter":
                    return data.data[3]*65536d + data.data[2]*256d + data.data[1];

                // CANID 2BC
                // BMW_FuelConsumption
                // 後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_FuelConsumption":
                    return 0;

                // CANID 2BC
                // BMW_Range
                // 後処理で必要になるので、ダミーで0を返すだけとする。
                case "#BMW_Range":
                    return 0;



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



                // CANID any
                // Data1
                case "#Data1":
                    return data.data[0];

                case "#HiData1":
                    return data.data[0] >> 4;

                case "#LoData1":
                    return (nibble & data.data[0]);

                case "#Data2":
                    return data.data[1];

                case "#HiData2":
                    return data.data[1] >> 4;

                case "#LoData2":
                    return (nibble & data.data[1]);

                case "#Data3":
                    return data.data[2];

                case "#HiData3":
                    return data.data[2] >> 4;

                case "#LoData3":
                    return (nibble & data.data[2]);

                case "#Data4":
                    return data.data[3];

                case "#HiData4":
                    return data.data[3] >> 4;

                case "#LoData4":
                    return (nibble & data.data[3]);

                case "#Data5":
                    return data.data[4];

                case "#HiData5":
                    return data.data[4] >> 4;

                case "#LoData5":
                    return (nibble & data.data[4]);

                case "#Data6":
                    return data.data[5];

                case "#HiData6":
                    return data.data[5] >> 4;

                case "#LoData6":
                    return (nibble & data.data[5]);

                case "#Data7":
                    return data.data[6];

                case "#HiData7":
                    return data.data[6] >> 4;

                case "#LoData7":
                    return (nibble & data.data[6]);

                case "#Data8":
                    return data.data[7];

                case "#HiData8":
                    return data.data[7] >> 4;

                case "#LoData8":
                    return (nibble & data.data[7]);



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
        /// <param name="chColor">チャンネル表示色</param>
        /// <param name="chMin">チャンネル下限値</param>
        /// <param name="chMax">チャンネル上限値</param>
        /// <param name="chPreview">SubChartに表示する/しない</param>
        /// <param name="chShow">MainChartに表示する/しない</param>
        /// <param name="chUseFilter">LPFをする/しない</param>
        /// <param name="chCutOff">LPFのカットオフ周波数</param>
        public void AddData(string chName, string id, string formula, string chColor, string chMin, string chMax, string chPreview, string chShow, string chUseFilter, string chFilterCutoff)
        {
            DecodeData newData = new DecodeData
            {
                ChName = chName,
                Id = (ushort.Parse(id, System.Globalization.NumberStyles.HexNumber)),
                Formula = formula,
                ChMin = int.Parse(chMin),
                ChMax = int.Parse(chMax),
                ChColor = int.Parse(chColor),
                ChPreview = bool.Parse(chPreview),
                ChShow = bool.Parse(chShow),
                ChFilter = bool.Parse(chUseFilter),
                ChCutOff = double.Parse(chFilterCutoff)
            };

            Data.Add(newData);
        }

        /// <summary>
        /// Decodeルールを編集する
        /// </summary>
        /// <param name="chName">デコードルールの識別名。</param>
        /// <param name="id">デコードルールのCAN ID（16進数表記）。</param>
        /// <param name="formula">デコード計算式。</param>
        /// <param name="chColor">チャンネル表示色</param>
        /// <param name="chMin">チャンネル下限値</param>
        /// <param name="chMax">チャンネル上限値</param>
        /// <param name="chPreview">SubChartに表示する/しない</param>
        /// <param name="chShow">MainChartに表示する/しない</param>
        /// <param name="chUseFilter">LPFをする/しない</param>
        /// <param name="chCutOff">LPFのカットオフ周波数</param>
        public void EditData(int idx,string chName, string id, string formula, string chColor, string chMin, string chMax, string chPreview, string chShow, string chUseFilter, string chCutOff)
        {
            if (idx < 0)
            {
                return;
            }

            if (idx >= Data.Count)
            {
                return;
            }

            DecodeData tempData = Data[idx];

            tempData.ChName = chName;
            tempData.Id = (ushort.Parse(id, System.Globalization.NumberStyles.HexNumber));
            tempData.Formula = formula;
            tempData.ChMin = int.Parse(chMin);
            tempData.ChMax = int.Parse(chMax);
            tempData.ChColor = int.Parse(chColor);
            tempData.ChPreview = bool.Parse(chPreview);
            tempData.ChShow = bool.Parse(chShow);
            tempData.ChFilter = bool.Parse(chUseFilter);
            tempData.ChCutOff = double.Parse(chCutOff);

            Data[idx] = tempData;
        }

        /// <summary>
        /// Decodeルールを削除する
        /// </summary>
        /// <param name="idx">削除するアイテムのインデックス</param>
        public void DelData(int idx)
        {
            if (idx < 0)
            {
                return;
            }

            if (idx >= Data.Count)
            {
                return;
            }
            Data.RemoveAt(idx);
        }

        /// <summary>
        /// Decodeルールをクリアする
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        /// <summary>
        /// 引数のCANIDのインデックスを検索する
        /// </summary>
        /// <param name="usid">検索するCANID</param>
        /// <returns>Dataのインデックス</returns>
        public int IndexOf(ushort usid)
        {
            return Data.FindIndex(x => x.Id == usid);
        }

        /// <summary>
        /// 引数のCANIDのインデックスを検索する
        /// </summary>
        /// <param name="usid">検索するCANID</param>
        /// <param name="index">検索を開始するindex</param>
        /// <returns>Dataのインデックス</returns>
        public int IndexOf(ushort usid, int index)
        {
            return Data.FindIndex(index, x => x.Id == usid);
        }

        /// <summary>
        /// 引数のFormulaのインデックスを返す
        /// </summary>
        /// <param name="formula">検索するFormula</param>
        /// <returns>Dataのインデックス</returns>
        public int FormulaIndexOf(string formula)
        {
            return Data.FindIndex(x => x.Formula == formula);
        }

        /// <summary>
        /// 引数のchNameのインデックスを返す
        /// </summary>
        /// <param name="chName">検索するchName</param>
        /// <returns>Dataのインデックス</returns>
        public int ChNameIndexOf(string chName)
        {
            return Data.FindIndex(x => x.ChName == chName);
        }

        /// <summary>
        /// DecodeRuleの要素数を返す
        /// </summary>
        public int Count
        {
            get
            {
                return Data.Count;
            }
        }

        /// <summary>
        /// 引数のインデックスのチャンネル名を返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public string GetChName(int index)
        {
            return Data[index].ChName;
        }

        /// <summary>
        /// 引数のCANIDを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public ushort GetCANID(int index)
        {
            return Data[index].Id;
        }

        /// <summary>
        /// 引数のインデックスのデコードルールを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public string GetDecodeRule(int index)
        {
            return Data[index].Formula;
        }

        /// <summary>
        /// 引数のインデックスのChartMinを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public int GetChartMin(int index)
        {
            return Data[index].ChMin;
        }

        /// <summary>
        /// 引数のインデックスのChartMaxを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public int GetChartMax(int index)
        {
            return Data[index].ChMax;
        }

        /// <summary>
        /// 引数のインデックスのChartColorを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public int GetChartColor(int index)
        {
            return Data[index].ChColor;
        }

        /// <summary>
        /// 引数のインデックスのChartPreviewを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public bool GetChartPreview(int index)
        {
            return Data[index].ChPreview;
        }

        /// <summary>
        /// 引数のインデックスのChartShowを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public bool GetChartShow(int index)
        {
            return Data[index].ChShow;
        }

        /// <summary>
        /// 引数のインデックスのUseFilterを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public bool GetChartUseFilter(int index)
        {
            return Data[index].ChFilter;
        }

        /// <summary>
        /// 引数のインデックスのCutOffを返す
        /// </summary>
        /// <param name="index">データ取得するインデックス</param>
        public double GetChartCutOff(int index)
        {
            return Data[index].ChCutOff;
        }
    }
}
