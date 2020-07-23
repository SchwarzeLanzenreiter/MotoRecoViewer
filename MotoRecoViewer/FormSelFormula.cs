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
            fm.ReceiveFixedColor = ListViewItem.SelectedItems[0].SubItems[3].Text;
            fm.ReceiveFixedMin = ListViewItem.SelectedItems[0].SubItems[4].Text;
            fm.ReceiveFixedMax = ListViewItem.SelectedItems[0].SubItems[5].Text;

            //閉じる
            this.Close();
        }

        private void ChangeColColor()
        {
            // ChColorのバックカラーをまとめて変更する
            for (int i = 0; i < ListViewItem.Items.Count; i++)
            {
                int j;

                ListViewItem.Items[i].UseItemStyleForSubItems = false;

                j = int.Parse(ListViewItem.Items[i].SubItems[3].Text);
                ListViewItem.Items[i].SubItems[3].BackColor = Color.FromArgb(j);
            }
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
                    string[] item1 = { "Data1", "", "#Data1", "-32640", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item1));
                 
                    string[] item2 = { "HiData1", "", "#HiData1", "-32640", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item2));

                    string[] item3 = { "LoData1", "", "#LoData1", "-32640", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item3));

                    string[] item4 = { "Data2", "", "#Data2", "-128", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item4));

                    string[] item5 = { "HiData2", "", "#HiData2", "-128", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item5));

                    string[] item6 = { "LoData2", "", "#LoData2", "-128", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item6));

                    string[] item7 = { "Data3", "", "#Data3", "-8323200", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item7));

                    string[] item8 = { "HiData3", "", "#HiData3", "-8323200", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item8));

                    string[] item9 = { "LoData3", "", "#LoData3", "-8323200", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item9));

                    string[] item10 = { "Data4", "", "#Data4", "-8323073", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item10));

                    string[] item11= { "HiData4", "", "#HiData4", "-8323073", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item11));

                    string[] item12 = { "LoData4", "", "#LoData4", "-8323073", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item12));

                    string[] item13 = { "Data5", "", "#Data5", "-16744193", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item13));

                    string[] item14 = { "HiData5", "", "#HiData5", "-16744193", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item14));

                    string[] item15 = { "LoData5", "", "#LoData5", "-16744193", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item15));

                    string[] item16 = { "Data6", "", "#Data6", "-32576", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item16));

                    string[] item17 = { "HiData6", "", "#HiData6", "-32576", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item17));

                    string[] item18 = { "LoData6", "", "#LoData6", "-32576", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item18));

                    string[] item19 = { "Data7", "", "#Data7", "-65281", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item19));

                    string[] item20 = { "HiData7", "", "#HiData7", "-65281", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item20));

                    string[] item21 = { "LoData7", "", "#LoData7", "-65281", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item21));

                    string[] item22 = { "Data8", "", "#Data8", "-32768", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item22));

                    string[] item23 = { "HiData8", "", "#HiData8", "-32768", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item23));

                    string[] item24 = { "LoData8", "", "#LoData8", "-32768", "0", "255" };
                    ListViewItem.Items.Add(new ListViewItem(item24));

                    // ChColorのバックカラーをまとめて変更する
                    ChangeColColor();

                    return;

                case "GPS":
                    // ListViewコントロールにデータ追加
                    // itemMAX 5
                    string[] item101 = { "GPS Latitude", "7FF", "#GPS_Latitude", "-16711936", "-90", "90" };
                    ListViewItem.Items.Add(new ListViewItem(item101));

                    string[] item102 = { "GPS Longitude", "7FF", "#GPS_Longitude", "-256", "-180", "180" };
                    ListViewItem.Items.Add(new ListViewItem(item102));

                    string[] item103 = { "GPS Speed", "7FE", "#GPS_Speed", "-65536", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item103));

                    string[] item104 = { "GPS Altitude", "7FE", "#GPS_Altitude", "-16711681", "0", "3500" };
                    ListViewItem.Items.Add(new ListViewItem(item104));

                    string[] item105 = { "GPS Distance", "7FE", "#GPS_Distance", "-65536", "0", "300" };
                    ListViewItem.Items.Add(new ListViewItem(item105));

                    // ChColorのバックカラーをまとめて変更する
                    ChangeColColor();

                    return;

                case "K51":
                    // ListViewコントロールにデータ追加
                    // itemMAX : 48
                    string[] item201 = { "RPM", "10C", "#K51_RPM", "-2866", "0", "10000" };
                    ListViewItem.Items.Add(new ListViewItem(item201));

                    string[] item202 = { "ThrottelValvePosition", "10C", "#K51_ThrottelValvePosition", "-16711872", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item202));

                    string[] item203 = { "ThrottelPosition", "10C", "#K51_ThrottelPosition", "-256", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item203));

                    string[] item204 = { "Throttel Grip Position", "110", "#K51_ThrottelGripPosition", "-16711681", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item204));

                    //string[] item205 = { "Ignition Timing", "110", "#K51_IgnitionTiming", "-65408", "0", "100" };
                    //ListViewItem.Items.Add(new ListViewItem(item205));

                    string[] item206 = { "FrBrake1", "120", "#K51_FrBrake1", "-65408", "0", "10" };
                    ListViewItem.Items.Add(new ListViewItem(item206));

                    string[] item207 = { "FrBrake2", "120", "#K51_FrBrake2", "-8372224", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item207));

                    string[] item208 = { "RrBrake1", "120", "#K51_RrBrake1", "-32768", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item208));

                    string[] item209 = { "RrBrake2", "120", "#K51_RrBrake2", "-8372224", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item209));

                    string[] item210 = { "FrStroke", "29C", "#K51_FrStroke", "-8355585", "0", "300" };
                    ListViewItem.Items.Add(new ListViewItem(item210));

                    string[] item211 = { "RrStroke", "29C", "#K51_RrStroke", "-16744193", "0", "300" };
                    ListViewItem.Items.Add(new ListViewItem(item211));

                    string[] item212 = { "LeanAngle", "10C", "#K51_LeanAngle", "-256", "-50", "50" };
                    ListViewItem.Items.Add(new ListViewItem(item212));

                    string[] item213 = { "FrSpeed1", "293", "#K51_FrSpeed1", "-65536", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item213));

                    string[] item214 = { "DistFrSpeed1", "293", "#K51_DistFrSpeed1", "-8323328", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item214));

                    string[] item215 = { "FrSpeed2", "293", "#K51_FrSpeed2", "-8323328", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item215));

                    string[] item216 = { "RrSpeed","293", "#K51_RrSpeed", "-16711681", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item216));

                    string[] item217 = { "SlipRate", "293", "#K51_SlipRate", "-128", "-10", "10" };
                    ListViewItem.Items.Add(new ListViewItem(item217));

                    string[] item218 = { "Gear", "2BC", "#K51_Gear", "-16744448", "-54", "6" };
                    ListViewItem.Items.Add(new ListViewItem(item218));


                    string[] item219 = { "X_Axis_G", "178", "#K51_XAxisG", "-8355840", "-2", "2" };
                    ListViewItem.Items.Add(new ListViewItem(item219));

                    string[] item220 = { "Y_Axis_G", "174", "#K51_YAxisG", "-16711936", "-2", "2" };
                    ListViewItem.Items.Add(new ListViewItem(item220));

                    string[] item221 = { "Z_Axis_G", "17C", "#K51_ZAxisG", "-16744448", "-2", "2" };
                    ListViewItem.Items.Add(new ListViewItem(item221));
                    
                    string[] item222 = { "RollRate", "178", "#K51_RollRate", "-32768", "-125", "125" };
                    ListViewItem.Items.Add(new ListViewItem(item222));

                    string[] item223 = { "YawRate", "174", "#K51_YawRate", "-65281", "-125", "125" };
                    ListViewItem.Items.Add(new ListViewItem(item223));


                    string[] item224 = { "AirTemp", "3FA", "#K51_AirTemp", "-16711681", "-10", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item224));

                    string[] item225 = { "WaterTemp", "2BC", "#K51_WaterTemp", "-16776961", "-10", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item225));

                    string[] item226 = { "OilTemp", "3FA", "#K51_OilTemp", "-32704", "-10", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item226));


                    string[] item227 = { "DistCountFr", "2B0", "#K51_DistCountFr", "-4144960", "0", "70000" };
                    ListViewItem.Items.Add(new ListViewItem(item227));

                    string[] item228 = { "AccumulatedDistCountFr", "2B0", "#K51_AccumulatedDistCountFr", "-32576", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item228));

                    string[] item229 = { "DistCountRr", "2B0", "#K51_DistCountRr", "-4144960", "0", "256" };
                    ListViewItem.Items.Add(new ListViewItem(item229));

                    string[] item230 = { "AccumulatedDistCountRr", "2B0", "#K51_AccumulatedDistCountRr", "-8355585", "0", "200" };
                    ListViewItem.Items.Add(new ListViewItem(item230));

                    string[] item231 = { "FuelCounter", "2BC", "#K51_FuelCount", "-4144960", "0", "70000" };
                    ListViewItem.Items.Add(new ListViewItem(item231));

                    string[] item232 = { "AccumulatedFuelCounter", "2BC", "#K51_AccumulatedFuelCount", "-32768", "0", "5" };
                    ListViewItem.Items.Add(new ListViewItem(item232));

                    string[] item233 = { "FuelLevel", "2D0", "#K51_FuelLevel", "-256", "0", "100" };
                    ListViewItem.Items.Add(new ListViewItem(item233));

                    string[] item234 = { "OdMeter", "3F8", "#K51_OdMeter", "-12550016", "0", "100000" };
                    ListViewItem.Items.Add(new ListViewItem(item234));

                    string[] item235 = { "FuelConsumption", "2BC", "#K51_FuelConsumption", "-16711936", "0", "30" };
                    ListViewItem.Items.Add(new ListViewItem(item235));

                    string[] item236 = { "Range", "2BC", "#K51_Range", "-16711681", "0", "700" };
                    ListViewItem.Items.Add(new ListViewItem(item236));

                    // ChColorのバックカラーをまとめて変更する
                    ChangeColColor();

                    return;

                default:
                    return;
            }
        }
    }
}
