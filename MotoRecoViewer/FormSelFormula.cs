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
                case "Data":
                    // ListViewコントロールにデータ追加
                    // itemMAX
                    string[] item1 = { "Data1", "", "#Data1" };
                    ListViewItem.Items.Add(new ListViewItem(item1));

                    string[] item2 = { "HiData1", "", "#HiData1" };
                    ListViewItem.Items.Add(new ListViewItem(item2));

                    string[] item3 = { "LoData1", "", "#LoData1" };
                    ListViewItem.Items.Add(new ListViewItem(item3));

                    string[] item4 = { "Data2", "", "#Data2" };
                    ListViewItem.Items.Add(new ListViewItem(item4));

                    string[] item5 = { "HiData2", "", "#HiData2" };
                    ListViewItem.Items.Add(new ListViewItem(item5));

                    string[] item6 = { "LoData2", "", "#LoData2" };
                    ListViewItem.Items.Add(new ListViewItem(item6));

                    string[] item7 = { "Data3", "", "#Data3" };
                    ListViewItem.Items.Add(new ListViewItem(item7));

                    string[] item8 = { "HiData3", "", "#HiData3" };
                    ListViewItem.Items.Add(new ListViewItem(item8));

                    string[] item9 = { "LoData3", "", "#LoData3" };
                    ListViewItem.Items.Add(new ListViewItem(item9));

                    string[] item10 = { "Data4", "", "#Data4" };
                    ListViewItem.Items.Add(new ListViewItem(item10));

                    string[] item11= { "HiData4", "", "#HiData4" };
                    ListViewItem.Items.Add(new ListViewItem(item11));

                    string[] item12 = { "LoData4", "", "#LoData4" };
                    ListViewItem.Items.Add(new ListViewItem(item12));

                    string[] item13 = { "Data5", "", "#Data5" };
                    ListViewItem.Items.Add(new ListViewItem(item13));

                    string[] item14 = { "HiData5", "", "#HiData5" };
                    ListViewItem.Items.Add(new ListViewItem(item14));

                    string[] item15 = { "LoData5", "", "#LoData5" };
                    ListViewItem.Items.Add(new ListViewItem(item15));

                    string[] item16 = { "Data6", "", "#Data6" };
                    ListViewItem.Items.Add(new ListViewItem(item16));

                    string[] item17 = { "HiData6", "", "#HiData6" };
                    ListViewItem.Items.Add(new ListViewItem(item17));

                    string[] item18 = { "LoData6", "", "#LoData6" };
                    ListViewItem.Items.Add(new ListViewItem(item18));

                    string[] item19 = { "Data7", "", "#Data7" };
                    ListViewItem.Items.Add(new ListViewItem(item19));

                    string[] item20 = { "HiData7", "", "#HiData7" };
                    ListViewItem.Items.Add(new ListViewItem(item20));

                    string[] item21 = { "LoData7", "", "#LoData7" };
                    ListViewItem.Items.Add(new ListViewItem(item21));

                    string[] item22 = { "Data8", "", "#Data8" };
                    ListViewItem.Items.Add(new ListViewItem(item22));

                    string[] item23 = { "HiData8", "", "#HiData8" };
                    ListViewItem.Items.Add(new ListViewItem(item23));

                    string[] item24 = { "LoData8", "", "#LoData8" };
                    ListViewItem.Items.Add(new ListViewItem(item24));
                    return;

                case "GPS":
                    // ListViewコントロールにデータ追加
                    // itemMAX 5
                    string[] item101 = { "GPS Latitude", "7FF", "#GPS_Latitude" };
                    ListViewItem.Items.Add(new ListViewItem(item101));

                    string[] item102 = { "GPS Longitude", "7FF", "#GPS_Longitude" };
                    ListViewItem.Items.Add(new ListViewItem(item102));

                    string[] item103 = { "GPS Speed", "7FE", "#GPS_Speed" };
                    ListViewItem.Items.Add(new ListViewItem(item103));

                    string[] item104 = { "GPS Altitude", "7FE", "#GPS_Altitude" };
                    ListViewItem.Items.Add(new ListViewItem(item104));

                    string[] item105 = { "GPS Distance", "7FE", "#GPS_Distance" };
                    ListViewItem.Items.Add(new ListViewItem(item105));

                    return;

                case "K51":
                    // ListViewコントロールにデータ追加
                    // itemMAX : 48
                    string[] item201 = { "RPM", "10C", "#K51_RPM" };
                    ListViewItem.Items.Add(new ListViewItem(item201));

                    string[] item202 = { "ThrottelValvePosition", "10C", "#K51_ThrottelValvePosition" };
                    ListViewItem.Items.Add(new ListViewItem(item202));

                    string[] item203 = { "ThrottelPosition", "10C", "#K51_ThrottelPosition" };
                    ListViewItem.Items.Add(new ListViewItem(item203));

                    string[] item204 = { "Throttel Grip Position", "110", "#K51_ThrottelGripPosition" };
                    ListViewItem.Items.Add(new ListViewItem(item204));

                    string[] item205 = { "Ignition Timing", "110", "#K51_IgnitionTiming" };
                    ListViewItem.Items.Add(new ListViewItem(item205));

                    string[] item206 = { "FrBrake1", "120", "#K51_FrBrake1" };
                    ListViewItem.Items.Add(new ListViewItem(item206));

                    string[] item207 = { "FrBrake2", "120", "#K51_FrBrake2" };
                    ListViewItem.Items.Add(new ListViewItem(item207));

                    string[] item208 = { "RrBrake1", "120", "#K51_RrBrake1" };
                    ListViewItem.Items.Add(new ListViewItem(item208));

                    string[] item209 = { "RrBrake2", "120", "#K51_RrBrake2" };
                    ListViewItem.Items.Add(new ListViewItem(item209));

                    string[] item210 = { "FrStroke", "29C", "#K51_FrStroke" };
                    ListViewItem.Items.Add(new ListViewItem(item210));

                    string[] item211 = { "RrStroke", "29C", "#K51_RrStroke" };
                    ListViewItem.Items.Add(new ListViewItem(item211));

                    string[] item212 = { "LeanAngle", "10C", "#K51_LeanAngle" };
                    ListViewItem.Items.Add(new ListViewItem(item212));

                    string[] item213 = { "FrSpeed1", "293", "#K51_FrSpeed1" };
                    ListViewItem.Items.Add(new ListViewItem(item213));

                    string[] item214 = { "DistFrSpeed1", "293", "#K51_DistFrSpeed1" };
                    ListViewItem.Items.Add(new ListViewItem(item214));

                    string[] item215 = { "FrSpeed2", "293", "#K51_FrSpeed2" };
                    ListViewItem.Items.Add(new ListViewItem(item215));

                    string[] item216 = { "RrSpeed","293", "#K51_RrSpeed" };
                    ListViewItem.Items.Add(new ListViewItem(item216));

                    string[] item217 = { "SlipRate", "293", "#K51_SlipRate" };
                    ListViewItem.Items.Add(new ListViewItem(item217));

                    string[] item218 = { "Gear", "2BC", "#K51_Gear" };
                    ListViewItem.Items.Add(new ListViewItem(item218));


                    string[] item219 = { "X_Axis_G", "178", "#K51_XAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item219));

                    string[] item220 = { "Y_Axis_G", "174", "#K51_YAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item220));

                    string[] item221 = { "Z_Axis_G", "17C", "#K51_ZAxisG" };
                    ListViewItem.Items.Add(new ListViewItem(item221));
                    
                    string[] item222 = { "RollRate", "178", "#K51_RollRate" };
                    ListViewItem.Items.Add(new ListViewItem(item222));

                    string[] item223 = { "YawRate", "174", "#K51_YawRate" };
                    ListViewItem.Items.Add(new ListViewItem(item223));


                    string[] item224 = { "AirTemp", "3FA", "#K51_AirTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item224));

                    string[] item225 = { "WaterTemp", "2BC", "#K51_WaterTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item225));

                    string[] item226 = { "OilTemp", "3FA", "#K51_OilTemp" };
                    ListViewItem.Items.Add(new ListViewItem(item226));


                    string[] item227 = { "DistCountFr", "2B0", "#K51_DistCountFr" };
                    ListViewItem.Items.Add(new ListViewItem(item227));

                    string[] item228 = { "AccumulatedDistCountFr", "2B0", "#K51_AccumulatedDistCountFr" };
                    ListViewItem.Items.Add(new ListViewItem(item228));

                    string[] item229 = { "DistanceCounterRr", "2B0", "#K51_DistCountRr" };
                    ListViewItem.Items.Add(new ListViewItem(item229));

                    string[] item230 = { "AccumulatedDistCountRr", "2B0", "#K51_AccumulatedDistCountRr" };
                    ListViewItem.Items.Add(new ListViewItem(item230));

                    string[] item231 = { "FuelCounter", "2BC", "#K51_FuelCount" };
                    ListViewItem.Items.Add(new ListViewItem(item231));

                    string[] item232 = { "AccumulatedFuelCounter", "2BC", "#K51_AccumulatedFuelCount" };
                    ListViewItem.Items.Add(new ListViewItem(item232));

                    string[] item233 = { "FuelLevel", "2D0", "#K51_FuelLevel" };
                    ListViewItem.Items.Add(new ListViewItem(item233));

                    string[] item234 = { "OdMeter", "3F8", "#K51_OdMeter" };
                    ListViewItem.Items.Add(new ListViewItem(item234));

                    string[] item235 = { "FuelConsumption", "2BC", "#K51_FuelConsumption" };
                    ListViewItem.Items.Add(new ListViewItem(item235));

                    string[] item236 = { "Range", "2BC", "#K51_Range" };
                    ListViewItem.Items.Add(new ListViewItem(item236));


                    return;

                default:
                    return;
            }
        }
    }
}
