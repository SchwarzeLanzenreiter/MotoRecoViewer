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
    public partial class FormSelFormula : Form
    {
        public FormSelFormula()
        {
            InitializeComponent();
        }

        private void ListViewItem_DoubleClick(object sender, EventArgs e)
        {
            //アイテムが選択状態ではなければ、何もしない
            if (ListViewItem.SelectedItems.Count == 0)
            {
                return;
            }

            FormDecodeOption fm = (FormDecodeOption)this.Owner;

            fm.ReceiveFixedChName = ListViewItem.SelectedItems[0].SubItems[0].Text;
            fm.ReceiveFixedCANID = ListViewItem.SelectedItems[0].SubItems[1].Text;
            fm.ReceiveFixedFormula = ListViewItem.SelectedItems[0].SubItems[2].Text;

            //閉じる
            this.Close();
        }

        private void TreeViewBike_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // まずリストビューをクリアする
            ListViewItem.Items.Clear();
         
            // TreeViewのノードを切り替える（車種を切り替える）毎に、その車種で利用可能な固定演算式候補をリストビューにロードする
            switch (TreeViewBike.SelectedNode.Text)
            {
                case "GPS":
                    // ListViewコントロールにデータ追加
                    string[] item1 = { "Latitude", "7FF", "#GPS_Latitude" };
                    ListViewItem.Items.Add(new ListViewItem(item1));

                    string[] item2 = { "Longitude", "7FF", "#GPS_Longitude" };
                    ListViewItem.Items.Add(new ListViewItem(item2));

                    string[] item3 = { "Speed", "7FE", "#GPS_Speed" };
                    ListViewItem.Items.Add(new ListViewItem(item3));

                    string[] item4 = { "Altitude", "7FE", "#GPS_Altitude" };
                    ListViewItem.Items.Add(new ListViewItem(item4));

                    string[] item5 = { "Distance", "7FE", "#GPS_Distance" };
                    ListViewItem.Items.Add(new ListViewItem(item5));

                    return;

                case "K51":
                    // ListViewコントロールにデータ追加
                    string[] item11 = { "RPM", "10C", "#K51_RPM" };
                    ListViewItem.Items.Add(new ListViewItem(item11));

                    string[] item12 = { "ThrottelValvePosition", "10C", "#K51_ThrottelValvePosition" };
                    ListViewItem.Items.Add(new ListViewItem(item12));

                    string[] item13 = { "ThrottelPosition", "10C", "#K51_ThrottelPosition" };
                    ListViewItem.Items.Add(new ListViewItem(item13));

                    string[] item14 = { "Throttel Grip Position", "110", "#K51_ThrottelGripPosition" };
                    ListViewItem.Items.Add(new ListViewItem(item14));

                    string[] item15 = { "FrBrake1", "120", "#K51_FrBrake1" };
                    ListViewItem.Items.Add(new ListViewItem(item15));

                    string[] item16 = { "FrBrake2", "120", "#K51_FrBrake2" };
                    ListViewItem.Items.Add(new ListViewItem(item16));

                    string[] item17 = { "RrBrake1", "120", "#K51_RrBrake1" };
                    ListViewItem.Items.Add(new ListViewItem(item17));

                    string[] item18 = { "RrBrake2", "120", "#K51_RrBrake2" };
                    ListViewItem.Items.Add(new ListViewItem(item18));

                    string[] item19 = { "FrStroke", "29C", "#K51_FrStroke" };
                    ListViewItem.Items.Add(new ListViewItem(item19));

                    string[] item20 = { "RrStroke", "29C", "#K51_RrStroke" };
                    ListViewItem.Items.Add(new ListViewItem(item20));

                    string[] item21 = { "LeanAngle", "10C", "#K51_LeanAngle" };
                    ListViewItem.Items.Add(new ListViewItem(item21));

                    string[] item22 = { "FrSpeed1", "293", "#K51_FrSpeed1" };
                    ListViewItem.Items.Add(new ListViewItem(item22));

                    string[] item23 = { "FrSpeed2", "293", "#K51_FrSpeed2" };
                    ListViewItem.Items.Add(new ListViewItem(item23));

                    string[] item24 = { "RrSpeed","293", "#K51_RrSpeed" };
                    ListViewItem.Items.Add(new ListViewItem(item24));

                    string[] item25 = { "SlipRate", "293", "#K51_SlipRate" };
                    ListViewItem.Items.Add(new ListViewItem(item25));

                    string[] item26 = { "X_Axis_G", "178", "#K51_XAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item26));

                    string[] item27 = { "Y_Axis_G", "174", "#K51_YAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item27));

                    string[] item28 = { "Z_Axis_G", "17C", "#K51_ZAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item28));
                    
                    string[] item29 = { "RollRate", "178", "#K51_RollRate" };
                    ListViewItem.Items.Add(new ListViewItem(item29));

                    string[] item30 = { "YawRate", "174", "#K51_YawRate" };
                    ListViewItem.Items.Add(new ListViewItem(item30));



                    return;

                default:
                    return;
            }
        }
    }
}
