﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using NUnit.Framework;
using QuantConnect.Configuration;
using QuantConnect.ToolBox.BitfinexDownloader;

namespace QuantConnect.Tests.Brokerages.Bitfinex
{
    [TestFixture]
    [Explicit("This test requires a configured Bitfinex account")]
    public class BitfinexDownloaderTests
    {
        [TestCase("--app=BFXDL --from-date=20171101-00:00:00 --tickers=1INCHUSD --resolution=Minute", 1)]
        [TestCase("--app=BFXDL --from-date=20171101-00:00:00 --tickers=1INCHUSD,ADAUSDT --resolution=Minute", 2)]
        [TestCase("--app=BFXDL --from-date=20171101-00:00:00 --tickers=1INCHUSD,tADAUST --resolution=Minute", 2)]
        public void CanParseBitfinexTickersCorrectly(string args, int expectedCount)
        {
            var options = ToolboxArgumentParser.ParseArguments(args.Split(' '));
            var tickers = ToolboxArgumentParser.GetTickers(options);

            using var downloader = new BitfinexDataDownloader();
            var count = 0;
            foreach (var ticker in tickers)
            {
                if (!string.IsNullOrEmpty(downloader.GetSymbol(ticker)))
                {
                    count++;
                }
            }

            Assert.AreEqual(expectedCount, count);
        }
    }
}