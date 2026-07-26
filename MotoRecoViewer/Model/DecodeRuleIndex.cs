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

using System.Collections.Generic;

namespace MotoRecoViewer
{
    /// <summary>
    /// CAN IDから、それに該当するDecodeRuleのインデックス列を引くための索引。
    ///
    /// DecodeRule.IndexOf(ushort)はList.FindIndexによる線形探索で、
    /// ラムダがCAN IDをキャプチャするため呼び出しごとにクロージャも確保される。
    /// CANデータ1件ごとに呼ぶには重すぎるので、デコード前に一度だけ索引を作る。
    ///
    /// DecodeRuleのスナップショットなので、作成後にDecodeRuleを変更した場合は作り直すこと。
    /// </summary>
    class DecodeRuleIndex
    {
        private static readonly int[] NoRule = new int[0];

        // 添字がCAN ID。該当ルールが無いIDはnull。
        private readonly int[][] rulesByCanId;

        public DecodeRuleIndex(DecodeRule decodeRule)
        {
            int maxCanId = 0;

            for (int i = 0; i < decodeRule.Count; i++)
            {
                if (decodeRule.GetCANID(i) > maxCanId)
                {
                    maxCanId = decodeRule.GetCANID(i);
                }
            }

            // ルール番号の昇順に積む。
            // 元のIndexOfによる探索も昇順に見つけていたので、デコード順序を変えないため。
            List<int>[] work = new List<int>[maxCanId + 1];

            for (int i = 0; i < decodeRule.Count; i++)
            {
                ushort canId = decodeRule.GetCANID(i);

                if (work[canId] == null)
                {
                    work[canId] = new List<int>();
                }

                work[canId].Add(i);
            }

            rulesByCanId = new int[maxCanId + 1][];

            for (int i = 0; i <= maxCanId; i++)
            {
                if (work[i] != null)
                {
                    rulesByCanId[i] = work[i].ToArray();
                }
            }
        }

        /// <summary>
        /// 指定したCAN IDに該当するDecodeRuleのインデックス列を返す。
        /// 該当が無ければ空配列を返す。
        /// </summary>
        public int[] this[ushort canId]
        {
            get
            {
                if (canId >= rulesByCanId.Length)
                {
                    return NoRule;
                }

                return rulesByCanId[canId] ?? NoRule;
            }
        }
    }
}
