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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MotoRecoViewer
{
    public partial class FormDecodeOption : Form
    {
        //==========================
        //
        //  Private変数
        //
        //==========================
        private string FixedFormula = "";

        /// <summary>
        /// 定形計算式定義を受け取る
        /// </summary>
        public string ReceiveFixedChName
        {
            set
            {
                FixedFormula = value;
                TextChName.Text = FixedFormula;
            }
        }

        /// <summary>
        /// 定形計算式定義を受け取る
        /// </summary>
        public string ReceiveFixedCANID
        {
            set
            {
                FixedFormula = value;
                TextCanId.Text = FixedFormula;
            }
        }

        /// <summary>
        /// 定形計算式定義を受け取る
        /// </summary>
        public string ReceiveFixedFormula
        {
            set
            {
                FixedFormula = value;
                TextFormula.Text = FixedFormula;
            }
        }

        /// <summary>
        /// ListViewの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        private void ListViewToCsv(string csvPath)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(csvPath, false, enc);

            int colCount = ListViewDecode.Columns.Count;
            int lastColIndex = colCount - 1;

            //レコードを書き込む
            foreach (ListViewItem row in ListViewDecode.Items)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row.SubItems[i].Text;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

        public FormDecodeOption()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem newItem = new ListViewItem
            {
                UseItemStyleForSubItems = false
            };

            int i;

            newItem.Text = TextChName.Text;
            newItem.SubItems.Add(TextCanId.Text);
            newItem.SubItems.Add(TextFormula.Text);
            i = TextColor.BackColor.ToArgb();
            newItem.SubItems.Add(i.ToString());
            newItem.SubItems[3].BackColor = TextColor.BackColor;
            newItem.SubItems.Add(TextMin.Text);
            newItem.SubItems.Add(TextMax.Text);
            newItem.SubItems.Add(CheckPreview.Checked.ToString());
            newItem.SubItems.Add(CheckShow.Checked.ToString());

            ListViewDecode.Items.Add(newItem);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ListViewToCsv(saveFileDialog.FileName);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {

            if (OpenFileDDF.ShowDialog() == DialogResult.OK)
            {
                string filePath = OpenFileDDF.FileName;
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("Shift_JIS"));

                while (reader.Peek() >= 0)
                {
                    ListViewItem newItem = new ListViewItem
                    {
                        UseItemStyleForSubItems = false
                    };

                    string[] cols = reader.ReadLine().Split(',');
                    for (int n = 0; n < cols.Length; n++)
                    {
                        if (n == 0)
                        {
                            newItem.SubItems[n].Text = cols[n];
                        }
                        else
                        {
                            newItem.SubItems.Add(cols[n]);

                            //Color指定する場合は、SubItems.BackColorに反映する
                            if (n==3)
                            {
                                int i;
                                i = int.Parse(newItem.SubItems[3].Text);
                                Color color = Color.FromArgb(i);
                                newItem.SubItems[3].BackColor = color;
                            }
                        }
                    }
                    ListViewDecode.Items.Add(newItem);
                }
                reader.Close();
            }
        }

        private void ListViewDecode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 一旦ListViewで選択したあとに他のItemを選択する際に、SelectedItems.Countが-1になっていることがあるので
            // その場合実際にSelectedItemsがないのにアクセスするとエラーになるので、処理を飛ばす
            if (ListViewDecode.SelectedItems.Count > 0)
            {
                TextChName.Text = ListViewDecode.SelectedItems[0].Text;
                TextCanId.Text = ListViewDecode.SelectedItems[0].SubItems[1].Text;
                TextFormula.Text = ListViewDecode.SelectedItems[0].SubItems[2].Text;
                int i;
                i = int.Parse(ListViewDecode.SelectedItems[0].SubItems[3].Text);
                Color color = Color.FromArgb(i);
                TextColor.BackColor = color;
                TextMin.Text = ListViewDecode.SelectedItems[0].SubItems[4].Text;
                TextMax.Text = ListViewDecode.SelectedItems[0].SubItems[5].Text;
                CheckPreview.Checked = System.Convert.ToBoolean(ListViewDecode.SelectedItems[0].SubItems[6].Text);
                CheckShow.Checked = System.Convert.ToBoolean(ListViewDecode.SelectedItems[0].SubItems[7].Text);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FormMain fm = (FormMain)this.Owner;

            //FormMainのDecodeRuleをクリア
            fm.decodeRule.Clear();

            foreach (ListViewItem row in ListViewDecode.Items)
            {
                //ListViewの内容をFormMainのListDecodeRuleに反映する
                fm.decodeRule.AddData(row.SubItems[0].Text,                                                             //Ch Name
                                      ushort.Parse(row.SubItems[1].Text, System.Globalization.NumberStyles.HexNumber),  //CAN ID
                                      row.SubItems[2].Text,                                                             //Formula
                                      int.Parse(row.SubItems[3].Text),                                                  //Ch Color
                                      int.Parse(row.SubItems[4].Text),                                                  //Min
                                      int.Parse(row.SubItems[5].Text),                                                  //Max
                                      bool.Parse(row.SubItems[6].Text),                                                 //flg Preview
                                      bool.Parse(row.SubItems[7].Text)                                                  //flg Show
                                      );
            }

            //閉じる
            this.Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            //TextBoxの内容をListViewに反映
            ListViewDecode.SelectedItems[0].SubItems[0].Text = TextChName.Text;
            ListViewDecode.SelectedItems[0].SubItems[1].Text = TextCanId.Text;
            ListViewDecode.SelectedItems[0].SubItems[2].Text = TextFormula.Text;
            int i;
            i = TextColor.BackColor.ToArgb();
            ListViewDecode.SelectedItems[0].SubItems[3].Text = i.ToString();
            ListViewDecode.SelectedItems[0].SubItems[3].BackColor = TextColor.BackColor;
            ListViewDecode.SelectedItems[0].SubItems[4].Text = TextMin.Text;
            ListViewDecode.SelectedItems[0].SubItems[5].Text = TextMax.Text;
            ListViewDecode.SelectedItems[0].SubItems[6].Text = CheckPreview.Checked.ToString();
            ListViewDecode.SelectedItems[0].SubItems[7].Text = CheckShow.Checked.ToString();
        }

        private void TextColor_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            ListViewDecode.SelectedItems[0].Remove();
        }

        private void TextColor_DoubleClick(object sender, EventArgs e)
        {
            //ダイアログを表示する
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                TextColor.BackColor = colorDialog.Color;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //List Viewクリア
            ListViewDecode.Items.Clear();
        }

        private void BtnBrowsRule_Click(object sender, EventArgs e)
        {
            FormSelFormula f = new FormSelFormula();

            //ToDo 固定式選択時は、CAN IDとChNameもデフォルトで入れること

            f.ShowDialog(this);
            f.Dispose();
        }

        private void BtnPreAna_Click(object sender, EventArgs e)
        {
            if (OpenFileDAT.ShowDialog() != DialogResult.OK){
                return;
            }
            
            string FileName = OpenFileDAT.FileName;

            //ファイルサイズを調べる
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            long fileSize = fs.Length;

            //CANDataは1データ16バイトなので、用意する配列は　( ファイルサイズ / 16 - 1)
            long arySize = fileSize / 16;

            //CANDataが壊れていると、16で割り切れないかもしれないのでチェックしておき、あまりが出たら読み込む個数を1減らす
            long checkSize = fileSize % 16;

            if (checkSize != 0)
            {
                arySize--;
            }

            //バイナリーリーダー生成
            BinaryReader reader = new BinaryReader(fs);

            //CANデータを1フレーム分読み込む
            FormMain.CanData tempCanData = new FormMain.CanData();

            List<ushort> ListCANID = new List<ushort>();

            for (int i = 0; i < arySize; i++)
            {
                tempCanData.timeSec = reader.ReadUInt32();
                tempCanData.timeMSec = reader.ReadUInt16();
                tempCanData.id = reader.ReadUInt16();
                tempCanData.data = new byte[8];

                for (int j = 0; j < 8; j++)
                {
                    tempCanData.data[j] = reader.ReadByte();
                }

                //ここで1フレーム分読み込み終了

                //CANIDがListに既存でなければ追加する
                if (ListCANID.IndexOf(tempCanData.id) == -1)
                {
                    ListCANID.Add(tempCanData.id);
                }
            }

            // fileクローズ
            reader.Close();

            FormAnalysys f = new FormAnalysys();

            //ToDo 固定式選択時は、CAN IDとChNameもデフォルトで入れること

            f.ShowDialog(this);
            f.Dispose();

        }

        private void FormDecodeOption_Load(object sender, EventArgs e)
        {
            FormMain fm = (FormMain)this.Owner;

            //FormMainのDecodeRuleがなければ何もしない
            if (fm.decodeRule.Count == 0)
            {
                return;
            }

            //ToDo DecodeRuleの内容をListViewDecodeに反映する
            for (int i=0; i<fm.decodeRule.Count; i++)
            {
                ListViewItem newItem = new ListViewItem
                {
                    UseItemStyleForSubItems = false
                };

                int j;

                //ChName
                newItem.Text = fm.decodeRule.GetChName(i);
                //CANID
                newItem.SubItems.Add(fm.decodeRule.GetCANID(i).ToString("X3"));
                //Formula
                newItem.SubItems.Add(fm.decodeRule.GetDecodeRule(i));
                //Pen Color
                newItem.SubItems.Add(fm.decodeRule.GetChartColor(i).ToString());
                newItem.SubItems[3].BackColor = Color.FromArgb(fm.decodeRule.GetChartColor(i));
                //Min
                newItem.SubItems.Add(fm.decodeRule.GetChartMin(i).ToString());
                //MAX
                newItem.SubItems.Add(fm.decodeRule.GetChartMax(i).ToString());
                //Preview
                newItem.SubItems.Add(fm.decodeRule.GetChartPreview(i).ToString());
                //Show
                newItem.SubItems.Add(fm.decodeRule.GetChartShow(i).ToString());

                ListViewDecode.Items.Add(newItem);
            }
        }
    }
}
