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

using System.Threading.Tasks;

namespace MotoRecoViewer
{
    /// <summary>
    /// デコード済チャンネルから、派生チャンネル(積算距離・消費燃料・燃費・走行可能距離)を計算する。
    /// 画面には依存せず、ChannelStoreとDecodeRuleだけを見る。
    /// </summary>
    class ChannelAnalyzer
    {
        private readonly ChannelStore channels;
        private readonly DecodeRule decodeRule;

        public ChannelAnalyzer(ChannelStore channels, DecodeRule decodeRule)
        {
            this.channels = channels;
            this.decodeRule = decodeRule;
        }

        /// <summary>
        /// 派生チャンネルを、依存関係の順に計算する
        /// </summary>
        public void CalcDerivedChannels()
        {
            // この5関数は依存関係なし→並列処理可
            Parallel.Invoke(
                () => CalcGPSDistance(),                    //GPS積算距離を計算
                () => CalcFrSpeedDistance(),                //FrSpeed積算距離を計算
                () => CalcAccumulatedDistCountFr(),         //Fr距離カウンタから積算距離を計算
                () => CalcAccumulatedDistCountRr(),         //Rr距離カウンタから積算距離を計算
                () => CalcAccumulatedFuelCount()            //燃料カウンタから消費燃料を計算
            );

            //燃費を出すには、積算距離と積算燃料が計算できていないといけない
            //燃費を計算
            CalcFuelConsumption();

            //Rangeを出すには、燃費が計算できていないといけない
            //走行可能距離を計算
            CalcRange();
        }

        /// <summary>
        /// 固定式に対応するチャンネルのインデックスを返す。存在しない場合は-1。
        /// </summary>
        public int IndexOfFixedFormula(string FixedFormula)
        {
            //FixedFormulaのChName取得
            int i = decodeRule.FormulaIndexOf(FixedFormula);

            //DecodeRuleに定義がない
            if (i == -1)
            {
                return -1;
            }

            string chNameDistCount = decodeRule.GetChName(i);

            //　該当CAN IDが存在しないケースも有りうることに注意
            // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
            if (!channels.Contains(chNameDistCount))
            {
                return -1;
            }
            return channels.IndexOf(chNameDistCount);
        }

