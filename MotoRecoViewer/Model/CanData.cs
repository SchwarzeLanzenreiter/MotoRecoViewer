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

namespace MotoRecoViewer
{
    /// <summary>
    /// datファイル上の1CANメッセージ。ファイル上は1件16バイト固定。
    ///
    /// データ8バイトはbyte[]ではなくフィールドとして直接持つ。
    /// 1件ごとにbyte[8]を確保すると、100万件規模のログでは確保コストと
    /// GC負荷が無視できず、配列が飛び飛びに置かれてデコード時のキャッシュ効率も落ちる為。
    /// 8バイトへは this[0]～this[7] でアクセスする。
    /// </summary>
    internal struct CanData
    {
        public uint timeSec;
        public ushort timeMSec;
        public ushort id;

        public byte d0;
        public byte d1;
        public byte d2;
        public byte d3;
        public byte d4;
        public byte d5;
        public byte d6;
        public byte d7;

        /// <summary>
        /// データ部の0～7バイト目
        /// </summary>
        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return d0;
                    case 1: return d1;
                    case 2: return d2;
                    case 3: return d3;
                    case 4: return d4;
                    case 5: return d5;
                    case 6: return d6;
                    case 7: return d7;
                    default: throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: d0 = value; break;
                    case 1: d1 = value; break;
                    case 2: d2 = value; break;
                    case 3: d3 = value; break;
                    case 4: d4 = value; break;
                    case 5: d5 = value; break;
                    case 6: d6 = value; break;
                    case 7: d7 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
