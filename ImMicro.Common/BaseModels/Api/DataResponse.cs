namespace ImMicro.Common.BaseModels.Api
{
    public class DataResponse : BaseResponse
    {
        public DataResponse()
        {
            Data = new object();
        }

        public object Data { get; set; }
    }
}