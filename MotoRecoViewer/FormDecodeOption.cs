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
        /// FormMainのDecodeRuleをListViewにロードする
        /// </summary>
        private void LoadDecodeRuleToListView()
        {
            //decodeRuleがなければ何もしない
            if (this.decodeRule.Count == 0)
            {
                return;
            }

            //ListView描画停止
            ListViewDecode.BeginUpdate();

            //ListViewDecodeクリア
            ListViewDecode.Items.Clear();

            //DecodeRuleの内容をListViewDecodeに反映する
            for (int i = 0; i < this.decodeRule.Count; i++)
            {
                ListViewItem newItem = new ListViewItem
                {
                    UseItemStyleForSubItems = false
                };

                //ChName
                newItem.Text = this.decodeRule.GetChName(i);
                //CANID
                newItem.SubItems.Add(this.decodeRule.GetCANID(i).ToString("X3"));
                //Formula
                newItem.SubItems.Add(this.decodeRule.GetDecodeRule(i));
                //Pen Color
                newItem.SubItems.Add(this.decodeRule.GetChartColor(i).ToString());
                newItem.SubItems[3].BackColor = Color.FromArgb(this.decodeRule.GetChartColor(i));
                //Min
                newItem.SubItems.Add(this.decodeRule.GetChartMin(i).ToString());
                //MAX
                newItem.SubItems.Add(this.decodeRule.GetChartMax(i).ToString());
                //Preview
                newItem.SubItems.Add(this.decodeRule.GetChartPreview(i).ToString());
                //Show
                newItem.SubItems.Add(this.decodeRule.GetChartShow(i).ToString());

                ListViewDecode.Items.Add(newItem);
            }

            //ListView描画再開
            ListViewDecode.EndUpdate();
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
                this.decodeRule.AddData(fm.decodeRule.GetChName(i),                                 //Ch Name
                                        fm.decodeRule.GetCANID(i),                                  //CAN ID
                                        fm.decodeRule.GetDecodeRule(i),                             //Formula
                                        fm.decodeRule.GetChartColor(i),                             //Ch Color
                                        fm.decodeRule.GetChartMin(i),                               //Min
                                        fm.decodeRule.GetChartMax(i),                               //Max
                                        fm.decodeRule.GetChartPreview(i),                           //flg Preview
                                        fm.decodeRule.GetChartShow(i));                             //flg Show
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
                fm.decodeRule.AddData(this.decodeRule.GetChName(i),                                 //Ch Name
                                      this.decodeRule.GetCANID(i),                                  //CAN ID
                                      this.decodeRule.GetDecodeRule(i),                             //Formula
                                      this.decodeRule.GetChartColor(i),                             //Ch Color
                                      this.decodeRule.GetChartMin(i),                               //Min
                                      this.decodeRule.GetChartMax(i),                               //Max
                                      this.decodeRule.GetChartPreview(i),                           //flg Preview
                                      this.decodeRule.GetChartShow(i));                             //flg Show
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
                decodeRule.AddData(TextChName.Text,                                                         //ChName
                                ushort.Parse(TextCanId.Text, System.Globalization.NumberStyles.HexNumber),  //ID
                                TextFormula.Text,                                                           //Formula
                                i_color,                                                                    //Ch Color
                                int.Parse(TextMin.Text),                                                    //Min
                                int.Parse(TextMax.Text),                                                    //Max
                                bool.Parse(CheckPreview.Checked.ToString()),                                //flg Preview
                                bool.Parse(CheckShow.Checked.ToString()));                                  //flg Show
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
                    ListViewItem newItem = new ListViewItem
                    {
                        UseItemStyleForSubItems = false
                    };

                    string[] cols = reader.ReadLine().Split(',');

                    decodeRule.AddData(cols[0],                                                                    //Ch Name
                                       ushort.Parse(cols[1], System.Globalization.NumberStyles.HexNumber),         //CAN ID
                                       cols[2],                                                                    //Formula
                                       int.Parse(cols[3]),                                                         //Ch Color
                                       int.Parse(cols[4]),                                                         //Min
                                       int.Parse(cols[5]),                                                         //Max
                                       bool.Parse(cols[6]),                                                        //flg Preview
                                       bool.Parse(cols[7]));                                                       //flg Show
                }
                reader.Close();
            }
            LoadDecodeRuleToListView();
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
            //ListViewで選択がなければ即抜ける
            if (ListViewDecode.SelectedItems.Count == 0)
            {
                return;
            }
            
            //選択中のListViewIdxを取得
            int idx = ListViewDecode.SelectedIndices[0];

            //線のカラーをINTに変換
            int i_color;
            i_color = TextColor.BackColor.ToArgb();

            //TextBoxの内容をdecodeRuleに反映
            decodeRule.EditData(idx,                                                                        //idx
                                TextChName.Text,                                                            //ChName
                                ushort.Parse(TextCanId.Text, System.Globalization.NumberStyles.HexNumber),  //ID
                                TextFormula.Text,                                                           //Formula
                                i_color,                                                                    //Ch Color
                                int.Parse(TextMin.Text),                                                    //Min
                                int.Parse(TextMax.Text),                                                    //Max
                                bool.Parse(CheckPreview.Checked.ToString()),                                //flg Preview
                                bool.Parse(CheckShow.Checked.ToString()));                                  //flg Show                                                     

            //ListViewをリロード
            LoadDecodeRuleToListView();
        }

        private void TextColor_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            //ListViewで選択がなければ即抜ける
            if (ListViewDecode.SelectedItems.Count == 0)
            {
                return;
            }

            //選択中のListViewIdxを取得
            int idx = ListViewDecode.SelectedIndices[0];

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

        private void Button1_Click(object sender, EventArgs e)
        {
            //decodeRuleクリア
            this.decodeRule.Clear();

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

            ListCANID.Sort();

            // fileクローズ
            reader.Close();

            // 収集したCANIDの選択ダイアログを表示
            FormAnalysys f = new FormAnalysys();
            f.ListCANID = ListCANID;

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

        private void BtnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
