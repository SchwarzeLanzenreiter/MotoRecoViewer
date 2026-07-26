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
using System.IO;

namespace MotoRecoViewer
{
    /// <summary>
    /// MotoRecoが記録したdatファイルの読み取り。
    /// 1レコード16バイトの固定長で、時刻(4+2バイト)・CAN ID(2バイト)・データ(8バイト)が並ぶ。
    /// </summary>
    static class CanDataFile
    {
        /// <summary>
        /// 1レコードのバイト数
        /// </summary>
        public const int RecordSize = 16;

        /// <summary>
        /// datファイルを全件読み込む。
        /// 本当はメモリにマルッとコピーしたくないが、後でParallel.Forで処理する都合で配列に確保する。
        /// </summary>
        /// <param name="fileName">読み込むdatファイル</param>
        /// <param name="onProgress">進捗通知(読み込み済件数, 全件数)。ファイル全体で最大100回程度呼ばれる。不要ならnull。</param>
        /// <returns>読み込んだCANデータ。読み取れるレコードが無い場合は空配列。</returns>
        public static CanData[] ReadAll(string fileName, Action<long, long> onProgress)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                long arySize = CountRecords(fs.Length);

                if (arySize <= 0)
                {
                    return new CanData[0];
                }

                CanData[] aryCanData = new CanData[arySize];

                //毎レコード通知するとクソ遅いので、トータル100回程度に間引く
                long div_num = arySize / 100;

                for (long i = 0; i < arySize; i++)
                {
                    if ((onProgress != null) && (div_num > 0) && (i % div_num == 0))
                    {
                        onProgress(i, arySize);
                    }

                    aryCanData[i] = ReadRecord(reader);
                }

                return aryCanData;
            }
        }

        /// <summary>
        /// datファイルを1件ずつ読み出す。
        /// 全件をメモリに載せる必要がない用途はこちらを使う。
        /// </summary>
        public static IEnumerable<CanData> Read(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                long arySize = CountRecords(fs.Length);

                for (long i = 0; i < arySize; i++)
                {
                    yield return ReadRecord(reader);
                }
            }
        }

        /// <summary>
        /// 現在位置から1レコード読み出す
        /// </summary>
        private static CanData ReadRecord(BinaryReader reader)
        {
            CanData record;

            record.timeSec = reader.ReadUInt32();
            record.timeMSec = reader.ReadUInt16();
            record.id = reader.ReadUInt16();
            record.data = new byte[8];

            for (int j = 0; j < 8; j++)
            {
                record.data[j] = reader.ReadByte();
            }

            return record;
        }

        /// <summary>
        /// ファイルサイズから読み取り可能なレコード数を求める。
        /// CANDataが壊れていると16で割り切れないかもしれないので、
        /// あまりが出たら読み込む個数を1減らす。
        /// </summary>
        private static long CountRecords(long fileSize)
        {
            long arySize = fileSize / RecordSize;

            if (fileSize % RecordSize != 0)
            {
                arySize--;
            }

            return arySize;
        }
    }
}
