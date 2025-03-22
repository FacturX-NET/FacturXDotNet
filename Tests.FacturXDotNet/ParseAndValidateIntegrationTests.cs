using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Parsing;
using FacturXDotNet.Validation;
using Shouldly;

namespace Tests.FacturXDotNet;

[TestClass]
public class ParseAndValidateIntegrationTests
{
    [Ignore]
    [DataTestMethod]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/0.MINIMUM/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_MINIMUM.pdf", FacturXProfile.Minimum)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/1.BASIC WL/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC_WL.pdf", FacturXProfile.BasicWl)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/2.BASIC/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_BASIC.pdf", FacturXProfile.Basic)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/3.EN16931/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_EN_16931.pdf", FacturXProfile.En16931)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_UC1_2023020_AFF-LE_FOURNISSEUR-POUR-L'ACHETEUR_EXTENDED.pdf", FacturXProfile.Extended)]
    [DataRow("TestFiles/FacturX/4.EXTENDED/Facture_UC1_2023025_F-LE_FOURNISSEUR-POUR-L'ACHETEUR_EXTENDED.pdf", FacturXProfile.Extended)]
    public async Task ParseAndValidateProfile(string filePath, FacturXProfile profile)
    {
        await using FileStream file = File.OpenRead(filePath);

        FacturXParser parser = new();
        FacturXDocument invoice = await parser.ParseFacturXPdfAsync(file);

        FacturXValidator validator = new();
        FacturXValidationResult validationResult = validator.GetValidationResult(invoice);

        validationResult.Fatal.ShouldBeEmpty();
        validationResult.ValidProfiles.ShouldBe(profile.AndLower());
    }
}
