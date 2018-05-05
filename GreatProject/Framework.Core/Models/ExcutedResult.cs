using Framework.Core.Common;

namespace Framework.Core.Models
{
    public class ExcutedResult
    {
        public Status Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public ExcutedResult(Status status, string msg, object rows)
        {
            this.Status = status;
            this.Message = msg;
            this.Data = rows;
        }
        public static ExcutedResult SuccessResult(string msg = null)
        {
            return new ExcutedResult(Status.Success, msg, null);
        }
        public static ExcutedResult SuccessResult(object data)
        {
            return new ExcutedResult(Status.Success, null, data);
        }
        public static ExcutedResult SuccessResult(string msg, object data)
        {
            return new ExcutedResult(Status.Success, msg, data);
        }

        public static ExcutedResult FailedResult(string msg)
        {
            return new ExcutedResult(Status.Failed, msg, null);
        }
    }

    public class PaginationResult : ExcutedResult
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount => total % pageSize == 0 ? total / pageSize : total / pageSize + 1;

        public PaginationResult(Status status, string msg, object data) : base(status, msg, data)
        {
        }

        public static PaginationResult PagedResult(object data, int total, int size, int index)
        {
            return new PaginationResult(Status.Success, null, data)
            {
                total = total,
                pageSize = size,
                pageIndex = index
            };
        }
    }
}
