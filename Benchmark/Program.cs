// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using FacturXDotNet.Parsing.FacturX;

BenchmarkRunner.Run<BenchmarkCii>();


namespace Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class BenchmarkCii
    {
        [Params(
            "FacturX/0.MINIMUM/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf",
            "FacturX/1.BASIC WL/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf",
            "FacturX/2.BASIC/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf",
            "FacturX/3.EN16931/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf",
            "FacturX/4.EXTENDED/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"
        )]
        public string FilePath { get; set; } = string.Empty;

        [Benchmark]
        public async Task CII_XML()
        {
            string sourceFileFolder = Path.GetDirectoryName(WhereAmI()) ?? string.Empty;
            await using FileStream file = File.OpenRead(Path.Join(sourceFileFolder, FilePath));

            FacturXParser parser = new();
            _ = await parser.ParseCiiXmlInFacturXPdfAsync(file);
        }

        static string WhereAmI([CallerFilePath] string callerFilePath = "") => callerFilePath;
    }
}
