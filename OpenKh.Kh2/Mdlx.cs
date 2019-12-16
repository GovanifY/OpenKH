using OpenKh.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.BinaryMapper;
using Xe.IO;

namespace OpenKh.Kh2
{
    public partial class Mdlx
    {
        private const int Map = 2;
        private const int Entity = 3;
        private const int ReservedArea = 0x90;

        private SubModelHeader[] _subModels;
        public List<SubModel> SubModels => _subModels.Select(x => x.SubModel).ToList();
        public M4 MapModel { get; }

        private byte[] _copy;

        private Mdlx(Stream stream)
        {
            _copy = stream.SetPosition(ReservedArea).ReadBytes();
            stream.Position = 0;

            var type = ReadMdlxType(stream);
            stream.Position = 0;

            switch (type)
            {
                case Map:
                    MapModel = ReadAsMap(new SubStream(stream, ReservedArea, stream.Length - ReservedArea));
                    break;
                case Entity:
                    _subModels = ReadAsModel(stream).ToArray();
                    break;
            }
        }

        public void Write(Stream stream)
        {
            stream.Position = ReservedArea;
            stream.Write(_copy, 0, _copy.Length);

            stream.Position = 0x90;
            BinaryMapping.WriteObject(stream, _subModels[0]);
            for (int i = 0; i < _subModels[0].DmaChainCount; i++)
            {
                BinaryMapping.WriteObject(stream, _subModels[0].DmaChains[i]);
            }
            for (int i = 0; i < _subModels[0].DmaChainCount; i++)
            {
                foreach (var dmaVif in _subModels[0].SubModel.DmaVifs)
                {
                    WriteDmaChain(stream, _subModels[0].DmaChains[i], dmaVif);
                }
            }
        }

        public static Mdlx Read(Stream stream) =>
            new Mdlx(stream.SetPosition(0));

        private static int ReadMdlxType(Stream stream) =>
            stream.SetPosition(ReservedArea).ReadInt32();

        private static T[] For<T>(int count, Func<T> func) =>
            Enumerable.Range(0, count).Select(_ => func()).ToArray();
    }
}
