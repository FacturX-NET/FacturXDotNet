using FacturXDotNet;
using FacturXDotNet.Parsers.FacturX;
using FacturXDotNet.Validation;
using FacturXDotNet.Validation.CII.Schematron;
using Shouldly;

namespace Tests.FacturXDotNet;

[TestClass]
public class ParseAndValidateIntegrationTests
{
    [DataTestMethod]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_MINIMUM.pdf", FacturXProfileFlags.Minimum)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC_WL.pdf", FacturXProfileFlags.BasicWl)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC.pdf", FacturXProfileFlags.Basic)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_EN_16931.pdf", FacturXProfileFlags.En16931)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_EXTENDED.pdf", FacturXProfileFlags.Extended)]
    public async Task ParseAndValidateProfile(string filePath, FacturXProfileFlags profile)
    {
        await using FileStream file = File.OpenRead(filePath);

        FacturXParser parser = new();
        CrossIndustryInvoice invoice = await parser.ParseCiiXmlInFacturXPdfAsync(file);

        CrossIndustryInvoiceSchematronValidator validator = new();
        FacturXValidationResult validationResult = validator.GetValidationResult(invoice);

        validationResult.Failed.ShouldBeEmpty();
        validationResult.ValidProfiles.ShouldBe(profile.AndLower());
    }
}
