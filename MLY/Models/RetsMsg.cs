using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLY.Models
{
    public sealed class RetsMsg
    {
        #region 消息体
        /// <summary>
        /// 无数据消息体
        /// </summary>
        public struct Msg
        {
            /// <summary>
            /// 成功编码1000，失败编码从-1001开始
            /// </summary>
            public int code { get; set; }
            public string tips { get; set; }
        }

        /// <summary>
        /// 带数据消息体
        /// </summary>
        public struct DataMsg
        {
            /// <summary>
            /// 成功编码1000，失败编码从-1001开始
            /// </summary>
            public int code { get; set; }
            public string tips { get; set; }
            public object data { get; set; }
        }
        #endregion

        private object _objMsg;

        /// <summary>
        /// 无数据消息包
        /// </summary>
        /// <param name="msg"></param>
        public RetsMsg(Msg msg)
        {
            this._objMsg = msg;
        }

        /// <summary>
        /// 有数据消息包
        /// </summary>
        /// <param name="msg"></param>
        public RetsMsg(DataMsg msg)
        {
            this._objMsg = msg;
        }

        /// <summary>
        /// 系列化Json消息串
        /// </summary>
        public void ToJsonMsg()
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(this._objMsg));
            HttpContext.Current.Response.End();
        }
    }
}