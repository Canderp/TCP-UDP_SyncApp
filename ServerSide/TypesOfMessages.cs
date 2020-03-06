using System;
using System.Collections.Generic;
using System.Text;

namespace SyncApp
{
    public class TypesOfMessages
    {
        public class CalMessageProcessor : Processor
        {
            public override void DoProcess(Queue<Message> q)
            {
                int.TryParse(Console.ReadLine(), out int val);
                Message calMsg = new CalMessage(val, TypeOfMessage.calT,Processor.revision+1);
                //CalCount = ;
                Populate(calMsg, q);
                //calMsg.
            }
        }
        public class CfgMessageProcessor : Processor
        {
            public override void DoProcess(Queue<Message> q)
            {
                Message cfgMsg = new CfgMessage(Console.ReadLine(), TypeOfMessage.cfgT,Processor.revision+1);
                //CfgCount += 1;
                Populate(cfgMsg, q);
            }
        }
    }
}