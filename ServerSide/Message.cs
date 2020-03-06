//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SyncApp
//{
//    public class Message
//    {
//        public int Revision { get; set; }
//        public TypeOfMessage MessageType { get; set; }
//        public virtual void Print()
//        {
//            Console.WriteLine();
//        }
//        public virtual void ValidateAndStore()
//        {
//            Console.ReadLine();
//        }
//    }

//    public class CalMessage : Message
//    {
//        public CalMessage(int calValue,TypeOfMessage type)
//        {
//            CalValue = calValue;
//            MessageType = type;
//        }
//        public int CalValue { get; set; }
//        public int CalCount { get; set; }
//        public override void Print()
//        {
//            Console.WriteLine(CalValue);
//        }
//    }
//    public class CfgMessage : Message
//    {
//        public CfgMessage(string cfgValue,TypeOfMessage type)
//        {
//             CfgValue = cfgValue;
//            MessageType = type;
//        }
//        public string CfgValue { get; set; }
//        public int CfgCount { get; set; }
//        public override void Print()
//        {
//            Console.WriteLine(CfgValue);
//        }
//        public override void ValidateAndStore()
//        {

//        }
//    }
//    public class Sync : Message
//    {
//        public Sync(int syncValue)
//        {
//            SyncValue = syncValue;
//        }
//        public int SyncValue { get; set; }
//        public int SyncCount { get; set; }
//        public int GetVal()
//        {
//            return SyncValue;
//        }
//    }
//    public enum TypeOfMessage
//    {
//        cal,
//        cfg,
//        sync
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncApp
{
    public interface IMessage
    {
        void Print();
        int GetIndividualCount();
    }
    public class Message : IMessage
    {
        public int Revision { get; set; }
        public TypeOfMessage MessageType { get; set; }
        public virtual void Print()
        {
            Console.WriteLine();
        }
        public virtual string GetValue()
        {
            return null;
        }

        public virtual int GetIndividualCount()
        {
            return 0;
        }
        public static List<ReturnRecentItems> FetchQueueContent(Queue<Message> q)
        {
            
            List<TypeOfMessage> tempType = new List<TypeOfMessage>();
            List<ReturnRecentItems> items = new List<ReturnRecentItems>();
            var list = q.ToList().OrderByDescending(x => x.Revision);

            foreach (var item in list)
            {
                if (!tempType.Contains(item.MessageType))
                {
                    tempType.Add(item.MessageType);
                    items.Add(new ReturnRecentItems(){ ValueOfItem = item.GetValue(), MessageTypeOfItem = item.MessageType});
                }
            }
            return items;
        }
    }

    public class ReturnRecentItems
    {
        public string ValueOfItem { get; set; }
        public TypeOfMessage MessageTypeOfItem { get; set; }
        public ReturnRecentItems(){}
    }
    public class CalMessage : Message
    {
        public CalMessage(int calValue, TypeOfMessage type,int temp)
        {
            CalValue = calValue;
            MessageType = type;
            CalCount = temp;
        }
        public int CalValue { get; set; }
        public int CalCount { get; set; }
        public override void Print()
        {
            Console.WriteLine(CalValue+"--"+CalCount);
        }
        public override string GetValue()
        {
            return CalValue.ToString();
        }

        public override int GetIndividualCount()
        {
            return CalCount;
        }
    }
    public class CfgMessage : Message
    {
        public CfgMessage(string cfgValue, TypeOfMessage type,int temp)
        {
            CfgValue = cfgValue;
            MessageType = type;
            CfgCount = temp;
        }
        public string CfgValue { get; set; }
        public int CfgCount { get; set; }
        public override void Print()
        {
            Console.WriteLine(CfgValue+"--"+CfgCount);
        }
        public override string GetValue()
        {
            return CfgValue;
        }

        public override int GetIndividualCount()
        {
            return CfgCount;
        }
    }
    public class Sync : Message
    {
        public Sync(int syncValue)
        {
            SyncValue = syncValue;
        }
        public int SyncValue { get; set; }
        public int SyncCount { get; set; }
        public int GetVal()
        {
            return SyncValue;
        }
    }
    public enum TypeOfMessage
    {
        calT,
        cfgT,
        sync
    }
}
