using System.IO;
using KS.Fiks.ASiC_E;
using KS.Fiks.ASiC_E.Model;

namespace KS.Fiks.IO.Client.Asic
{
    public class AsiceBuilderFactory : IAsiceBuilderFactory
    {
        public IAsiceBuilder<AsiceArchive> GetBuilder(
            Stream outStream,
            MessageDigestAlgorithm messageDigestAlgorithm)
        {
            return AsiceBuilder.Create(outStream, messageDigestAlgorithm, null);
        }
    }
}