namespace CommonLib.Protocols
{

    public class REQ_Header
    {
        public long AccIdx { get; set; }

        public MARKET_TYPE MktType { get; set; }
    }


    public class RES_Header
    {
        public int Rsp_Code { get; set; } = (int)RESPONSE_CODE.SUCCESS;
    }
}
