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
        //  internal変数
        //
        //==========================
        internal DecodeRule decodeRule;

        //==========================
        //
        //  Private変数
        //
        //==========================
        private string FixedFormula = "";

        /// <summary>
        /// FixedFormulaのChName
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
        /// FixedFormulaのCAN ID
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
        /// FixedFormulaの識別名
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
        /// FixedFormulaのColor
        /// </summary>
        public string ReceiveFixedColor
        {
            set
            {
                FixedFormula = value;
                TextColor.Text = FixedFormula;

                int i;
                i = int.Parse(FixedFormula);
                Color color = Color.FromArgb(i);
                TextColor.BackColor = color;
            }
        }

        /// <summary>
        /// FixedFormulaのMax
        /// </summary>
        public string ReceiveFixedMax
        {
            set
            {
                FixedFormula = value;
                TextMax.Text = FixedFormula;
            }
        }

        /// <summary>
        /// FixedFormulaのMin
        /// </summary>
        public string ReceiveFixedMin
        {
            set
            {
                FixedFormula = value;
                TextMin.Text = FixedFormula;
            }
        }

        /// <summary>
        /// FormMainのDecodeRuleをListViewにロードする
        /// </summary>
        private void LoadDecodeRuleToListView()
        {
            //decodeRuleがなければ何もしない
            if (this.decodeRule.Count == 0)
            {
                return;
            }

            //ListViewのSelectedChangeイベント削除
            dGVDecodeRule.SelectionChanged -= LdGVDecdeRule_SelectedIndexChanged;

            //ListView描画停止
            //ListViewDecode.BeginUpdate();

            //ListViewDecodeクリア
            dGVDecodeRule.Rows.Clear();

            //DecodeRuleの内容をListViewDecodeに反映する
            for (int i = 0; i < this.decodeRule.Count; i++)
            {
                dGVDecodeRule.Rows.Add(new string[] { this.decodeRule.GetChName(i),
                                                           this.decodeRule.GetCANID(i).ToString("X3"),
                                                           this.decodeRule.GetDecodeRule(i),
                                                           this.decodeRule.GetChartColor(i).ToString(),
                                                           this.decodeRule.GetChartMin(i).ToString(),
                                                           this.decodeRule.GetChartMax(i).ToString(),
                                                           this.decodeRule.GetChartPreview(i).ToString(),
                                                           this.decodeRule.GetChartShow(i).ToString(),
                                                           this.decodeRule.GetChartUseFilter(i).ToString()
                                                            });

            }

            //ListViewのSelectedChangeイベント復活
            dGVDecodeRule.SelectionChanged += LdGVDecdeRule_SelectedIndexChanged;
        }

        /// <summary>
        /// ListViewの内容をthis.decodeRuleに反映する
        /// </summary>
        private void ListViewToDecodeRule()
        {
            //DecodeRuleをクリア
            this.decodeRule.Clear();

            for (int i = 0; i < this.dGVDecodeRule.Rows.Count; i++)
            {
                //ListViewの内容をFormMainのListDecodeRuleに反映する
                this.decodeRule.AddData(dGVDecodeRule.Rows[i].Cells[0].Value.ToString(),         //Ch Name
                                        dGVDecodeRule.Rows[i].Cells[1].Value.ToString(),         //CAN ID
                                        dGVDecodeRule.Rows[i].Cells[2].Value.ToString(),         //Formula
                                        dGVDecodeRule.Rows[i].Cells[3].Value.ToString(),         //Ch Color
                                        dGVDecodeRule.Rows[i].Cells[4].Value.ToString(),         //Min
                                        dGVDecodeRule.Rows[i].Cells[5].Value.ToString(),         //Max
                                        dGVDecodeRule.Rows[i].Cells[6].Value.ToString(),         //flg Preview
                                        dGVDecodeRule.Rows[i].Cells[7].Value.ToString(),         //flg Show
                                        dGVDecodeRule.Rows[i].Cells[8].Value.ToString()          //flg Use Filter
                                        );
            }
        }

        /// <summary>
        /// FormMainのDecodeRuleをFormDecodeOptionのDecodeRuleにコピーする
        /// </summary>
        private void CopyDecodeRuleFromMain()
        {
            FormMain fm = (FormMain)this.Owner;

            //FormMainのDecodeRuleがなければ何もしない
            if (fm.decodeRule.Count == 0)
            {
                return;
            }

            //DecodeRuleクリア
            this.decodeRule.Clear();

            //DecodeRuleの内容をListViewDecodeに反映する
            for (int i = 0; i < fm.decodeRule.Count; i++)
            {
                this.decodeRule.AddData(fm.decodeRule.GetChName(i),                        //Ch Name
                                        fm.decodeRule.GetCANID(i).ToString("X3"),          //CAN ID
                                        fm.decodeRule.GetDecodeRule(i),                    //Formula
                                        fm.decodeRule.GetChartColor(i).ToString(),         //Ch Color
                                        fm.decodeRule.GetChartMin(i).ToString(),           //Min
                                        fm.decodeRule.GetChartMax(i).ToString(),           //Max
                                        fm.decodeRule.GetChartPreview(i).ToString(),       //flg Preview
                                        fm.decodeRule.GetChartShow(i).ToString(),          //flg Show
                                        fm.decodeRule.GetChartUseFilter(i).ToString()      //flg UseFilter
                                        );
            }
        }

        /// <summary>
        /// this.decodeRuleをFormMainのdecodeRuleにコピーする
        /// </summary>
        public void TransferDecodeRule()
        {
            FormMain fm = (FormMain)this.Owner;

            //DecodeRuleクリア
            fm.decodeRule.Clear();

            //DecodeRuleの内容をListViewDecodeに反映する
            for (int i = 0; i < this.decodeRule.Count; i++)
            {
                fm.decodeRule.AddData(this.decodeRule.GetChName(i),                        //Ch Name
                                      this.decodeRule.GetCANID(i).ToString("X3"),          //CAN ID
                                      this.decodeRule.GetDecodeRule(i),                    //Formula
                                      this.decodeRule.GetChartColor(i).ToString(),         //Ch Color
                                      this.decodeRule.GetChartMin(i).ToString(),           //Min
                                      this.decodeRule.GetChartMax(i).ToString(),           //Max
                                      this.decodeRule.GetChartPreview(i).ToString(),       //flg Preview
                                        fm.decodeRule.GetChartShow(i).ToString(),          //flg Show
                                        fm.decodeRule.GetChartUseFilter(i).ToString()      //flg UseFilter
                                      );
            }
        }

        public FormDecodeOption()
        {
            InitializeComponent();

            // private internal変数のnew
            decodeRule = new DecodeRule();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //線のカラーをINTに変換
            int i_color;
            i_color = TextColor.BackColor.ToArgb();

            //TextBoxの内容をdecodeRuleに反映
            try
            {
                // もしListViewで最後尾が選択されているか、何も選択されていない場合、最後尾に追加
                if ((dGVDecodeRule.SelectedRows.Count == 0) || (dGVDecodeRule.SelectedRows[0].Index == dGVDecodeRule.SelectedRows.Count - 1)) {
                    dGVDecodeRule.Rows.Add(new string[] {   TextChName.Text,                                              //ChName
                                                            TextCanId.Text,                                               //ID
                                                            TextFormula.Text,                                             //Formula
                                                            i_color.ToString(),                                           //Ch Color
                                                            TextMin.Text,                                                 //Min
                                                            TextMax.Text,                                                 //Max
                                                            CheckPreview.Checked.ToString(),                              //flg Preview
                                                            CheckShow.Checked.ToString(),                                 //flg Show
                                                            CheckUseFilter.Checked.ToString()                             //flg UseFilter
                                                         });
                } else
                {
                    // それ以外の場合、選択されている位置に挿入
                    dGVDecodeRule.Rows.Insert(dGVDecodeRule.SelectedRows[0].Index
                                            , new string[] {TextChName.Text,                                              //ChName
                                                            TextCanId.Text,                                               //ID
                                                            TextFormula.Text,                                             //Formula
                                                            i_color.ToString(),                                           //Ch Color
                                                            TextMin.Text,                                                 //Min
                                                            TextMax.Text,                                                 //Max
                                                            CheckPreview.Checked.ToString(),                              //flg Preview
                                                            CheckShow.Checked.ToString(),                                 //flg Show
                                                            CheckUseFilter.Checked.ToString()                             //flg UseFilter
                                                         });
                }
            }
            catch (Exception)
            {

                throw;
            }
                                                                 

            //ListViewをリロード
            LoadDecodeRuleToListView();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.decodeRule.Count == 0)
            {
                return;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.decodeRule.SaveToCSV(saveFileDialog.FileName);
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
                    string[] cols = reader.ReadLine().Split(',');
                    if (cols.Length == 9) {
                        decodeRule.AddData(cols[0],   //Ch Name
                                           cols[1],   //CAN ID
                                           cols[2],   //Formula
                                           cols[3],   //Ch Color
                                           cols[4],   //Min
                                           cols[5],   //Max
                                           cols[6],   //flg Preview
                                           cols[7],   //flg Show
                                           cols[8]    //flg UseFilter
                                           );
                    } else
                    {
                        decodeRule.AddData(cols[0],   //Ch Name
                                           cols[1],   //CAN ID
                                           cols[2],   //Formula
                                           cols[3],   //Ch Color
                                           cols[4],   //Min
                                           cols[5],   //Max
                                           cols[6],   //flg Preview
                                           cols[7],　 //flg Show
                                           "false"    //flg UseFilter
                                           );
                    }
                }
                reader.Close();
            }
            LoadDecodeRuleToListView();
        }

        private void LdGVDecdeRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 一旦ListViewで選択したあとに他のItemを選択する際に、SelectedItems.Countが-1になっていることがあるので
            // その場合実際にSelectedItemsがないのにアクセスするとエラーになるので、処理を飛ばす
            if (dGVDecodeRule.SelectedRows.Count > 0)
            {
                TextChName.Text = dGVDecodeRule.SelectedRows[0].Cells[0].Value.ToString();
                TextCanId.Text = dGVDecodeRule.SelectedRows[0].Cells[1].Value.ToString();
                TextFormula.Text = dGVDecodeRule.SelectedRows[0].Cells[2].Value.ToString();
                int i;
                i = int.Parse(dGVDecodeRule.SelectedRows[0].Cells[3].Value.ToString());
                Color color = Color.FromArgb(i);
                TextColor.BackColor = color;
                TextMin.Text = dGVDecodeRule.SelectedRows[0].Cells[4].Value.ToString();
                TextMax.Text = dGVDecodeRule.SelectedRows[0].Cells[5].Value.ToString();
                CheckPreview.Checked = System.Convert.ToBoolean(dGVDecodeRule.SelectedRows[0].Cells[6].Value.ToString());
                CheckShow.Checked = System.Convert.ToBoolean(dGVDecodeRule.SelectedRows[0].Cells[7].Value.ToString());
                CheckUseFilter.Checked = System.Convert.ToBoolean(dGVDecodeRule.SelectedRows[0].Cells[8].Value.ToString());
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FormMain fm = (FormMain)this.Owner;

            //FormMainのDecodeRuleをクリア
            fm.decodeRule.Clear();

            for (int i = 0; i < this.dGVDecodeRule.Rows.Count; i++)
            {
                //ListViewの内容をFormMainのListDecodeRuleに反映する
                fm.decodeRule.AddData(dGVDecodeRule.Rows[i].Cells[0].Value.ToString(),           //Ch Name
                                        dGVDecodeRule.Rows[i].Cells[1].Value.ToString(),         //CAN ID
                                        dGVDecodeRule.Rows[i].Cells[2].Value.ToString(),         //Formula
                                        dGVDecodeRule.Rows[i].Cells[3].Value.ToString(),         //Ch Color
                                        dGVDecodeRule.Rows[i].Cells[4].Value.ToString(),         //Min
                                        dGVDecodeRule.Rows[i].Cells[5].Value.ToString(),         //Max
                                        dGVDecodeRule.Rows[i].Cells[6].Value.ToString(),         //flg Preview
                                        dGVDecodeRule.Rows[i].Cells[7].Value.ToString(),         //flg Show
                                        dGVDecodeRule.Rows[i].Cells[8].Value.ToString()          //flg Use Filter
                                        );
            }


            //閉じる
            this.Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            //ListViewで選択がなければ即抜ける
            if (dGVDecodeRule.SelectedRows.Count == 0)
            {
                return;
            }
            
            //選択中のListViewIdxを取得
            int idx = dGVDecodeRule.SelectedRows[0].Index;

            //線のカラーをINTに変換
            int i_color;
            i_color = TextColor.BackColor.ToArgb();

            //TextBoxの内容をdecodeRuleに反映
            decodeRule.EditData(idx,                              //idx
                                TextChName.Text,                  //ChName
                                TextCanId.Text,                   //ID
                                TextFormula.Text,                 //Formula
                                i_color.ToString(),               //Ch Color
                                TextMin.Text,                     //Min
                                TextMax.Text,                     //Max
                                CheckPreview.Checked.ToString(),  //flg Preview
                                CheckShow.Checked.ToString(), 　  //flg Show                                                     
                                CheckUseFilter.Checked.ToString() //flg UseFilter
                                );

            //ListViewをリロード
            LoadDecodeRuleToListView();
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            //ListViewで選択がなければ即抜ける
            if (dGVDecodeRule.SelectedRows.Count == 0)
            {
                return;
            }

            //選択中のListViewIdxを取得
            int idx = dGVDecodeRule.SelectedRows[0].Index;

            //該当インデックスのdecodeRule削除
            this.decodeRule.DelData(idx);

            //ListViewをリロード
            LoadDecodeRuleToListView();
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

            ListCANID.Sort();

            // fileクローズ
            reader.Close();

            // 収集したCANIDの選択ダイアログを表示
            FormAnalysys f = new FormAnalysys
            {
                ListCANID = ListCANID
            };

            f.ShowDialog(this);
            f.Dispose();

            // このタイミングでDecodeRuleに、FormAnalysysで追加したルールが追加されているので、DecodeRuleを再ロードする
            LoadDecodeRuleToListView();
        }

        private void FormDecodeOption_Load(object sender, EventArgs e)
        {
            //decodeRule内容をFormMainからコピー
            CopyDecodeRuleFromMain();

            //decodeRule内容をListViewにロード
            LoadDecodeRuleToListView();
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            //アイテムが選択されていない場合抜ける
            if (dGVDecodeRule.SelectedRows.Count == 0) { return; }

            //先頭のアイテムが選択されている場合抜ける
            if (dGVDecodeRule.SelectedRows[0].Index == 0) { return; }

            //挿入先のインデックス
            int insPos = dGVDecodeRule.SelectedRows[0].Index - 1;

            //選択されたアイテム
            DataGridViewRow selItem = dGVDecodeRule.SelectedRows[0];

            //選択されたアイテムを削除する
            dGVDecodeRule.Rows.Remove(selItem);

            //アイテムを挿入する
            dGVDecodeRule.Rows.Insert(insPos, selItem);

            //ListViewの内容をdecodeRuleにコピー
            ListViewToDecodeRule();

        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            //アイテムが選択されていない場合抜ける
            if (dGVDecodeRule.SelectedRows.Count == 0) { return; }

            //末尾のアイテムが選択されている場合抜ける
            if (dGVDecodeRule.SelectedRows[0].Index == dGVDecodeRule.Rows.Count -1) { return; }

            //挿入先のインデックス
            int insPos = dGVDecodeRule.SelectedRows[0].Index + 1;

            //選択されたアイテム
            DataGridViewRow selItem = dGVDecodeRule.SelectedRows[0];

            //選択されたアイテムを削除する
            dGVDecodeRule.Rows.Remove(selItem);

            //アイテムを挿入する
            dGVDecodeRule.Rows.Insert(insPos, selItem);

            //ListViewの内容をdecodeRuleにコピー
            ListViewToDecodeRule();

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            //decodeRuleクリア
            this.decodeRule.Clear();

            //List Viewクリア
            dGVDecodeRule.Rows.Clear();
        }
    }
}
