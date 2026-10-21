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
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DWriteFactory = SharpDX.DirectWrite.Factory;
using Device = SharpDX.Direct3D11.Device;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using RenderTarget = SharpDX.Direct2D1.RenderTarget;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace MotoRecoViewer
{
    /// <summary>
    /// 1つのコントロールに対するDirect2D描画先。
    /// D3Dデバイス・スワップチェーン・レンダーターゲット・フォントを1組として持つ。
    /// MainChartとSubChartでそれぞれ1インスタンス生成する。
    /// </summary>
    class D2DCanvas : IDisposable
    {
        private const string FontFamily = "MS UI Gothic";
        private const float FontSize = 9.0f;

        private Device device;
        private SwapChain swapChain;
        private Texture2D backBuffer;
        private D2DFactory d2dFactory;
        private DWriteFactory factoryDWrite;
        private Surface surface;
        private RenderTarget renderTarget;
        private TextFormat textFont;
        private SolidColorBrush textBrush;

        /// <summary>
        /// Direct2Dの描画対象
        /// </summary>
        public RenderTarget RenderTarget
        {
            get { return renderTarget; }
        }

        /// <summary>
        /// 描画に使うフォント
        /// </summary>
        public TextFormat TextFont
        {
            get { return textFont; }
        }

        /// <summary>
        /// PathGeometryやStrokeStyleを作るためのDirect2Dファクトリ
        /// </summary>
        public D2DFactory Factory
        {
            get { return d2dFactory; }
        }

        /// <summary>
        /// 指定したコントロールのウィンドウハンドルに対して、D3Dデバイスとスワップチェーンを作成する
        /// </summary>
        /// <param name="outputHandle">描画先コントロールのHandle</param>
        public D2DCanvas(IntPtr outputHandle)
        {
            //SwapChain description
            SwapChainDescription desc = new SwapChainDescription()
            {
                BufferCount = 2,                                                                 //ToDo これで良いのか？
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = outputHandle,                                                     //パネルに描画する
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.FlipSequential,
                Usage = Usage.RenderTargetOutput
            };

            //D3Dデバイスとスワップチェーンの作成
            Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware
                , DeviceCreationFlags.BgraSupport, desc
                , out device, out swapChain);

            CreateSizeDependentResources();
        }

        /// <summary>
        /// バックバッファのサイズに依存するリソースを作成する
        /// </summary>
        private void CreateSizeDependentResources()
        {
            //バックバッファの作成
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);

            //D2Dレンダーターゲットの作成
            d2dFactory = new D2DFactory(SharpDX.Direct2D1.FactoryType.MultiThreaded);　　　　　　　　　　　　//ToDo これで良いのか？
            surface = backBuffer.QueryInterface<Surface>();
            PixelFormat pixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Premultiplied);
            RenderTargetProperties targetProp = new RenderTargetProperties(pixelFormat);
            renderTarget = new RenderTarget(d2dFactory, surface, targetProp);
            renderTarget.AntialiasMode = AntialiasMode.Aliased;
            renderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;

            //DirectWriteオブジェクトを生成するために必要なファクトリオブジェクトを生成
            factoryDWrite = new DWriteFactory();

            //文字描画用のブラシ。DrawText毎に作ると破棄されないまま溜まるので1本持っておく
            textBrush = new SolidColorBrush(renderTarget, SharpDX.Color.White);

            //D2Dフォント作成
            textFont = new TextFormat(factoryDWrite, FontFamily, FontSize)
            {
                // レイアウトに沿った文字の左右配置
                // ※読み方向軸に沿った段落テキストの相対的な配置を指定します
                TextAlignment = TextAlignment.Leading,
                // レイアウトに沿った文字の上下配置
                // ※相対フロー方向軸に沿った段落テキストの配置を指定します
                ParagraphAlignment = ParagraphAlignment.Near,
            };
        }

        /// <summary>
        /// バックバッファのサイズに依存するリソースを開放する。
        /// ResizeBuffersする前に、backBuffer含めすべての関係するリソースを開放する必要がある。
        /// </summary>
        private void ReleaseSizeDependentResources()
        {
            textBrush.Dispose();
            renderTarget.Dispose();
            backBuffer.Dispose();
            d2dFactory.Dispose();
            factoryDWrite.Dispose();
            textFont.Dispose();
            surface.Dispose();
        }

        /// <summary>
        /// 描画先コントロールのサイズ変更に追従する
        /// </summary>
        public void Resize()
        {
            device.ImmediateContext.ClearState();

            ReleaseSizeDependentResources();

            //SwapChainのりサイズ
            swapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);

            //再度初期化
            CreateSizeDependentResources();
        }

        /// <summary>
        /// バックバッファを画面に転送する
        /// </summary>
        public void Present()
        {
            swapChain.Present(0, PresentFlags.None);
        }

        /// <summary>
        /// 文字を画面上に描画
        /// </summary>
        public void DrawText(string text, Vector2 pos)
        {
            if (renderTarget == null)
            {
                return;
            }

            // 文字を描画する領域
            // ※「改行の目安」や「文字のAlignment」などで使用される
            float maxWidth = 1000.0f;
            float maxHeight = 1000.0f;

            // 文字描画
            renderTarget.DrawText(text, textFont,
                                  new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, pos.X + maxWidth, pos.Y + maxHeight),
                                  textBrush);
        }

        public void Dispose()
        {
            ReleaseSizeDependentResources();
            swapChain.Dispose();
            device.Dispose();
        }
    }
}
