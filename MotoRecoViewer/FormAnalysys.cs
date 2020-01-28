using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotoRecoViewer
{
    public partial class FormAnalysys : Form
    {
        internal List<ushort> ListCANID;

        public FormAnalysys()
        {
            InitializeComponent();

            ListCANID = new List<ushort>();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FormDecodeOption fm = (FormDecodeOption)this.Owner;

            //LBAddのCANIDをFormMain.decodeRuleに追加する
            for (int i = 0; i< LBAdd.Items.Count; i++){
                //CANIDにData1～Data8まで存在するので一通り追加する
                for (int j = 1; j < 9; j++)
                {
                    var i_color = j switch
                    {
                        // ToDo もうちょっと色指定いい方法ないか…
                        1 => -32640,
                        2 => -128,
                        3 => -8323200,
                        4 => -8323073,
                        5 => -16744193,
                        6 => -32576,
                        7 => -65281,
                        8 => -32768,
                        _ => 0,
                    };
                    if (CBByte.Checked) { 
                        fm.decodeRule.AddData(LBAdd.Items[i] + "_Data" + j.ToString(),                                              //Ch Name
                                              ushort.Parse(LBAdd.Items[i].ToString(), System.Globalization.NumberStyles.HexNumber), //CAN ID
                                              "Data" + j.ToString(),                                                                //Formula
                                              i_color,                                                                              //Ch Color
                                              0,                                                                                    //Min
                                              255,                                                                                  //Max
                                              false,                                                                                //flg Preview
                                              false);                                                                               //flg Show
                    }

                    if (CBHigh.Checked)
                    {
                        fm.decodeRule.AddData(LBAdd.Items[i] + "_HiData" + j.ToString(),                                            //Ch Name
                                              ushort.Parse(LBAdd.Items[i].ToString(), System.Globalization.NumberStyles.HexNumber), //CAN ID
                                              "HiData" + j.ToString(),                                                              //Formula
                                              i_color,                                                                              //Ch Color
                                              0,                                                                                    //Min
                                              255,                                                                                  //Max
                                              false,                                                                                //flg Preview
                                              false);                                                                               //flg Show
                    }

                    if (CBLow.Checked)
                    {
                        fm.decodeRule.AddData(LBAdd.Items[i] + "_LoData" + j.ToString(),                                            //Ch Name
                                              ushort.Parse(LBAdd.Items[i].ToString(), System.Globalization.NumberStyles.HexNumber), //CAN ID
                                              "LoData" + j.ToString(),                                                              //Formula
                                              i_color,                                                                              //Ch Color
                                              0,                                                                                    //Min
                                              255,                                                                                  //Max
                                              false,                                                                                //flg Preview
                                              false);                                                                               //flg Show
                    }
                }
            }

            //閉じる
            this.Close();
        }

        private void FormAnalysys_Load(object sender, EventArgs e)
        {
            LBRemove.Items.Clear();

            // ListCANIDのIDをLBRemoveに登録
            for (int i = 0; i < ListCANID.Count; i++)
            {
                LBRemove.Items.Add(ListCANID[i].ToString("X3"));
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //まず選択されたものを右に移動する
            for (int i=0; i<LBRemove.SelectedItems.Count; i++)
            {
                LBAdd.Items.Add(LBRemove.SelectedItems[i]);
            }

            //つぎに左側で選択されたものを削除する
            while (LBRemove.SelectedItems.Count != 0)
            {
                LBRemove.Items.RemoveAt(LBRemove.SelectedIndices[0]);
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            //まず選択されたものを右に移動する
            for (int i = 0; i < LBAdd.SelectedItems.Count; i++)
            {
                LBRemove.Items.Add(LBAdd.SelectedItems[i]);
            }

            //つぎに左側で選択されたものを削除する
            while (LBAdd.SelectedItems.Count != 0)
            {
                LBAdd.Items.RemoveAt(LBAdd.SelectedIndices[0]);
            }
        }

        private void BtnAddAll_Click(object sender, EventArgs e)
        {
            //全て右に移動する
            for (int i = 0; i < LBRemove.Items.Count; i++)
            {
                LBAdd.Items.Add(LBRemove.Items[i]);
            }

            //つぎに左側をクリアする
            LBRemove.Items.Clear();
        }

        private void BtnRemoveAll_Click(object sender, EventArgs e)
        {
            //全て左に移動する
            for (int i = 0; i < LBAdd.Items.Count; i++)
            {
                LBRemove.Items.Add(LBAdd.Items[i]);
            }

            //つぎに左側をクリアする
            LBAdd.Items.Clear();
        }
    }
}
