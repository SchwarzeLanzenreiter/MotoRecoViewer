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

                    string[] item5 = { "GPSDistance", "7FE", "#GPS_Distance" };
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

                    string[] item221 = { "DistFrSpeed1", "293", "#K51_DistFrSpeed1" };
                    ListViewItem.Items.Add(new ListViewItem(item221));

                    string[] item23 = { "FrSpeed2", "293", "#K51_FrSpeed2" };
                    ListViewItem.Items.Add(new ListViewItem(item23));

                    string[] item24 = { "RrSpeed","293", "#K51_RrSpeed" };
                    ListViewItem.Items.Add(new ListViewItem(item24));

                    string[] item25 = { "SlipRate", "293", "#K51_SlipRate" };
                    ListViewItem.Items.Add(new ListViewItem(item25));

                    string[] item26 = { "Gear", "2BC", "#K51_Gear" };
                    ListViewItem.Items.Add(new ListViewItem(item26));


                    string[] item30 = { "X_Axis_G", "178", "#K51_XAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item30));

                    string[] item31 = { "Y_Axis_G", "174", "#K51_YAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item31));

                    string[] item32 = { "Z_Axis_G", "17C", "#K51_ZAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item32));
                    
                    string[] item33 = { "RollRate", "178", "#K51_RollRate" };
                    ListViewItem.Items.Add(new ListViewItem(item33));

                    string[] item34 = { "YawRate", "174", "#K51_YawRate" };
                    ListViewItem.Items.Add(new ListViewItem(item34));


                    string[] item35 = { "AirTemp", "3FA", "#K51_AirTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item35));

                    string[] item36 = { "WaterTemp", "2BC", "#K51_WaterTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item36));

                    string[] item37 = { "OilTemp", "3FA", "#K51_OilTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item37));


                    string[] item38 = { "DistCountFr", "2B0", "#K51_DistCountFr" };
                    ListViewItem.Items.Add(new ListViewItem(item38));

                    string[] item39 = { "AccumulatedDistanceCounterFr", "2B0", "#K51_AccumulatedDistCountFr" };
                    ListViewItem.Items.Add(new ListViewItem(item39));

                    string[] item40 = { "DistanceCounterRr", "2B0", "#K51_DistCountRr" };
                    ListViewItem.Items.Add(new ListViewItem(item40));

                    string[] item41 = { "AccumulatedDistanceCounterRr", "2B0", "#K51_AccumulatedDistCountRr" };
                    ListViewItem.Items.Add(new ListViewItem(item41));

                    string[] item42 = { "FuelCounter", "2BC", "#K51_FuelCount" };
                    ListViewItem.Items.Add(new ListViewItem(item42));

                    string[] item43 = { "AccumulatedFuelCounter", "2BC", "#K51_AccumulatedFuelCount" };
                    ListViewItem.Items.Add(new ListViewItem(item43));

                    string[] item44 = { "FuelLevel", "2D0", "#K51_FuelLevel" };
                    ListViewItem.Items.Add(new ListViewItem(item44));

                    string[] item45 = { "OdMeter", "3F8", "#K51_OdMeter" };
                    ListViewItem.Items.Add(new ListViewItem(item45));



                    return;

                default:
                    return;
            }
        }
    }
}
