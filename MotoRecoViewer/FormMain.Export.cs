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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Net;
using Microsoft.Win32;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.ComponentModel.DataAnnotations;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace MotoRecoViewer
{
    /// <summary>
    /// CSVエクスポート
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// DatファイルをAsciiのCSVに変換する
        /// CSVのフォーマットは、"秒","ミリ秒","CAN ID(HEX)","Data1のHEX","Data2のHEX","Data3のHEX","Data4のHEX","Data5のHEX","Data6のHEX","Data7のHEX","Data8のHEX"
        /// </summary>
        private void ConvertCANData(string SrcFileName, string DstFileName)
        {
            //CANデータ読み取り中フラグ
            IsReadingCanData = true;

            //プログレスバーは1回目ループ(読み込み)で50%, 2回目ループ(書き込み)で100%にしたい。
            //先に全体レコード数がわからないので、Maximumは読み込み後に設定する。
            progressBar.Value = 0;

            //datファイルを全件読み込む
            CanData[] aryCanData = CanDataFile.ReadAll(SrcFileName, (read, total) =>
            {
                progressBar.Maximum = (int)total * 2;
                progressBar.Value = (int)read;
                this.statusLabel.Text = string.Format("{0}%", read * 100 / total / 2);
                Application.DoEvents();
            });

            long arySize = aryCanData.Length;

            //プログレスバー計算用
            long div_num = arySize / 100;
            int counter = (int)arySize;   // 読み込み分で50%消化済
            progressBar.Maximum = (int)arySize * 2;

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(DstFileName, false, enc);

            //レコードを書き込む
            // ToDo fieldはStringBuilderに置き換えること
            System.Text.StringBuilder field = new System.Text.StringBuilder("");

            for (int i = 0; i < arySize; i++)
            {
                //毎ループメッセージ発行するとクソ遅いので、トータル50回送るようにする。
                if (div_num > 0)
                {
                    if (i % div_num == 0)
                    {
                        progressBar.Value = (int)counter;
                        this.statusLabel.Text = string.Format("{0}%", ((int)counter) * 100 / arySize / 2);
                        Application.DoEvents();
                    }
                    counter++;
                }

                field.Append(aryCanData[i].timeSec.ToString() + ",");
                field.Append(aryCanData[i].timeMSec.ToString() + ",");
                field.Append(aryCanData[i].id.ToString("X3") + ",");
                field.Append(aryCanData[i][0].ToString("X2") + ",");
                field.Append(aryCanData[i][1].ToString("X2") + ",");
                field.Append(aryCanData[i][2].ToString("X2") + ",");
                field.Append(aryCanData[i][3].ToString("X2") + ",");
                field.Append(aryCanData[i][4].ToString("X2") + ",");
                field.Append(aryCanData[i][5].ToString("X2") + ",");
                field.Append(aryCanData[i][6].ToString("X2") + ",");
                field.Append(aryCanData[i][7].ToString("X2") + "\r\n");

                //フィールドを書き込む
                sr.Write(field);

                //field初期化
                field.Clear();
            }

            //閉じる
            sr.Close();

            //CANデータ読み取り終了
            IsReadingCanData = false;

            //プログレスバー初期化
            progressBar.Value = 0;
            this.statusLabel.Text = "";
        }

        /// <summary>
        /// 画面に表示中のデコード済データをCSVに変換する
        /// CSVのフォーマットは、"秒","ミリ秒","ch1 物理値","ch2物理値"...とする
        /// <param name="DstFileName">エクスポートファイル名</param>
        /// <param name="Mode">0:ファイル全体 1:カーソル間</param>
        /// </summary>
        private void ConvertDecodeData(string DstFileName, int Mode, int Type)
        {
            //MODE_ALL / MODE_CURSOR はクラス側の定数を使う。
            //以前はここで同名のローカル定数を再宣言しており、MODE_ALLが未使用で警告が出ていた。

            //開始時間と終了時間を取得
            double exportStartTime;
            double exportEndTime;

            if (Mode == MODE_CURSOR)
            {
                // MainChartのカーソル位置1に対応するタイムスタンプを計算
                // mainCur1Posは、PanelMainChart上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
                double ratioMainCurPos1 = (mainCur1Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
                exportStartTime = subPosTime + (divTime * 20) * ratioMainCurPos1;

                double ratioMainCurPos2 = (mainCur2Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
                exportEndTime = subPosTime + (divTime * 20) * ratioMainCurPos2;
            }
            else
            {
                //デフォルトファイル全体
                exportStartTime = startTime;
                exportEndTime = endTime;
            }

            //プログレスバー計算用
            long div_num = (long)(exportEndTime - exportStartTime);
            int counter = 0;
            progressBar.Value = 0;
            progressBar.Maximum = (int)((exportEndTime - exportStartTime)*100);

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(DstFileName, false, enc);

            //レコードを書き込む
            // ToDo fieldはStringBuilderに置き換えること
            System.Text.StringBuilder field = new System.Text.StringBuilder("");

            // CSVのヘッダー行を書き込む
            if (Type == TYPE_DASHWARE)
            {
                field.Append("MotoReco Logger File\r\n");
                field.Append("Time" + ",");
            } else if (Type == TYPE_TELEMETRYOVERLAY)
            {
                field.Append("TO Custom v1\r\n");
                field.Append("time (ms)" + ",");
            }



            for (int i=0; i < channels.Count - 1 ; i++)
            {
                field.Append(channels[i].ChName + ",");
            }
            field.Append(channels[channels.Count - 1].ChName  + "\r\n");

            //フィールドを書き込む
            sr.Write(field);

            //field初期化
            field.Clear();

            // CSVはタイムスタンプごとにデータを保管して出力したいのでタイムスタンプでループする
            // とりあえず10msごとのタイムスタンプにする
            double timeStamp;

            timeStamp = exportStartTime;

            while (timeStamp < exportEndTime)
            {
                //毎ループメッセージ発行するとクソ遅いので、トータル100回送るようにする。
                if (div_num > 0)
                {
                    if ((int)(timeStamp * 100) % div_num == 0)
                    {
                        progressBar.Value = (int)counter;
                        this.statusLabel.Text = string.Format("{0}%", ((int)counter) * 100 / progressBar.Maximum);
                        Application.DoEvents();
                    }
                    counter++;
                }

                //タイムスタンプ出力
                if (Type == TYPE_DASHWARE)
                {
                    field.Append(timeStamp.ToString("F2") + ",");
                } else if (Type == TYPE_TELEMETRYOVERLAY)
                {
                    field.Append((timeStamp*1000).ToString("F0") + ",");
                }
                for (int i=0; i< channels.Count-1; i++)
                {
                    // タイムスタンプに最も近いChDataのインデックスを取得
                    int targetIdx = channels[i].FindLeftIndex(timeStamp);
                    double value = channels[i].LogData[targetIdx].DataValue;
                    double valueFiltered = channels[i].LogData[targetIdx].DataValueFiltered;

                    if (channels[i].ChFilter)
                    {
                        field.Append(valueFiltered.ToString() + ",");
                    } else
                    {
                        field.Append(value.ToString() + ",");
                    }
                }

                int targetIdx2 = channels[channels.Count-1].FindLeftIndex(timeStamp);
                double value2 = channels[channels.Count-1].LogData[targetIdx2].DataValue;
                double value2Filtered = channels[channels.Count - 1].LogData[targetIdx2].DataValueFiltered;

                if (channels[channels.Count - 1].ChFilter)
                {
                    field.Append(value2Filtered.ToString() + "\r\n");
                } else
                {
                    field.Append(value2.ToString() + "\r\n");
                }

                //フィールドを書き込む
                sr.Write(field);

                //field初期化
                field.Clear();

                // 次のタイムスタンプ
                timeStamp += 0.01;   // とりあえず10ms固定
            }  

            //閉じる
            sr.Close();

            //プログレスバー初期化
            progressBar.Value = 0;
            this.statusLabel.Text = "";
        }

        private void CSVConvertFolder(int Type)
        {
            // ファイルがOpenされたがデコード条件が空
            if (decodeRule.Count == 0)
            {
                CANDecodeSettingToolStripMenuItem_Click(null, null);
            }

            //フォルダ選択ダイアログ表示
            if (folderBrowsDialog.ShowDialog() != DialogResult.OK) { return; }

            //リストにサブフォルダ含めて*.dat一覧取得
            string[] names = Directory.GetFiles(folderBrowsDialog.SelectedPath, "*.dat", System.IO.SearchOption.AllDirectories);
            foreach (string name in names)
            {
                //選択したファイル名を保持する
                currentDatFile = name;

                //まずChDataをクリアする
                channels.Clear();
                initTimeOffset = 0.0;
                startTime = 0.0;
                endTime = 0.0;

                // バイナリファイルからCANデータ抽出する
                ReadCANData(name);

                //有効なデータを読み込めたらCSV変換する
                if (channels.Count > 0) {

                    // 現在保持している表示中のデコード済データをCSVエクスポートする
                    // 名前をつけて保存
                    string newname = Path.ChangeExtension(name, ".csv");
                    ConvertDecodeData(newname, MODE_ALL, Type);
                }
            }

            return;
        }

    }
}
