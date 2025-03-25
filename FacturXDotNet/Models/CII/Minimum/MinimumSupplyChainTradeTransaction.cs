﻿namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SupplyChainTradeTransaction" />
public class MinimumSupplyChainTradeTransaction
{
    MinimumApplicableHeaderTradeAgreement _applicableHeaderTradeAgreement;
    MinimumApplicableHeaderTradeDelivery _applicableHeaderTradeDelivery;
    MinimumApplicableHeaderTradeSettlement? _applicableHeaderTradeSettlement;

    internal MinimumSupplyChainTradeTransaction(SupplyChainTradeTransaction supplyChainTradeTransaction)
    {
        SupplyChainTradeTransaction = supplyChainTradeTransaction;
        _applicableHeaderTradeAgreement = new MinimumApplicableHeaderTradeAgreement(supplyChainTradeTransaction.ApplicableHeaderTradeAgreement!);
        _applicableHeaderTradeDelivery = new MinimumApplicableHeaderTradeDelivery(supplyChainTradeTransaction.ApplicableHeaderTradeDelivery!);
        _applicableHeaderTradeSettlement = supplyChainTradeTransaction.ApplicableHeaderTradeSettlement == null
            ? null
            : new MinimumApplicableHeaderTradeSettlement(supplyChainTradeTransaction.ApplicableHeaderTradeSettlement);
    }

    internal SupplyChainTradeTransaction SupplyChainTradeTransaction { get; }

    /// <inheritdoc cref="CII.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement" />
    public MinimumApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement {
        get => _applicableHeaderTradeAgreement;

        set {
            _applicableHeaderTradeAgreement = value;
            SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement = value.ApplicableHeaderTradeAgreement;
        }
    }

    /// <inheritdoc cref="CII.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery" />
    public MinimumApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery {
        get => _applicableHeaderTradeDelivery;

        set {
            _applicableHeaderTradeDelivery = value;
            SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery = value.ApplicableHeaderTradeDelivery;
        }
    }

    /// <inheritdoc cref="CII.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement" />
    public MinimumApplicableHeaderTradeSettlement? ApplicableHeaderTradeSettlement {
        get => _applicableHeaderTradeSettlement;

        set {
            _applicableHeaderTradeSettlement = value;
            SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = value?.ApplicableHeaderTradeSettlement;
        }
    }
}
