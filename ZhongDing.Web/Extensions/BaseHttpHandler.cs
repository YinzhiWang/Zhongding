using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZhongDing.Common;

namespace ZhongDing.Web.Extensions
{
    public class BaseHttpHandler : IRequiresSessionState
    {
        #region Members

        /// <summary>
        /// The request data
        /// </summary>
        private AjaxRequestData _RequestData;
        /// <summary>
        /// Gets the request data.
        /// </summary>
        /// <value>The request data.</value>
        protected AjaxRequestData RequestData
        {
            get
            {
                if (_RequestData == null)
                {
                    string requestData = HttpContext.Current.Request["RequestData"];

                    if (!string.IsNullOrEmpty(requestData))
                        _RequestData = Utility.JsonDeserializeObject<AjaxRequestData>(requestData);
                    else
                        _RequestData = new AjaxRequestData();
                }

                return _RequestData;
            }
        }

        #endregion
    }
}