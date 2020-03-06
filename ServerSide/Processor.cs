using System;
using System.Collections.Generic;
using System.Text;

namespace SyncApp
{
    public interface IMethodProvider
    {
        void DoProcess(Queue<Message> q);
        void InitializeProcess(Queue<Message> q, string selection);
        void Populate(Message msgObject, Queue<Message> q);
    }
    
    public class Processor : IMethodProvider
    {
        public static int revision = -1;
        public virtual void DoProcess(Queue<Message> q)
        {
            Console.WriteLine();
        }

        public virtual void InitializeProcess(Queue<Message> q, string selection)
        {
            Console.WriteLine();
        }

        public void Populate(Message msgObject, Queue<Message> q)
        {
            revision += 1;
            msgObject.Revision = revision;
            q.Enqueue(msgObject);
        }

    }
    public class DisplayProcessor : Processor
    {
        public override void InitializeProcess(Queue<Message> q, string selection)
        {
            DoProcess(q);
        }
        public override void DoProcess(Queue<Message> q)
        {
            ShowOutput(q);
        }
        public void ShowOutput(Queue<Message> q)
        {
            //int k;
            Console.WriteLine("Revision--MessageType--Value");
            foreach (var obj in q)
            {
                Console.Write(obj.Revision + "--" + obj.MessageType + "--");
                obj.Print();
                //k = obj.Count;
            }
            Console.WriteLine("----------------------------------\n");
            //Console.WriteLine();
            //Console.WriteLine(obj.Count);
            Console.WriteLine("\n---------------------------------");
        }
    }
    public class MessageProcessor : Processor
    {
        Processor p;
        private List<OptionsMessageProcessor> messageProcessorOptions = new List<OptionsMessageProcessor>
        {
            new OptionsMessageProcessor("1",new TypesOfMessages.CalMessageProcessor()),
            new OptionsMessageProcessor("2",new TypesOfMessages.CfgMessageProcessor())
        };
        public override void InitializeProcess(Queue<Message> q, string selection)
        {
            var handle = InitiateMessageProcessor(selection);
            handle.DoProcess(q);
        }
        public Processor InitiateMessageProcessor(string input)
        {
            foreach (var temp in messageProcessorOptions)
            {
                if (temp.KeyOption.Contains(input))
                {
                    p = temp.MessageProcessorInstance;
                }
            }
            return p;
        }
    }

    public class OptionsMessageProcessor
    {
        public List<string> KeyOption { get; set; }
        public Processor MessageProcessorInstance { get; set; }

        public OptionsMessageProcessor(string Key, Processor ProcessorInstance)
        {
            KeyOption = new List<string>
            {
                Key
            };
            MessageProcessorInstance = ProcessorInstance;
        }
    }

}

