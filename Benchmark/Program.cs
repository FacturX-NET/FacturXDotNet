// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using FacturXDotNet;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing;
using FacturXDotNet.Validation;

BenchmarkRunner.Run<BenchmarkCii>();


namespace Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.Method)]
    public class BenchmarkCii
    {
        readonly Dictionary<GuidelineSpecifiedDocumentContextParameterId, string> _examples = new()
        {
            { GuidelineSpecifiedDocumentContextParameterId.Minimum, "FacturX/0.MINIMUM/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf" },
            { GuidelineSpecifiedDocumentContextParameterId.BasicWl, "FacturX/1.BASIC WL/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf" },
            { GuidelineSpecifiedDocumentContextParameterId.Basic, "FacturX/2.BASIC/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf" },
            { GuidelineSpecifiedDocumentContextParameterId.En16931, "FacturX/3.EN16931/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf" },
            { GuidelineSpecifiedDocumentContextParameterId.Extended, "FacturX/4.EXTENDED/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf" }
        };

        [Params(
            GuidelineSpecifiedDocumentContextParameterId.Minimum,
            GuidelineSpecifiedDocumentContextParameterId.BasicWl,
            GuidelineSpecifiedDocumentContextParameterId.Basic,
            GuidelineSpecifiedDocumentContextParameterId.En16931,
            GuidelineSpecifiedDocumentContextParameterId.Extended
        )]
        public GuidelineSpecifiedDocumentContextParameterId Profile { get; set; }

        [Benchmark]
        public async Task ExtractCiiXml()
        {
            string sourceFilePath = GetSourceFilePath();
            await using FileStream file = File.OpenRead(sourceFilePath);

            FacturXParser parser = new();
            _ = await parser.ParseFacturXPdfAsync(file);
        }

        [Benchmark]
        public async Task ValidateFacturX()
        {
            string sourceFilePath = GetSourceFilePath();
            await using FileStream file = File.OpenRead(sourceFilePath);

            FacturXParser parser = new();
            FacturX facturX = await parser.ParseFacturXPdfAsync(file);

            FacturXValidator validator = new();
            _ = validator.IsValid(facturX);
        }

        string GetSourceFilePath()
        {
            string sourceFileFolder = Path.GetDirectoryName(WhereAmI()) ?? string.Empty;
            string sourceFileName = _examples[Profile];
            return Path.Join(sourceFileFolder, sourceFileName);
        }

        static string WhereAmI([CallerFilePath] string callerFilePath = "") => callerFilePath;
    }
}
