using Bogus;
using FacturXDotNet;

namespace Tests.FacturXDotNet.TestTools;

static class CiiGenerators
{
    public static readonly Faker<CrossIndustryInvoice> CrossIndustryInvoice = new();
}
