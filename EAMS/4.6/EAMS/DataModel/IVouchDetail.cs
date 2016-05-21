namespace DataModel
{
    public interface IVouchDetail
    {
        long autoid { get; set; }
        int Mid { get; set; }
        int Did { get; set; }
        Inventory inventory { get; set; }
        double iPrice { get; set; }
        double iSum { get; set; }
        string Memo { get; set; }
        Quantity quantity { get; set; }
        Warehouse warehouse { get; set; }

        void getDetailField(string field, string code);
    }
}