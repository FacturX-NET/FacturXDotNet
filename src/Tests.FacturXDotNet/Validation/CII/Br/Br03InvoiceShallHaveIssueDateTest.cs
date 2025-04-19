using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br03InvoiceShallHaveIssueDateTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocument!.IssueDateTime = DateOnly.FromDateTime(DateTime.Now);

        Br03InvoiceShallHaveIssueDate rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br03InvoiceShallHaveIssueDate rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocument!.IssueDateTime = default;

        Br03InvoiceShallHaveIssueDate rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
