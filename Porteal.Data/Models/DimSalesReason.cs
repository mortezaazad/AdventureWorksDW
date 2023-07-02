using System;
using System.Collections.Generic;

namespace Portal.Data.Models;

public partial class DimSalesReason
{
    public int SalesReasonKey { get; set; }

    public int SalesReasonAlternateKey { get; set; }

    public string SalesReasonName { get; set; } = null!;

    public string SalesReasonReasonType { get; set; } = null!;

    public virtual ICollection<FactInternetSale> SalesOrders { get; set; } = new List<FactInternetSale>();
}