        /// <summary>
        /// ＃GPS_Speedから#GPS_Distanceを計算する
        /// </summary>
        private void CalcGPSDistance() {
            //#GPS_Speed のindex取得
            int idx_GPSSpeed = IndexOfFixedFormula("#GPS_Speed");
            if (idx_GPSSpeed < 0) { return; }

            //#GPS_Distanc のChName取得
            int idx_GPSDistance = IndexOfFixedFormula("#GPS_Distance");
            if (idx_GPSDistance < 0) { return; }


            //#GPS_Distanceの積分計算
            TVData tvData;

            tvData = channels[idx_GPSDistance].LogData[0];
            tvData.DataValue = 0.0;
            channels[idx_GPSDistance].LogData[0] = tvData;

            for (int i=1; i<channels[idx_GPSSpeed].Count; i++)
            { 
                // GPSSpeed unit:km/h
                double dSpeed = channels[idx_GPSSpeed].LogData[i].DataValue;

                // time diff unit:sec
                double timeDiff = channels[idx_GPSSpeed].LogData[i].DataTime - channels[idx_GPSSpeed].LogData[i-1].DataTime;

                tvData = channels[idx_GPSDistance].LogData[i];

                // 積算距離は km で考える
                tvData.DataValue = channels[idx_GPSDistance].LogData[i - 1].DataValue + (dSpeed / 3600) * timeDiff;
                channels[idx_GPSDistance].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_FrSpeed1から#BMW_DistFrSpeed1を計算する
        /// </summary>
        private void CalcFrSpeedDistance()
        {
            //#BMW_FrSpeed1 のChName取得
            int idx_FrSpeed = IndexOfFixedFormula("#BMW_FrSpeed1");
            if (idx_FrSpeed < 0) { return; }

            //#BMW_DistFrSpeed1 のChName取得
            int idx_DistFrSpeed = IndexOfFixedFormula("#BMW_DistFrSpeed1");
            if (idx_DistFrSpeed < 0) { return; }

            //#GPS_Distanceの積分計算
            TVData tvData;

            tvData = channels[idx_DistFrSpeed].LogData[0];
            tvData.DataValue = 0.0;
            channels[idx_DistFrSpeed].LogData[0] = tvData;

            for (int i = 1; i < channels[idx_FrSpeed].Count; i++)
            {
                // GPSSpeed unit:km/h
                double dSpeed = channels[idx_FrSpeed].LogData[i].DataValue;

                // time diff unit:sec
                double timeDiff = channels[idx_FrSpeed].LogData[i].DataTime - channels[idx_FrSpeed].LogData[i - 1].DataTime;

                tvData = channels[idx_DistFrSpeed].LogData[i];

                // 積算距離は km で考える
                tvData.DataValue = channels[idx_DistFrSpeed].LogData[i - 1].DataValue + (dSpeed / 3600) * timeDiff;
                channels[idx_DistFrSpeed].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_DistCountFr から K51_AccumulatedDistCountFrを計算する
        /// </summary>
        private void CalcAccumulatedDistCountFr()
        {
            //#BMW_DistCountFr のChName取得
            int idx_DistCount = IndexOfFixedFormula("#BMW_DistCountFr");
            if (idx_DistCount < 0) { return; }

            //#BMW_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = IndexOfFixedFormula("#BMW_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = channels[idx_AccumulatedDistCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            channels[idx_AccumulatedDistCount].LogData[0] = tvData;

            for (int i = 1; i < channels[idx_DistCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = channels[idx_DistCount].LogData[i].DataValue - channels[idx_DistCount].LogData[i-1].DataValue;

                //ToDo File Append時、工夫が必要

                // カウンタ1周またはリセット時、負の値になる。
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+3951する
                    if (dCounter < -3900)
                    {
                        // DistCountFrは、MAX3951
                        dCounter += 3951;
                    }
                    else
                    {
                        dCounter = 0;
                    }
                }

                AccumulatedCounter += dCounter;

                tvData = channels[idx_AccumulatedDistCount].LogData[i];

                // 積算カウンタ
                // カウンタ値に0.000255掛けると、ちょうどFr車速を積分した積算距離に等しくなっている
                tvData.DataValue = AccumulatedCounter * 0.000255;
                channels[idx_AccumulatedDistCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_DistCountRr から K51_AccumulatedDistCountRr　を計算する
        /// </summary>
        private void CalcAccumulatedDistCountRr()
        {
            //#BMW_DistCountRr のChName取得
            int idx_DistCount = IndexOfFixedFormula("#BMW_DistCountRr");
            if (idx_DistCount < 0) { return; }

            //#BMW_AccumulatedDistCountRr のChName取得
            int idx_AccumulatedDistCount = IndexOfFixedFormula("#BMW_AccumulatedDistCountRr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = channels[idx_AccumulatedDistCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            channels[idx_AccumulatedDistCount].LogData[0] = tvData;

            for (int i = 1; i < channels[idx_DistCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = channels[idx_DistCount].LogData[i].DataValue - channels[idx_DistCount].LogData[i - 1].DataValue;

                //ToDo File Append時、工夫が必要

                // カウンタ1周またはリセット時、負の値になる。
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+255する
                    if (dCounter < -250)
                    {
                        // DistCountFrは、MAX255
                        dCounter += 255;
                    }
                    else
                    {
                        dCounter = 0;
                    }
                }

                AccumulatedCounter += dCounter;

                tvData = channels[idx_AccumulatedDistCount].LogData[i];

                // 積算カウンタ
                // カウンタ値に0.004掛けると、大体Fr車速を積分した積算距離に等しくなっている。
                // Rrは加速中Fr車速よりSlipするため早くなるので、Fr車速の積分よりは多少距離が伸びる
                tvData.DataValue = AccumulatedCounter * 0.004;
                channels[idx_AccumulatedDistCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// #BMW_FuelCount から #BMW_AccumulatedFuelCount
        /// </summary>
        private void CalcAccumulatedFuelCount()
        {
            //#BMW_FuelCount のChName取得
            int idx_FuelCount = IndexOfFixedFormula("#BMW_FuelCount");
            if (idx_FuelCount < 0) { return; }

            //#BMW_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = IndexOfFixedFormula("#BMW_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#BMW_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = channels[idx_AccumulatedFuelCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            channels[idx_AccumulatedFuelCount].LogData[0] = tvData;

            for (int i = 1; i < channels[idx_FuelCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = channels[idx_FuelCount].LogData[i].DataValue - channels[idx_FuelCount].LogData[i - 1].DataValue;

                //ToDo File Append時、工夫が必要

                // EGリセット時又はカウンタ1周すると負の値になる
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+256*256する
                    if (dCounter < -65000)
                    {
                        dCounter += 256 * 256;
                    } else {
                        dCounter = 0;
                    }
                }

                AccumulatedCounter += dCounter;

                tvData = channels[idx_AccumulatedFuelCount].LogData[i];

                // 積算カウンタ
                // 燃料カウンタは1000000で割るとちょうど良さそう
                tvData.DataValue = AccumulatedCounter / 1000000;
                channels[idx_AccumulatedFuelCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// 燃料カウンタと距離カウンタから、燃費を計算する
        /// 燃費カウンタデータは、2BCと同じデータタイミングとする
        /// </summary>
        private void CalcFuelConsumption()
        {
            //#BMW_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = IndexOfFixedFormula("#BMW_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#BMW_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = IndexOfFixedFormula("#BMW_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_FuelConsumption のChName取得
            int idx_FuelConsumption = IndexOfFixedFormula("#BMW_FuelConsumption");
            if (idx_FuelConsumption < 0) { return; }

            //燃費計算
            TVData tvData;
            
            for (int i = 0; i < channels[idx_FuelConsumption].Count; i++)
            {
                //積算燃料量取得
                double time = channels[idx_AccumulatedFuelCount].LogData[i].DataTime;
                double fuel = channels[idx_AccumulatedFuelCount].LogData[i].DataValue;

                //積算燃料取得時間に対応した、積算距離を取得
                int idx_dist = channels[idx_AccumulatedDistCount].FindLeftIndex(time);
                double dist = channels[idx_AccumulatedDistCount].LogData[idx_dist].DataValue;

                tvData = channels[idx_FuelConsumption].LogData[i];

                // 燃費
                if (fuel != 0) {
                    tvData.DataValue = dist  / fuel ;
                } else
                {
                    tvData.DataValue = 0;
                }
                channels[idx_FuelConsumption].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// 燃費とFuelLevelから走行可能距離を計算する
        /// 2BCと同じデータタイミングとする
        /// </summary>
        private void CalcRange()
        {
            const double K51_TANK_CAPA = 30d;

            //#BMW_FuelLevel のChName取得
            int idx_FuelLevel = IndexOfFixedFormula("#BMW_FuelLevel");
            if (idx_FuelLevel < 0) { return; }

            //#BMW_FuelConsumption のChName取得
            int idx_FuelConsumption = IndexOfFixedFormula("#BMW_FuelConsumption");
            if (idx_FuelConsumption < 0) { return; }

            //#BMW_Range のChName取得
            int idx_Range = IndexOfFixedFormula("#BMW_Range");
            if (idx_Range < 0) { return; }

            //Range計算
            TVData tvData;

            for (int i = 0; i < channels[idx_Range].Count; i++)
            {
                //燃費取得
                double time = channels[idx_FuelConsumption].LogData[i].DataTime;
                double fuelConsump = channels[idx_FuelConsumption].LogData[i].DataValue;

                //燃費計算時点のFuelLevel取得
                int idx_dist = channels[idx_FuelLevel].FindLeftIndex(time);
                double fuelLevel = channels[idx_FuelLevel].LogData[idx_dist].DataValue;

                tvData = channels[idx_Range].LogData[i];

                // 燃費
                if (fuelConsump != 0)
                {
                    tvData.DataValue = K51_TANK_CAPA * (fuelLevel / 100d) * fuelConsump;
                }
                else
                {
                    tvData.DataValue = 0;
                }
                channels[idx_Range].LogData[i] = tvData;
            }
        }
    }
}
