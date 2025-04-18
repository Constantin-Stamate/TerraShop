using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.BusinessLogic
{
    public class BusinessLogicManager
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
    }
}
