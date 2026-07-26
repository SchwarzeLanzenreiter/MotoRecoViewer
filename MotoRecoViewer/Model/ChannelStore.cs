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

namespace MotoRecoViewer
{
    /// <summary>
    /// 読み込み済みチャンネルの集合。
    /// ChDataのListと、ChName→インデックスの辞書を対で保持する。
    /// 並び替えや削除で両者の整合が崩れないよう、更新操作はすべてこのクラスを通す。
    /// </summary>
    class ChannelStore
    {
        private readonly List<ChData> channels = new List<ChData>();
        private readonly Dictionary<string, int> indexByName = new Dictionary<string, int>();

        /// <summary>
        /// 保持しているチャンネル数
        /// </summary>
        public int Count
        {
            get { return channels.Count; }
        }

        /// <summary>
        /// インデックス指定でチャンネルを取得する
        /// </summary>
        public ChData this[int index]
        {
            get { return channels[index]; }
        }

        /// <summary>
        /// 指定したChNameのチャンネルが存在するか
        /// </summary>
        public bool Contains(string chName)
        {
            return indexByName.ContainsKey(chName);
        }

        /// <summary>
        /// 指定したChNameのインデックスを返す。存在しない場合は-1。
        /// </summary>
        public int IndexOf(string chName)
        {
            int index;
            if (!indexByName.TryGetValue(chName, out index))
            {
                return -1;
            }
            return index;
        }

        /// <summary>
        /// チャンネルを追加し、そのインデックスを返す。
        /// 同名チャンネルが既に存在する場合は追加せず、既存のインデックスを返す。
        /// </summary>
        public int Add(ChData chData)
        {
            int existing = IndexOf(chData.ChName);
            if (existing >= 0)
            {
                return existing;
            }

            int index = channels.Count;
            channels.Add(chData);
            indexByName.Add(chData.ChName, index);
            return index;
        }

        /// <summary>
        /// 全チャンネルを破棄する
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < channels.Count; i++)
            {
                channels[i].Clear();
            }

            channels.Clear();
            indexByName.Clear();
        }

        /// <summary>
        /// ChName順に並び替える。
        /// CANデータ読み込みのたびにChNameとインデックスの対応が変化すると、
        /// CSVエクスポート時に列の位置が毎回変わってしまうため。
        /// </summary>
        public void SortByName()
        {
            channels.Sort((a, b) => a.ChName.CompareTo(b.ChName));
            RebuildIndex();
        }

        /// <summary>
        /// 条件に合致しないチャンネルを削除する
        /// </summary>
        /// <param name="keep">残す場合trueを返す判定</param>
        public void RemoveAllExcept(Predicate<ChData> keep)
        {
            int i = 0;
            bool removed = false;

            while (i < channels.Count)
            {
                if (keep(channels[i]))
                {
                    i++;
                }
                else
                {
                    channels[i].Clear();
                    channels.RemoveAt(i);
                    removed = true;
                }
            }

            if (removed)
            {
                RebuildIndex();
            }
        }

        /// <summary>
        /// ChName→インデックスの辞書を、現在のList順に合わせて作り直す
        /// </summary>
        private void RebuildIndex()
        {
            indexByName.Clear();

            for (int i = 0; i < channels.Count; i++)
            {
                indexByName.Add(channels[i].ChName, i);
            }
        }
    }
}
