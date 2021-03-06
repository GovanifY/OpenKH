﻿using OpenKh.Kh2.Messages;
using System.Collections.Generic;
using Xunit;

namespace OpenKh.Tests.kh2
{
    public class MsgEncoderTests
    {
        [Fact]
        public void DecodeTextCorrectly()
        {
            var decoded = Encoders.InternationalSystem.Decode(new byte[]
            {
                0x35, 0x9E, 0xA5, 0xA5, 0xA8, 0x01, 0xB0, 0xA8, 0xAB, 0xA5, 0x9D, 0x48, 0x00
            });

            Assert.Equal(MessageCommand.PrintText, decoded[0].Command);
            Assert.Equal("Hello world!", decoded[0].Text);
            Assert.Equal(MessageCommand.End, decoded[1].Command);
        }

        [Fact]
        public void DecodeComplexTextCorrectly()
        {
            var decoded = Encoders.InternationalSystem.Decode(new byte[]
            {
                0x90, 0x7a, 0x91, 0x00
            });

            Assert.Equal(MessageCommand.PrintText, decoded[0].Command);
            Assert.Equal("0", decoded[0].Text);
            Assert.Equal(MessageCommand.PrintComplex, decoded[1].Command);
            Assert.Equal("VII", decoded[1].Text);
            Assert.Equal(MessageCommand.PrintText, decoded[2].Command);
            Assert.Equal("1", decoded[2].Text);
            Assert.Equal(MessageCommand.End, decoded[3].Command);
        }

        [Fact]
        public void DecodeIconCorrectly()
        {
            var decoded = Encoders.InternationalSystem.Decode(new byte[]
            {
                0x09, 0x00, 0x09, 0x01, 0x00
            });

            Assert.Equal(MessageCommand.PrintIcon, decoded[0].Command);
            Assert.Equal(0, decoded[0].Data[0]);
            Assert.Equal(MessageCommand.PrintIcon, decoded[1].Command);
            Assert.Equal(1, decoded[1].Data[0]);
            Assert.Equal(MessageCommand.End, decoded[2].Command);
        }

        [Fact]
        public void DecodeColorCommandCorrectly()
        {
            var decoded = Encoders.InternationalSystem.Decode(new byte[]
            {
                0x07, 0x00, 0xFF, 0x00, 0x00, 0x00
            });

            Assert.Equal(MessageCommand.Color, decoded[0].Command);
            Assert.Equal(0, decoded[0].Data[0]);
            Assert.Equal(0xFF, decoded[0].Data[1]);
            Assert.Equal(0, decoded[0].Data[2]);
            Assert.Equal(0, decoded[0].Data[3]);
            Assert.Equal(MessageCommand.End, decoded[1].Command);
        }

        [Theory]
        [InlineData(0x02, "0123456789")]
        [InlineData(0x03, "0123456789")]
        [InlineData(0x04, "123456789")]
        [InlineData(0x05, "6789")]
        [InlineData(0x06, "123456789")]
        [InlineData(0x07, "456789")]
        [InlineData(0x08, "3456789")]
        [InlineData(0x09, "123456789")]
        [InlineData(0x0a, "123456789")]
        [InlineData(0x0b, "123456789")]
        [InlineData(0x0c, "123456789")]
        [InlineData(0x0d, "0123456789")]
        [InlineData(0x0e, "123456789")]
        [InlineData(0x0f, "56789")]
        //[InlineData(0x10, "")]
        [InlineData(0x11, "456789")]
        [InlineData(0x12, "23456789")]
        [InlineData(0x13, "456789")]
        [InlineData(0x14, "23456789")]
        [InlineData(0x15, "23456789")]
        [InlineData(0x16, "123456789")]
        //[InlineData(0x17, "")]
        [InlineData(0x18, "23456789")]
        [InlineData(0x19, "123456789")]
        [InlineData(0x1a, "123456789")]
        [InlineData(0x1b, "123456789")]
        [InlineData(0x1c, "123456789")]
        [InlineData(0x1d, "123456789")]
        [InlineData(0x1e, "123456789")]
        [InlineData(0x1f, "123456789")]
        public void DecodeTheRightAmountOfCharacters(byte commandId, string expectedText)
        {
            var decoded = Encoders.InternationalSystem.Decode(new byte[]
            {
                commandId, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99
            });

            Assert.Equal(expectedText, decoded[1].Text);
        }

        [Theory]
        [InlineData("I", 0x74)]
        [InlineData("II", 0x75)]
        [InlineData("III", 0x76)]
        [InlineData("IV", 0x77)]
        [InlineData("V", 0x78)]
        [InlineData("VI", 0x79)]
        [InlineData("♪", 0x8e)]
        public void EncodeComplexTextCorrectly(string value, byte expected)
        {
            var actual = Encoders.InternationalSystem.Encode(new List<MessageCommandModel>
            {
                new MessageCommandModel
                {
                    Command = MessageCommand.PrintComplex,
                    Text = value
                }
            })[0];

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SimpleReEncodeTest()
        {
            var expected = new byte[]
            {
                0x01, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96,
                0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e,
            };
            var decoded = Encoders.InternationalSystem.Decode(expected);
            var encoded = Encoders.InternationalSystem.Encode(decoded);

            Assert.Equal(expected, encoded);
        }

        [Theory]
        [InlineData(0x02)]
        [InlineData(0x03)]
        [InlineData(0x04)]
        [InlineData(0x05)]
        [InlineData(0x06)]
        [InlineData(0x07)]
        [InlineData(0x08)]
        [InlineData(0x09)]
        [InlineData(0x0a)]
        [InlineData(0x0b)]
        [InlineData(0x0c)]
        [InlineData(0x0d)]
        [InlineData(0x0e)]
        [InlineData(0x0f)]
        //[InlineData(0x10)]
        [InlineData(0x11)]
        [InlineData(0x12)]
        [InlineData(0x13)]
        [InlineData(0x14)]
        [InlineData(0x15)]
        [InlineData(0x16)]
        //[InlineData(0x17)]
        [InlineData(0x18)]
        [InlineData(0x19)]
        [InlineData(0x1a)]
        [InlineData(0x1b)]
        [InlineData(0x1c)]
        [InlineData(0x1d)]
        [InlineData(0x1e)]
        [InlineData(0x1f)]
        public void AdvancedReEncodeTest(byte commandByte)
        {
            var expected = new byte[]
            {
                commandByte, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96,
                0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e,
            };
            var decoded = Encoders.InternationalSystem.Decode(expected);
            var encoded = Encoders.InternationalSystem.Encode(decoded);

            Assert.Equal(expected, encoded);
        }
    }
}
