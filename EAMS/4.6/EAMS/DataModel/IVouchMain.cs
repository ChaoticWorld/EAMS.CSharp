using System;

namespace DataModel
{
    public interface IVouchMain
    {
        IcorporatioBase corporatio { get; set; }
        DateTime CreateDate { get; set; }
        string CreateMan { get; set; }
        string cSender { get; set; }
        string cShipAddress { get; set; }
        decimal Je { get; set; }
        string Maker { get; set; }
        string Memo { get; set; }
        int Mid { get; set; }
        DateTime ModifyDate { get; set; }
        string ModifyMan { get; set; }
        string Verifier { get; set; }
        string vouchCode { get; set; }
        DateTime vouchDate { get; set; }
    }
}