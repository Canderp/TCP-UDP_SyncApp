using System;
using System.Collections.Generic;
using System.Text;

namespace SyncApp
{
    public class ProcessorProvider
    {
        // communicate between the 2 processes over UDP.
        Processor p;

        private List<OptionsProcessorMap> optionsProcessor = new List<OptionsProcessorMap>
        {
            new OptionsProcessorMap(new List<string> {"1", "2"}, new MessageProcessor()),
            new OptionsProcessorMap(new List<string> {"3"}, new DisplayProcessor())
        };

        public Processor GetProcessor(string userInput)
        {
            foreach (var map in optionsProcessor)
            {
                if (map.Options.Contains(userInput))
                {
                    p = map.ProcessorInstance;
                }
            }
            return p;
        }
    }

    public class OptionsProcessorMap
    {
        public List<string> Options { get; }
        public Processor ProcessorInstance { get; }

        public OptionsProcessorMap(List<string> options, Processor processorInstance)
        {
            Options = options;
            ProcessorInstance = processorInstance;
        }

    }
}
