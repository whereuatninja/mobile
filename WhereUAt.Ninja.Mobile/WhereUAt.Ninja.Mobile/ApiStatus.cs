using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereUAt.Ninja.Mobile
{
    class ApiStatus
    {
        public DateTime LastSentDateTime { get; private set; }
        public int StoredRequestsCount { get; private set; }

        public ApiStatus(int storedRequestsCount, DateTime lastSentDateTime)
        {
            StoredRequestsCount = storedRequestsCount;
            LastSentDateTime = lastSentDateTime;
        }
    }
}
